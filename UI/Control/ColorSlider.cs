using System.Drawing;
using FoxTrader.UI.Skin;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>HSV hue selector</summary>
    internal class ColorSlider : GameControl
    {
        private bool m_isDepressed;
        private int m_selectedDist;
        private Texture m_texture;

        /// <summary>Initializes a new instance of the <see cref="ColorSlider" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ColorSlider(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(32, 128);
            MouseInputEnabled = true;
            m_isDepressed = false;
        }

        /// <summary>Selected color</summary>
        public Color SelectedColor
        {
            get
            {
                return GetColorAtHeight(m_selectedDist);
            }
            set
            {
                SetColor(value);
            }
        }

        /// <summary>Invoked when the selected color has been changed</summary>
        public event ColorEventHandler ColorChanged;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public override void Dispose()
        {
            if (m_texture != null)
            {
                m_texture.Dispose();
            }

            base.Dispose();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            // TODO: Is there any way to move this into skin? Not for now, no idea how we'll "actually" render these
            if (m_texture == null)
            {
                var a_pixelData = new byte[Width * Height * 4];

                for (var a_idx = 0; a_idx < Height; a_idx++)
                {
                    var a_color = GetColorAtHeight(a_idx);

                    for (var a_x = 0; a_x < Width; a_x++)
                    {
                        a_pixelData[4 * (a_x + a_idx * Width)] = a_color.R;
                        a_pixelData[4 * (a_x + a_idx * Width) + 1] = a_color.G;
                        a_pixelData[4 * (a_x + a_idx * Width) + 2] = a_color.B;
                        a_pixelData[4 * (a_x + a_idx * Width) + 3] = a_color.A;
                    }
                }

                m_texture = new Texture(c_skin.Renderer);
                m_texture.Width = Width;
                m_texture.Height = Height;
                m_texture.LoadRaw(Width, Height, a_pixelData);
            }

            c_skin.Renderer.DrawColor = Color.White;
            c_skin.Renderer.DrawTexturedRect(m_texture, new Rectangle(5, 0, Width - 10, Height));

            var a_drawHeight = m_selectedDist - 3;

            //Draw our selectors
            c_skin.Renderer.DrawColor = Color.Black;
            c_skin.Renderer.DrawFilledRect(new Rectangle(0, a_drawHeight + 2, Width, 1));
            c_skin.Renderer.DrawFilledRect(new Rectangle(0, a_drawHeight, 5, 5));
            c_skin.Renderer.DrawFilledRect(new Rectangle(Width - 5, a_drawHeight, 5, 5));
            c_skin.Renderer.DrawColor = Color.White;
            c_skin.Renderer.DrawFilledRect(new Rectangle(1, a_drawHeight + 1, 3, 3));
            c_skin.Renderer.DrawFilledRect(new Rectangle(Width - 4, a_drawHeight + 1, 3, 3));

            base.Render(c_skin);
        }

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            m_isDepressed = c_down;

            if (c_down)
            {
                FoxTraderWindow.Instance.MouseFocus = this;
            }
            else
            {
                FoxTraderWindow.Instance.MouseFocus = null;
            }

            OnMouseMoved(new MouseState(), c_x, c_y, 0, 0);
        }

        /// <summary>Handler invoked on mouse moved event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_dx">X change</param>
        /// <param name="c_dy">Y change</param>
        protected override void OnMouseMoved(MouseState c_mouseState, int c_x, int c_y, int c_dx, int c_dy)
        {
            if (m_isDepressed)
            {
                var a_cursorPos = CanvasPosToLocal(new Point(c_x, c_y));

                if (a_cursorPos.Y < 0)
                {
                    a_cursorPos.Y = 0;
                }

                if (a_cursorPos.Y > Height)
                {
                    a_cursorPos.Y = Height;
                }

                m_selectedDist = a_cursorPos.Y;

                if (ColorChanged != null)
                {
                    ColorChanged.Invoke(this);
                }
            }
        }

        private Color GetColorAtHeight(int c_y)
        {
            var a_yPercent = c_y / (float)Height;

            return Util.HSVToColor(a_yPercent * 360, 1, 1);
        }

        private void SetColor(Color c_color)
        {
            var a_hsvColor = c_color.ToHSV();

            m_selectedDist = (int)(a_hsvColor.m_h / 360 * Height);

            if (ColorChanged != null)
            {
                ColorChanged.Invoke(this);
            }
        }
    }
}