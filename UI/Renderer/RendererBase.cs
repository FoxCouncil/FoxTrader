using System;
using System.Drawing;
using System.IO;

namespace FoxTrader.UI.Renderer
{
    /// <summary>Base renderer</summary>
    public abstract class RendererBase : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="RendererBase" /> class</summary>
        protected RendererBase()
        {
            RenderOffset = Point.Empty;
            Scale = 1.0f;
        }

        public float Scale
        {
            get;
            set;
        }

        /// <summary>Gets or sets the current drawing color</summary>
        public virtual Color DrawColor
        {
            get;
            set;
        }

        /// <summary>Rendering offset. No need to touch it usually</summary>
        public Point RenderOffset
        {
            get;
            set;
        }

        /// <summary>Clipping rectangle</summary>
        public Rectangle ClipRegion
        {
            get;
            set;
        }

        /// <summary>Indicates whether the clip region is visible</summary>
        public bool ClipRegionVisible => ClipRegion.Width > 0 && ClipRegion.Height > 0;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

#if DEBUG
        ~RendererBase()
        {
            throw new InvalidOperationException($"IDisposable object finalized: {GetType()}");
            //Debug.Print(String.Format("IDisposable object finalized: {0}", GetType()));
        }
#endif

        /// <summary>Starts rendering</summary>
        public virtual void Begin()
        {
        }

        /// <summary>Stops rendering</summary>
        public virtual void End()
        {
        }

        /// <summary>Draws a line</summary>
        /// <param name="c_x"></param>
        /// <param name="c_y"></param>
        /// <param name="c_a"></param>
        /// <param name="c_b"></param>
        public virtual void DrawLine(int c_x, int c_y, int c_a, int c_b)
        {
        }

        /// <summary>Draws a solid filled rectangle</summary>
        /// <param name="c_rect"></param>
        public abstract void DrawFilledRect(Rectangle c_rect);

        /// <summary>Starts clipping to the current clipping rectangle</summary>
        public virtual void StartClip()
        {
        }

        /// <summary>Stops clipping</summary>
        public virtual void EndClip()
        {
        }

        /// <summary>Loads the specified texture</summary>
        /// <param name="c_t"></param>
        public abstract void LoadTexture(Texture c_t);

        /// <summary>Initializes texture from raw pixel data</summary>
        /// <param name="c_t">Texture to initialize. Dimensions need to be set</param>
        /// <param name="c_pixelData">Pixel data in RGBA format</param>
        public abstract void LoadTextureRaw(Texture c_t, byte[] c_pixelData);

        /// <summary>Initializes texture from image file data</summary>
        /// <param name="c_t">Texture to initialize</param>
        /// <param name="c_data">Image file as stream</param>
        public abstract void LoadTextureStream(Texture c_t, Stream c_data);

        /// <summary>Frees the specified texture</summary>
        /// <param name="c_t">Texture to free</param>
        public abstract void FreeTexture(Texture c_t);

        /// <summary>Draws textured rectangle</summary>
        /// <param name="c_t">Texture to use</param>
        /// <param name="c_rect">Rectangle bounds</param>
        /// <param name="c_u1">Texture coordinate u1</param>
        /// <param name="c_v1">Texture coordinate v1</param>
        /// <param name="c_u2">Texture coordinate u2</param>
        /// <param name="c_v2">Texture coordinate v2</param>
        public abstract void DrawTexturedRect(Texture c_t, Rectangle c_rect, float c_u1 = 0, float c_v1 = 0, float c_u2 = 1, float c_v2 = 1);

        /// <summary>Draws "missing image" default texture</summary>
        /// <param name="c_rect">Target rectangle</param>
        public virtual void DrawMissingImage(Rectangle c_rect)
        {
            //DrawColor = Color.FromArgb(255, rnd.Next(0,255), rnd.Next(0,255), rnd.Next(0, 255));
            DrawColor = Color.Red;
            DrawFilledRect(c_rect);
        }

        /// <summary>Loads the specified font</summary>
        /// <param name="c_font">Font to load</param>
        /// <returns>True if succeeded</returns>
        public virtual bool LoadFont(GameFont c_font)
        {
            return false;
        }

        /// <summary>Frees the specified font</summary>
        /// <param name="c_font">Font to free</param>
        public virtual void FreeFont(GameFont c_font)
        {
        }

        /// <summary>Returns dimensions of the text using specified font</summary>
        /// <param name="c_font">Font to use</param>
        /// <param name="c_text">Text to measure</param>
        /// <returns>Width and height of the rendered text</returns>
        public virtual Point MeasureText(GameFont c_font, string c_text)
        {
            var a_p = new Point((int)(c_font.Size * Scale * c_text.Length * 0.4f), (int)(c_font.Size * Scale));

            return a_p;
        }

        /// <summary>Renders text using specified font</summary>
        /// <param name="c_font">Font to use</param>
        /// <param name="c_position">Top-left corner of the text</param>
        /// <param name="c_text">Text to render</param>
        public abstract void RenderText(GameFont c_font, Point c_position, string c_text);

        //
        // No need to implement these functions in your derived class, but if 
        // you can do them faster than the default implementation it's a good idea to.
        //

