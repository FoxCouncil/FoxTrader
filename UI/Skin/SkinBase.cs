using System;
using System.Drawing;
using FoxTrader.UI.Control;
using FoxTrader.UI.Renderer;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Skin
{
    /// <summary>Base skin</summary>
    internal class SkinBase : IDisposable
    {
        protected readonly RendererBase m_renderer;

        /// <summary>Colors of various UI elements</summary>
        public SkinColors m_colors;

        protected GameFont m_defaultFont;

        /// <summary>Initializes a new instance of the <see cref="SkinBase" /> class</summary>
        /// <param name="c_renderer">Renderer to use</param>
        protected SkinBase(RendererBase c_renderer)
        {
            m_defaultFont = new GameFont(c_renderer);
            m_renderer = c_renderer;
        }

        /// <summary>Default font to use when rendering text if none specified</summary>
        public GameFont DefaultFont
        {
            get
            {
                return m_defaultFont;
            }
            set
            {
                m_defaultFont.Dispose();
                m_defaultFont = value;
            }
        }

        /// <summary>Renderer used</summary>
        public RendererBase Renderer => m_renderer;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public virtual void Dispose()
        {
            m_defaultFont.Dispose();
            GC.SuppressFinalize(this);
        }

#if DEBUG
        ~SkinBase()
        {
            throw new InvalidOperationException($"IDisposable object finalized: {GetType()}");
            //Debug.Print(String.Format("IDisposable object finalized: {0}", GetType()));
        }
#endif

        /// <summary>Releases the specified font</summary>
        /// <param name="c_font">Font to release</param>
        protected virtual void ReleaseFont(GameFont c_font)
        {
            if (c_font == null)
            {
                return;
            }
            if (m_renderer == null)
            {
                return;
            }
            m_renderer.FreeFont(c_font);
        }

        /// <summary>Sets the default text font</summary>
        /// <param name="c_faceName">Font name. Meaning can vary depending on the renderer</param>
        /// <param name="c_size">Font size</param>
        public virtual void SetDefaultFont(string c_faceName, int c_size = 10)
        {
            m_defaultFont.FaceName = c_faceName;
            m_defaultFont.Size = c_size;
        }

        public virtual void DrawButton(GameControl c_control, bool c_depressed, bool c_hovered, bool c_disabled)
        {
        }

        public virtual void DrawTabButton(GameControl c_control, bool c_active, Pos c_dir)
        {
        }

        public virtual void DrawTabControl(GameControl c_control)
        {
        }

        public virtual void DrawTabTitleBar(GameControl c_control)
        {
        }

        public virtual void DrawMenuItem(GameControl c_control, bool c_submenuOpen, bool c_isChecked)
        {
        }

        public virtual void DrawMenuRightArrow(GameControl c_control)
        {
        }

        public virtual void DrawMenuStrip(GameControl c_control)
        {
        }

        public virtual void DrawMenu(GameControl c_control, bool c_paddingDisabled)
        {
        }

        public virtual void DrawRadioButton(GameControl c_control, bool c_selected, bool c_depressed)
        {
        }

        public virtual void DrawCheckBox(GameControl c_control, bool c_selected, bool c_depressed)
        {
        }

        public virtual void DrawGroupBox(GameControl c_control, int c_textStart, int c_textHeight, int c_textWidth)
        {
        }

        public virtual void DrawTextBox(GameControl c_control)
        {
        }

        public virtual void DrawWindow(GameControl c_control, int c_topHeight, bool c_inFocus)
        {
        }

        public virtual void DrawWindowCloseButton(GameControl c_control, bool c_depressed, bool c_hovered, bool c_disabled)
        {
        }

        public virtual void DrawHighlight(GameControl c_control)
        {
        }

        public virtual void DrawStatusBar(GameControl c_control)
        {
        }

        public virtual void DrawShadow(GameControl c_control)
        {
        }

        public virtual void DrawScrollBarBar(GameControl c_control, bool c_depressed, bool c_hovered, bool c_horizontal)
        {
        }

        public virtual void DrawScrollBar(GameControl c_control, bool c_horizontal, bool c_depressed)
        {
        }

        public virtual void DrawScrollButton(GameControl c_control, Pos c_direction, bool c_depressed, bool c_hovered, bool c_disabled)
        {
        }

        public virtual void DrawProgressBar(GameControl c_control, bool c_horizontal, float c_progress)
        {
        }

        public virtual void DrawListBox(GameControl c_control)
        {
        }

        public virtual void DrawListBoxLine(GameControl c_control, bool c_selected, bool c_even)
        {
        }

        public virtual void DrawSlider(GameControl c_control, bool c_horizontal, int c_numNotches, int c_barSize)
        {
        }

        public virtual void DrawSliderButton(GameControl c_control, bool c_depressed, bool c_horizontal)
        {
        }

        public virtual void DrawComboBox(GameControl c_control, bool c_down, bool c_open)
        {
        }

        public virtual void DrawComboBoxArrow(GameControl c_control, bool c_hovered, bool c_down, bool c_open, bool c_disabled)
        {
        }

        public virtual void DrawKeyboardHighlight(GameControl c_control, Rectangle c_r, int c_offset)
        {
        }

        public virtual void DrawToolTip(GameControl c_control)
        {
        }

        public virtual void DrawNumericUpDownButton(GameControl c_control, bool c_depressed, bool c_up)
        {
        }

        public virtual void DrawTreeButton(GameControl c_control, bool c_open)
        {
        }

        public virtual void DrawTreeControl(GameControl c_control)
        {
        }

        public virtual void DrawDebugOutlines(GameControl c_control)
        {
            m_renderer.DrawColor = c_control.PaddingOutlineColor;
            var a_inner = new Rectangle(c_control.Bounds.Left + c_control.Padding.m_left, c_control.Bounds.Top + c_control.Padding.m_top, c_control.Bounds.Width - c_control.Padding.m_right - c_control.Padding.m_left, c_control.Bounds.Height - c_control.Padding.m_bottom - c_control.Padding.m_top);
            m_renderer.DrawLinedRect(a_inner);

            m_renderer.DrawColor = c_control.MarginOutlineColor;
            var a_outer = new Rectangle(c_control.Bounds.Left - c_control.Margin.m_left, c_control.Bounds.Top - c_control.Margin.m_top, c_control.Bounds.Width + c_control.Margin.m_right + c_control.Margin.m_left, c_control.Bounds.Height + c_control.Margin.m_bottom + c_control.Margin.m_top);
            m_renderer.DrawLinedRect(a_outer);

            m_renderer.DrawColor = c_control.BoundsOutlineColor;
            m_renderer.DrawLinedRect(c_control.Bounds);
        }

        public virtual void DrawTreeNode(GameControl c_ctrl, bool c_open, bool c_selected, int c_labelHeight, int c_labelWidth, int c_halfWay, int c_lastBranch, bool c_isRoot)
        {
            Renderer.DrawColor = m_colors.m_tree.m_lines;

            if (!c_isRoot)
            {
                Renderer.DrawFilledRect(new Rectangle(8, c_halfWay, 16 - 9, 1));
            }

            if (!c_open)
            {
                return;
            }

            Renderer.DrawFilledRect(new Rectangle(14 + 7, c_labelHeight + 1, 1, c_lastBranch + c_halfWay - c_labelHeight));
        }

        public virtual void DrawPropertyRow(GameControl c_control, int c_iWidth, bool c_bBeingEdited, bool c_hovered)
        {
            var a_rect = c_control.RenderBounds;

            if (c_bBeingEdited)
            {
                m_renderer.DrawColor = m_colors.m_properties.m_columnSelected;
            }
            else if (c_hovered)
            {
                m_renderer.DrawColor = m_colors.m_properties.m_columnHover;
            }
            else
            {
                m_renderer.DrawColor = m_colors.m_properties.m_columnNormal;
            }

            m_renderer.DrawFilledRect(new Rectangle(0, a_rect.Y, c_iWidth, a_rect.Height));

            if (c_bBeingEdited)
            {
                m_renderer.DrawColor = m_colors.m_properties.m_lineSelected;
            }
            else if (c_hovered)
            {
                m_renderer.DrawColor = m_colors.m_properties.m_lineHover;
            }
            else
            {
                m_renderer.DrawColor = m_colors.m_properties.m_lineNormal;
            }

            m_renderer.DrawFilledRect(new Rectangle(c_iWidth, a_rect.Y, 1, a_rect.Height));

            a_rect.Y += a_rect.Height - 1;
            a_rect.Height = 1;

            m_renderer.DrawFilledRect(a_rect);
        }

        public virtual void DrawColorDisplay(GameControl c_control, Color c_color)
        {
        }

        public virtual void DrawModalControl(GameControl c_control)
        {
        }

        public virtual void DrawMenuDivider(GameControl c_control)
        {
        }

        public virtual void DrawCategoryHolder(GameControl c_control)
        {
        }

        public virtual void DrawCategoryInner(GameControl c_control, bool c_collapsed)
        {
        }

        public virtual void DrawPropertyTreeNode(GameControl c_control, int c_borderLeft, int c_borderTop)
        {
            var a_rect = c_control.RenderBounds;

            m_renderer.DrawColor = m_colors.m_properties.m_border;

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X, a_rect.Y, c_borderLeft, a_rect.Height));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + c_borderLeft, a_rect.Y, a_rect.Width - c_borderLeft, c_borderTop));
        }

        /*
        Here we're drawing a few symbols such as the directional arrows and the checkbox check

        Texture'd skins don't generally use these - but the Simple skin does. We did originally
        use the marlett font to draw these.. but since that's a Windows font it wasn't a very
        good cross platform solution.
        */

        public virtual void DrawArrowDown(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 0.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 1.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 3.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 4.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 1.0f));
        }

        public virtual void DrawArrowUp(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 0.0f, c_rect.Y + a_y * 3.0f, a_x, a_y * 1.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 2.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 3.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 2.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 4.0f, c_rect.Y + a_y * 3.0f, a_x, a_y * 1.0f));
        }

        public virtual void DrawArrowLeft(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 0.0f, a_x * 1.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 1.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 2.0f, a_x * 3.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 3.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 4.0f, a_x * 1.0f, a_y));
        }

        public virtual void DrawArrowRight(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 0.0f, a_x * 1.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 1.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 2.0f, a_x * 3.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 3.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 4.0f, a_x * 1.0f, a_y));
        }

        public virtual void DrawCheck(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 0.0f, c_rect.Y + a_y * 3.0f, a_x * 2, a_y * 2));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 4.0f, a_x * 2, a_y * 2));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 3.0f, a_x * 2, a_y * 2));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 1.0f, a_x * 2, a_y * 2));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 4.0f, c_rect.Y + a_y * 0.0f, a_x * 2, a_y * 2));
        }
    }
}