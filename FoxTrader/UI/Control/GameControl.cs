//   !!  // FoxTrader - GameControl.cs
// *.-". // Created: 01-02-2016 [9:36 PM]
//  | |  // ʇɟǝʃʎdoƆ 2016 FoxCouncil 

#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FoxTrader.UI.Anim;
using FoxTrader.UI.Anim.Size;
using FoxTrader.UI.DragDrop;
using FoxTrader.UI.Platform;
using OpenTK.Input;
using static FoxTrader.Constants;

#endregion

namespace FoxTrader.UI.Control
{
    /// <summary>Base game control class</summary>
    internal class GameControl : IDisposable
    {
        private readonly Dictionary<string, GameControlEventHandler> m_accelerators;

        private readonly List<GameControl> m_children;
        private readonly long[] m_lastClickedTime = new long[kMaxMouseButtons];
        private GameControl m_actualParent;
        private Rectangle m_bounds;
        private bool m_cacheTextureDirty;
        private Pos m_dock;
        private bool m_drawDebugOutlines;
        private bool m_hidden;

        protected GameControl m_innerControl;
        private bool m_isDisposed;
        private Margin m_margin;
        private bool m_needsLayout;
        private Package m_package;
        private Padding m_padding;
        private GameControl m_parent;
        private Rectangle m_renderBounds;
        private Skin m_skin;
        private GameControl m_toolTip;

        /// <summary>Initializes a new instance of the <see cref="GameControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        internal GameControl(GameControl c_parentControl = null)
        {
            m_children = new List<GameControl>();
            m_accelerators = new Dictionary<string, GameControlEventHandler>();

            Parent = c_parentControl;

            m_hidden = false;
            m_bounds = new Rectangle(0, 0, 10, 10);
            m_padding = Padding.kZero;
            m_margin = Margin.kZero;

            RestrictToParent = false;

            MouseInputEnabled = true;
            KeyboardInputEnabled = false;

            Invalidate();

            Cursor = Cursors.Default;
            IsTabable = false;
            ShouldDrawBackground = true;
            IsDisabled = false;
            m_cacheTextureDirty = true;
            ShouldCacheToTexture = false;

            BoundsOutlineColor = Color.Red;
            MarginOutlineColor = Color.Green;
            PaddingOutlineColor = Color.Blue;
        }

        /// <summary>Control's size and position relative to the parent</summary>
        public Rectangle Bounds => m_bounds;

        /// <summary>Bounds for the renderer</summary>
        public Rectangle RenderBounds => m_renderBounds;

        /// <summary>Determines whether hover should be drawn during rendering</summary>
        protected bool ShouldDrawHover => GetCanvas().MouseFocus == this || GetCanvas().MouseFocus == null;

        protected virtual bool AccelOnlyFocus => false;

        protected virtual bool NeedsInputChars => false;

        /// <summary>Indicates whether the control is on top of its parent's children</summary>
        public virtual bool IsOnTop => Parent != null && this == Parent.m_children.First();

        /// <summary>Logical list of children. If InnerControl is not null, returns InnerControl's children</summary>
        public List<GameControl> Children => m_innerControl != null ? m_innerControl.Children : m_children;

        /// <summary>Indicates whether the control is hovered by mouse pointer</summary>
        public virtual bool IsHovered => GetCanvas().HoveredControl == this;

        /// <summary>Indicates whether the control has focus</summary>
        public bool HasFocus => GetCanvas().KeyboardFocus == this;

        /// <summary>Indicates whether this control is a menu component</summary>
        internal virtual bool IsMenuComponent => m_parent != null && m_parent.IsMenuComponent;

        /// <summary>Determines whether the control should be clipped to its bounds while rendering</summary>
        protected virtual bool ShouldClip => true;

        public int Bottom => m_bounds.Bottom + m_margin.m_bottom;

        public int Right => m_bounds.Right + m_margin.m_right;

        /// <summary>The logical parent</summary>
        public GameControl Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                if (m_parent == value)
                {
                    return;
                }

                m_parent?.RemoveChild(this, false);

                m_parent = value;
                m_actualParent = null;

                m_parent?.AddChild(this);

