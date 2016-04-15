using System;
using System.Drawing;
using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Skin
{
    /// <summary>Simple skin (non-textured). Deprecated and incomplete, do not use</summary>
    [Obsolete]
    internal class SimpleSkin : SkinBase
    {
        private readonly Color m_colBgDark;
        private readonly Color m_colBorderColor;
        private readonly Color m_colControl;
        private readonly Color m_colControlBright;
        private readonly Color m_colControlDark;
        private readonly Color m_colControlDarker;
        private readonly Color m_colControlOutlineLight;
        private readonly Color m_colControlOutlineLighter;
        private readonly Color m_colControlOutlineNormal;
        private readonly Color m_colHighlightBg;
        private readonly Color m_colHighlightBorder;
        private readonly Color m_colModal;
        private readonly Color m_colToolTipBackground;
        private readonly Color m_colToolTipBorder;

        public SimpleSkin(Renderer c_renderer) : base(c_renderer)
        {
            m_colBorderColor = Color.FromArgb(255, 80, 80, 80);
            //m_colBG = Color.FromArgb(255, 248, 248, 248);
            m_colBgDark = Color.FromArgb(255, 235, 235, 235);

            m_colControl = Color.FromArgb(255, 240, 240, 240);
            m_colControlBright = Color.FromArgb(255, 255, 255, 255);
            m_colControlDark = Color.FromArgb(255, 214, 214, 214);
            m_colControlDarker = Color.FromArgb(255, 180, 180, 180);

            m_colControlOutlineNormal = Color.FromArgb(255, 112, 112, 112);
            m_colControlOutlineLight = Color.FromArgb(255, 144, 144, 144);
            m_colControlOutlineLighter = Color.FromArgb(255, 210, 210, 210);

            m_colHighlightBg = Color.FromArgb(255, 192, 221, 252);
            m_colHighlightBorder = Color.FromArgb(255, 51, 153, 255);

            m_colToolTipBackground = Color.FromArgb(255, 255, 255, 225);
            m_colToolTipBorder = Color.FromArgb(255, 0, 0, 0);

            m_colModal = Color.FromArgb(150, 25, 25, 25);
        }

        public override void DrawButton(GameControl c_control, bool c_depressed, bool c_hovered, bool c_disabled)
        {
            var a_w = c_control.Width;
            var a_h = c_control.Height;

            DrawButton(a_w, a_h, c_depressed, c_hovered);
        }

        public override void DrawMenuItem(GameControl c_control, bool c_submenuOpen, bool c_isChecked)
        {
            var a_rect = c_control.RenderBounds;
            if (c_submenuOpen || c_control.IsHovered)
            {
                m_renderer.DrawColor = m_colHighlightBg;
                m_renderer.DrawFilledRect(a_rect);

                m_renderer.DrawColor = m_colHighlightBorder;
                m_renderer.DrawLinedRect(a_rect);
            }

            if (c_isChecked)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 0, 0, 0);

                var a_r = new Rectangle(c_control.Width / 2 - 2, c_control.Height / 2 - 2, 5, 5);
                DrawCheck(a_r);
            }
        }

        public override void DrawMenuStrip(GameControl c_control)
        {
            var a_w = c_control.Width;
            var a_h = c_control.Height;

            m_renderer.DrawColor = Color.FromArgb(255, 246, 248, 252);
            m_renderer.DrawFilledRect(new Rectangle(0, 0, a_w, a_h));

            m_renderer.DrawColor = Color.FromArgb(150, 218, 224, 241);

            m_renderer.DrawFilledRect(Util.FloatRect(0, a_h * 0.4f, a_w, a_h * 0.6f));
            m_renderer.DrawFilledRect(Util.FloatRect(0, a_h * 0.5f, a_w, a_h * 0.5f));
        }

        public override void DrawMenu(GameControl c_control, bool c_paddingDisabled)
        {
            var a_w = c_control.Width;
            var a_h = c_control.Height;

            m_renderer.DrawColor = m_colControlBright;
            m_renderer.DrawFilledRect(new Rectangle(0, 0, a_w, a_h));

            if (!c_paddingDisabled)
            {
                m_renderer.DrawColor = m_colControl;
                m_renderer.DrawFilledRect(new Rectangle(1, 0, 22, a_h));
            }

            m_renderer.DrawColor = m_colControlOutlineNormal;
            m_renderer.DrawLinedRect(new Rectangle(0, 0, a_w, a_h));
        }

        public override void DrawShadow(GameControl c_control)
        {
            var a_w = c_control.Width;
            var a_h = c_control.Height;

            var a_x = 4;
            var a_y = 6;

            m_renderer.DrawColor = Color.FromArgb(10, 0, 0, 0);

            m_renderer.DrawFilledRect(new Rectangle(a_x, a_y, a_w, a_h));
            a_x += 2;
            m_renderer.DrawFilledRect(new Rectangle(a_x, a_y, a_w, a_h));
            a_y += 2;
            m_renderer.DrawFilledRect(new Rectangle(a_x, a_y, a_w, a_h));
        }

        public virtual void DrawButton(int c_w, int c_h, bool c_depressed, bool c_bHovered, bool c_bSquared = false)
        {
            if (c_depressed)
            {
                m_renderer.DrawColor = m_colControlDark;
            }
            else if (c_bHovered)
            {
                m_renderer.DrawColor = m_colControlBright;
            }
            else
            {
                m_renderer.DrawColor = m_colControl;
            }

            m_renderer.DrawFilledRect(new Rectangle(1, 1, c_w - 2, c_h - 2));

            if (c_depressed)
            {
                m_renderer.DrawColor = m_colControlDark;
            }
            else if (c_bHovered)
            {
                m_renderer.DrawColor = m_colControl;
            }
            else
            {
                m_renderer.DrawColor = m_colControlDark;
            }

            m_renderer.DrawFilledRect(Util.FloatRect(1, c_h * 0.5f, c_w - 2, c_h * 0.5f - 2));

            if (!c_depressed)
            {
                m_renderer.DrawColor = m_colControlBright;
            }
            else
            {
                m_renderer.DrawColor = m_colControlDarker;
            }
            m_renderer.DrawShavedCornerRect(new Rectangle(1, 1, c_w - 2, c_h - 2), c_bSquared);

            // Border
            m_renderer.DrawColor = m_colControlOutlineNormal;
            m_renderer.DrawShavedCornerRect(new Rectangle(0, 0, c_w, c_h), c_bSquared);
        }

        public override void DrawRadioButton(GameControl c_control, bool c_selected, bool c_depressed)
        {
            var a_rect = c_control.RenderBounds;

            // Inside colour
            if (c_control.IsHovered)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 220, 242, 254);
            }
            else
            {
                m_renderer.DrawColor = m_colControlBright;
            }

            m_renderer.DrawFilledRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height - 2));

            // Border
            if (c_control.IsHovered)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 85, 130, 164);
            }
            else
            {
                m_renderer.DrawColor = m_colControlOutlineLight;
            }

            m_renderer.DrawShavedCornerRect(a_rect);

            m_renderer.DrawColor = Color.FromArgb(15, 0, 50, 60);
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + 2, a_rect.Width - 4, a_rect.Height - 4));
            m_renderer.DrawFilledRect(Util.FloatRect(a_rect.X + 2, a_rect.Y + 2, a_rect.Width * 0.3f, a_rect.Height - 4));
            m_renderer.DrawFilledRect(Util.FloatRect(a_rect.X + 2, a_rect.Y + 2, a_rect.Width - 4, a_rect.Height * 0.3f));

            if (c_control.IsHovered)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 121, 198, 249);
            }
            else
            {
                m_renderer.DrawColor = Color.FromArgb(50, 0, 50, 60);
            }

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + 3, 1, a_rect.Height - 5));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 3, a_rect.Y + 2, a_rect.Width - 5, 1));

            if (c_selected)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 40, 230, 30);
                m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + 2, a_rect.Width - 4, a_rect.Height - 4));
            }
        }

        public override void DrawCheckBox(GameControl c_control, bool c_selected, bool c_depressed)
        {
            var a_rect = c_control.RenderBounds;

            // Inside colour
            if (c_control.IsHovered)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 220, 242, 254);
            }
            else
            {
                m_renderer.DrawColor = m_colControlBright;
            }

            m_renderer.DrawFilledRect(a_rect);

            // Border
            if (c_control.IsHovered)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 85, 130, 164);
            }
            else
            {
                m_renderer.DrawColor = m_colControlOutlineLight;
            }

            m_renderer.DrawLinedRect(a_rect);

            m_renderer.DrawColor = Color.FromArgb(15, 0, 50, 60);
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + 2, a_rect.Width - 4, a_rect.Height - 4));
            m_renderer.DrawFilledRect(Util.FloatRect(a_rect.X + 2, a_rect.Y + 2, a_rect.Width * 0.3f, a_rect.Height - 4));
            m_renderer.DrawFilledRect(Util.FloatRect(a_rect.X + 2, a_rect.Y + 2, a_rect.Width - 4, a_rect.Height * 0.3f));

            if (c_control.IsHovered)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 121, 198, 249);
            }
            else
            {
                m_renderer.DrawColor = Color.FromArgb(50, 0, 50, 60);
            }

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + 2, 1, a_rect.Height - 4));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + 2, a_rect.Width - 4, 1));

            if (c_depressed)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 100, 100, 100);
                var a_r = new Rectangle(c_control.Width / 2 - 2, c_control.Height / 2 - 2, 5, 5);
                DrawCheck(a_r);
            }
            else if (c_selected)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 0, 0, 0);
                var a_r = new Rectangle(c_control.Width / 2 - 2, c_control.Height / 2 - 2, 5, 5);
                DrawCheck(a_r);
            }
        }

        public override void DrawGroupBox(GameControl c_control, int c_textStart, int c_textHeight, int c_textWidth)
        {
            var a_rect = c_control.RenderBounds;

            a_rect.Y += (int)(c_textHeight * 0.5f);
            a_rect.Height -= (int)(c_textHeight * 0.5f);

            var a_colDarker = Color.FromArgb(50, 0, 50, 60);
            var a_colLighter = Color.FromArgb(150, 255, 255, 255);

            m_renderer.DrawColor = a_colLighter;

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + 1, c_textStart - 3, 1));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1 + c_textStart + c_textWidth, a_rect.Y + 1, a_rect.Width - c_textStart + c_textWidth - 2, 1));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, (a_rect.Y + a_rect.Height) - 1, a_rect.Width - 2, 1));

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + 1, 1, a_rect.Height));
            m_renderer.DrawFilledRect(new Rectangle((a_rect.X + a_rect.Width) - 2, a_rect.Y + 1, 1, a_rect.Height - 1));

            m_renderer.DrawColor = a_colDarker;

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y, c_textStart - 3, 1));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1 + c_textStart + c_textWidth, a_rect.Y, a_rect.Width - c_textStart - c_textWidth - 2, 1));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, (a_rect.Y + a_rect.Height) - 1, a_rect.Width - 2, 1));

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X, a_rect.Y + 1, 1, a_rect.Height - 1));
            m_renderer.DrawFilledRect(new Rectangle((a_rect.X + a_rect.Width) - 1, a_rect.Y + 1, 1, a_rect.Height - 1));
        }

        public override void DrawTextBox(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;
            var a_bHasFocus = c_control.HasFocus;

            // Box inside
            m_renderer.DrawColor = Color.FromArgb(255, 255, 255, 255);
            m_renderer.DrawFilledRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height - 2));

            m_renderer.DrawColor = m_colControlOutlineLight;
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y, a_rect.Width - 2, 1));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X, a_rect.Y + 1, 1, a_rect.Height - 2));

            m_renderer.DrawColor = m_colControlOutlineLighter;
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, (a_rect.Y + a_rect.Height) - 1, a_rect.Width - 2, 1));
            m_renderer.DrawFilledRect(new Rectangle((a_rect.X + a_rect.Width) - 1, a_rect.Y + 1, 1, a_rect.Height - 2));

            if (a_bHasFocus)
            {
                m_renderer.DrawColor = Color.FromArgb(150, 50, 200, 255);
                m_renderer.DrawLinedRect(a_rect);
            }
        }

        public override void DrawTabButton(GameControl c_control, bool c_active, Pos c_dir)
        {
            var a_rect = c_control.RenderBounds;
            var a_bHovered = c_control.IsHovered;

            if (a_bHovered)
            {
                m_renderer.DrawColor = m_colControlBright;
            }
            else
            {
                m_renderer.DrawColor = m_colControl;
            }

            m_renderer.DrawFilledRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height - 1));

            if (a_bHovered)
            {
                m_renderer.DrawColor = m_colControl;
            }
            else
            {
                m_renderer.DrawColor = m_colControlDark;
            }

            m_renderer.DrawFilledRect(Util.FloatRect(1, a_rect.Height * 0.5f, a_rect.Width - 2, a_rect.Height * 0.5f - 1));

            m_renderer.DrawColor = m_colControlBright;
            m_renderer.DrawShavedCornerRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height));

            m_renderer.DrawColor = m_colBorderColor;

            m_renderer.DrawShavedCornerRect(new Rectangle(0, 0, a_rect.Width, a_rect.Height));
        }

        public override void DrawTabControl(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;

            m_renderer.DrawColor = m_colControl;
            m_renderer.DrawFilledRect(a_rect);

            m_renderer.DrawColor = m_colBorderColor;
            m_renderer.DrawLinedRect(a_rect);

            //m_Renderer.DrawColor = m_colControl;
            //m_Renderer.DrawFilledRect(CurrentButtonRect);
        }

        public override void DrawWindow(GameControl c_control, int c_topHeight, bool c_inFocus)
        {
            var a_rect = c_control.RenderBounds;

            // Titlebar
            if (c_inFocus)
            {
                m_renderer.DrawColor = Color.FromArgb(230, 87, 164, 232);
            }
            else
            {
                m_renderer.DrawColor = Color.FromArgb(230, (int)(87 * 0.70f), (int)(164 * 0.70f), (int)(232 * 0.70f));
            }

            var a_iBorderSize = 5;
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + 1, a_rect.Width - 2, c_topHeight - 1));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + c_topHeight - 1, a_iBorderSize, a_rect.Height - 2 - c_topHeight));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + a_rect.Width - a_iBorderSize, a_rect.Y + c_topHeight - 1, a_iBorderSize, a_rect.Height - 2 - c_topHeight));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + a_rect.Height - a_iBorderSize, a_rect.Width - 2, a_iBorderSize));

            // Main inner
            m_renderer.DrawColor = Color.FromArgb(230, m_colControlDark.R, m_colControlDark.G, m_colControlDark.B);
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + a_iBorderSize + 1, a_rect.Y + c_topHeight, a_rect.Width - a_iBorderSize * 2 - 2, a_rect.Height - c_topHeight - a_iBorderSize - 1));

            // Light inner border
            m_renderer.DrawColor = Color.FromArgb(100, 255, 255, 255);
            m_renderer.DrawShavedCornerRect(new Rectangle(a_rect.X + 1, a_rect.Y + 1, a_rect.Width - 2, a_rect.Height - 2));

            // Dark line between titlebar and main
            m_renderer.DrawColor = m_colBorderColor;

            // Inside border
            m_renderer.DrawColor = m_colControlOutlineNormal;
            m_renderer.DrawLinedRect(new Rectangle(a_rect.X + a_iBorderSize, a_rect.Y + c_topHeight - 1, a_rect.Width - 10, a_rect.Height - c_topHeight - (a_iBorderSize - 1)));

            // Dark outer border
            m_renderer.DrawColor = m_colBorderColor;
            m_renderer.DrawShavedCornerRect(new Rectangle(a_rect.X, a_rect.Y, a_rect.Width, a_rect.Height));
        }

        public override void DrawWindowCloseButton(GameControl c_control, bool c_depressed, bool c_hovered, bool c_disabled)
        {
            // TODO
            DrawButton(c_control, c_depressed, c_hovered, c_disabled);
        }

        public override void DrawHighlight(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;
            m_renderer.DrawColor = Color.FromArgb(255, 255, 100, 255);
            m_renderer.DrawFilledRect(a_rect);
        }

        public override void DrawScrollBar(GameControl c_control, bool c_horizontal, bool c_depressed)
        {
            var a_rect = c_control.RenderBounds;
            if (c_depressed)
            {
                m_renderer.DrawColor = m_colControlDarker;
            }
            else
            {
                m_renderer.DrawColor = m_colControlBright;
            }
            m_renderer.DrawFilledRect(a_rect);
        }

        public override void DrawScrollBarBar(GameControl c_control, bool c_depressed, bool c_hovered, bool c_horizontal)
        {
            // TODO: something specialized
            DrawButton(c_control, c_depressed, c_hovered, false);
        }

        public override void DrawTabTitleBar(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;

            m_renderer.DrawColor = Color.FromArgb(255, 177, 193, 214);
            m_renderer.DrawFilledRect(a_rect);

            m_renderer.DrawColor = m_colBorderColor;
            a_rect.Height += 1;
            m_renderer.DrawLinedRect(a_rect);
        }

        public override void DrawProgressBar(GameControl c_control, bool c_horizontal, float c_progress)
        {
            var a_rect = c_control.RenderBounds;
            var a_fillColour = Color.FromArgb(255, 0, 211, 40);

            if (c_horizontal)
            {
                //Background
                m_renderer.DrawColor = m_colControlDark;
                m_renderer.DrawFilledRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height - 2));

                //Right half
                m_renderer.DrawColor = a_fillColour;
                m_renderer.DrawFilledRect(Util.FloatRect(1, 1, a_rect.Width * c_progress - 2, a_rect.Height - 2));

                m_renderer.DrawColor = Color.FromArgb(150, 255, 255, 255);
                m_renderer.DrawFilledRect(Util.FloatRect(1, 1, a_rect.Width - 2, a_rect.Height * 0.45f));
            }
            else
            {
                //Background 
                m_renderer.DrawColor = m_colControlDark;
                m_renderer.DrawFilledRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height - 2));

                //Top half
                m_renderer.DrawColor = a_fillColour;
                m_renderer.DrawFilledRect(Util.FloatRect(1, 1 + (a_rect.Height * (1 - c_progress)), a_rect.Width - 2, a_rect.Height * c_progress - 2));

                m_renderer.DrawColor = Color.FromArgb(150, 255, 255, 255);
                m_renderer.DrawFilledRect(Util.FloatRect(1, 1, a_rect.Width * 0.45f, a_rect.Height - 2));
            }

            m_renderer.DrawColor = Color.FromArgb(150, 255, 255, 255);
            m_renderer.DrawShavedCornerRect(new Rectangle(1, 1, a_rect.Width - 2, a_rect.Height - 2));

            m_renderer.DrawColor = Color.FromArgb(70, 255, 255, 255);
            m_renderer.DrawShavedCornerRect(new Rectangle(2, 2, a_rect.Width - 4, a_rect.Height - 4));

            m_renderer.DrawColor = m_colBorderColor;
            m_renderer.DrawShavedCornerRect(a_rect);
        }

        public override void DrawListBox(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;

            m_renderer.DrawColor = m_colControlBright;
            m_renderer.DrawFilledRect(a_rect);

            m_renderer.DrawColor = m_colBorderColor;
            m_renderer.DrawLinedRect(a_rect);
        }

        public override void DrawListBoxLine(GameControl c_control, bool c_selected, bool c_even)
        {
            var a_rect = c_control.RenderBounds;

            if (c_selected)
            {
                m_renderer.DrawColor = m_colHighlightBorder;
                m_renderer.DrawFilledRect(a_rect);
            }
            else if (c_control.IsHovered)
            {
                m_renderer.DrawColor = m_colHighlightBg;
                m_renderer.DrawFilledRect(a_rect);
            }
        }

        public override void DrawSlider(GameControl c_control, bool c_horizontal, int c_numNotches, int c_barSize)
        {
            var a_rect = c_control.RenderBounds;
            var a_notchRect = a_rect;

            if (c_horizontal)
            {
                a_rect.Y += (int)(a_rect.Height * 0.4f);
                a_rect.Height -= (int)(a_rect.Height * 0.8f);
            }
            else
            {
                a_rect.X += (int)(a_rect.Width * 0.4f);
                a_rect.Width -= (int)(a_rect.Width * 0.8f);
            }

            m_renderer.DrawColor = m_colBgDark;
            m_renderer.DrawFilledRect(a_rect);

            m_renderer.DrawColor = m_colControlDarker;
            m_renderer.DrawLinedRect(a_rect);
        }

        public override void DrawComboBox(GameControl c_control, bool c_down, bool c_open)
        {
            DrawTextBox(c_control);
        }

        public override void DrawKeyboardHighlight(GameControl c_control, Rectangle c_r, int c_offset)
        {
            var a_rect = c_r;

            a_rect.X += c_offset;
            a_rect.Y += c_offset;
            a_rect.Width -= c_offset * 2;
            a_rect.Height -= c_offset * 2;

            //draw the top and bottom
            var a_skip = true;
            for (var a_i = 0; a_i < a_rect.Width * 0.5; a_i++)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 0, 0, 0);
                if (!a_skip)
                {
                    m_renderer.DrawPixel(a_rect.X + (a_i * 2), a_rect.Y);
                    m_renderer.DrawPixel(a_rect.X + (a_i * 2), a_rect.Y + a_rect.Height - 1);
                }
                else
                {
                    a_skip = false;
                }
            }

            for (var a_i = 0; a_i < a_rect.Height * 0.5; a_i++)
            {
                m_renderer.DrawColor = Color.FromArgb(255, 0, 0, 0);
                m_renderer.DrawPixel(a_rect.X, a_rect.Y + a_i * 2);
                m_renderer.DrawPixel(a_rect.X + a_rect.Width - 1, a_rect.Y + a_i * 2);
            }
        }

        public override void DrawToolTip(GameControl c_control)
        {
            var a_rct = c_control.RenderBounds;
            a_rct.X -= 3;
            a_rct.Y -= 3;
            a_rct.Width += 6;
            a_rct.Height += 6;

            m_renderer.DrawColor = m_colToolTipBackground;
            m_renderer.DrawFilledRect(a_rct);

            m_renderer.DrawColor = m_colToolTipBorder;
            m_renderer.DrawLinedRect(a_rct);
        }

        public override void DrawScrollButton(GameControl c_control, Pos c_direction, bool c_depressed, bool c_hovered, bool c_disabled)
        {
            DrawButton(c_control, c_depressed, false, false);

            m_renderer.DrawColor = Color.FromArgb(240, 0, 0, 0);

            var a_r = new Rectangle(c_control.Width / 2 - 2, c_control.Height / 2 - 2, 5, 5);

            if (c_direction == Pos.Top)
            {
                DrawArrowUp(a_r);
            }
            else if (c_direction == Pos.Bottom)
            {
                DrawArrowDown(a_r);
            }
            else if (c_direction == Pos.Left)
            {
                DrawArrowLeft(a_r);
            }
            else
            {
                DrawArrowRight(a_r);
            }
        }

        public override void DrawComboBoxArrow(GameControl c_control, bool c_hovered, bool c_down, bool c_open, bool c_disabled)
        {
            //DrawButton( control.Width, control.Height, depressed, false, true );

            m_renderer.DrawColor = Color.FromArgb(240, 0, 0, 0);

            var a_r = new Rectangle(c_control.Width / 2 - 2, c_control.Height / 2 - 2, 5, 5);
            DrawArrowDown(a_r);
        }

        public override void DrawNumericUpDownButton(GameControl c_control, bool c_depressed, bool c_up)
        {
            //DrawButton( control.Width, control.Height, depressed, false, true );

            m_renderer.DrawColor = Color.FromArgb(240, 0, 0, 0);

            var a_r = new Rectangle(c_control.Width / 2 - 2, c_control.Height / 2 - 2, 5, 5);

            if (c_up)
            {
                DrawArrowUp(a_r);
            }
            else
            {
                DrawArrowDown(a_r);
            }
        }

        public override void DrawTreeButton(GameControl c_control, bool c_open)
        {
            var a_rect = c_control.RenderBounds;
            a_rect.X += 2;
            a_rect.Y += 2;
            a_rect.Width -= 4;
            a_rect.Height -= 4;

            m_renderer.DrawColor = m_colControlBright;
            m_renderer.DrawFilledRect(a_rect);

            m_renderer.DrawColor = m_colBorderColor;
            m_renderer.DrawLinedRect(a_rect);

            m_renderer.DrawColor = m_colBorderColor;

            if (!c_open) // ! because the button shows intention, not the current state
            {
                m_renderer.DrawFilledRect(new Rectangle(a_rect.X + a_rect.Width / 2, a_rect.Y + 2, 1, a_rect.Height - 4));
            }

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + 2, a_rect.Y + a_rect.Height / 2, a_rect.Width - 4, 1));
        }

        public override void DrawTreeControl(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;

            m_renderer.DrawColor = m_colControlBright;
            m_renderer.DrawFilledRect(a_rect);

            m_renderer.DrawColor = m_colBorderColor;
            m_renderer.DrawLinedRect(a_rect);
        }

        public override void DrawTreeNode(GameControl c_ctrl, bool c_open, bool c_selected, int c_labelHeight, int c_labelWidth, int c_halfWay, int c_lastBranch, bool c_isRoot)
        {
            if (c_selected)
            {
                Renderer.DrawColor = Color.FromArgb(100, 0, 150, 255);
                Renderer.DrawFilledRect(new Rectangle(17, 0, c_labelWidth + 2, c_labelHeight - 1));
                Renderer.DrawColor = Color.FromArgb(200, 0, 150, 255);
                Renderer.DrawLinedRect(new Rectangle(17, 0, c_labelWidth + 2, c_labelHeight - 1));
            }

            base.DrawTreeNode(c_ctrl, c_open, c_selected, c_labelHeight, c_labelWidth, c_halfWay, c_lastBranch, c_isRoot);
        }

        public override void DrawStatusBar(GameControl c_control)
        {
            // TODO
        }

        public override void DrawColorDisplay(GameControl c_control, Color c_color)
        {
            var a_rect = c_control.RenderBounds;

            if (c_color.A != 255)
            {
                Renderer.DrawColor = Color.FromArgb(255, 255, 255, 255);
                Renderer.DrawFilledRect(a_rect);

                Renderer.DrawColor = Color.FromArgb(128, 128, 128, 128);

                Renderer.DrawFilledRect(Util.FloatRect(0, 0, a_rect.Width * 0.5f, a_rect.Height * 0.5f));
                Renderer.DrawFilledRect(Util.FloatRect(a_rect.Width * 0.5f, a_rect.Height * 0.5f, a_rect.Width * 0.5f, a_rect.Height * 0.5f));
            }

            Renderer.DrawColor = c_color;
            Renderer.DrawFilledRect(a_rect);

            Renderer.DrawColor = Color.FromArgb(255, 0, 0, 0);
            Renderer.DrawLinedRect(a_rect);
        }

        public override void DrawModalControl(GameControl c_control)
        {
            if (c_control.ShouldDrawBackground)
            {
                var a_rect = c_control.RenderBounds;
                Renderer.DrawColor = m_colModal;
                Renderer.DrawFilledRect(a_rect);
            }
        }

        public override void DrawMenuDivider(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;
            Renderer.DrawColor = m_colBgDark;
            Renderer.DrawFilledRect(a_rect);
            Renderer.DrawColor = m_colControlDarker;
            Renderer.DrawLinedRect(a_rect);
        }

        public override void DrawMenuRightArrow(GameControl c_control)
        {
            DrawArrowRight(c_control.RenderBounds);
        }

        public override void DrawSliderButton(GameControl c_control, bool c_depressed, bool c_horizontal)
        {
            DrawButton(c_control, c_depressed, c_control.IsHovered, c_control.IsDisabled);
        }

        public override void DrawCategoryHolder(GameControl c_control)
        {
            // TODO
        }

        public override void DrawCategoryInner(GameControl c_control, bool c_collapsed)
        {
            // TODO
        }
    }
}