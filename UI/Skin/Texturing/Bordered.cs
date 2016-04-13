using System.Drawing;
using FoxTrader.UI.Renderer;

namespace FoxTrader.UI.Skin.Texturing
{
    public struct SubRect
    {
        public float[] m_uv;
    }

    public struct Bordered
    {
        private Texture m_texture;

        private readonly SubRect[] m_rects;

        private Margin m_margin;

        private float m_width;
        private float m_height;

        public Bordered(Texture c_texture, float c_x, float c_y, float c_w, float c_h, Margin c_inMargin, float c_drawMarginScale = 1.0f) : this()
        {
            m_rects = new SubRect[9];

            for (var a_i = 0; a_i < m_rects.Length; a_i++)
            {
                m_rects[a_i].m_uv = new float[4];
            }

            Init(c_texture, c_x, c_y, c_w, c_h, c_inMargin, c_drawMarginScale);
        }

        private void DrawRect(RendererBase c_render, int c_i, int c_x, int c_y, int c_w, int c_h)
        {
            c_render.DrawTexturedRect(m_texture, new Rectangle(c_x, c_y, c_w, c_h), m_rects[c_i].m_uv[0], m_rects[c_i].m_uv[1], m_rects[c_i].m_uv[2], m_rects[c_i].m_uv[3]);
        }

        private void SetRect(int c_num, float c_x, float c_y, float c_w, float c_h)
        {
            float a_texw = m_texture.Width;
            float a_texh = m_texture.Height;

            m_rects[c_num].m_uv[0] = c_x / a_texw;
            m_rects[c_num].m_uv[1] = c_y / a_texh;

            m_rects[c_num].m_uv[2] = (c_x + c_w) / a_texw;
            m_rects[c_num].m_uv[3] = (c_y + c_h) / a_texh;
        }

        private void Init(Texture c_texture, float c_x, float c_y, float c_w, float c_h, Margin c_inMargin, float c_drawMarginScale = 1.0f)
        {
            m_texture = c_texture;

            m_margin = c_inMargin;

            SetRect(0, c_x, c_y, m_margin.m_left, m_margin.m_top);
            SetRect(1, c_x + m_margin.m_left, c_y, c_w - m_margin.m_left - m_margin.m_right, m_margin.m_top);
            SetRect(2, (c_x + c_w) - m_margin.m_right, c_y, m_margin.m_right, m_margin.m_top);

            SetRect(3, c_x, c_y + m_margin.m_top, m_margin.m_left, c_h - m_margin.m_top - m_margin.m_bottom);
            SetRect(4, c_x + m_margin.m_left, c_y + m_margin.m_top, c_w - m_margin.m_left - m_margin.m_right, c_h - m_margin.m_top - m_margin.m_bottom);
            SetRect(5, (c_x + c_w) - m_margin.m_right, c_y + m_margin.m_top, m_margin.m_right, c_h - m_margin.m_top - m_margin.m_bottom - 1);

            SetRect(6, c_x, (c_y + c_h) - m_margin.m_bottom, m_margin.m_left, m_margin.m_bottom);
            SetRect(7, c_x + m_margin.m_left, (c_y + c_h) - m_margin.m_bottom, c_w - m_margin.m_left - m_margin.m_right, m_margin.m_bottom);
            SetRect(8, (c_x + c_w) - m_margin.m_right, (c_y + c_h) - m_margin.m_bottom, m_margin.m_right, m_margin.m_bottom);

            m_margin.m_left = (int)(m_margin.m_left * c_drawMarginScale);
            m_margin.m_right = (int)(m_margin.m_right * c_drawMarginScale);
            m_margin.m_top = (int)(m_margin.m_top * c_drawMarginScale);
            m_margin.m_bottom = (int)(m_margin.m_bottom * c_drawMarginScale);

            m_width = c_w - c_x;
            m_height = c_h - c_y;
        }

        // can't have this as default param
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

            if (c_r.Width < m_width && c_r.Height < m_height)
            {
                c_render.DrawTexturedRect(m_texture, c_r, m_rects[0].m_uv[0], m_rects[0].m_uv[1], m_rects[8].m_uv[2], m_rects[8].m_uv[3]);
                return;
            }

            DrawRect(c_render, 0, c_r.X, c_r.Y, m_margin.m_left, m_margin.m_top);
            DrawRect(c_render, 1, c_r.X + m_margin.m_left, c_r.Y, c_r.Width - m_margin.m_left - m_margin.m_right, m_margin.m_top);
            DrawRect(c_render, 2, (c_r.X + c_r.Width) - m_margin.m_right, c_r.Y, m_margin.m_right, m_margin.m_top);

            DrawRect(c_render, 3, c_r.X, c_r.Y + m_margin.m_top, m_margin.m_left, c_r.Height - m_margin.m_top - m_margin.m_bottom);
            DrawRect(c_render, 4, c_r.X + m_margin.m_left, c_r.Y + m_margin.m_top, c_r.Width - m_margin.m_left - m_margin.m_right, c_r.Height - m_margin.m_top - m_margin.m_bottom);
            DrawRect(c_render, 5, (c_r.X + c_r.Width) - m_margin.m_right, c_r.Y + m_margin.m_top, m_margin.m_right, c_r.Height - m_margin.m_top - m_margin.m_bottom);

            DrawRect(c_render, 6, c_r.X, (c_r.Y + c_r.Height) - m_margin.m_bottom, m_margin.m_left, m_margin.m_bottom);
            DrawRect(c_render, 7, c_r.X + m_margin.m_left, (c_r.Y + c_r.Height) - m_margin.m_bottom, c_r.Width - m_margin.m_left - m_margin.m_right, m_margin.m_bottom);
            DrawRect(c_render, 8, (c_r.X + c_r.Width) - m_margin.m_right, (c_r.Y + c_r.Height) - m_margin.m_bottom, m_margin.m_right, m_margin.m_bottom);
        }
    }
}