                ParentChanged?.Invoke(this);
            }
        }

        /// <summary>Indicates whether the control and its parents are visible</summary>
        public bool IsVisible
        {
            get
            {
                if (IsHidden)
                {
                    return false;
                }

                return Parent == null || Parent.IsVisible;
            }
        }

        // TODO: Bottom/Right includes margin but X/Y not?
        /// <summary>Leftmost coordinate of the control</summary>
        public int X
        {
            get
            {
                return m_bounds.X;
            }
            set
            {
                SetPosition(value, Y);
            }
        }

        /// <summary>Topmost coordinate of the control</summary>
        public int Y
        {
            get
            {
                return m_bounds.Y;
            }
            set
            {
                SetPosition(X, value);
            }
        }

        public int Width
        {
            get
            {
                return m_bounds.Width;
            }
            set
            {
                SetSize(value, Height);
            }
        }

        public int Height
        {
            get
            {
                return m_bounds.Height;
            }
            set
            {
                SetSize(Width, value);
            }
        }

        /// <summary>Determines whether margin, padding and bounds outlines for the control will be drawn. Applied recursively to all children</summary>
        public bool DrawDebugOutlines
        {
            get
            {
                return m_drawDebugOutlines;
            }
            set
            {
                if (m_drawDebugOutlines == value)
                {
                    return;
                }

                m_drawDebugOutlines = value;

                foreach (var a_child in Children)
                {
                    a_child.DrawDebugOutlines = value;
                }
            }
        }

        /// <summary>Dock position</summary>
        public Pos Dock
        {
            get
            {
                return m_dock;
            }
            set
            {
                if (m_dock == value)
                {
                    return;
                }

                m_dock = value;

                Invalidate();
                InvalidateParent();
            }
        }

        /// <summary>Current skin</summary>
        protected Skin Skin
        {
            get
            {
                if (m_skin != null)
                {
                    return m_skin;
                }

                if (m_parent != null)
                {
                    return m_parent.Skin;
                }

                throw new NullReferenceException("GameControl");
            }
        }

        /// <summary>Current ToolTip</summary>
        public GameControl ToolTip
        {
            get
            {
                return m_toolTip;
            }

            set
            {
                m_toolTip = value;

                if (m_toolTip == null)
                {
                    return;
                }

                m_toolTip.Parent = this;
                m_toolTip.IsHidden = true;
            }
        }

        /// <summary>Current padding - inner spacing</summary>
        public Padding Padding
        {
            get
            {
                return m_padding;
            }

            set
            {
                if (m_padding == value)
                {
                    return;
                }

                m_padding = value;

                Invalidate();
                InvalidateParent();
            }
        }

        /// <summary>Current margin - outer spacing</summary>
        public Margin Margin
        {
            get
            {
                return m_margin;
            }
            set
            {
                if (m_margin == value)
                {
                    return;
                }

                m_margin = value;

                Invalidate();
                InvalidateParent();
            }
        }

        /// <summary>Indicates whether the control is hidden</summary>
        public virtual bool IsHidden
        {
            get
            {
                return m_hidden;
            }
            set
            {
                if (value == m_hidden)
                {
                    return;
                }

                m_hidden = value;

                Invalidate();
            }
        }

        /// <summary>User data associated with the control</summary>
        public object UserData
        {
            get;
            set;
        }

        /// <summary>Indicates whether the control is disabled</summary>
        public virtual bool IsDisabled
        {
            get;
            set;
        }

        /// <summary>Determines whether the control's position should be restricted to parent's bounds</summary>
        public bool RestrictToParent
        {
            get;
            set;
        }

        /// <summary>Determines whether the control receives mouse input events</summary>
        public bool MouseInputEnabled
        {
            get;
            set;
        }

        /// <summary>Determines whether the control receives keyboard input events</summary>
        public bool KeyboardInputEnabled
        {
            get;
            set;
        }

        /// <summary>Gets or sets the mouse cursor when the cursor is hovering the control</summary>
        public Cursor Cursor
        {
            get;
            set;
        }

        /// <summary>Indicates whether the control is tabable (can be focused by pressing Tab)</summary>
        public bool IsTabable
        {
            get;
            set;
        }

        /// <summary>Indicates whether control's background should be drawn during rendering</summary>
        public bool ShouldDrawBackground
        {
            get;
            set;
        }

        /// <summary>Indicates whether the renderer should cache drawing to a texture to improve performance (at the cost of memory)</summary>
        public bool ShouldCacheToTexture
        {
            get;
            set;
        }

        /// <summary>Gets or sets the control's internal name</summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>Bounds adjusted by padding</summary>
        public Rectangle InnerBounds
        {
            get;
            private set;
        }

        /// <summary>Size restriction</summary>
        public Size MinimumSize
        {
            get;
            set;
        } = new Size(1, 1);

        /// <summary>Size restriction</summary>
        public Size MaximumSize
        {
            get;
            set;
        } = new Size(kMaxUIControlSize, kMaxUIControlSize);

        public Color PaddingOutlineColor
        {
            get;
            set;
        }

        public Color MarginOutlineColor
        {
            get;
            set;
        }

        public Color BoundsOutlineColor
        {
            get;
            set;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public virtual void Dispose()
        {
            if (m_isDisposed)
            {
#if DEBUG
                throw new ObjectDisposedException(string.Format("GameControl [{1:X}] disposed twice: {0}", this, GetHashCode()));
#else
                return;
#endif
            }

            if (GetCanvas().HoveredControl == this)
            {
                GetCanvas().HoveredControl = null;
            }

            if (GetCanvas().KeyboardFocus == this)
            {
                GetCanvas().KeyboardFocus = null;
            }

            if (GetCanvas().MouseFocus == this)
            {
                GetCanvas().MouseFocus = null;
            }

            DragAndDrop.ControlDeleted(this);
            UI.ToolTip.ControlDeleted(this);
            Animation.Cancel(this);

            foreach (var a_childControl in m_children)
            {
                a_childControl.Dispose();
            }

            m_children.Clear();

            m_isDisposed = true;

            GC.SuppressFinalize(this);
        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void RenderFocus(Skin c_skin)
        {
            if (GetCanvas().KeyboardFocus != this)
            {
                return;
            }

            if (!IsTabable)
            {
                return;
            }

            c_skin.DrawKeyboardHighlight(this, RenderBounds, 3);
        }

        public override string ToString()
        {
            var a_menuItem = this as MenuItem;
            if (a_menuItem != null)
            {
                return "[MenuItem: " + a_menuItem.Text + "]";
            }

            var a_label = this as Label;
            if (a_label != null)
            {
                return "[Label: " + a_label.Text + "]";
            }

            return GetType().ToString();
        }

        // Keyboard Based Events
        public event KeyboardEventHandler KeyDown;

        public event KeyboardEventHandler KeyPress;

        public event KeyboardEventHandler KeyUp;

        // Mouse Based Events
        /// <summary></summary>
        public event MouseWheelEventHandler MouseWheel;

        /// <summary></summary>
        public event MouseButtonEventHandler MouseDown;

        /// <summary>Invoked when both a down and up press of the mouse happened on the control</summary>
        public event MouseButtonEventHandler Clicked;

        /// <summary>Invoked when both a down and up press of the mouse happened on the control</summary>
        public event MouseButtonEventHandler DoubleClicked;

        /// <summary></summary>
        public event MouseButtonEventHandler MouseUp;

        /// <summary>Invoked when mouse pointer enters the control</summary>
        public event MouseMoveEventHandler MouseOver;

        /// <summary></summary>
        public event MouseMoveEventHandler MouseMove;

        /// <summary>Invoked when mouse pointer leaves the control</summary>
        public event MouseMoveEventHandler MouseOut;

        // Control Based Events
        public event GameControlEventHandler Focus;
        public event GameControlEventHandler Blur;

        /// <summary>Invoked when control's bounds have been changed</summary>
        public event GameControlEventHandler BoundsChanged;

        /// <summary>Invoked when control's bounds have been changed</summary>
        public event GameControlEventHandler ParentChanged;

        /// <summary>Adds keyboard accelerator</summary>
        /// <param name="c_accelerator">Accelerator text</param>
        /// <param name="c_handler">Handler</param>
        public void AddAccelerator(string c_accelerator, GameControlEventHandler c_handler)
        {
            c_accelerator = c_accelerator.Trim().ToUpperInvariant();
            m_accelerators[c_accelerator] = c_handler;
        }

        /// <summary>Disables the control</summary>
        public void Disable()
        {
            IsDisabled = true;
        }

        /// <summary>Enables the control</summary>
        public void Enable()
        {
            IsDisabled = false;
        }

        /// <summary>Checks if the given control is a child of this instance</summary>
        /// <param name="c_childControl">Control to examine</param>
        /// <returns>True if the control is out child</returns>
        public bool IsChild(GameControl c_childControl)
        {
            return m_children.Contains(c_childControl);
        }

        /// <summary>Invalidates control's parent</summary>
        public void InvalidateParent()
        {
            m_parent?.Invalidate();
        }

        /// <summary>Called during rendering</summary>
        public virtual void Think()
        {
        }

        /// <summary>Hides the control</summary>
        public virtual void Hide()
        {
            IsHidden = true;
        }

        /// <summary>Shows the control</summary>
        public virtual void Show()
        {
            IsHidden = false;
        }

        /// <summary>Creates a tooltip for the control</summary>
        /// <param name="c_text">Tooltip text</param>
        public virtual void SetToolTipText(string c_text)
        {
            var a_tooltip = new Label(this) { AutoSizeToContents = true, Text = c_text, TextColorOverride = Skin.m_colors.m_tooltipText, Padding = new Padding(5, 3, 5, 3) };
            a_tooltip.SizeToContents();

            ToolTip = a_tooltip;
        }

        /// <summary>Invalidates the control</summary>
        /// <remarks>Causes layout, repaint, invalidates cached texture</remarks>
        public virtual void Invalidate()
        {
            m_needsLayout = true;
            m_cacheTextureDirty = true;
        }

        /// <summary>Sends the control to the bottom of paren't visibility stack</summary>
        public virtual void SendToBack()
        {
            if (m_actualParent == null || m_actualParent.m_children.Count == 0 || m_actualParent.m_children.First() == this)
            {
                return;
            }

            m_actualParent.m_children.Remove(this);
            m_actualParent.m_children.Insert(0, this);

            InvalidateParent();
        }

        /// <summary>Brings the control to the top of parent visibility stack</summary>
        public virtual void BringToFront()
        {
            if (m_actualParent == null || m_actualParent.m_children.Last() == this)
            {
                return;
            }

            m_actualParent.m_children.Remove(this);
            m_actualParent.m_children.Add(this);

            InvalidateParent();

            Redraw();
        }

        public virtual void BringNextToControl(GameControl c_childControl, bool c_isBehind)
        {
            if (null == m_actualParent)
            {
                return;
            }

            m_actualParent.m_children.Remove(this);

            // TODO: validate
            var a_index = m_actualParent.m_children.IndexOf(c_childControl);

            if (a_index == m_actualParent.m_children.Count - 1)
            {
                BringToFront();

                return;
            }

            if (c_isBehind)
            {
                ++a_index;

                if (a_index == m_actualParent.m_children.Count - 1)
                {
                    BringToFront();

                    return;
                }
            }

            m_actualParent.m_children.Insert(a_index, this);

            InvalidateParent();
        }

        /// <summary>Finds a child by name</summary>
        /// <param name="c_name">Child name</param>
        /// <param name="c_recursive">Determines whether the search should be recursive</param>
        /// <returns>Found control or null</returns>
        public virtual GameControl FindChildByName(string c_name, bool c_recursive = false)
        {
            var a_foundControl = m_children.Find(c_x => c_x.Name == c_name);

            if (a_foundControl != null)
            {
                return a_foundControl;
            }

            if (c_recursive)
            {
                foreach (var a_childControl in m_children)
                {
                    a_foundControl = a_childControl.FindChildByName(c_name, true);

                    if (a_foundControl != null)
                    {
                        return a_foundControl;
                    }
                }
            }

            return null;
        }

        /// <summary>Attaches specified control as a child of this one</summary>
        /// <remarks>If InnerPanel is not null, it will become the parent</remarks>
        /// <param name="c_childControl">Control to be added as a child</param>
        public virtual void AddChild(GameControl c_childControl)
        {
            if (m_innerControl != null)
            {
                m_innerControl.AddChild(c_childControl);

                return;
            }

            m_children.Add(c_childControl);
            OnChildAdded(c_childControl);

            c_childControl.m_actualParent = this;
        }

        /// <summary>Detaches specified control from this one</summary>
        /// <param name="c_childControl">Child to be removed</param>
        /// <param name="c_dispose">Determines whether the child should be disposed (added to delayed delete queue)</param>
        public virtual void RemoveChild(GameControl c_childControl, bool c_dispose)
        {
            // If we removed our innerpanel
            // remove our pointer to it
            if (m_innerControl == c_childControl)
            {
                m_children.Remove(m_innerControl);
                m_innerControl.DelayedDelete();
                m_innerControl = null;

                return;
            }

            if (m_innerControl != null && m_innerControl.Children.Contains(c_childControl))
            {
                m_innerControl.RemoveChild(c_childControl, c_dispose);

                return;
            }

            m_children.Remove(c_childControl);
            OnChildRemoved(c_childControl);

            if (c_dispose)
            {
                c_childControl.DelayedDelete();
            }
        }

        /// <summary>Removes all children (and disposes them)</summary>
        public virtual void DeleteAllChildren()
        {
            // TODO: probably shouldn't invalidate after each removal
            while (m_children.Count > 0)
            {
                RemoveChild(m_children[0], true);
            }
        }

        /// <summary>Moves the control by a specific amount</summary>
        /// <param name="c_x">X-axis movement</param>
        /// <param name="c_y">Y-axis movement</param>
        public virtual void MoveBy(int c_x, int c_y)
        {
            SetBounds(X + c_x, Y + c_y, Width, Height);
        }

        /// <summary>Moves the control to a specific point</summary>
        /// <param name="c_x">Target x coordinate</param>
        /// <param name="c_y">Target y coordinate</param>
        public virtual void MoveTo(float c_x, float c_y)
        {
            MoveTo((int)c_x, (int)c_y);
        }

        /// <summary>Moves the control to a specific point, clamping on paren't bounds if RestrictToParent is set</summary>
        /// <param name="c_x">Target x coordinate</param>
        /// <param name="c_y">Target y coordinate</param>
        public virtual void MoveTo(int c_x, int c_y)
        {
            if (RestrictToParent && (Parent != null))
            {
                var a_parentControl = Parent;

                if (c_x - Padding.Left < a_parentControl.Margin.m_left)
                {
                    c_x = a_parentControl.Margin.m_left + Padding.Left;
                }

                if (c_y - Padding.Top < a_parentControl.Margin.m_top)
                {
                    c_y = a_parentControl.Margin.m_top + Padding.Top;
                }

                if (c_x + Width + Padding.Right > a_parentControl.Width - a_parentControl.Margin.m_right)
                {
                    c_x = a_parentControl.Width - a_parentControl.Margin.m_right - Width - Padding.Right;
                }

                if (c_y + Height + Padding.Bottom > a_parentControl.Height - a_parentControl.Margin.m_bottom)
                {
                    c_y = a_parentControl.Height - a_parentControl.Margin.m_bottom - Height - Padding.Bottom;
                }
            }

            SetBounds(c_x, c_y, Width, Height);
        }

        /// <summary>Sets the control position</summary>
        /// <param name="c_x">Target x coordinate</param>
        /// <param name="c_y">Target y coordinate</param>
        public virtual void SetPosition(float c_x, float c_y)
        {
            SetPosition((int)c_x, (int)c_y);
        }

        /// <summary>Sets the control position</summary>
        /// <param name="c_x">Target x coordinate</param>
        /// <param name="c_y">Target y coordinate</param>
        public virtual void SetPosition(int c_x, int c_y)
        {
            SetBounds(c_x, c_y, Width, Height);
        }

        /// <summary>Sets the control size</summary>
        /// <param name="c_size">The size object to set the control to</param>
        /// <returns>True if bounds changed</returns>
        public virtual bool SetSize(Size c_size)
        {
            return SetBounds(X, Y, c_size.Width, c_size.Height);
        }

        /// <summary>Sets the control size</summary>
        /// <param name="c_width">New width</param>
        /// <param name="c_height">New height</param>
        /// <returns>True if bounds changed</returns>
        public virtual bool SetSize(int c_width, int c_height)
        {
            return SetBounds(X, Y, c_width, c_height);
        }

        /// <summary>Sets the control bounds</summary>
        /// <param name="c_bounds">New bounds</param>
        /// <returns>True if bounds changed</returns>
        public virtual bool SetBounds(Rectangle c_bounds)
        {
            return SetBounds(c_bounds.X, c_bounds.Y, c_bounds.Width, c_bounds.Height);
        }

        /// <summary>Sets the control bounds</summary>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        /// <param name="c_width">Width</param>
        /// <param name="c_height">Height</param>
        /// <returns>True if bounds changed</returns>
        public virtual bool SetBounds(float c_x, float c_y, float c_width, float c_height)
        {
            return SetBounds((int)c_x, (int)c_y, (int)c_width, (int)c_height);
        }

        /// <summary>Sets the control bounds</summary>
        /// <param name="c_x">X position</param>
        /// <param name="c_y">Y position</param>
        /// <param name="c_width">Width</param>
        /// <param name="c_height">Height</param>
        /// <returns>True if bounds changed</returns>
        public virtual bool SetBounds(int c_x, int c_y, int c_width, int c_height)
        {
            var a_newBounds = new Rectangle(c_x, c_y, c_width, c_height);
            var a_oldBounds = Bounds;

            if (a_newBounds == a_oldBounds)
            {
                return false;
            }

            m_bounds.X = c_x;
            m_bounds.Y = c_y;

            if (a_newBounds.Size.Width > MaximumSize.Width)
            {
                m_bounds.Width = MaximumSize.Width;
            }
            else if (a_newBounds.Size.Width < MinimumSize.Width)
            {
                m_bounds.Width = MinimumSize.Width;
            }
            else
            {
                m_bounds.Width = c_width;
            }

            if (a_newBounds.Size.Height > MaximumSize.Height)
            {
                m_bounds.Height = MaximumSize.Height;
            }
            else if (a_newBounds.Size.Height < MinimumSize.Height)
            {
                m_bounds.Height = MinimumSize.Height;
            }
            else
            {
                m_bounds.Height = c_height;
            }

            OnBoundsChanged(a_oldBounds);

            return true;
        }

        /// <summary>Positions the control inside its parent</summary>
        /// <param name="c_position">Target position</param>
        /// <param name="c_xPadding">X padding</param>
        /// <param name="c_yPadding">Y padding</param>
        public virtual void SetRelativePosition(Pos c_position, int c_xPadding = 0, int c_yPadding = 0)
        {
            var a_x = X;
            var a_y = Y;

            var a_width = Parent.Width;
            var a_height = Parent.Height;

            var a_padding = Parent.Padding;

            if (0 != (c_position & Pos.Left))
            {
                a_x = a_padding.Left + c_xPadding;
            }

            if (0 != (c_position & Pos.Right))
            {
                a_x = a_width - Width - a_padding.Right - c_xPadding;
            }

            if (0 != (c_position & Pos.CenterH))
            {
                a_x = (int)(a_padding.Left + c_xPadding + (a_width - Width - a_padding.Left - a_padding.Right) * 0.5f);
            }

            if (0 != (c_position & Pos.Top))
            {
                a_y = a_padding.Top + c_yPadding;
            }

            if (0 != (c_position & Pos.Bottom))
            {
                a_y = a_height - Height - a_padding.Bottom - c_yPadding;
            }

            if (0 != (c_position & Pos.CenterV))
            {
                a_y = (int)(a_padding.Top + c_yPadding + (a_height - Height - a_padding.Bottom - a_padding.Top) * 0.5f);
            }

            SetPosition(a_x, a_y);
        }

        /// <summary>Sets the control's skin</summary>
        /// <param name="c_skin">New skin</param>
        /// <param name="c_changeChildren">Deterines whether to change children skin</param>
        public virtual void SetSkin(Skin c_skin, bool c_changeChildren = false)
        {
            if (m_skin == c_skin)
            {
                return;
            }

            m_skin = c_skin;

            Invalidate();
            Redraw();

            if (c_changeChildren)
            {
                foreach (var a_childControl in m_children)
                {
                    a_childControl.SetSkin(c_skin, true);
                }
            }
        }

        /// <summary>Focuses the control</summary>
        public virtual void OnFocus()
        {
            if (GetCanvas().KeyboardFocus == this)
            {
                return;
            }

            if (GetCanvas().KeyboardFocus != null)
            {
                GetCanvas().KeyboardFocus.OnBlur();
            }

            GetCanvas().KeyboardFocus = this;

            Focus?.Invoke(this);
            Redraw();
        }

        /// <summary>Unfocuses the control</summary>
        public virtual void OnBlur()
        {
            if (GetCanvas().KeyboardFocus != this)
            {
                return;
            }

            GetCanvas().KeyboardFocus = null;

            Blur?.Invoke(this);
            Redraw();
        }

        /// <summary>Control has been clicked - invoked by input system. Windows use it to propagate activation</summary>
        public virtual void Touch()
        {
            Parent?.OnChildTouched(this);
        }

        /// <summary>Gets a child by its coordinates</summary>
        /// <param name="c_x">Child X</param>
        /// <param name="c_y">Child Y</param>
        /// <returns>Control or null if not found</returns>
        public virtual GameControl GetControlAt(int c_x, int c_y)
        {
            if (IsHidden)
            {
                return null;
            }

            if (c_x < 0 || c_y < 0 || c_x >= Width || c_y >= Height)
            {
                return null;
            }

            var a_reverseList = ((IList<GameControl>)m_children).Reverse(); // IList.Reverse creates new list, List.Reverse works in place.. go figure

            foreach (var a_foundControl in a_reverseList.Select(c_childControl => c_childControl.GetControlAt(c_x - c_childControl.X, c_y - c_childControl.Y)).Where(c_foundControl => c_foundControl != null))
            {
                return a_foundControl;
            }

            return !MouseInputEnabled ? null : this;
        }

        /// <summary>Converts local coordinates to canvas coordinates</summary>
        /// <param name="c_point">Local coordinates</param>
        /// <returns>Canvas coordinates</returns>
        public virtual Point LocalPosToCanvas(Point c_point)
        {
            if (m_parent == null)
            {
                return c_point;
            }

            var a_x = c_point.X + X;
            var a_y = c_point.Y + Y;

            // If our parent has an innerpanel and we're a child of it
            // add its offset onto us.
            if (m_parent.m_innerControl != null && m_parent.m_innerControl.IsChild(this))
            {
                a_x += m_parent.m_innerControl.X;
                a_y += m_parent.m_innerControl.Y;
            }

            return m_parent.LocalPosToCanvas(new Point(a_x, a_y));
        }

        /// <summary>Converts canvas coordinates to local coordinates</summary>
        /// <param name="c_point">Canvas coordinates</param>
        /// <returns>Local coordinates</returns>
        public virtual Point CanvasPosToLocal(Point c_point)
        {
            if (m_parent == null)
            {
                return c_point;
            }

            var a_x = c_point.X - X;
            var a_y = c_point.Y - Y;

            // If our parent has an innerpanel and we're a child of it
            // add its offset onto us.
            if (m_parent.m_innerControl != null && m_parent.m_innerControl.IsChild(this))
            {
                a_x -= m_parent.m_innerControl.X;
                a_y -= m_parent.m_innerControl.Y;
            }

            return m_parent.CanvasPosToLocal(new Point(a_x, a_y));
        }

        /// <summary>Closes all menus recursively</summary>
        public virtual void CloseMenus()
        {
            // TODO: Bad Perf, store open menus somewhere
            var a_childrenCopy = m_children.FindAll(c_x => true);

            foreach (var a_childControl in a_childrenCopy)
            {
                a_childControl.CloseMenus();
            }
        }

        /// <summary>Sets mouse cursor to current cursor</summary>
        public virtual void UpdateCursor()
        {
            Neutral.SetCursor(Cursor);
        }

        // giver
        public virtual Package DragAndDrop_GetPackage(int c_x, int c_y)
        {
            return m_package;
        }

        // giver
        public virtual bool DragAndDrop_Draggable()
        {
            return m_package != null && m_package.m_isDraggable;
        }

        // giver
        public virtual void DragAndDrop_SetPackage(bool c_isDraggable, string c_name = "", object c_userData = null)
        {
            if (m_package != null)
            {
                return;
            }

            m_package = new Package { m_isDraggable = c_isDraggable, m_name = c_name, m_userData = c_userData };
        }

        // giver
        public virtual bool DragAndDrop_ShouldStartDrag()
        {
            return true;
        }

        // giver
        public virtual void DragAndDrop_StartDragging(Package c_package, int c_x, int c_y)
        {
            c_package.m_holdOffset = CanvasPosToLocal(new Point(c_x, c_y));
            c_package.m_drawControl = this;
        }

        // giver
        public virtual void DragAndDrop_EndDragging(bool c_success, int c_x, int c_y)
        {
        }

        // receiver
        public virtual bool DragAndDrop_HandleDrop(Package c_p, int c_x, int c_y)
        {
            DragAndDrop.m_sourceControl.Parent = this;

            return true;
        }

        // receiver
        public virtual void DragAndDrop_HoverEnter(Package c_p, int c_x, int c_y)
        {
        }

        // receiver
        public virtual void DragAndDrop_HoverLeave(Package c_p)
        {
        }

        // receiver
        public virtual void DragAndDrop_Hover(Package c_p, int c_x, int c_y)
        {
        }

        // receiver
        public virtual bool DragAndDrop_CanAcceptPackage(Package c_p)
        {
            return false;
        }

        /// <summary>Resizes the control to fit its children</summary>
        /// <param name="c_width">Determines whether to change control's width</param>
        /// <param name="c_height">Determines whether to change control's height</param>
        /// <returns>True if bounds changed</returns>
        public virtual bool SizeToChildren(bool c_width = true, bool c_height = true)
        {
            var a_size = GetChildrenSize();

            a_size.X += Padding.Right;
            a_size.Y += Padding.Bottom;

            return SetSize(c_width ? a_size.X : Width, c_height ? a_size.Y : Height);
        }

        /// <summary>Returns the total width and height of all children</summary>
        /// <remarks>Default implementation returns maximum size of children since the layout is unknown; implement this in derived compound controls to properly return their size</remarks>
        /// <returns></returns>
        public virtual Point GetChildrenSize()
        {
            var a_size = Point.Empty;

            foreach (var a_childControl in m_children.Where(c_childControl => !c_childControl.IsHidden))
            {
                a_size.X = Math.Max(a_size.X, a_childControl.Right);
                a_size.Y = Math.Max(a_size.Y, a_childControl.Bottom);
            }

            return a_size;
        }

        /// <summary>Re-renders the control, invalidates cached texture</summary>
        public virtual void Redraw()
        {
            UpdateColors();

            m_cacheTextureDirty = true;

            m_parent?.Redraw();
        }

        /// <summary>Updates control colors</summary>
        /// <remarks>Used in composite controls like lists to differentiate row colors etc</remarks>
        public virtual void UpdateColors()
        {
        }

        /// <summary>Gets the canvas (root parent) of the control</summary>
        /// <returns></returns>
        internal virtual Canvas GetCanvas()
        {
            return m_parent?.GetCanvas();
        }

        /// <summary>Updating logic implementation</summary>
        internal virtual void DoUpdate()
        {
            if (m_children.Count == 0 || IsHidden)
            {
                return;
            }

            // Now render my kids
            m_children.ToList().ForEach(c_child => c_child.DoUpdate());
        }

        /// <summary>Rendering logic implementation</summary>
        /// <param name="c_skin">Skin to use</param>
        internal virtual void DoRender(Skin c_skin)
        {
            if (m_skin != null)
            {
                c_skin = m_skin;
            }

            Think();

            RenderRecursive(c_skin, Bounds);

            if (DrawDebugOutlines)
            {
                c_skin.DrawDebugOutlines(this);
            }
        }

        /// <summary>Handler for Paste event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected virtual void OnPaste(GameControl c_fromControl)
        {
        }

        /// <summary>Handler for Copy event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected virtual void OnCopy(GameControl c_fromControl)
        {
        }

        /// <summary>Handler for Cut event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected virtual void OnCut(GameControl c_fromControl)
        {
        }

        /// <summary>Handler for Select All event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected virtual void OnSelectAll(GameControl c_fromControl)
        {
        }

        /// <summary>Renders under the actual control (shadows etc)</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void RenderUnder(Skin c_skin)
        {
        }

        /// <summary>Renders over the actual control (overlays)</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void RenderOver(Skin c_skin)
        {
        }

        /// <summary>Invalidates the control's children (relayout/repaint)</summary>
        /// <param name="c_recursive">Determines whether the operation should be carried recursively</param>
        protected virtual void InvalidateChildren(bool c_recursive = false)
        {
            foreach (var a_childControl in m_children)
            {
                a_childControl.Invalidate();

                if (c_recursive)
                {
                    a_childControl.InvalidateChildren(true);
                }
            }

            if (m_innerControl != null)
            {
                foreach (var a_child in m_innerControl.m_children)
                {
                    a_child.Invalidate();

                    if (c_recursive)
                    {
                        a_child.InvalidateChildren(true);
                    }
                }
            }
        }

        /// <summary>Handler invoked when a child is added</summary>
        /// <param name="c_childControl">Child added</param>
        protected virtual void OnChildAdded(GameControl c_childControl)
        {
            Invalidate();
        }

        /// <summary>Handler invoked when a child is removed</summary>
        /// <param name="c_childControl">Child removed</param>
        protected virtual void OnChildRemoved(GameControl c_childControl)
        {
            Invalidate();
        }

        /// <summary>Handler invoked when control's bounds change</summary>
        /// <param name="c_oldBounds">Old bounds</param>
        protected virtual void OnBoundsChanged(Rectangle c_oldBounds)
        {
            // Parent?.OnChildBoundsChanged(c_oldBounds, this);

            if (m_bounds.Width != c_oldBounds.Width || m_bounds.Height != c_oldBounds.Height)
            {
                Invalidate();
            }

            Redraw();

            UpdateRenderBounds();
        }

        /// <summary>Handler invoked when control's scale changes</summary>
        protected virtual void OnScaleChanged()
        {
            foreach (var a_childControl in m_children)
            {
                a_childControl.OnScaleChanged();
            }
        }

        /// <summary>Allows control to run logic</summary>
        protected virtual void Update()
        {
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void Render(Skin c_skin)
        {
        }

        /// <summary>Recursive rendering logic</summary>
        /// <param name="c_skin">Skin to use</param>
        /// <param name="c_clipRect">Clipping rectangle</param>
        protected virtual void RenderRecursive(Skin c_skin, Rectangle c_clipRect)
        {
            var a_renderer = c_skin.Renderer;
            var a_oldRenderOffset = a_renderer.RenderOffset;

            a_renderer.AddRenderOffset(c_clipRect);

            RenderUnder(c_skin);

            var a_oldRegion = a_renderer.ClipRegion;

            if (ShouldClip)
            {
                a_renderer.AddClipRegion(c_clipRect);

                if (!a_renderer.ClipRegionVisible)
                {
                    a_renderer.RenderOffset = a_oldRenderOffset;
                    a_renderer.ClipRegion = a_oldRegion;

                    return;
                }

                a_renderer.StartClip();
            }

            //Render myself first
            Render(c_skin);

            if (m_children.Count > 0)
            {
                //Now render my kids
                foreach (var a_childControl in m_children.Where(c_childControl => !c_childControl.IsHidden))
                {
                    a_childControl.DoRender(c_skin);
                }
            }

            a_renderer.ClipRegion = a_oldRegion;
            a_renderer.StartClip();

            RenderOver(c_skin);
            RenderFocus(c_skin);

            a_renderer.RenderOffset = a_oldRenderOffset;
        }

        public virtual void OnKeyDown(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            KeyDown?.Invoke(this, c_keyboardKeyEventArgs);

            OnKeyPress(c_keyboardKeyEventArgs);
        }

        public virtual void OnKeyPress(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            KeyPress?.Invoke(this, c_keyboardKeyEventArgs);
        }

        public virtual void OnKeyUp(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            KeyUp?.Invoke(this, c_keyboardKeyEventArgs);
        }

        public virtual void OnMouseWheel(MouseWheelEventArgs c_mouseWheelEventArgs)
        {
            if (m_actualParent != null)
            {
                m_actualParent.OnMouseWheel(c_mouseWheelEventArgs);
            }
            else
            {
                MouseWheel?.Invoke(this, c_mouseWheelEventArgs);
            }
        }

        public virtual void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            MouseMove?.Invoke(this, c_mouseEventArgs);
        }

        public virtual void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            GetCanvas().MouseFocus = this;
            MouseDown?.Invoke(this, c_mouseButtonEventArgs);
        }

        public virtual void OnClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            Clicked?.Invoke(this, c_mouseButtonEventArgs);
        }

        public virtual void OnDoubleClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            DoubleClicked?.Invoke(this, c_mouseButtonEventArgs);
        }

        public virtual void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            var a_buttonIdx = (int)c_mouseButtonEventArgs.Button;

            var a_timeNow = DateTime.UtcNow.Ticks;
            var a_timeDifference = new TimeSpan(a_timeNow - m_lastClickedTime[a_buttonIdx]);

            if (a_timeDifference.TotalSeconds > kMouseDoubleClickSpeed)
            {
                OnClicked(c_mouseButtonEventArgs);
            }
            else
            {
                OnDoubleClicked(c_mouseButtonEventArgs);
            }

            Console.WriteLine(@"{0} - {1} = {2}", a_timeNow, m_lastClickedTime[a_buttonIdx], a_timeDifference.TotalSeconds);

            m_lastClickedTime[a_buttonIdx] = DateTime.UtcNow.Ticks;

            MouseUp?.Invoke(this, c_mouseButtonEventArgs);
        }

        /// <summary>Handler invoked on mouse cursor entering control's bounds</summary>
        public virtual void OnMouseOver(MouseMoveEventArgs c_mouseEventArgs)
        {
            MouseOver?.Invoke(this, c_mouseEventArgs);

            if (ToolTip != null)
            {
                UI.ToolTip.Enable(this);
            }
            else if (Parent?.ToolTip != null)
            {
                UI.ToolTip.Enable(Parent);
            }

            Redraw();
        }

        public virtual void OnMouseOut(MouseMoveEventArgs c_mouseEventArgs)
        {
            MouseOut?.Invoke(this, c_mouseEventArgs);
        }

        protected virtual void OnChildTouched(GameControl c_control)
        {
            Touch();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void OnLayout(Skin c_skin)
        {
        }

        /// <summary>Recursively lays out the control's interior according to alignment, margin, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void RecurseLayout(Skin c_skin)
        {
            if (m_skin != null)
            {
                c_skin = m_skin;
            }

            if (IsHidden)
            {
                return;
            }

            if (m_needsLayout)
            {
                OnLayout(c_skin);
                m_needsLayout = false;
            }

            var a_bounds = RenderBounds;

            // Adjust bounds for padding
            a_bounds.X += m_padding.Left;
            a_bounds.Width -= m_padding.Left + m_padding.Right;
            a_bounds.Y += m_padding.Top;
            a_bounds.Height -= m_padding.Top + m_padding.Bottom;

            foreach (var a_childControl in m_children)
            {
                if (a_childControl.IsHidden)
                {
                    continue;
                }

                var a_dockPosition = a_childControl.Dock;

                if (0 != (a_dockPosition & Pos.Fill))
                {
                    continue;
                }

                if (0 != (a_dockPosition & Pos.Top))
                {
                    var a_margin = a_childControl.Margin;

                    a_childControl.SetBounds(a_bounds.X + a_margin.m_left, a_bounds.Y + a_margin.m_top, a_bounds.Width - a_margin.m_left - a_margin.m_right, a_childControl.Height);

                    var a_height = a_margin.m_top + a_margin.m_bottom + a_childControl.Height;

                    a_bounds.Y += a_height;
                    a_bounds.Height -= a_height;
                }

                if (0 != (a_dockPosition & Pos.Left))
                {
                    var a_margin = a_childControl.Margin;

                    a_childControl.SetBounds(a_bounds.X + a_margin.m_left, a_bounds.Y + a_margin.m_top, a_childControl.Width, a_bounds.Height - a_margin.m_top - a_margin.m_bottom);

                    var a_width = a_margin.m_left + a_margin.m_right + a_childControl.Width;

                    a_bounds.X += a_width;
                    a_bounds.Width -= a_width;
                }

                if (0 != (a_dockPosition & Pos.Right))
                {
                    // TODO: THIS MARGIN CODE MIGHT NOT BE FULLY FUNCTIONAL
                    var a_margin = a_childControl.Margin;

                    a_childControl.SetBounds((a_bounds.X + a_bounds.Width) - a_childControl.Width - a_margin.m_right, a_bounds.Y + a_margin.m_top, a_childControl.Width, a_bounds.Height - a_margin.m_top - a_margin.m_bottom);

                    var a_width = a_margin.m_left + a_margin.m_right + a_childControl.Width;

                    a_bounds.Width -= a_width;
                }

                if (0 != (a_dockPosition & Pos.Bottom))
                {
                    // TODO: THIS MARGIN CODE MIGHT NOT BE FULLY FUNCTIONAL
                    var a_margin = a_childControl.Margin;

                    a_childControl.SetBounds(a_bounds.X + a_margin.m_left, (a_bounds.Y + a_bounds.Height) - a_childControl.Height - a_margin.m_bottom, a_bounds.Width - a_margin.m_left - a_margin.m_right, a_childControl.Height);
                    a_bounds.Height -= a_childControl.Height + a_margin.m_bottom + a_margin.m_top;
                }

                a_childControl.RecurseLayout(c_skin);
            }

            InnerBounds = a_bounds;

            //
            // Fill uses the left over space, so do that now.
            //
            foreach (var a_childControl in m_children)
            {
                var a_dockPosition = a_childControl.Dock;

                if ((a_dockPosition & Pos.Fill) == 0)
                {
                    continue;
                }

                var a_margin = a_childControl.Margin;

                a_childControl.SetBounds(a_bounds.X + a_margin.m_left, a_bounds.Y + a_margin.m_top, a_bounds.Width - a_margin.m_left - a_margin.m_right, a_bounds.Height - a_margin.m_top - a_margin.m_bottom);
                a_childControl.RecurseLayout(c_skin);
            }

            PostLayout(c_skin);

            if (IsTabable)
            {
                if (GetCanvas().m_firstTab == null)
                {
                    GetCanvas().m_firstTab = this;
                }

                if (GetCanvas().m_nextTab == null)
                {
                    GetCanvas().m_nextTab = this;
                }
            }

            if (GetCanvas().KeyboardFocus == this)
            {
                GetCanvas().m_nextTab = null;
            }
        }

        /// <summary>Copies Bounds to RenderBounds</summary>
        protected virtual void UpdateRenderBounds()
        {
            m_renderBounds.X = 0;
            m_renderBounds.Y = 0;

            m_renderBounds.Width = m_bounds.Width;
            m_renderBounds.Height = m_bounds.Height;
        }

        /// <summary>Function invoked after layout</summary>
        /// <param name="c_skin">Skin to use</param>
        protected virtual void PostLayout(Skin c_skin)
        {
        }

        public virtual void Anim_WidthIn(float c_length, float c_delay = 0.0f, float c_ease = 1.0f)
        {
            Animation.Add(this, new Width(0, Width, c_length, false, c_delay, c_ease));

            Width = 0;
        }

        public virtual void Anim_HeightIn(float c_length, float c_delay, float c_ease)
        {
            Animation.Add(this, new Height(0, Height, c_length, false, c_delay, c_ease));

            Height = 0;
        }

        public virtual void Anim_WidthOut(float c_length, bool c_hide, float c_delay, float c_ease)
        {
            Animation.Add(this, new Width(Width, 0, c_length, c_hide, c_delay, c_ease));
        }

        public virtual void Anim_HeightOut(float c_length, bool c_hide, float c_delay, float c_ease)
        {
            Animation.Add(this, new Height(Height, 0, c_length, c_hide, c_delay, c_ease));
        }

#if DEBUG
        ~GameControl()
        {
            throw new InvalidOperationException(string.Format("IDisposable object finalized [{1:X}]: {0}", this, GetHashCode()));
        }
#endif

        /// <summary>Detaches the control from canvas and adds to the deletion queue (processed in Canvas.DoThink)</summary>
        public void DelayedDelete()
        {
            GetCanvas().AddDelayedDelete(this);
        }
    }
}