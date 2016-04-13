using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FoxTrader.UI.Anim;
using FoxTrader.UI.DragDrop;
using FoxTrader.UI.Input;
using FoxTrader.UI.Skin;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>Base class to display UI elements, handles drawing and colors..</summary>
    internal class Canvas : GameControl
    {
        private readonly KeyData m_keyData = new KeyData();
        private readonly List<IDisposable> m_disposeQueue; // dictionary for faster access?

        // These are not created by us, so no disposing
        internal GameControl m_firstTab;
        internal GameControl m_nextTab;

        private readonly GameControlDevices m_gameControlDevices;

        private float m_scale;
        private Point m_lastClickedPosition;


        public GameControl MouseFocus
        {
            get;
            internal set;
        }

        public GameControl HoveredControl
        {
            get;
            internal set;
        }

        public GameControl KeyboardFocus
        {
            get;
            internal set;
        }

        /// <summary>Initializes a new instance of the <see cref="Canvas" /> class</summary>
        public Canvas()
        {
            m_gameControlDevices = FoxTraderWindow.Instance.GetGameControlDevices();

            HookKeyboard();
            HookMouse();

            MouseInputEnabled = true;
            KeyboardInputEnabled = true;

            SetBounds(0, 0, 10000, 10000);
            SetSkin(FoxTraderWindow.Instance.Skin);
            Scale = 1.0f;
            BackgroundColor = Color.White;
            ShouldDrawBackground = false;

            m_disposeQueue = new List<IDisposable>();
        }

        private void HookKeyboard()
        {
            m_gameControlDevices.Keyboard.KeyDown += (c_sender, c_args) =>
            {
                if (IsHidden || !KeyboardInputEnabled)
                {
                    return;
                }

                HoveredControl.OnKeyDown(c_args);
            };

            m_gameControlDevices.Keyboard.KeyUp += (c_sender, c_args) =>
            {
                if (IsHidden || !KeyboardInputEnabled)
                {
                    return;
                }

                if (MouseFocus is Button && c_args.Key == Key.Space)
                {
                    var a_button = HoveredControl as Button;

                    a_button?.OnClicked(new MouseButtonEventArgs());
                }

                HoveredControl.OnKeyUp(c_args);
            };
        }

        private void HookMouse()
        {
            m_gameControlDevices.Mouse.Move += (c_sender, c_args) =>
            {
                if (IsHidden || !MouseInputEnabled)
                {
                    return;
                }

                if (HoveredControl != null)
                {
                    HoveredControl.OnMouseMoved(c_args);

                    DragAndDrop.OnMouseMoved(HoveredControl, c_args);
                }

                var a_hoveredControl = GetControlAt(c_args.X, c_args.Y);

                if (HoveredControl != a_hoveredControl)
                {
                    if (HoveredControl != null)
                    {
                        HoveredControl.OnMouseOut(c_args);
                        HoveredControl = null;
                    }

                    HoveredControl = a_hoveredControl;
                    HoveredControl?.OnMouseOver(c_args);
                }

                if (MouseFocus == null)
                {
                    return;
                }

                if (HoveredControl != null)
                {
                    HoveredControl.Redraw();
                    HoveredControl = null;
                }

                HoveredControl = MouseFocus;
            };

            m_gameControlDevices.Mouse.ButtonDown += (c_sender, c_args) =>
            {
                if (IsHidden || !MouseInputEnabled)
                {
                    return;
                }

                if (HoveredControl == null || !HoveredControl.IsMenuComponent)
                {
                    CloseMenus();
                }

                if (HoveredControl == null || !HoveredControl.IsVisible || HoveredControl == this)
                {
                    return;
                }

                FindKeyboardFocusedControl(HoveredControl);

                if (c_args.Button == MouseButton.Left && DragAndDrop.OnMouseButton(HoveredControl, c_args))
                {
                    return;
                }

                HoveredControl.OnMouseDown(c_args);
            };

            m_gameControlDevices.Mouse.ButtonUp += (c_sender, c_args) =>
            {
                if (IsHidden || !MouseInputEnabled)
                {
                    return;
                }

                HoveredControl?.OnMouseUp(c_args);
            };

            m_gameControlDevices.Mouse.WheelChanged += (c_sender, c_args) =>
            {
                if (IsHidden || !MouseInputEnabled)
                {
                    return;
                }

                HoveredControl.OnMouseWheel(c_args);
            };
        }

        private void FindKeyboardFocusedControl(GameControl c_hoveredControl)
        {
            while (true)
            {
                if (c_hoveredControl == null)
                {
                    return;
                }

                if (c_hoveredControl.KeyboardInputEnabled)
                {
                    if (c_hoveredControl.Children.Any(c_childControl => c_childControl == KeyboardFocus))
                    {
                        return;
                    }

                    c_hoveredControl.OnFocus();

                    return;
                }

                c_hoveredControl = c_hoveredControl.Parent;
            }
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

                if (Skin?.Renderer != null)
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

            if (MouseFocus != null && !MouseFocus.IsVisible)
            {
                MouseFocus = null;
            }

            if (KeyboardFocus != null && (!KeyboardFocus.IsVisible || !KeyboardFocus.KeyboardInputEnabled))
            {
                KeyboardFocus = null;
            }

            if (KeyboardFocus == null)
            {
                return;
            }


            // Key Repeat On OpenTK??
            /*var a_time = Platform.Neutral.GetTimeInSeconds();

            for (var a_idx = 0; a_idx < 1024; a_idx++)
            {
                if (m_keyData.m_keyState[a_idx] && m_keyData.m_target != KeyboardFocus)
                {
                    m_keyData.m_keyState[a_idx] = false;
                    continue;
                }

                if (m_keyData.m_keyState[a_idx] && a_time > m_keyData.m_nextRepeat[a_idx])
                {
                    m_keyData.m_nextRepeat[a_idx] = Platform.Neutral.GetTimeInSeconds() + kKeyRepeatDelay;

                    KeyboardFocus?.OnKeyDown((Key)a_idx);
                }
            }*/
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

        /*

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

      */
    }
}