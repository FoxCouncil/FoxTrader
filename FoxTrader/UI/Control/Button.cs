using System;
using OpenTK.Input;
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
            MouseInputEnabled = true;
            Alignment = Pos.Center;
            TextPadding = new Padding(3, 3, 3, 3);

            UpdateTextColorState(false);
        }

        public override bool IsDisabled
        {
            get
            {
                return base.IsDisabled;
            }
            set
            {
                base.IsDisabled = value;
                UpdateTextColorState(false);
            }
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

                Toggled?.Invoke(this, new MouseButtonEventArgs());

                if (m_toggleStatus)
                {
                    ToggledOn?.Invoke(this, new MouseButtonEventArgs());
                }
                else
                {
                    ToggledOff?.Invoke(this, new MouseButtonEventArgs());
                }

                Redraw();
            }
        }

        /// <summary>Invoked when the button's toggle state has changed</summary>
        internal event MouseButtonEventHandler Toggled;

        /// <summary>Invoked when the button's toggle state has changed to On</summary>
        internal event MouseButtonEventHandler ToggledOn;

        /// <summary>Invoked when the button's toggle state has changed to Off</summary>
        internal event MouseButtonEventHandler ToggledOff;

        /// <summary>Toggles the button</summary>
        public virtual void Toggle()
        {
            ToggleState = !ToggleState;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            if (!ShouldDrawBackground)
            {
                return;
            }

            var a_drawDepressed = IsDepressed && IsHovered;

            if (IsToggle)
            {
                a_drawDepressed = a_drawDepressed || ToggleState;
            }

            var a_bDrawHovered = IsHovered && ShouldDrawHover;

            c_skin.DrawButton(this, a_drawDepressed, a_bDrawHovered, IsDisabled);

            RenderText(c_skin.Renderer);
        }

        /// <summary>Internal OnPressed implementation</summary>
        public override void OnClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (IsToggle)
            {
                Toggle();
            }

            base.OnClicked(c_mouseButtonEventArgs);
        }

        /// <summary>Sets the button's image</summary>
        /// <param name="c_textureName">Texture name. Null to remove</param>
        /// <param name="c_center">Determines whether the image should be centered</param>
        public void SetImage(string c_textureName, bool c_center = false)
        {
            if (string.IsNullOrEmpty(c_textureName))
            {
                m_image?.Dispose();
                m_image = null;
                return;
            }

            if (m_image == null)
            {
                m_image = new ImagePanel(this);
            }

            m_image.ImageName = c_textureName;
            m_image.SizeToContents();
            m_image.SetPosition(Math.Max(Padding.Left, 2), 2);
            m_centerImage = c_center;

            TextPadding = new Padding(m_image.Right + 2, TextPadding.Top, TextPadding.Right, TextPadding.Bottom);
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

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            base.OnLayout(c_skin);

            if (m_image != null)
            {
                Align.CenterVertically(m_image);

                if (m_centerImage)
                {
                    Align.CenterHorizontally(m_image);
                }
            }
        }

        public override void OnMouseOver(MouseMoveEventArgs c_mouseEventArgs)
        {
            base.OnMouseOver(c_mouseEventArgs);

            UpdateTextColorState(true);
        }

        public override void OnMouseOut(MouseMoveEventArgs c_mouseEventArgs)
        {
            base.OnMouseOut(c_mouseEventArgs);

            UpdateTextColorState(false);
        }

        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            base.OnMouseDown(c_mouseButtonEventArgs);

            if (!IsDisabled)
            {
                m_isDepressed = true;
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            base.OnMouseUp(c_mouseButtonEventArgs);

            m_isDepressed = false;
        }

        private void UpdateTextColorState(bool c_isHovered)
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

            if (c_isHovered)
            {
                TextColor = Skin.m_colors.m_button.m_hover;
                return;
            }

            TextColor = Skin.m_colors.m_button.m_normal;
        }
    }
}