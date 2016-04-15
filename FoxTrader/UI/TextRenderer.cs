using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using OpenTK.Graphics;

namespace FoxTrader.UI
{
    /// <summary>Uses System.Drawing for 2d text rendering</summary>
    public sealed class TextRenderer : IDisposable
    {
        private readonly Bitmap m_bitmap;
        private readonly Graphics m_graphics;
        private bool m_isDisposed;

        /// <summary>Constructs a new instance</summary>
        /// <param name="c_width">The width of the backing store in pixels</param>
        /// <param name="c_height">The height of the backing store in pixels</param>
        /// <param name="c_renderer">The renderer</param>
        public TextRenderer(int c_width, int c_height, Renderer c_renderer)
        {
            if (c_width <= 0)
            {
                throw new ArgumentOutOfRangeException("width");
            }

            if (c_height <= 0)
            {
                throw new ArgumentOutOfRangeException("height");
            }

            if (GraphicsContext.CurrentContext == null)
            {
                throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");
            }

            m_bitmap = new Bitmap(c_width, c_height, PixelFormat.Format32bppArgb);
            m_graphics = Graphics.FromImage(m_bitmap);

            // NOTE:    TextRenderingHint.AntiAliasGridFit looks sharper and in most cases better
            //          but it comes with a some problems.
            //
            //          1.  Graphic.MeasureString and format.MeasureCharacterRanges 
            //              seem to return wrong values because of this.
            //
            //          2.  While typing the kerning changes in random places in the sentence.
            // 
            //          Until 1st problem is fixed we should use TextRenderingHint.AntiAlias...  :-(

            m_graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            m_graphics.Clear(Color.Transparent);
            Texture = new Texture(c_renderer) { Width = c_width, Height = c_height };
        }

        public Texture Texture
        {
            get;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Draws the specified string to the backing store</summary>
        /// <param name="c_text">The <see cref="System.String" /> to draw</param>
        /// <param name="c_font">The <see cref="Font" /> that will be used</param>
        /// <param name="c_brush">The <see cref="System.Drawing.Brush" /> that will be used</param>
        /// <param name="c_point">The location of the text on the backing store, in 2d pixel coordinates. The origin (0, 0) lies at the top-left corner of the backing store</param>
        /// <param name="c_format">The string format for this string</param>
        public void DrawString(string c_text, Font c_font, Brush c_brush, Point c_point, StringFormat c_format)
        {
            m_graphics.DrawString(c_text, c_font, c_brush, c_point, c_format); // render text on the bitmap
            Renderer.LoadTextureInternal(Texture, m_bitmap); // copy bitmap to gl texture
        }

        private void Dispose(bool c_isManual)
        {
            if (!m_isDisposed)
            {
                if (c_isManual)
                {
                    m_bitmap.Dispose();
                    m_graphics.Dispose();
                    Texture.Dispose();
                }

                m_isDisposed = true;
            }
        }

        ~TextRenderer()
        {
            Console.WriteLine("[Warning] Resource leaked: {0}", typeof(TextRenderer));
        }
    }
}