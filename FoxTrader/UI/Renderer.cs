using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using FoxTrader.UI.Font;
using OpenTK.Graphics.OpenGL;

namespace FoxTrader.UI
{
    public class Renderer : IDisposable
    {
        private const string kLogName = "RENDER";

        public static int LastTextureID
        {
            get;
            set;
        }

        private readonly bool m_restoreRenderState;

        private readonly StringFormat m_stringFormat;

        private readonly int m_quadVertexSize;
        private readonly Vertex[] m_quadVertices;
        private int m_quadVerticesCount;

        private readonly int m_lineVerticesSize;
        private readonly Vertex[] m_lineVertices;
        private int m_lineVerticesCount;

        private bool m_clipEnabled;

        private int m_prevAlphaFunc;
        private float m_prevAlphaRef;
        private int m_prevBlendDst;
        private int m_prevBlendSrc;
        private bool m_textureEnabled;

        private bool m_wasBlendEnabled;
        private bool m_wasDepthTestEnabled;
        private bool m_wasTexture2DEnabled;

        public Renderer(bool c_restoreRenderState = true)
        {
            RenderOffset = Point.Empty;
            Scale = 1.0f;

            m_quadVertices = new Vertex[Constants.kMaxQuadVertices];
            m_quadVertexSize = Marshal.SizeOf(m_quadVertices[0]);

            m_lineVertices = new Vertex[Constants.kMaxLineVertices];
            m_lineVerticesSize = Marshal.SizeOf(m_lineVertices[0]);

            m_stringFormat = new StringFormat(StringFormat.GenericTypographic);
            m_stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            m_restoreRenderState = c_restoreRenderState;
        }

        public int DrawCallCount
        {
            get;
            private set;
        }

        public int VertexTotalCount
        {
            get;
            private set;
        }

        public Color DrawColor
        {
            get;
            set;
        }

        public float Scale
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Begin()
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

            m_quadVerticesCount = 0;

            DrawCallCount = 0;

            m_clipEnabled = false;
            m_textureEnabled = false;
            LastTextureID = -1;

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
        }

        public void End()
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

        private void Flush()
        {
            FlushLines();
            FlushQuads();
        }

        private unsafe void FlushLines()
        {
            if (m_lineVerticesCount == 0)
            {
                return;
            }

            fixed (short* a_ptr1 = &m_lineVertices[0].X)
            {
                fixed (byte* a_ptr2 = &m_lineVertices[0].R)
                {
                    fixed (float* a_ptr3 = &m_lineVertices[0].U)
                    {
                        GL.VertexPointer(2, VertexPointerType.Short, m_lineVerticesSize, (IntPtr)a_ptr1);
                        GL.ColorPointer(4, ColorPointerType.UnsignedByte, m_lineVerticesSize, (IntPtr)a_ptr2);
                        GL.TexCoordPointer(2, TexCoordPointerType.Float, m_lineVerticesSize, (IntPtr)a_ptr3);

#pragma warning disable 618
                        GL.DrawArrays(BeginMode.Lines, 0, m_lineVerticesCount);
#pragma warning restore 618
                    }
                }
            }

            DrawCallCount++;
            VertexTotalCount += m_lineVerticesCount;
            m_lineVerticesCount = 0;
        }

        private unsafe void FlushQuads()
        {
            if (m_quadVerticesCount == 0)
            {
                return;
            }

            fixed (short* a_ptr1 = &m_quadVertices[0].X)
            {
                fixed (byte* a_ptr2 = &m_quadVertices[0].R)
                {
                    fixed (float* a_ptr3 = &m_quadVertices[0].U)
                    {
                        GL.VertexPointer(2, VertexPointerType.Short, m_quadVertexSize, (IntPtr)a_ptr1);
                        GL.ColorPointer(4, ColorPointerType.UnsignedByte, m_quadVertexSize, (IntPtr)a_ptr2);
                        GL.TexCoordPointer(2, TexCoordPointerType.Float, m_quadVertexSize, (IntPtr)a_ptr3);

#pragma warning disable 618
                        GL.DrawArrays(BeginMode.Quads, 0, m_quadVerticesCount);
#pragma warning restore 618
                    }
                }
            }

            DrawCallCount++;
            VertexTotalCount += m_quadVerticesCount;
            m_quadVerticesCount = 0;
        }

        public void DrawFilledRect(Rectangle c_rect)
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

        public void StartClip()
        {
            m_clipEnabled = true;
        }

        public void EndClip()
        {
            m_clipEnabled = false;
        }

        public void DrawTexturedRect(Texture c_t, Rectangle c_rect, float c_u1 = 0, float c_v1 = 0, float c_u2 = 1, float c_v2 = 1)
        {
            // Missing image, not loaded properly?
            if (c_t.RendererData == null)
            {
                DrawMissingImage(c_rect);
                return;
            }

            var a_tex = (int)c_t.RendererData;
            c_rect = Translate(c_rect);

            var a_differentTexture = (a_tex != LastTextureID);
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
                LastTextureID = a_tex;
            }

