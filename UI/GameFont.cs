using System;
using FoxTrader.UI.Renderer;

namespace FoxTrader.UI
{
    /// <summary>Represents font resource</summary>
    public class GameFont : IDisposable
    {
        private readonly RendererBase m_renderer;

        /// <summary>Initializes a new instance of the <see cref="GameFont" /> class</summary>
        public GameFont(RendererBase c_renderer) : this(c_renderer, Constants.kNormalUIFontName, Constants.kNormalUIFontSize)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GameFont" /> class</summary>
        /// <param name="c_renderer">Renderer to use</param>
        /// <param name="c_faceName">Face name</param>
        /// <param name="c_fontSize">Font size</param>
        public GameFont(RendererBase c_renderer, string c_faceName, int c_fontSize = Constants.kNormalUIFontSize)
        {
            m_renderer = c_renderer;
            FaceName = c_faceName;
            Size = c_fontSize;
            Smooth = false;
        }

        /// <summary>Renderer dependent font name</summary>
        public string FaceName
        {
            get;
            set;
        }

        /// <summary>Font size</summary>
        public int Size
        {
            get;
            set;
        }

        /// <summary> Enables or disables font smoothing (default: disabled)</summary>
        public bool Smooth
        {
            get;
            set;
        }

        /// <summary>This should be set by the renderer if it tries to use a font where it's null</summary>
        public object RendererData
        {
            get;
            set;
        }

        /// <summary>This is the real font size, after it's been scaled by Renderer.Scale()</summary>
        public float RealSize
        {
            get;
            set;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public void Dispose()
        {
            m_renderer.FreeFont(this);
            GC.SuppressFinalize(this);
        }

        /// <summary>Duplicates font data (except renderer data which must be reinitialized)</summary>
        /// <returns>The duplicated font</returns>
        public GameFont Copy()
        {
            var a_newFont = new GameFont(m_renderer, FaceName);
            a_newFont.Size = Size;
            a_newFont.RealSize = RealSize;
            a_newFont.RendererData = null;
            return a_newFont;
        }
    }
}