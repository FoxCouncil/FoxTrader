using System;
using System.IO;

namespace FoxTrader.UI
{
    /// <summary>Represents a texture</summary>
    public class Texture : IDisposable
    {
        private readonly Renderer m_renderer;

        /// <summary>Initializes a new instance of the <see cref="Texture" /> class</summary>
        /// <param name="c_renderer">Renderer to use</param>
        public Texture(Renderer c_renderer)
        {
            m_renderer = c_renderer;
            Width = 4;
            Height = 4;
            Failed = false;
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
            m_renderer.FreeTexture(this);
            GC.SuppressFinalize(this);
        }

        /// <summary>Loads the specified texture</summary>
        /// <param name="c_textureName">Texture name</param>
        public void Load(string c_textureName)
        {
            Name = c_textureName;
            m_renderer.LoadTexture(this);
        }

        /// <summary>Initializes the texture from raw pixel data</summary>
        /// <param name="c_width">Texture width</param>
        /// <param name="c_height">Texture height</param>
        /// <param name="c_pixelData">Color array in RGBA format</param>
        public void LoadRaw(int c_width, int c_height, byte[] c_pixelData)
        {
            Width = c_width;
            Height = c_height;
            m_renderer.LoadTextureRaw(this, c_pixelData);
        }

        public void LoadStream(Stream c_data)
        {
            m_renderer.LoadTextureStream(this, c_data);
        }
    }
}