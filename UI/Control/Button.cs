using System;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Button control</summary>
    internal class Button : Label
    {
        private bool m_centerImage;
        private ImagePanel m_image;
        private bool m_isDepressed;
        private bool m_toggleStatus;

        /// <summary>Control constructor</summary>
        /// <param name="c_parentControl">Parent control</param>
        internal Button(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(100, 20);
            MouseInputEnabled = true;
            Alignment = Pos.Center;
            TextPadding = new Padding(3, 3, 3, 3);
            MouseOut += c_childControl =>
            {
                FoxTraderWindow.Instance.MouseFocus = null;
            };
        }

        /// <summary>Indicates whether the button is depressed</summary>
        public bool IsDepressed
        {
            get
            {
                return m_isDepressed;
            }
            set
            {
                if (m_isDepressed == value)
                {
                    return;
                }
                m_isDepressed = value;
                Redraw();
            }
        }

        /// <summary>Indicates whether the button is toggleable</summary>
        public bool IsToggle
        {
            get;
            set;
        }

        /// <summary>Determines the button's toggle state</summary>
        public bool ToggleState
        {
            get
            {
                return m_toggleStatus;
            }
            set
            {
                if (!IsToggle)
                {
                    return;
                }
                if (m_toggleStatus == value)
                {
                    return;
                }

                m_toggleStatus = value;

                if (Toggled != null)
                {
                    Toggled.Invoke(this);
                }

                if (m_toggleStatus)
                {
                    if (ToggledOn != null)
                    {
                        ToggledOn.Invoke(this);
                    }
                }
                else
                {
                    if (ToggledOff != null)
                    {
                        ToggledOff.Invoke(this);
                    }
                }

                Redraw();
            }
        }

        /// <summary>Invoked when the button is released</summary>
        internal event ButtonEventHandler Clicked;

        /// <summary>Invoked when the button is pressed</summary>
        internal event ButtonEventHandler Pressed;

        /// <summary>Invoked when the button is released</summary>
        internal event ButtonEventHandler Released;

        /// <summary>Invoked when the button's toggle state has changed</summary>
        internal event ButtonEventHandler Toggled;

        /// <summary>Invoked when the button's toggle state has changed to On</summary>
        internal event ButtonEventHandler ToggledOn;

        /// <summary>Invoked when the button's toggle state has changed to Off</summary>
        internal event ButtonEventHandler ToggledOff;

        /// <summary>Invoked when the button has been double clicked</summary>
        internal event ButtonEventHandler DoubleClickedLeft;

        /// <summary>Toggles the button</summary>
        public virtual void Toggle()
        {
            ToggleState = !ToggleState;
        }

        /// <summary>"Clicks" the button</summary>
        public virtual void Press(GameControl c_control = null)
        {
            OnClicked();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            if (ShouldDrawBackground)
            {
                var a_drawDepressed = IsDepressed && IsHovered;
                if (IsToggle)
                {
                    a_drawDepressed = a_drawDepressed || ToggleState;
                }

                var a_bDrawHovered = IsHovered && ShouldDrawHover;

                c_skin.DrawButton(this, a_drawDepressed, a_bDrawHovered, IsDisabled);
            }
        }

        /// <summary>Handler invoked on mouse left clicked event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            if (c_down)
            {
                IsDepressed = true;

                FoxTraderWindow.Instance.MouseFocus = this;

                if (Pressed != null)
                {
                    Pressed.Invoke(this);
                }
            }
            else
            {
                if (IsHovered && m_isDepressed)
                {
                    OnClicked();
                }

                IsDepressed = false;

                if (Released != null)
                {
                    Released.Invoke(this);
                }
            }

            Redraw();
        }

        /// <summary>Internal OnPressed implementation</summary>
        protected virtual void OnClicked()
        {
            if (IsToggle)
            {
                Toggle();
            }

            if (Clicked != null)
            {
                Clicked.Invoke(this);
            }
        }

        /// <summary>Sets the button's image</summary>
        /// <param name="c_textureName">Texture name. Null to remove</param>
        /// <param name="c_center">Determines whether the image should be centered</param>
        public virtual void SetImage(string c_textureName, bool c_center = false)
        {
            if (string.IsNullOrEmpty(c_textureName))
            {
                if (m_image != null)
                {
                    m_image.Dispose();
                }
                m_image = null;
                return;
            }

            if (m_image == null)
            {
                m_image = new ImagePanel(this);
            }

            m_image.ImageName = c_textureName;
            m_image.SizeToContents();
            m_image.SetPosition(Math.Max(Padding.m_left, 2), 2);
            m_centerImage = c_center;

            TextPadding = new Padding(m_image.Right + 2, TextPadding.m_top, TextPadding.m_right, TextPadding.m_bottom);
        }

        /// <summary>Sizes to contents</summary>
        public override void SizeToContents()
        {
            base.SizeToContents();
            if (m_image != null)
            {
                var a_height = m_image.Height + 4;
                if (Height < a_height)
                {
                    Height = a_height;
                }
            }
        }

        /// <summary>Handler for Space keyboard event</summary>
        /// <param name="c_down">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeySpace(bool c_down)
        {
            if (c_down)
            {
                OnClicked();
            }
            return true;
        }

        /// <summary>Default accelerator handler</summary>
        protected override void OnAccelerator()
        {
            OnClicked();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);
            if (m_image != null)
            {
                Align.CenterVertically(m_image);

                if (m_centerImage)
                {
                    Align.CenterHorizontally(m_image);
                }
            }
        }

        /// <summary>Updates control colors</summary>
        public override void UpdateColors()
        {
            if (IsDisabled)
            {
                TextColor = Skin.m_colors.m_button.m_disabled;
                return;
            }

            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.m_colors.m_button.m_down;
                return;
            }

            if (IsHovered)
            {
                TextColor = Skin.m_colors.m_button.m_hover;
                return;
            }

            TextColor = Skin.m_colors.m_button.m_normal;
        }

        /// <summary>Handler invoked on mouse double click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_xY">Y coordinate</param>
        protected override void OnMouseDoubleClickedLeft(int c_x, int c_xY)
        {
            OnMouseClickedLeft(c_x, c_xY, true);
            if (DoubleClickedLeft != null)
            {
                DoubleClickedLeft.Invoke(this);
            }
        }
    }
}