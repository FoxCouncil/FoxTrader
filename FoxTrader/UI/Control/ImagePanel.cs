using System.Drawing;

namespace FoxTrader.UI.Control
{
    /// <summary>Image container</summary>
    internal class ImagePanel : GameControl
    {
        private readonly Texture m_texture;
        private readonly float[] m_uv;
        private readonly Color m_drawColor;

        /// <summary>Initializes a new instance of the <see cref="ImagePanel" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ImagePanel(GameControl c_parentControl) : base(c_parentControl)
        {
            m_uv = new float[4];
            m_texture = new Texture(Skin.Renderer);
            SetUV(0, 0, 1, 1);
            MouseInputEnabled = false;
            m_drawColor = Color.White;
        }

        /// <summary>Texture name</summary>
        public string ImageName
        {
            get
            {
                return m_texture.Name;
            }
            set
            {
                m_texture.Load(value);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public override void Dispose()
        {
            m_texture.Dispose();
            base.Dispose();
        }

        /// <summary>Sets the texture coordinates of the image</summary>
        public virtual void SetUV(float c_u1, float c_v1, float c_u2, float c_v2)
        {
            m_uv[0] = c_u1;
            m_uv[1] = c_v1;
            m_uv[2] = c_u2;
            m_uv[3] = c_v2;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            base.Render(c_skin);
            c_skin.Renderer.DrawColor = m_drawColor;
            c_skin.Renderer.DrawTexturedRect(m_texture, RenderBounds, m_uv[0], m_uv[1], m_uv[2], m_uv[3]);
        }

        /// <summary>Sizes the control to its contents</summary>
        public virtual void SizeToContents()
        {
            SetSize(m_texture.Width, m_texture.Height);
        }
    }
}