        /// <summary>Draws a lined rectangle. Used for keyboard focus overlay</summary>
        /// <param name="c_rect">Target rectangle</param>
        public virtual void DrawLinedRect(Rectangle c_rect)
        {
            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y, c_rect.Width, 1));
            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y + c_rect.Height - 1, c_rect.Width, 1));

            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y, 1, c_rect.Height));
            DrawFilledRect(new Rectangle(c_rect.X + c_rect.Width - 1, c_rect.Y, 1, c_rect.Height));
        }

        /// <summary>Draws a single pixel. Very slow, do not use</summary>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        public virtual void DrawPixel(int c_x, int c_y)
        {
            DrawFilledRect(new Rectangle(c_x, c_y, 1, 1));
        }

        /// <summary>Gets pixel color of a specified texture. Slow</summary>
        /// <param name="c_texture">Texture</param>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        /// <returns>Pixel color</returns>
        public virtual Color PixelColor(Texture c_texture, uint c_x, uint c_y)
        {
            return PixelColor(c_texture, c_x, c_y, Color.White);
        }

        /// <summary>Gets pixel color of a specified texture, returning default if otherwise failed. Slow</summary>
        /// <param name="c_texture">Texture</param>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        /// <param name="c_defaultColor">Color to return on failure</param>
        /// <returns>Pixel color</returns>
        public virtual Color PixelColor(Texture c_texture, uint c_x, uint c_y, Color c_defaultColor)
        {
            return c_defaultColor;
        }

        /// <summary>Draws a round-corner rectangle</summary>
        /// <param name="c_rect">Target rectangle</param>
        /// <param name="c_slight"></param>
        public virtual void DrawShavedCornerRect(Rectangle c_rect, bool c_slight = false)
        {
            // Draw INSIDE the w/h.
            c_rect.Width -= 1;
            c_rect.Height -= 1;

            if (c_slight)
            {
                DrawFilledRect(new Rectangle(c_rect.X + 1, c_rect.Y, c_rect.Width - 1, 1));
                DrawFilledRect(new Rectangle(c_rect.X + 1, c_rect.Y + c_rect.Height, c_rect.Width - 1, 1));

                DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y + 1, 1, c_rect.Height - 1));
                DrawFilledRect(new Rectangle(c_rect.X + c_rect.Width, c_rect.Y + 1, 1, c_rect.Height - 1));
                return;
            }

            DrawPixel(c_rect.X + 1, c_rect.Y + 1);
            DrawPixel(c_rect.X + c_rect.Width - 1, c_rect.Y + 1);

            DrawPixel(c_rect.X + 1, c_rect.Y + c_rect.Height - 1);
            DrawPixel(c_rect.X + c_rect.Width - 1, c_rect.Y + c_rect.Height - 1);

            DrawFilledRect(new Rectangle(c_rect.X + 2, c_rect.Y, c_rect.Width - 3, 1));
            DrawFilledRect(new Rectangle(c_rect.X + 2, c_rect.Y + c_rect.Height, c_rect.Width - 3, 1));

            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y + 2, 1, c_rect.Height - 3));
            DrawFilledRect(new Rectangle(c_rect.X + c_rect.Width, c_rect.Y + 2, 1, c_rect.Height - 3));
        }

        private int TranslateX(int c_x)
        {
            var a_x1 = c_x + RenderOffset.X;
            return Util.Round(a_x1 * Scale);
        }

        private int TranslateY(int c_y)
        {
            var a_y1 = c_y + RenderOffset.Y;
            return Util.Round(a_y1 * Scale);
        }

        /// <summary>Translates a panel's local drawing coordinate into view space, taking offsets into account</summary>
        /// <param name="c_x"></param>
        /// <param name="c_y"></param>
        public void Translate(ref int c_x, ref int c_y)
        {
            c_x += RenderOffset.X;
            c_y += RenderOffset.Y;

            c_x = Util.Round(c_x * Scale);
            c_y = Util.Round(c_y * Scale);
        }

        /// <summary>Translates a panel's local drawing coordinate into view space, taking offsets into account</summary>
        public Point Translate(Point c_p)
        {
            var a_x = c_p.X;
            var a_y = c_p.Y;
            Translate(ref a_x, ref a_y);
            return new Point(a_x, a_y);
        }

        /// <summary>Translates a panel's local drawing coordinate into view space, taking offsets into account</summary>
        public Rectangle Translate(Rectangle c_rect)
        {
            return new Rectangle(TranslateX(c_rect.X), TranslateY(c_rect.Y), Util.Round(c_rect.Width * Scale), Util.Round(c_rect.Height * Scale));
        }

        /// <summary>Adds a point to the render offset</summary>
        /// <param name="c_offset">Point to add</param>
        public void AddRenderOffset(Rectangle c_offset)
        {
            RenderOffset = new Point(RenderOffset.X + c_offset.X, RenderOffset.Y + c_offset.Y);
        }

        /// <summary>Adds a rectangle to the clipping region</summary>
        /// <param name="c_rect">Rectangle to add</param>
        public void AddClipRegion(Rectangle c_rect)
        {
            c_rect.X = RenderOffset.X;
            c_rect.Y = RenderOffset.Y;

            var a_r = c_rect;
            if (c_rect.X < ClipRegion.X)
            {
                a_r.Width -= (ClipRegion.X - a_r.X);
                a_r.X = ClipRegion.X;
            }

            if (c_rect.Y < ClipRegion.Y)
            {
                a_r.Height -= (ClipRegion.Y - a_r.Y);
                a_r.Y = ClipRegion.Y;
            }

            if (c_rect.Right > ClipRegion.Right)
            {
                a_r.Width = ClipRegion.Right - a_r.X;
            }

            if (c_rect.Bottom > ClipRegion.Bottom)
            {
                a_r.Height = ClipRegion.Bottom - a_r.Y;
            }

            ClipRegion = a_r;
        }
    }
}