            DrawRect(c_rect, c_u1, c_v1, c_u2, c_v2);
        }

        private void DrawPointsRect(Point c_firstPoint, Point c_secondPoint)
        {
            if (m_quadVerticesCount + 4 >= Constants.kMaxQuadVertices)
            {
                FlushQuads();
            }

            var a_quadVertexArray = VertexHelpers.TwoPointsToQuadVertexArray(c_firstPoint, c_secondPoint, DrawColor);

            DrawQuadVertex(a_quadVertexArray);
        }

        private void DrawRect(Rectangle c_rect, float c_u1 = 0, float c_v1 = 0, float c_u2 = 1, float c_v2 = 1)
        {
            if (m_quadVerticesCount + 4 >= Constants.kMaxQuadVertices)
            {
                FlushQuads();
            }

            if (ClipLogic(c_rect, ref c_u1, ref c_v1, ref c_u2, ref c_v2))
                return;

            var a_quadVertexArray = VertexHelpers.RectToQuadVertexArray(c_rect, c_u1, c_v1, c_u2, c_v2, DrawColor);

            DrawQuadVertex(a_quadVertexArray);
        }

        private void DrawQuadVertex(IReadOnlyList<Vertex> c_quadVertices)
        {
            if (c_quadVertices.Count != 4)
            {
                throw new ArgumentException("Not a quad vertex array...shame....shame...");
            }

            var a_quadVerticesIndex = m_quadVerticesCount;

            m_quadVertices[a_quadVerticesIndex++] = c_quadVertices[0];
            m_quadVertices[a_quadVerticesIndex++] = c_quadVertices[1];
            m_quadVertices[a_quadVerticesIndex++] = c_quadVertices[2];
            m_quadVertices[a_quadVerticesIndex++] = c_quadVertices[3];

            m_quadVerticesCount += 4;
        }

        private bool ClipLogic(Rectangle c_rect, ref float c_u1, ref float c_v1, ref float c_u2, ref float c_v2)
        {
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
                        return true;
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
                        return true;
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
                        return true;
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
                        return true;
                    }

                    var a_du = a_delta / (float)a_oldWidth;

                    c_u2 -= a_du * (c_u2 - c_u1);
                }
            }

            return false;
        }

        public Size MeasureText(GameFont c_font, string c_text)
        {
            return c_font.MeasureFont(c_text);
        }


        public unsafe Color PixelColor(Texture c_texture, uint c_x, uint c_y, Color c_defaultColor)
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

#if DEBUG
        ~Renderer()
        {
            throw new InvalidOperationException($"IDisposable object finalized: {GetType()}");
            //Debug.Print(String.Format("IDisposable object finalized: {0}", GetType()));
        }
#endif

        /// <summary>Draws a line</summary>
        /// <param name="c_x"></param>
        /// <param name="c_y"></param>
        /// <param name="c_a"></param>
        /// <param name="c_b"></param>
        public void DrawLine(int c_x, int c_y, int c_a, int c_b)
        {
            DrawPointsRect(new Point(TranslateX(c_x), TranslateY(c_y)), new Point(TranslateX(c_a), TranslateY(c_b)));
        }

        /// <summary>Draws "missing image" default texture</summary>
        /// <param name="c_rect">Target rectangle</param>
        public void DrawMissingImage(Rectangle c_rect)
        {
            //DrawColor = Color.FromArgb(255, rnd.Next(0,255), rnd.Next(0,255), rnd.Next(0, 255));
            DrawColor = Color.Red;
            DrawFilledRect(c_rect);
        }

        /// <summary>Draws a lined rectangle. Used for keyboard focus overlay</summary>
        /// <param name="c_rect">Target rectangle</param>
        public void DrawLinedRect(Rectangle c_rect)
        {
            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y, c_rect.Width, 1));
            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y + c_rect.Height - 1, c_rect.Width, 1));

            DrawFilledRect(new Rectangle(c_rect.X, c_rect.Y, 1, c_rect.Height));
            DrawFilledRect(new Rectangle(c_rect.X + c_rect.Width - 1, c_rect.Y, 1, c_rect.Height));
        }

        /// <summary>Draws a single pixel. Very slow, do not use</summary>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        public void DrawPixel(int c_x, int c_y)
        {
            DrawFilledRect(new Rectangle(c_x, c_y, 1, 1));
        }

        /// <summary>Gets pixel color of a specified texture. Slow</summary>
        /// <param name="c_texture">Texture</param>
        /// <param name="c_x">X</param>
        /// <param name="c_y">Y</param>
        /// <returns>Pixel color</returns>
        public Color PixelColor(Texture c_texture, uint c_x, uint c_y)
        {
            return PixelColor(c_texture, c_x, c_y, Color.White);
        }

        /// <summary>Draws a round-corner rectangle</summary>
        /// <param name="c_rect">Target rectangle</param>
        /// <param name="c_slight"></param>
        public void DrawShavedCornerRect(Rectangle c_rect, bool c_slight = false)
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
