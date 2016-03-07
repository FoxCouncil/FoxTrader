using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FoxTrader.UI.Renderer
{
    public class OpenTKRenderer : RendererBase
    {
        private static int m_lastTextureID;
        private readonly Graphics m_graphicsContext; // TODO: Platformize...

        private readonly Dictionary<Tuple<string, GameFont>, TextRenderer> m_stringCache;
        private readonly int m_vertexSize;
        private readonly Vertex[] m_vertices;
        private bool m_clipEnabled;

        private Color m_color;
        private int m_prevAlphaFunc;
        private float m_prevAlphaRef;
        private int m_prevBlendDst;
        private int m_prevBlendSrc;
        private readonly bool m_restoreRenderState;

        private readonly StringFormat m_stringFormat;
        private bool m_textureEnabled;

        private bool m_wasBlendEnabled;
        private bool m_wasDepthTestEnabled;
        private bool m_wasTexture2DEnabled;
        private PrivateFontCollection m_privateFontCollection;
        private Dictionary<string, GameFont> m_fontStore;

        public OpenTKRenderer(bool c_restoreRenderState = true)
        {
            m_vertices = new Vertex[Constants.kMaxVertices];
            m_vertexSize = Marshal.SizeOf(m_vertices[0]);
            m_stringCache = new Dictionary<Tuple<string, GameFont>, TextRenderer>();
            m_graphicsContext = Graphics.FromImage(new Bitmap(1024, 1024, PixelFormat.Format32bppArgb));
            m_stringFormat = new StringFormat(StringFormat.GenericTypographic);
            m_stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            m_restoreRenderState = c_restoreRenderState;

            InitFonts();
        }

        /// <summary>Returns number of cached strings in the text cache</summary>
        public int TextCacheSize => m_stringCache.Count;

        public int DrawCallCount
        {
            get;
            private set;
        }

        public int VertexCount
        {
            get;
            private set;
        }

        public override Color DrawColor
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
            }
        }

        public override void Dispose()
        {
            FlushTextCache();
            base.Dispose();
        }

        public override void Begin()
        {
            if (m_restoreRenderState)
            {
                // Get previous parameter values before changing them.
                GL.GetInteger(GetPName.BlendSrc, out m_prevBlendSrc);
                GL.GetInteger(GetPName.BlendDst, out m_prevBlendDst);
                GL.GetInteger(GetPName.AlphaTestFunc, out m_prevAlphaFunc);
                GL.GetFloat(GetPName.AlphaTestRef, out m_prevAlphaRef);

                m_wasBlendEnabled = GL.IsEnabled(EnableCap.Blend);
                m_wasTexture2DEnabled = GL.IsEnabled(EnableCap.Texture2D);
                m_wasDepthTestEnabled = GL.IsEnabled(EnableCap.DepthTest);
            }

            // Set default values and enable/disable caps.
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Texture2D);

            VertexCount = 0;
            DrawCallCount = 0;
            m_clipEnabled = false;
            m_textureEnabled = false;
            m_lastTextureID = -1;

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
        }

        public override void End()
        {
            Flush();

            if (m_restoreRenderState)
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);

                // Restore the previous parameter values.
                GL.BlendFunc((BlendingFactorSrc)m_prevBlendSrc, (BlendingFactorDest)m_prevBlendDst);
                GL.AlphaFunc((AlphaFunction)m_prevAlphaFunc, m_prevAlphaRef);

                if (!m_wasBlendEnabled)
                {
                    GL.Disable(EnableCap.Blend);
                }

                if (m_wasTexture2DEnabled && !m_textureEnabled)
                {
                    GL.Enable(EnableCap.Texture2D);
                }

                if (m_wasDepthTestEnabled)
                {
                    GL.Enable(EnableCap.DepthTest);
                }
            }

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
        }

        /// <summary>Clears the text rendering cache. Make sure to call this if cached strings size becomes too big (check TextCacheSize)</summary>
        public void FlushTextCache()
        {
            // TODO: some auto-expiring cache? based on number of elements or age
            foreach (var a_textRenderer in m_stringCache.Values)
            {
                a_textRenderer.Dispose();
            }
            m_stringCache.Clear();
        }

        private unsafe void Flush()
        {
            if (VertexCount == 0)
            {
                return;
            }

            fixed (short* a_ptr1 = &m_vertices[0].x)
            {
                fixed (byte* a_ptr2 = &m_vertices[0].r)
                {
                    fixed (float* a_ptr3 = &m_vertices[0].u)
                    {
                        GL.VertexPointer(2, VertexPointerType.Short, m_vertexSize, (IntPtr)a_ptr1);
                        GL.ColorPointer(4, ColorPointerType.UnsignedByte, m_vertexSize, (IntPtr)a_ptr2);
                        GL.TexCoordPointer(2, TexCoordPointerType.Float, m_vertexSize, (IntPtr)a_ptr3);

#pragma warning disable 618
                        GL.DrawArrays(BeginMode.Quads, 0, VertexCount);
#pragma warning restore 618
                    }
                }
            }

            DrawCallCount++;
            VertexCount = 0;
        }

        public override void DrawFilledRect(Rectangle c_rect)
        {
            if (m_textureEnabled)
            {
                Flush();
                GL.Disable(EnableCap.Texture2D);
                m_textureEnabled = false;
            }

            c_rect = Translate(c_rect);

            DrawRect(c_rect);
        }

        public override void StartClip()
        {
            m_clipEnabled = true;
        }

        public override void EndClip()
        {
            m_clipEnabled = false;
        }

        public override void DrawTexturedRect(Texture c_t, Rectangle c_rect, float c_u1 = 0, float c_v1 = 0, float c_u2 = 1, float c_v2 = 1)
        {
            // Missing image, not loaded properly?
            if (null == c_t.RendererData)
            {
                DrawMissingImage(c_rect);
                return;
            }

            var a_tex = (int)c_t.RendererData;
            c_rect = Translate(c_rect);

            var a_differentTexture = (a_tex != m_lastTextureID);
            if (!m_textureEnabled || a_differentTexture)
            {
                Flush();
            }

            if (!m_textureEnabled)
            {
                GL.Enable(EnableCap.Texture2D);
                m_textureEnabled = true;
            }

            if (a_differentTexture)
            {
                GL.BindTexture(TextureTarget.Texture2D, a_tex);
                m_lastTextureID = a_tex;
            }

            DrawRect(c_rect, c_u1, c_v1, c_u2, c_v2);
        }

        private void DrawRect(Rectangle c_rect, float c_u1 = 0, float c_v1 = 0, float c_u2 = 1, float c_v2 = 1)
        {
            if (VertexCount + 4 >= Constants.kMaxVertices)
            {
                Flush();
            }

            if (m_clipEnabled)
            {
                // cpu scissors test
                var a_scaledClipRegion = Util.ScaledRect(ClipRegion, Scale);

                if (c_rect.Y < a_scaledClipRegion.Y)
                {
                    var a_oldHeight = c_rect.Height;
                    var a_delta = a_scaledClipRegion.Y - c_rect.Y;
                    c_rect.Y = a_scaledClipRegion.Y;
                    c_rect.Height -= a_delta;

                    if (c_rect.Height <= 0)
                    {
                        return;
                    }

                    var a_dv = a_delta / (float)a_oldHeight;

                    c_v1 += a_dv * (c_v2 - c_v1);
                }

                if ((c_rect.Y + c_rect.Height) > (a_scaledClipRegion.Y + a_scaledClipRegion.Height))
                {
                    var a_oldHeight = c_rect.Height;
                    var a_delta = (c_rect.Y + c_rect.Height) - (a_scaledClipRegion.Y + a_scaledClipRegion.Height);

                    c_rect.Height -= a_delta;

                    if (c_rect.Height <= 0)
                    {
                        return;
                    }

                    var a_dv = a_delta / (float)a_oldHeight;

                    c_v2 -= a_dv * (c_v2 - c_v1);
                }

                if (c_rect.X < a_scaledClipRegion.X)
                {
                    var a_oldWidth = c_rect.Width;
                    var a_delta = a_scaledClipRegion.X - c_rect.X;
                    c_rect.X = a_scaledClipRegion.X;
                    c_rect.Width -= a_delta;

                    if (c_rect.Width <= 0)
                    {
                        return;
                    }

                    var a_du = a_delta / (float)a_oldWidth;

                    c_u1 += a_du * (c_u2 - c_u1);
                }

                if ((c_rect.X + c_rect.Width) > (a_scaledClipRegion.X + a_scaledClipRegion.Width))
                {
                    var a_oldWidth = c_rect.Width;
                    var a_delta = (c_rect.X + c_rect.Width) - (a_scaledClipRegion.X + a_scaledClipRegion.Width);

                    c_rect.Width -= a_delta;

                    if (c_rect.Width <= 0)
                    {
                        return;
                    }

                    var a_du = a_delta / (float)a_oldWidth;

                    c_u2 -= a_du * (c_u2 - c_u1);
                }
            }

            var a_vertexIndex = VertexCount;
            m_vertices[a_vertexIndex].x = (short)c_rect.X;
            m_vertices[a_vertexIndex].y = (short)c_rect.Y;
            m_vertices[a_vertexIndex].u = c_u1;
            m_vertices[a_vertexIndex].v = c_v1;
            m_vertices[a_vertexIndex].r = m_color.R;
            m_vertices[a_vertexIndex].g = m_color.G;
            m_vertices[a_vertexIndex].b = m_color.B;
            m_vertices[a_vertexIndex].a = m_color.A;

            a_vertexIndex++;
            m_vertices[a_vertexIndex].x = (short)(c_rect.X + c_rect.Width);
            m_vertices[a_vertexIndex].y = (short)c_rect.Y;
            m_vertices[a_vertexIndex].u = c_u2;
            m_vertices[a_vertexIndex].v = c_v1;
            m_vertices[a_vertexIndex].r = m_color.R;
            m_vertices[a_vertexIndex].g = m_color.G;
            m_vertices[a_vertexIndex].b = m_color.B;
            m_vertices[a_vertexIndex].a = m_color.A;

            a_vertexIndex++;
            m_vertices[a_vertexIndex].x = (short)(c_rect.X + c_rect.Width);
            m_vertices[a_vertexIndex].y = (short)(c_rect.Y + c_rect.Height);
            m_vertices[a_vertexIndex].u = c_u2;
            m_vertices[a_vertexIndex].v = c_v2;
            m_vertices[a_vertexIndex].r = m_color.R;
            m_vertices[a_vertexIndex].g = m_color.G;
            m_vertices[a_vertexIndex].b = m_color.B;
            m_vertices[a_vertexIndex].a = m_color.A;

            a_vertexIndex++;
            m_vertices[a_vertexIndex].x = (short)c_rect.X;
            m_vertices[a_vertexIndex].y = (short)(c_rect.Y + c_rect.Height);
            m_vertices[a_vertexIndex].u = c_u1;
            m_vertices[a_vertexIndex].v = c_v2;
            m_vertices[a_vertexIndex].r = m_color.R;
            m_vertices[a_vertexIndex].g = m_color.G;
            m_vertices[a_vertexIndex].b = m_color.B;
            m_vertices[a_vertexIndex].a = m_color.A;

            VertexCount += 4;
        }

        private void InitFonts()
        {
            // Load Internal Fonts
            m_privateFontCollection = new PrivateFontCollection();

            var a_chicagoFont = Properties.Resources.ttf_chicago;

            var a_rawFontData = Marshal.AllocCoTaskMem(a_chicagoFont.Length);

            Marshal.Copy(a_chicagoFont, 0, a_rawFontData, a_chicagoFont.Length);

            m_privateFontCollection.AddMemoryFont(a_rawFontData, a_chicagoFont.Length);

            Marshal.FreeCoTaskMem(a_rawFontData);

            m_fontStore = new Dictionary<string, GameFont>();

            foreach (var a_font in Constants.kDefaultGameFonts)
            {
                var a_fontData = a_font.Split(',');
                m_fontStore.Add(a_font, new GameFont(this, a_fontData[0], int.Parse(a_fontData[1])));
            }
        }

        public Font GetSystemFont(GameFont c_gameFont)
        {
            var a_customFontFamily = m_privateFontCollection.Families.FirstOrDefault(c_childFontFamily => c_childFontFamily.Name == c_gameFont.FaceName);

            if (a_customFontFamily != null)
            {
                return new Font(a_customFontFamily, c_gameFont.Size);
            }

            return new Font(c_gameFont.FaceName, c_gameFont.Size);
        }

        public GameFont GetFont(string c_fontNameFormatted)
        {
            if (!c_fontNameFormatted.Contains(","))
            {
                throw new ArgumentException($"A formatted string was expected, except we got this shit: {c_fontNameFormatted}");
            }

            var a_fontData = c_fontNameFormatted.Split(',');

            return GetFont(a_fontData[0], int.Parse(a_fontData[1]));
        }

        public GameFont GetFont(string c_fontName, int c_fontSize)
        {
            if (string.IsNullOrWhiteSpace(c_fontName) || c_fontSize <= 0)
            {
                return m_fontStore[Constants.kDefaultGameFontName];
            }

            var a_fontKey = $"{c_fontName},{c_fontSize}";

            if (m_fontStore.ContainsKey(a_fontKey))
            {
                return m_fontStore[a_fontKey];
            }

            return m_fontStore[Constants.kDefaultGameFontName];
        }

        public override void LoadFont(GameFont c_font)
        {
            Debug.Print("LoadFont {0}", c_font.FaceName);

            c_font.RealSize = c_font.Size * Scale;
            c_font.RendererData = GetSystemFont(c_font);
        }

        public override void FreeFont(GameFont c_font)
        {
            Debug.Print("FreeFont {0}", c_font.FaceName);

            if (!(c_font.RendererData is Font))
            {
                return;
            }

            ((Font)c_font.RendererData).Dispose();

            c_font.RendererData = null;
        }

        public override Point MeasureText(GameFont c_font, string c_text)
        {
            //Debug.Print(String.Format("MeasureText '{0}'", text));
            var a_sysFont = c_font.RendererData as Font;

            if (a_sysFont == null || Math.Abs(c_font.RealSize - c_font.Size * Scale) > 2)
            {
                FreeFont(c_font);
                LoadFont(c_font);
                a_sysFont = c_font.RendererData as Font;
            }

            var a_key = new Tuple<string, GameFont>(c_text, c_font);

            if (m_stringCache.ContainsKey(a_key))
            {
                var a_tex = m_stringCache[a_key].Texture;
                return new Point(a_tex.Width, a_tex.Height);
            }

            var a_size = m_graphicsContext.MeasureString(c_text, a_sysFont, Point.Empty, m_stringFormat);

            return new Point((int)Math.Round(a_size.Width), (int)Math.Round(a_size.Height));
        }

        public override void RenderText(GameFont c_font, Point c_position, string c_text)
        {
            //Debug.Print(String.Format("RenderText {0}", font.FaceName));

            // The DrawString(...) below will bind a new texture
            // so make sure everything is rendered!
            Flush();

            var a_sysFont = c_font.RendererData as Font;

            if (a_sysFont == null || Math.Abs(c_font.RealSize - c_font.Size * Scale) > 2)
            {
                FreeFont(c_font);
                LoadFont(c_font);
                a_sysFont = c_font.RendererData as Font;
            }

            var a_key = new Tuple<string, GameFont>(c_text, c_font);

            if (!m_stringCache.ContainsKey(a_key))
            {
                // not cached - create text renderer
                // Debug.Print(String.Format("RenderText: caching \"{0}\", {1}", text, font.FaceName));

                var a_size = MeasureText(c_font, c_text);
                var a_tr = new TextRenderer(a_size.X, a_size.Y, this);
                a_tr.DrawString(c_text, a_sysFont, Brushes.White, Point.Empty, m_stringFormat); // renders string on the texture

                DrawTexturedRect(a_tr.Texture, new Rectangle(c_position.X, c_position.Y, a_tr.Texture.Width, a_tr.Texture.Height));

                m_stringCache[a_key] = a_tr;
            }
            else
            {
                var a_tr = m_stringCache[a_key];
                DrawTexturedRect(a_tr.Texture, new Rectangle(c_position.X, c_position.Y, a_tr.Texture.Width, a_tr.Texture.Height));
            }
        }

        public static void LoadTextureInternal(Texture c_texture, Bitmap c_bitmap)
        {
            // TODO: convert to proper format
            var a_lockFormat = PixelFormat.Undefined;

            switch (c_bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                {
                    a_lockFormat = PixelFormat.Format32bppArgb;
                }
                break;

                case PixelFormat.Format24bppRgb:
                {
                    a_lockFormat = PixelFormat.Format32bppArgb;
                }
                break;

                default:
                {
                    c_texture.Failed = true;
                }
                return;
            }

            int a_glTexture;

            // Create the opengl texture
            GL.GenTextures(1, out a_glTexture);
            GL.BindTexture(TextureTarget.Texture2D, a_glTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            c_texture.RendererData = a_glTexture;
            c_texture.Width = c_bitmap.Width;
            c_texture.Height = c_bitmap.Height;

            var a_data = c_bitmap.LockBits(new Rectangle(0, 0, c_bitmap.Width, c_bitmap.Height), ImageLockMode.ReadOnly, a_lockFormat);

            switch (a_lockFormat)
            {
                case PixelFormat.Format32bppArgb:
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, c_texture.Width, c_texture.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, a_data.Scan0);
                break;
                default:
                // invalid
                break;
            }

            c_bitmap.UnlockBits(a_data);

            m_lastTextureID = a_glTexture;
        }

        public override void LoadTexture(Texture c_t)
        {
            Bitmap a_bmp;

            try
            {
                a_bmp = (Bitmap)I18N.GetObject(c_t.Name);

                if (a_bmp == null)
                {
                    a_bmp = new Bitmap(c_t.Name);
                }
            }
            catch (Exception)
            {
                c_t.Failed = true;
                return;
            }

            LoadTextureInternal(c_t, a_bmp);
            a_bmp.Dispose();
        }

        public override void LoadTextureStream(Texture c_t, Stream c_data)
        {
            Bitmap a_bmp;
            try
            {
                a_bmp = new Bitmap(c_data);
            }
            catch (Exception)
            {
                c_t.Failed = true;
                return;
            }

            LoadTextureInternal(c_t, a_bmp);
            a_bmp.Dispose();
        }

        public override void LoadTextureRaw(Texture c_t, byte[] c_pixelData)
        {
            Bitmap a_bmp;
            try
            {
                unsafe
                {
                    fixed (byte* a_ptr = &c_pixelData[0])
                    {
                        a_bmp = new Bitmap(c_t.Width, c_t.Height, 4 * c_t.Width, PixelFormat.Format32bppArgb, (IntPtr)a_ptr);
                    }
                }
            }
            catch (Exception)
            {
                c_t.Failed = true;
                return;
            }

            int a_glTex;

            // Create the opengl texture
            GL.GenTextures(1, out a_glTex);
            GL.BindTexture(TextureTarget.Texture2D, a_glTex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            c_t.RendererData = a_glTex;

            var a_data = a_bmp.LockBits(new Rectangle(0, 0, a_bmp.Width, a_bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, c_t.Width, c_t.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, a_data.Scan0);

            m_lastTextureID = a_glTex;

            a_bmp.UnlockBits(a_data);
            a_bmp.Dispose();
        }

        public override void FreeTexture(Texture c_t)
        {
            if (c_t.RendererData == null)
            {
                return;
            }
            var a_tex = (int)c_t.RendererData;
            if (a_tex == 0)
            {
                return;
            }
            GL.DeleteTextures(1, ref a_tex);
            c_t.RendererData = null;
        }

        public override unsafe Color PixelColor(Texture c_texture, uint c_x, uint c_y, Color c_defaultColor)
        {
            if (c_texture.RendererData == null)
            {
                return c_defaultColor;
            }

            var a_tex = (int)c_texture.RendererData;
            if (a_tex == 0)
            {
                return c_defaultColor;
            }

            Color a_pixel;
            GL.BindTexture(TextureTarget.Texture2D, a_tex);
            var a_offset = 4 * (c_x + c_y * c_texture.Width);
            var a_data = new byte[4 * c_texture.Width * c_texture.Height];
            fixed (byte* a_ptr = &a_data[0])
            {
                GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, (IntPtr)a_ptr);
                a_pixel = Color.FromArgb(a_data[a_offset + 3], a_data[a_offset + 0], a_data[a_offset + 1], a_data[a_offset + 2]);
            }
            // Retrieving the entire texture for a single pixel read
            // is kind of a waste - maybe cache this pointer in the texture
            // data and then release later on? It's never called during runtime
            // - only during initialization.
            return a_pixel;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Vertex
        {
            public short x, y;
            public float u, v;
            public byte r, g, b, a;
        }
    }
}