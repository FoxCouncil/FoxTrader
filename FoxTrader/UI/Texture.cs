using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace FoxTrader.UI
{
    /// <summary>Represents a texture</summary>
    public class Texture : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="Texture" /> class</summary>
        /// <param name="c_renderer">Renderer to use</param>
        public Texture()
        {
            Width = 4;
            Height = 4;
            Failed = false;
        }

        public Texture(string c_textureName)
        {
            Name = c_textureName;

            Bitmap a_bmp;

            try
            {
                a_bmp = (Bitmap)I18N.GetObject(Name) ?? new Bitmap(Name);
            }
            catch (Exception)
            {
                Failed = true;
                return;
            }

            InitializeFromBitmap(a_bmp);

            a_bmp.Dispose();
        }

        public Texture(Bitmap c_textureBitmap)
        {
            if (c_textureBitmap == null)
            {
                Failed = true;
                return;
            }

            InitializeFromBitmap(c_textureBitmap);
        }

        private void InitializeFromBitmap(Bitmap c_bitmap)
        {
            // TODO: convert to proper format
            System.Drawing.Imaging.PixelFormat a_lockFormat;

            switch (c_bitmap.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                {
                    a_lockFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                }
                break;

                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                {
                    a_lockFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                }
                break;

                default:
                {
                    Failed = true;
                }
                return;
            }

            int a_glTexture;

            // Create the opengl texture
            GL.GenTextures(1, out a_glTexture);
            GL.BindTexture(TextureTarget.Texture2D, a_glTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            RendererData = a_glTexture;
            Width = c_bitmap.Width;
            Height = c_bitmap.Height;

            var a_data = c_bitmap.LockBits(new Rectangle(0, 0, c_bitmap.Width, c_bitmap.Height), ImageLockMode.ReadOnly, a_lockFormat);

            switch (a_lockFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, a_data.Scan0);
                break;
                // invalid
            }

            c_bitmap.UnlockBits(a_data);

            Renderer.LastTextureID = a_glTexture;
        }

        /// <summary>Texture name. Usually file name, but exact meaning depends on renderer</summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>Renderer data</summary>
        public object RendererData
        {
            get;
            set;
        }

        /// <summary>Indicates that the texture failed to load</summary>
        public bool Failed
        {
            get;
            set;
        }

        /// <summary>Texture width</summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>Texture height</summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public void Dispose()
        {
            if (RendererData == null)
            {
                return;
            }

            var a_tex = (int)RendererData;

            if (a_tex == 0)
            {
                return;
            }

            GL.DeleteTextures(1, ref a_tex);
            RendererData = null;

            GC.SuppressFinalize(this);
        }

        /// <summary>Loads the specified texture</summary>
        /// <param name="c_textureName">Texture name</param>
        public void Load(string c_textureName)
        {
            Name = c_textureName;

            Bitmap a_bmp;

            try
            {
                a_bmp = (Bitmap)I18N.GetObject(Name) ?? new Bitmap(Name);
            }
            catch (Exception)
            {
                Failed = true;
                return;
            }

            InitializeFromBitmap(a_bmp);
            a_bmp.Dispose();
        }

        /// <summary>Initializes the texture from raw pixel data</summary>
        /// <param name="c_width">Texture width</param>
        /// <param name="c_height">Texture height</param>
        /// <param name="c_pixelData">Color array in RGBA format</param>
        public void LoadRaw(int c_width, int c_height, byte[] c_pixelData)
        {
            Width = c_width;
            Height = c_height;

            Bitmap a_bmp;

            try
            {
                unsafe
                {
                    fixed (byte* a_ptr = &c_pixelData[0])
                    {
                        a_bmp = new Bitmap(Width, Height, 4 * Width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, (IntPtr)a_ptr);
                    }
                }
            }
            catch (Exception)
            {
                Failed = true;
                return;
            }

            int a_glTex;

            // Create the opengl texture
            GL.GenTextures(1, out a_glTex);
            GL.BindTexture(TextureTarget.Texture2D, a_glTex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            RendererData = a_glTex;

            var a_data = a_bmp.LockBits(new Rectangle(0, 0, a_bmp.Width, a_bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, a_data.Scan0);

            Renderer.LastTextureID = a_glTex;

            a_bmp.UnlockBits(a_data);
            a_bmp.Dispose();
        }

        public void LoadStream(Stream c_data)
        {
            Bitmap a_bmp;
            try
            {
                a_bmp = new Bitmap(c_data);
            }
            catch (Exception)
            {
                Failed = true;
                return;
            }

            InitializeFromBitmap(a_bmp);

            a_bmp.Dispose();
        }
    }
}