using System.Drawing;
using FoxTrader.UI.Renderer;

namespace FoxTrader.UI.Skin.Texturing
{
    public struct Single
    {
        private readonly Texture m_texture;
        private readonly float[] m_uv;
        private readonly int m_width;
        private readonly int m_height;

        public Single(Texture c_texture, float c_x, float c_y, float c_w, float c_h)
        {
            m_texture = c_texture;

            float a_texw = m_texture.Width;
            float a_texh = m_texture.Height;

            m_uv = new float[4];
            m_uv[0] = c_x / a_texw;
            m_uv[1] = c_y / a_texh;
            m_uv[2] = (c_x + c_w) / a_texw;
            m_uv[3] = (c_y + c_h) / a_texh;

            m_width = (int)c_w;
            m_height = (int)c_h;
        }

        public void Draw(RendererBase c_render, Rectangle c_r)
        {
            Draw(c_render, c_r, Color.White);
        }

        public void Draw(RendererBase c_render, Rectangle c_r, Color c_col)
        {
            if (m_texture == null)
            {
                return;
            }

            c_render.DrawColor = c_col;
            c_render.DrawTexturedRect(m_texture, c_r, m_uv[0], m_uv[1], m_uv[2], m_uv[3]);
        }

        public void DrawCenter(RendererBase c_render, Rectangle c_r)
        {
            if (m_texture == null)
            {
                return;
            }

            DrawCenter(c_render, c_r, Color.White);
        }

        public void DrawCenter(RendererBase c_render, Rectangle c_r, Color c_col)
        {
            c_r.X += (int)((c_r.Width - m_width) * 0.5);
            c_r.Y += (int)((c_r.Height - m_height) * 0.5);
            c_r.Width = m_width;
            c_r.Height = m_height;

            Draw(c_render, c_r, c_col);
        }
    }
}