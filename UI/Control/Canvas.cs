using System;
using System.Collections.Generic;
using System.Drawing;
using FoxTrader.UI.Anim;
using FoxTrader.UI.DragDrop;
using FoxTrader.UI.Skin;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>Base class to display UI elements, handles drawing and colors..</summary>
    internal class Canvas : GameControl
    {
        private readonly List<IDisposable> m_disposeQueue; // dictionary for faster access?

        // These are not created by us, so no disposing
        internal GameControl m_firstTab;
        internal GameControl m_nextTab;
        private float m_scale;

        /// <summary>Initializes a new instance of the <see cref="Canvas" /> class</summary>
        public Canvas()
        {
            SetBounds(0, 0, 10000, 10000);
            SetSkin(FoxTraderWindow.Instance.Skin);
            Scale = 1.0f;
            BackgroundColor = Color.White;
            ShouldDrawBackground = false;

            m_disposeQueue = new List<IDisposable>();
        }

        /// <summary>Scale for rendering</summary>
        public float Scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                if (m_scale == value)
                {
                    return;
                }

                m_scale = value;

                if (Skin != null && Skin.Renderer != null)
                {
                    Skin.Renderer.Scale = m_scale;
                }

                OnScaleChanged();
                Redraw();
            }
        }

        /// <summary>Background color</summary>
        public Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>In most situations you will be rendering the canvas every frame. But in some situations you will only want to render when there have been changes. You can do this by checking NeedsRedraw</summary>
        public bool NeedsRedraw
        {
            get;
            set;
        }

        public override void Dispose()
        {
            ProcessDelayedDeletes();
            base.Dispose();
        }

        /// <summary>Re-renders the control, invalidates cached texture</summary>
        public override void Redraw()
        {
            NeedsRedraw = true;
            base.Redraw();
        }

        /// <summary>Final stop for children requesting their canvas</summary>
        /// <returns>The canvas the child is from</returns>
        internal override Canvas GetCanvas()
        {
            return this;
        }

        /// <summary>Updates the canvas</summary>
        public void UpdateCanvas()
        {
            DoUpdate();
        }

        /// <summary>Renders the canvas</summary>
        public void RenderCanvas()
        {
            DoThink();

            var a_render = Skin.Renderer;

            a_render.Begin();

            RecurseLayout(Skin);

            a_render.ClipRegion = Bounds;
            a_render.RenderOffset = Point.Empty;
            a_render.Scale = Scale;

            if (ShouldDrawBackground)
            {
                a_render.DrawColor = BackgroundColor;
                a_render.DrawFilledRect(RenderBounds);
            }

            DoRender(Skin);

            DragAndDrop.RenderOverlay(this, Skin);

            UI.ToolTip.RenderToolTip(Skin);

            a_render.EndClip();

            a_render.End();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            NeedsRedraw = false;
        }

        /// <summary>Handler invoked when control's bounds change</summary>
        /// <param name="c_oldBounds">Old bounds</param>
        protected override void OnBoundsChanged(Rectangle c_oldBounds)
        {
            base.OnBoundsChanged(c_oldBounds);

            InvalidateChildren(true);
        }

        /// <summary>Processes input and layout. Also purges delayed delete queue</summary>
        private void DoThink()
        {
            if (IsHidden)
            {
                return;
            }

            Animation.GlobalThink();

            // Reset tabbing
            m_nextTab = null;
            m_firstTab = null;

            ProcessDelayedDeletes();

            // Check has focus etc..
            RecurseLayout(Skin);

            // If we didn't have a next tab, cycle to the start.
            if (m_nextTab == null)
            {
                m_nextTab = m_firstTab;
            }

            FoxTraderWindow.Instance.OnCanvasThink();
        }

        /// <summary>Adds given control to the delete queue and detaches it from canvas. Don't call from Dispose, it modifies child list</summary>
        /// <param name="c_control">Control to delete</param>
        public void AddDelayedDelete(GameControl c_control)
        {
            if (!m_disposeQueue.Contains(c_control))
            {
                m_disposeQueue.Add(c_control);
                RemoveChild(c_control, false);
            }

#if DEBUG
            else
            {
                throw new InvalidOperationException("Control deleted twice");
            }
#endif
        }

        private void ProcessDelayedDeletes()
        {
            if (m_disposeQueue.Count != 0)
            {
                foreach (var a_control in m_disposeQueue)
                {
                    a_control.Dispose();
                }

                m_disposeQueue.Clear();
            }
        }

        /// <summary>Handles mouse movement events. Called from Input subsystems</summary>
        /// <returns>True if handled</returns>
        public bool Input_MouseMoved(int c_x, int c_y, int c_dx, int c_dy)
        {
            if (IsHidden)
            {
                return false;
            }

            var a_inverseScale = 1.0f / Scale;

            c_x = Util.Round(c_x * a_inverseScale);
            c_y = Util.Round(c_y * a_inverseScale);
            c_dx = Util.Round(c_dx * a_inverseScale);
            c_dy = Util.Round(c_dy * a_inverseScale);

            FoxTraderWindow.Instance.OnMouseMoved(c_x, c_y, c_dx, c_dy);

            if (FoxTraderWindow.Instance.HoveredControl == null)
            {
                return false;
            }
            if (FoxTraderWindow.Instance.HoveredControl == this)
            {
                return false;
            }
            if (FoxTraderWindow.Instance.HoveredControl.GetCanvas() != this)
            {
                return false;
            }

            FoxTraderWindow.Instance.HoveredControl.InputMouseMoved(c_x, c_y, c_dx, c_dy);
            FoxTraderWindow.Instance.HoveredControl.UpdateCursor();

            DragAndDrop.OnMouseMoved(FoxTraderWindow.Instance.HoveredControl, c_x, c_y);

            return true;
        }

        /// <summary>Handles mouse button events. Called from Input subsystems</summary>
        /// <returns>True if handled</returns>
        public bool Input_MouseButton(int c_mouseButton, bool c_isButtonDown)
        {
            if (IsHidden)
            {
                return false;
            }

            return FoxTraderWindow.Instance.OnMouseClicked(c_mouseButton, c_isButtonDown);
        }

        /// <summary>Handles keyboard events. Called from Input subsystems</summary>
        /// <returns>True if handled</returns>
        public bool Input_Key(Key c_key, bool c_isButtonDown)
        {
            return FoxTraderWindow.Instance.OnKeyEvent(c_key, c_isButtonDown);
        }

        /// <summary>Handles keyboard events. Called from Input subsystems</summary>
        /// <returns>True if handled</returns>
        public bool Input_Character(char c_char)
        {
            if (IsHidden || char.IsControl(c_char))
            {
                return false;
            }

            //Handle Accelerators
            if (FoxTraderWindow.Instance.HandleAccelerator(c_char))
            {
                return true;
            }

            //Handle characters
            if (FoxTraderWindow.Instance.KeyboardFocus == null || FoxTraderWindow.Instance.KeyboardFocus.GetCanvas() != this || !FoxTraderWindow.Instance.KeyboardFocus.IsVisible || FoxTraderWindow.Instance.IsControlDown)
            {
                return false;
            }

            return FoxTraderWindow.Instance.KeyboardFocus.InputChar(c_char);
        }

        /// <summary>Handles the mouse wheel events. Called from Input subsystems</summary>
        /// <returns>True if handled</returns>
        public bool Input_MouseWheel(int c_value)
        {
            if (IsHidden || FoxTraderWindow.Instance.HoveredControl == null || FoxTraderWindow.Instance.HoveredControl == this || FoxTraderWindow.Instance.HoveredControl.GetCanvas() != this)
            {
                return false;
            }

            return FoxTraderWindow.Instance.HoveredControl.InputMouseWheeled(c_value);
        }
    }
}