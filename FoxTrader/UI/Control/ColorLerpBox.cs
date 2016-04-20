using System.Drawing;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>Linear-interpolated HSV color box</summary>
    internal class ColorLerpBox : GameControl
    {
        private Point m_cursorPosition;
        private float m_hueColor;
        private bool m_isDepressed;
        private Texture m_texture;

        /// <summary>Initializes a new instance of the <see cref="ColorLerpBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ColorLerpBox(GameControl c_parentControl) : base(c_parentControl)
        {
            SetColor(Color.FromArgb(255, 255, 128, 0));
            SetSize(128, 128);
            MouseInputEnabled = true;
            m_isDepressed = false;
        }

        /// <summary>Selected color</summary>
        public Color SelectedColor => GetColorAt(m_cursorPosition.X, m_cursorPosition.Y);

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

        /// <summary>Linear color interpolation</summary>
        public static Color Lerp(Color c_toColor, Color c_fromColor, float c_amount)
        {
            var a_delta = c_toColor.Subtract(c_fromColor);

            a_delta = a_delta.Multiply(c_amount);

            return c_fromColor.Add(a_delta);
        }

        /// <summary>Sets the selected color</summary>
        /// <param name="c_colorValue">Value to set</param>
        /// <param name="c_onlyHue">Deetrmines whether to only set H value (not SV)</param>
        public void SetColor(Color c_colorValue, bool c_onlyHue = true)
        {
            var a_hsvColor = c_colorValue.ToHSV();

            m_hueColor = a_hsvColor.m_h;

            if (!c_onlyHue)
            {
                m_cursorPosition.X = (int)(a_hsvColor.m_s * Width);
                m_cursorPosition.Y = (int)((1 - a_hsvColor.m_v) * Height);
            }

            Invalidate();

            ColorChanged?.Invoke(this);
        }

        /// <summary>Handler invoked on mouse moved event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_dx">X change</param>
        /// <param name="c_dy">Y change</param>
        public override void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            if (!m_isDepressed)
            {
                return;
            }

            m_cursorPosition = CanvasPosToLocal(c_mouseEventArgs.Position);

            //Do we have clamp?
            if (m_cursorPosition.X < 0)
            {
                m_cursorPosition.X = 0;
            }

            if (m_cursorPosition.X > Width)
            {
                m_cursorPosition.X = Width;
            }

            if (m_cursorPosition.Y < 0)
            {
                m_cursorPosition.Y = 0;
            }

            if (m_cursorPosition.Y > Height)
            {
                m_cursorPosition.Y = Height;
            }

            ColorChanged?.Invoke(this);
        }

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            m_isDepressed = true;
            GetCanvas().MouseFocus = this;

            OnMouseMoved(new MouseMoveEventArgs(c_mouseButtonEventArgs.Position.X, c_mouseButtonEventArgs.Position.Y, 0, 0));
        }

        public override void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            m_isDepressed = false;
            GetCanvas().MouseFocus = null;

            OnMouseMoved(new MouseMoveEventArgs(c_mouseButtonEventArgs.Position.X, c_mouseButtonEventArgs.Position.Y, 0, 0));
        }

        /// <summary>Gets the color from specified coordinates</summary>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        /// <returns>Color value</returns>
        private Color GetColorAt(int c_x, int c_y)
        {
            var a_xPercent = (c_x / (float)Width);
            var a_yPercent = 1 - (c_y / (float)Height);

            var a_result = Util.HSVToColor(m_hueColor, a_xPercent, a_yPercent);

            return a_result;
        }

        /// <summary>Invalidates the control</summary>
        public override void Invalidate()
        {
            if (m_texture != null)
            {
                m_texture.Dispose();
                m_texture = null;
            }

            base.Invalidate();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            if (m_texture == null)
            {
                var a_pixelData = new byte[Width * Height * 4];

                for (var a_x = 0; a_x < Width; a_x++)
                {
                    for (var a_y = 0; a_y < Height; a_y++)
                    {
                        var a_color = GetColorAt(a_x, a_y);

                        a_pixelData[4 * (a_x + a_y * Width)] = a_color.R;
                        a_pixelData[4 * (a_x + a_y * Width) + 1] = a_color.G;
                        a_pixelData[4 * (a_x + a_y * Width) + 2] = a_color.B;
                        a_pixelData[4 * (a_x + a_y * Width) + 3] = a_color.A;
                    }
                }

                m_texture = new Texture(c_skin.Renderer);
                m_texture.Width = Width;
                m_texture.Height = Height;
                m_texture.LoadRaw(Width, Height, a_pixelData);
            }

            c_skin.Renderer.DrawColor = Color.White;
            c_skin.Renderer.DrawTexturedRect(m_texture, RenderBounds);

            c_skin.Renderer.DrawColor = Color.Black;
            c_skin.Renderer.DrawLinedRect(RenderBounds);

            var a_selectedColor = SelectedColor;

            if ((a_selectedColor.R + a_selectedColor.G + a_selectedColor.B) / 3 < 170)
            {
                c_skin.Renderer.DrawColor = Color.White;
            }
            else
            {
                c_skin.Renderer.DrawColor = Color.Black;
            }

            var a_testRect = new Rectangle(m_cursorPosition.X - 3, m_cursorPosition.Y - 3, 6, 6);

            c_skin.Renderer.DrawShavedCornerRect(a_testRect);

            base.Render(c_skin);
        }
    }
}