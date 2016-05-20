using System;
using System.Drawing;
using System.IO;
using FoxTrader.UI.Control;
using FoxTrader.UI.Font;
using FoxTrader.UI.Texturing;
using static FoxTrader.Constants;
using Single = FoxTrader.UI.Texturing.Single;

namespace FoxTrader.UI
{
    /// <summary>Base skin</summary>
    internal class Skin : IDisposable
    {
        protected readonly Renderer m_renderer;
        private readonly Texture m_texture;
        public SkinColors m_colors;
        private GameFont m_defaultFont;
        private SkinTextures m_textures;

        /// <summary>Initializes a new instance of the <see cref="Skin" /> class</summary>
        /// <param name="c_renderer">Renderer to use</param>
        /// <param name="c_textureName"></param>
        public Skin(Renderer c_renderer, string c_textureName)
        {
            m_defaultFont = GameFont.LoadFont("regular");
            m_renderer = c_renderer;

            m_texture = new Texture();
            m_texture.Load(c_textureName);

            InitializeColors();
            InitializeTextures();
        }

        public Skin(Renderer c_renderer, Stream c_textureData)
        {
            m_defaultFont = GameFont.LoadFont("regular");
            m_renderer = c_renderer;
            m_texture = new Texture();
            m_texture.LoadStream(c_textureData);

            InitializeColors();
            InitializeTextures();
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
                m_defaultFont = value;
            }
        }

        /// <summary>Renderer used</summary>
        public Renderer Renderer => m_renderer;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources</summary>
        public virtual void Dispose()
        {
            m_texture.Dispose();
            GC.SuppressFinalize(this);
        }

#if DEBUG
        ~Skin()
        {
            throw new InvalidOperationException($"IDisposable object finalized: {GetType()}");
            //Debug.Print(String.Format("IDisposable object finalized: {0}", GetType()));
        }
#endif

        private void InitializeColors()
        {
            m_colors.m_window.m_titleActive = Renderer.PixelColor(m_texture, 4 + 8 * 0, 508, Color.Red);
            m_colors.m_window.m_titleInactive = Renderer.PixelColor(m_texture, 4 + 8 * 1, 508, Color.Yellow);

            m_colors.m_button.m_normal = Renderer.PixelColor(m_texture, 4 + 8 * 2, 508, Color.Yellow);
            m_colors.m_button.m_hover = Renderer.PixelColor(m_texture, 4 + 8 * 3, 508, Color.Yellow);
            m_colors.m_button.m_down = Renderer.PixelColor(m_texture, 4 + 8 * 2, 500, Color.Yellow);
            m_colors.m_button.m_disabled = Renderer.PixelColor(m_texture, 4 + 8 * 3, 500, Color.Yellow);

            m_colors.m_tab.m_active.m_normal = Renderer.PixelColor(m_texture, 4 + 8 * 4, 508, Color.Yellow);
            m_colors.m_tab.m_active.m_hover = Renderer.PixelColor(m_texture, 4 + 8 * 5, 508, Color.Yellow);
            m_colors.m_tab.m_active.m_down = Renderer.PixelColor(m_texture, 4 + 8 * 4, 500, Color.Yellow);
            m_colors.m_tab.m_active.m_disabled = Renderer.PixelColor(m_texture, 4 + 8 * 5, 500, Color.Yellow);
            m_colors.m_tab.m_inactive.m_normal = Renderer.PixelColor(m_texture, 4 + 8 * 6, 508, Color.Yellow);
            m_colors.m_tab.m_inactive.m_hover = Renderer.PixelColor(m_texture, 4 + 8 * 7, 508, Color.Yellow);
            m_colors.m_tab.m_inactive.m_down = Renderer.PixelColor(m_texture, 4 + 8 * 6, 500, Color.Yellow);
            m_colors.m_tab.m_inactive.m_disabled = Renderer.PixelColor(m_texture, 4 + 8 * 7, 500, Color.Yellow);

            m_colors.m_label.m_default = Renderer.PixelColor(m_texture, 4 + 8 * 8, 508, Color.Yellow);
            m_colors.m_label.m_bright = Renderer.PixelColor(m_texture, 4 + 8 * 9, 508, Color.Yellow);
            m_colors.m_label.m_dark = Renderer.PixelColor(m_texture, 4 + 8 * 8, 500, Color.Yellow);
            m_colors.m_label.m_highlight = Renderer.PixelColor(m_texture, 4 + 8 * 9, 500, Color.Yellow);

            m_colors.m_tree.m_lines = Renderer.PixelColor(m_texture, 4 + 8 * 10, 508, Color.Yellow);
            m_colors.m_tree.m_normal = Renderer.PixelColor(m_texture, 4 + 8 * 11, 508, Color.Yellow);
            m_colors.m_tree.m_hover = Renderer.PixelColor(m_texture, 4 + 8 * 10, 500, Color.Yellow);
            m_colors.m_tree.m_selected = Renderer.PixelColor(m_texture, 4 + 8 * 11, 500, Color.Yellow);

            m_colors.m_properties.m_lineNormal = Renderer.PixelColor(m_texture, 4 + 8 * 12, 508, Color.Yellow);
            m_colors.m_properties.m_lineSelected = Renderer.PixelColor(m_texture, 4 + 8 * 13, 508, Color.Yellow);
            m_colors.m_properties.m_lineHover = Renderer.PixelColor(m_texture, 4 + 8 * 12, 500, Color.Yellow);
            m_colors.m_properties.m_title = Renderer.PixelColor(m_texture, 4 + 8 * 13, 500, Color.Yellow);
            m_colors.m_properties.m_columnNormal = Renderer.PixelColor(m_texture, 4 + 8 * 14, 508, Color.Yellow);
            m_colors.m_properties.m_columnSelected = Renderer.PixelColor(m_texture, 4 + 8 * 15, 508, Color.Yellow);
            m_colors.m_properties.m_columnHover = Renderer.PixelColor(m_texture, 4 + 8 * 14, 500, Color.Yellow);
            m_colors.m_properties.m_border = Renderer.PixelColor(m_texture, 4 + 8 * 15, 500, Color.Yellow);
            m_colors.m_properties.m_labelNormal = Renderer.PixelColor(m_texture, 4 + 8 * 16, 508, Color.Yellow);
            m_colors.m_properties.m_labelSelected = Renderer.PixelColor(m_texture, 4 + 8 * 17, 508, Color.Yellow);
            m_colors.m_properties.m_labelHover = Renderer.PixelColor(m_texture, 4 + 8 * 16, 500, Color.Yellow);

            m_colors.m_modalBackground = Renderer.PixelColor(m_texture, 4 + 8 * 18, 508, Color.Yellow);

            m_colors.m_tooltipText = Renderer.PixelColor(m_texture, 4 + 8 * 19, 508, Color.Yellow);

            m_colors.m_category.m_header = Renderer.PixelColor(m_texture, 4 + 8 * 18, 500, Color.Yellow);
            m_colors.m_category.m_headerClosed = Renderer.PixelColor(m_texture, 4 + 8 * 19, 500, Color.Yellow);
            m_colors.m_category.m_line.m_text = Renderer.PixelColor(m_texture, 4 + 8 * 20, 508, Color.Yellow);
            m_colors.m_category.m_line.m_textHover = Renderer.PixelColor(m_texture, 4 + 8 * 21, 508, Color.Yellow);
            m_colors.m_category.m_line.m_textSelected = Renderer.PixelColor(m_texture, 4 + 8 * 20, 500, Color.Yellow);
            m_colors.m_category.m_line.m_button = Renderer.PixelColor(m_texture, 4 + 8 * 21, 500, Color.Yellow);
            m_colors.m_category.m_line.m_buttonHover = Renderer.PixelColor(m_texture, 4 + 8 * 22, 508, Color.Yellow);
            m_colors.m_category.m_line.m_buttonSelected = Renderer.PixelColor(m_texture, 4 + 8 * 23, 508, Color.Yellow);
            m_colors.m_category.m_lineAlt.m_text = Renderer.PixelColor(m_texture, 4 + 8 * 22, 500, Color.Yellow);
            m_colors.m_category.m_lineAlt.m_textHover = Renderer.PixelColor(m_texture, 4 + 8 * 23, 500, Color.Yellow);
            m_colors.m_category.m_lineAlt.m_textSelected = Renderer.PixelColor(m_texture, 4 + 8 * 24, 508, Color.Yellow);
            m_colors.m_category.m_lineAlt.m_button = Renderer.PixelColor(m_texture, 4 + 8 * 25, 508, Color.Yellow);
            m_colors.m_category.m_lineAlt.m_buttonHover = Renderer.PixelColor(m_texture, 4 + 8 * 24, 500, Color.Yellow);
            m_colors.m_category.m_lineAlt.m_buttonSelected = Renderer.PixelColor(m_texture, 4 + 8 * 25, 500, Color.Yellow);
        }

        private void InitializeTextures()
        {
            m_textures.m_shadow = new Bordered(m_texture, 448, 0, 31, 31, Margin.kEight);
            m_textures.m_tooltip = new Bordered(m_texture, 128, 320, 127, 31, Margin.kEight);
            m_textures.m_statusBar = new Bordered(m_texture, 128, 288, 127, 31, Margin.kEight);
            m_textures.m_selection = new Bordered(m_texture, 384, 32, 31, 31, Margin.kFour);

            m_textures.m_panel.m_normal = new Bordered(m_texture, 256, 0, 63, 63, new Margin(16, 16, 16, 16));
            m_textures.m_panel.m_bright = new Bordered(m_texture, 256 + 64, 0, 63, 63, new Margin(16, 16, 16, 16));
            m_textures.m_panel.m_dark = new Bordered(m_texture, 256, 64, 63, 63, new Margin(16, 16, 16, 16));
            m_textures.m_panel.m_highlight = new Bordered(m_texture, 256 + 64, 64, 63, 63, new Margin(16, 16, 16, 16));

            m_textures.m_window.m_normal = new Bordered(m_texture, 0, 0, 127, 127, new Margin(8, 32, 8, 8));
            m_textures.m_window.m_inactive = new Bordered(m_texture, 128, 0, 127, 127, new Margin(8, 32, 8, 8));

            m_textures.m_checkBox.m_active.m_checked = new Single(m_texture, 448, 32, 15, 15);
            m_textures.m_checkBox.m_active.m_normal = new Single(m_texture, 464, 32, 15, 15);
            m_textures.m_checkBox.m_disabled.m_normal = new Single(m_texture, 448, 48, 15, 15);
            m_textures.m_checkBox.m_disabled.m_normal = new Single(m_texture, 464, 48, 15, 15);

            m_textures.m_radioButton.m_active.m_checked = new Single(m_texture, 448, 64, 15, 15);
            m_textures.m_radioButton.m_active.m_normal = new Single(m_texture, 464, 64, 15, 15);
            m_textures.m_radioButton.m_disabled.m_normal = new Single(m_texture, 448, 80, 15, 15);
            m_textures.m_radioButton.m_disabled.m_normal = new Single(m_texture, 464, 80, 15, 15);

            m_textures.m_textBox.m_normal = new Bordered(m_texture, 0, 150, 127, 21, Margin.kFour);
            m_textures.m_textBox.m_focus = new Bordered(m_texture, 0, 172, 127, 21, Margin.kFour);
            m_textures.m_textBox.m_disabled = new Bordered(m_texture, 0, 193, 127, 21, Margin.kFour);

            m_textures.m_menu.m_strip = new Bordered(m_texture, 0, 128, 127, 21, Margin.kOne);
            m_textures.m_menu.m_backgroundWithMargin = new Bordered(m_texture, 128, 128, 127, 63, new Margin(24, 8, 8, 8));
            m_textures.m_menu.m_background = new Bordered(m_texture, 128, 192, 127, 63, Margin.kEight);
            m_textures.m_menu.m_hover = new Bordered(m_texture, 128, 256, 127, 31, Margin.kEight);
            m_textures.m_menu.m_rightArrow = new Single(m_texture, 464, 112, 15, 15);
            m_textures.m_menu.m_check = new Single(m_texture, 448, 112, 15, 15);

            m_textures.m_tab.m_control = new Bordered(m_texture, 0, 256, 127, 127, Margin.kEight);
            m_textures.m_tab.m_bottom.m_active = new Bordered(m_texture, 0, 416, 63, 31, Margin.kEight);
            m_textures.m_tab.m_bottom.m_inactive = new Bordered(m_texture, 0 + 128, 416, 63, 31, Margin.kEight);
            m_textures.m_tab.m_top.m_active = new Bordered(m_texture, 0, 384, 63, 31, Margin.kEight);
            m_textures.m_tab.m_top.m_inactive = new Bordered(m_texture, 0 + 128, 384, 63, 31, Margin.kEight);
            m_textures.m_tab.m_left.m_active = new Bordered(m_texture, 64, 384, 31, 63, Margin.kEight);
            m_textures.m_tab.m_left.m_inactive = new Bordered(m_texture, 64 + 128, 384, 31, 63, Margin.kEight);
            m_textures.m_tab.m_right.m_active = new Bordered(m_texture, 96, 384, 31, 63, Margin.kEight);
            m_textures.m_tab.m_right.m_inactive = new Bordered(m_texture, 96 + 128, 384, 31, 63, Margin.kEight);
            m_textures.m_tab.m_headerBar = new Bordered(m_texture, 128, 352, 127, 31, Margin.kFour);

            m_textures.m_window.m_close = new Single(m_texture, 0, 224, 24, 24);
            m_textures.m_window.m_closeHover = new Single(m_texture, 32, 224, 24, 24);
            m_textures.m_window.m_closeHover = new Single(m_texture, 64, 224, 24, 24);
            m_textures.m_window.m_closeHover = new Single(m_texture, 96, 224, 24, 24);

            m_textures.m_scroller.m_trackV = new Bordered(m_texture, 384, 208, 15, 127, Margin.kFour);
            m_textures.m_scroller.m_buttonVNormal = new Bordered(m_texture, 384 + 16, 208, 15, 127, Margin.kFour);
            m_textures.m_scroller.m_buttonVHover = new Bordered(m_texture, 384 + 32, 208, 15, 127, Margin.kFour);
            m_textures.m_scroller.m_buttonVDown = new Bordered(m_texture, 384 + 48, 208, 15, 127, Margin.kFour);
            m_textures.m_scroller.m_buttonVDisabled = new Bordered(m_texture, 384 + 64, 208, 15, 127, Margin.kFour);
            m_textures.m_scroller.m_trackH = new Bordered(m_texture, 384, 128, 127, 15, Margin.kFour);
            m_textures.m_scroller.m_buttonHNormal = new Bordered(m_texture, 384, 128 + 16, 127, 15, Margin.kFour);
            m_textures.m_scroller.m_buttonHHover = new Bordered(m_texture, 384, 128 + 32, 127, 15, Margin.kFour);
            m_textures.m_scroller.m_buttonHDown = new Bordered(m_texture, 384, 128 + 48, 127, 15, Margin.kFour);
            m_textures.m_scroller.m_buttonHDisabled = new Bordered(m_texture, 384, 128 + 64, 127, 15, Margin.kFour);

            m_textures.m_scroller.m_button.m_normal = new Bordered[4];
            m_textures.m_scroller.m_button.m_disabled = new Bordered[4];
            m_textures.m_scroller.m_button.m_hover = new Bordered[4];
            m_textures.m_scroller.m_button.m_down = new Bordered[4];

            m_textures.m_tree.m_background = new Bordered(m_texture, 256, 128, 127, 127, new Margin(16, 16, 16, 16));
            m_textures.m_tree.m_plus = new Single(m_texture, 448, 96, 15, 15);
            m_textures.m_tree.m_minus = new Single(m_texture, 464, 96, 15, 15);

            m_textures.m_input.m_button.m_normal = new Bordered(m_texture, 480, 0, 31, 31, Margin.kEight);
            m_textures.m_input.m_button.m_hovered = new Bordered(m_texture, 480, 32, 31, 31, Margin.kEight);
            m_textures.m_input.m_button.m_disabled = new Bordered(m_texture, 480, 64, 31, 31, Margin.kEight);
            m_textures.m_input.m_button.m_pressed = new Bordered(m_texture, 480, 96, 31, 31, Margin.kEight);

            for (var a_i = 0; a_i < 4; a_i++)
            {
                m_textures.m_scroller.m_button.m_normal[a_i] = new Bordered(m_texture, 464 + 0, 208 + a_i * 16, 15, 15, Margin.kTwo);
                m_textures.m_scroller.m_button.m_hover[a_i] = new Bordered(m_texture, 480, 208 + a_i * 16, 15, 15, Margin.kTwo);
                m_textures.m_scroller.m_button.m_down[a_i] = new Bordered(m_texture, 464, 272 + a_i * 16, 15, 15, Margin.kTwo);
                m_textures.m_scroller.m_button.m_disabled[a_i] = new Bordered(m_texture, 480 + 48, 272 + a_i * 16, 15, 15, Margin.kTwo);
            }

            m_textures.m_input.m_listBox.m_background = new Bordered(m_texture, 256, 256, 63, 127, Margin.kEight);
            m_textures.m_input.m_listBox.m_hovered = new Bordered(m_texture, 320, 320, 31, 31, Margin.kEight);
            m_textures.m_input.m_listBox.m_evenLine = new Bordered(m_texture, 352, 256, 31, 31, Margin.kEight);
            m_textures.m_input.m_listBox.m_oddLine = new Bordered(m_texture, 352, 288, 31, 31, Margin.kEight);
            m_textures.m_input.m_listBox.m_evenLineSelected = new Bordered(m_texture, 320, 270, 31, 31, Margin.kEight);
            m_textures.m_input.m_listBox.m_oddLineSelected = new Bordered(m_texture, 320, 288, 31, 31, Margin.kEight);

            m_textures.m_input.m_comboBox.m_normal = new Bordered(m_texture, 384, 336, 127, 31, new Margin(8, 8, 32, 8));
            m_textures.m_input.m_comboBox.m_hover = new Bordered(m_texture, 384, 336 + 32, 127, 31, new Margin(8, 8, 32, 8));
            m_textures.m_input.m_comboBox.m_down = new Bordered(m_texture, 384, 336 + 64, 127, 31, new Margin(8, 8, 32, 8));
            m_textures.m_input.m_comboBox.m_disabled = new Bordered(m_texture, 384, 336 + 96, 127, 31, new Margin(8, 8, 32, 8));

            m_textures.m_input.m_comboBox.m_button.m_normal = new Single(m_texture, 496, 272, 15, 15);
            m_textures.m_input.m_comboBox.m_button.m_hover = new Single(m_texture, 496, 272 + 16, 15, 15);
            m_textures.m_input.m_comboBox.m_button.m_down = new Single(m_texture, 496, 272 + 32, 15, 15);
            m_textures.m_input.m_comboBox.m_button.m_disabled = new Single(m_texture, 496, 272 + 48, 15, 15);

            m_textures.m_input.m_upDown.m_up.m_normal = new Single(m_texture, 384, 112, 7, 7);
            m_textures.m_input.m_upDown.m_up.m_hover = new Single(m_texture, 384 + 8, 112, 7, 7);
            m_textures.m_input.m_upDown.m_up.m_down = new Single(m_texture, 384 + 16, 112, 7, 7);
            m_textures.m_input.m_upDown.m_up.m_disabled = new Single(m_texture, 384 + 24, 112, 7, 7);
            m_textures.m_input.m_upDown.m_down.m_normal = new Single(m_texture, 384, 120, 7, 7);
            m_textures.m_input.m_upDown.m_down.m_hover = new Single(m_texture, 384 + 8, 120, 7, 7);
            m_textures.m_input.m_upDown.m_down.m_down = new Single(m_texture, 384 + 16, 120, 7, 7);
            m_textures.m_input.m_upDown.m_down.m_disabled = new Single(m_texture, 384 + 24, 120, 7, 7);

            m_textures.m_progressBar.m_back = new Bordered(m_texture, 384, 0, 31, 31, Margin.kEight);
            m_textures.m_progressBar.m_front = new Bordered(m_texture, 384 + 32, 0, 31, 31, Margin.kEight);

            m_textures.m_input.m_slider.m_h.m_normal = new Single(m_texture, 416, 32, 15, 15);
            m_textures.m_input.m_slider.m_h.m_hover = new Single(m_texture, 416, 32 + 16, 15, 15);
            m_textures.m_input.m_slider.m_h.m_down = new Single(m_texture, 416, 32 + 32, 15, 15);
            m_textures.m_input.m_slider.m_h.m_disabled = new Single(m_texture, 416, 32 + 48, 15, 15);

            m_textures.m_input.m_slider.m_v.m_normal = new Single(m_texture, 416 + 16, 32, 15, 15);
            m_textures.m_input.m_slider.m_v.m_hover = new Single(m_texture, 416 + 16, 32 + 16, 15, 15);
            m_textures.m_input.m_slider.m_v.m_down = new Single(m_texture, 416 + 16, 32 + 32, 15, 15);
            m_textures.m_input.m_slider.m_v.m_disabled = new Single(m_texture, 416 + 16, 32 + 48, 15, 15);

            m_textures.m_categoryList.m_outer = new Bordered(m_texture, 256, 384, 63, 63, Margin.kEight);
            m_textures.m_categoryList.m_inner = new Bordered(m_texture, 256 + 64, 384, 63, 63, new Margin(8, 21, 8, 8));
            m_textures.m_categoryList.m_header = new Bordered(m_texture, 320, 352, 63, 31, Margin.kEight);
        }

        public void DrawButton(GameControl c_control, bool c_depressed, bool c_hovered, bool c_disabled)
        {
            if (c_disabled)
            {
                m_textures.m_input.m_button.m_disabled.Draw(Renderer, c_control.RenderBounds);
                return;
            }
            if (c_depressed)
            {
                m_textures.m_input.m_button.m_pressed.Draw(Renderer, c_control.RenderBounds);
                return;
            }
            if (c_hovered)
            {
                m_textures.m_input.m_button.m_hovered.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_input.m_button.m_normal.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawMenuRightArrow(GameControl c_control)
        {
            m_textures.m_menu.m_rightArrow.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawMenuItem(GameControl c_control, bool c_submenuOpen, bool c_isChecked)
        {
            if (c_submenuOpen || c_control.IsHovered)
            {
                m_textures.m_menu.m_hover.Draw(Renderer, c_control.RenderBounds);
            }

            if (c_isChecked)
            {
                m_textures.m_menu.m_check.Draw(Renderer, new Rectangle(c_control.RenderBounds.X + 4, c_control.RenderBounds.Y + 3, 15, 15));
            }
        }

        public void DrawMenuStrip(GameControl c_control)
        {
            m_textures.m_menu.m_strip.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawMenu(GameControl c_control, bool c_paddingDisabled)
        {
            if (!c_paddingDisabled)
            {
                m_textures.m_menu.m_backgroundWithMargin.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_menu.m_background.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawShadow(GameControl c_control)
        {
            var a_r = c_control.RenderBounds;
            a_r.X -= 4;
            a_r.Y -= 4;
            a_r.Width += 10;
            a_r.Height += 10;
            m_textures.m_shadow.Draw(Renderer, a_r);
        }

        public void DrawRadioButton(GameControl c_control, bool c_selected, bool c_depressed)
        {
            if (c_selected)
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_radioButton.m_disabled.m_checked.Draw(Renderer, c_control.RenderBounds);
                }
                else
                {
                    m_textures.m_radioButton.m_active.m_checked.Draw(Renderer, c_control.RenderBounds);
                }
            }
            else
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_radioButton.m_disabled.m_normal.Draw(Renderer, c_control.RenderBounds);
                }
                else
                {
                    m_textures.m_radioButton.m_active.m_normal.Draw(Renderer, c_control.RenderBounds);
                }
            }
        }

        public void DrawCheckBox(GameControl c_control, bool c_selected, bool c_depressed)
        {
            if (c_selected)
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_checkBox.m_disabled.m_checked.Draw(Renderer, c_control.RenderBounds);
                }
                else
                {
                    m_textures.m_checkBox.m_active.m_checked.Draw(Renderer, c_control.RenderBounds);
                }
            }
            else
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_checkBox.m_disabled.m_normal.Draw(Renderer, c_control.RenderBounds);
                }
                else
                {
                    m_textures.m_checkBox.m_active.m_normal.Draw(Renderer, c_control.RenderBounds);
                }
            }
        }

        public void DrawGroupBox(GameControl c_control, int c_textStart, int c_textHeight, int c_textWidth)
        {
            var a_rect = c_control.RenderBounds;

            a_rect.Y += (int)(c_textHeight * 0.5f);
            a_rect.Height -= (int)(c_textHeight * 0.5f);

            var a_colDarker = Color.FromArgb(50, 0, 50, 60);
            var a_colLighter = Color.FromArgb(150, 255, 255, 255);

            Renderer.DrawColor = a_colLighter;

            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + 1, c_textStart - 3, 1));
            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1 + c_textStart + c_textWidth, a_rect.Y + 1, a_rect.Width - c_textStart + c_textWidth - 2, 1));
            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, (a_rect.Y + a_rect.Height) - 1, a_rect.X + a_rect.Width - 2, 1));

            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y + 1, 1, a_rect.Height));
            Renderer.DrawFilledRect(new Rectangle((a_rect.X + a_rect.Width) - 2, a_rect.Y + 1, 1, a_rect.Height - 1));

            Renderer.DrawColor = a_colDarker;

            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, a_rect.Y, c_textStart - 3, 1));
            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1 + c_textStart + c_textWidth, a_rect.Y, a_rect.Width - c_textStart - c_textWidth - 2, 1));
            Renderer.DrawFilledRect(new Rectangle(a_rect.X + 1, (a_rect.Y + a_rect.Height) - 1, a_rect.X + a_rect.Width - 2, 1));

            Renderer.DrawFilledRect(new Rectangle(a_rect.X, a_rect.Y + 1, 1, a_rect.Height - 1));
            Renderer.DrawFilledRect(new Rectangle((a_rect.X + a_rect.Width) - 1, a_rect.Y + 1, 1, a_rect.Height - 1));
        }

        public void DrawTextBox(GameControl c_control)
        {
            if (c_control.IsDisabled)
            {
                m_textures.m_textBox.m_disabled.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.HasFocus)
            {
                m_textures.m_textBox.m_focus.Draw(Renderer, c_control.RenderBounds);
            }
            else
            {
                m_textures.m_textBox.m_normal.Draw(Renderer, c_control.RenderBounds);
            }
        }

        public void DrawTabButton(GameControl c_control, bool c_active, Pos c_dir)
        {
            if (c_active)
            {
                DrawActiveTabButton(c_control, c_dir);
                return;
            }

            if (c_dir == Pos.Top)
            {
                m_textures.m_tab.m_top.m_inactive.Draw(Renderer, c_control.RenderBounds);
                return;
            }
            if (c_dir == Pos.Left)
            {
                m_textures.m_tab.m_left.m_inactive.Draw(Renderer, c_control.RenderBounds);
                return;
            }
            if (c_dir == Pos.Bottom)
            {
                m_textures.m_tab.m_bottom.m_inactive.Draw(Renderer, c_control.RenderBounds);
                return;
            }
            if (c_dir == Pos.Right)
            {
                m_textures.m_tab.m_right.m_inactive.Draw(Renderer, c_control.RenderBounds);
            }
        }

        private void DrawActiveTabButton(GameControl c_control, Pos c_dir)
        {
            if (c_dir == Pos.Top)
            {
                m_textures.m_tab.m_top.m_active.Draw(Renderer, c_control.RenderBounds.Add(new Rectangle(0, 0, 0, 8)));
                return;
            }
            if (c_dir == Pos.Left)
            {
                m_textures.m_tab.m_left.m_active.Draw(Renderer, c_control.RenderBounds.Add(new Rectangle(0, 0, 8, 0)));
                return;
            }
            if (c_dir == Pos.Bottom)
            {
                m_textures.m_tab.m_bottom.m_active.Draw(Renderer, c_control.RenderBounds.Add(new Rectangle(0, -8, 0, 8)));
                return;
            }
            if (c_dir == Pos.Right)
            {
                m_textures.m_tab.m_right.m_active.Draw(Renderer, c_control.RenderBounds.Add(new Rectangle(-8, 0, 8, 0)));
            }
        }

        public void DrawTabControl(GameControl c_control)
        {
            m_textures.m_tab.m_control.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawTabTitleBar(GameControl c_control)
        {
            m_textures.m_tab.m_headerBar.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawWindow(GameControl c_control, int c_topHeight, bool c_inFocus)
        {
            if (c_inFocus)
            {
                m_textures.m_window.m_normal.Draw(Renderer, c_control.RenderBounds);
            }
            else
            {
                m_textures.m_window.m_inactive.Draw(Renderer, c_control.RenderBounds);
            }
        }

        public void DrawHighlight(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;
            Renderer.DrawColor = Color.FromArgb(255, 255, 100, 255);
            Renderer.DrawFilledRect(a_rect);
        }

        public void DrawScrollBar(GameControl c_control, bool c_horizontal, bool c_depressed)
        {
            if (c_horizontal)
            {
                m_textures.m_scroller.m_trackH.Draw(Renderer, c_control.RenderBounds);
            }
            else
            {
                m_textures.m_scroller.m_trackV.Draw(Renderer, c_control.RenderBounds);
            }
        }

        public void DrawScrollBarBar(GameControl c_control, bool c_depressed, bool c_hovered, bool c_horizontal)
        {
            if (!c_horizontal)
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_scroller.m_buttonVDisabled.Draw(Renderer, c_control.RenderBounds);
                    return;
                }

                if (c_depressed)
                {
                    m_textures.m_scroller.m_buttonVDown.Draw(Renderer, c_control.RenderBounds);
                    return;
                }

                if (c_hovered)
                {
                    m_textures.m_scroller.m_buttonVHover.Draw(Renderer, c_control.RenderBounds);
                    return;
                }

                m_textures.m_scroller.m_buttonVNormal.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsDisabled)
            {
                m_textures.m_scroller.m_buttonHDisabled.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_depressed)
            {
                m_textures.m_scroller.m_buttonHDown.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_hovered)
            {
                m_textures.m_scroller.m_buttonHHover.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_scroller.m_buttonHNormal.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawProgressBar(GameControl c_control, bool c_horizontal, float c_progress)
        {
            var a_rect = c_control.RenderBounds;

            if (c_horizontal)
            {
                m_textures.m_progressBar.m_back.Draw(Renderer, a_rect);
                a_rect.Width = (int)(a_rect.Width * c_progress);
                m_textures.m_progressBar.m_front.Draw(Renderer, a_rect);
            }
            else
            {
                m_textures.m_progressBar.m_back.Draw(Renderer, a_rect);
                a_rect.Y = (int)(a_rect.Y + a_rect.Height * (1 - c_progress));
                a_rect.Height = (int)(a_rect.Height * c_progress);
                m_textures.m_progressBar.m_front.Draw(Renderer, a_rect);
            }
        }

        public void DrawListBox(GameControl c_control)
        {
            m_textures.m_input.m_listBox.m_background.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawListBoxLine(GameControl c_control, bool c_selected, bool c_even)
        {
            if (c_selected)
            {
                if (c_even)
                {
                    m_textures.m_input.m_listBox.m_evenLineSelected.Draw(Renderer, c_control.RenderBounds);
                    return;
                }
                m_textures.m_input.m_listBox.m_oddLineSelected.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsHovered)
            {
                m_textures.m_input.m_listBox.m_hovered.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_even)
            {
                m_textures.m_input.m_listBox.m_evenLine.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_input.m_listBox.m_oddLine.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawSliderNotchesH(Rectangle c_rect, int c_numNotches, float c_dist)
        {
            if (c_numNotches == 0)
            {
                return;
            }

            var a_iSpacing = c_rect.Width / (float)c_numNotches;
            for (var a_i = 0; a_i < c_numNotches + 1; a_i++)
            {
                Renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_iSpacing * a_i, c_rect.Y + c_dist - 2, 1, 5));
            }
        }

        public void DrawSliderNotchesV(Rectangle c_rect, int c_numNotches, float c_dist)
        {
            if (c_numNotches == 0)
            {
                return;
            }

            var a_iSpacing = c_rect.Height / (float)c_numNotches;
            for (var a_i = 0; a_i < c_numNotches + 1; a_i++)
            {
                Renderer.DrawFilledRect(Util.FloatRect(c_rect.X + c_dist - 2, c_rect.Y + a_iSpacing * a_i, 5, 1));
            }
        }

        public void DrawSlider(GameControl c_control, bool c_horizontal, int c_numNotches, int c_barSize)
        {
            var a_rect = c_control.RenderBounds;
            Renderer.DrawColor = Color.FromArgb(100, 0, 0, 0);

            if (c_horizontal)
            {
                a_rect.X += (int)(c_barSize * 0.5);
                a_rect.Width -= c_barSize;
                a_rect.Y += (int)(a_rect.Height * 0.5 - 1);
                a_rect.Height = 1;
                DrawSliderNotchesH(a_rect, c_numNotches, c_barSize * 0.5f);
                Renderer.DrawFilledRect(a_rect);
                return;
            }

            a_rect.Y += (int)(c_barSize * 0.5);
            a_rect.Height -= c_barSize;
            a_rect.X += (int)(a_rect.Width * 0.5 - 1);
            a_rect.Width = 1;
            DrawSliderNotchesV(a_rect, c_numNotches, c_barSize * 0.4f);
            Renderer.DrawFilledRect(a_rect);
        }

        public void DrawComboBox(GameControl c_control, bool c_down, bool c_open)
        {
            if (c_control.IsDisabled)
            {
                m_textures.m_input.m_comboBox.m_disabled.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_down || c_open)
            {
                m_textures.m_input.m_comboBox.m_down.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsHovered)
            {
                m_textures.m_input.m_comboBox.m_down.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_input.m_comboBox.m_normal.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawKeyboardHighlight(GameControl c_control, Rectangle c_r, int c_offset)
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
                m_renderer.DrawColor = Color.Black;
                if (!a_skip)
                {
                    Renderer.DrawPixel(a_rect.X + (a_i * 2), a_rect.Y);
                    Renderer.DrawPixel(a_rect.X + (a_i * 2), a_rect.Y + a_rect.Height - 1);
                }
                else
                {
                    a_skip = false;
                }
            }

            for (var a_i = 0; a_i < a_rect.Height * 0.5; a_i++)
            {
                Renderer.DrawColor = Color.Black;
                Renderer.DrawPixel(a_rect.X, a_rect.Y + a_i * 2);
                Renderer.DrawPixel(a_rect.X + a_rect.Width - 1, a_rect.Y + a_i * 2);
            }
        }

        public void DrawToolTip(GameControl c_control)
        {
            m_textures.m_tooltip.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawScrollButton(GameControl c_control, Pos c_direction, bool c_depressed, bool c_hovered, bool c_disabled)
        {
            var a_i = 0;
            if (c_direction == Pos.Top)
            {
                a_i = 1;
            }
            if (c_direction == Pos.Right)
            {
                a_i = 2;
            }
            if (c_direction == Pos.Bottom)
            {
                a_i = 3;
            }

            if (c_disabled)
            {
                m_textures.m_scroller.m_button.m_disabled[a_i].Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_depressed)
            {
                m_textures.m_scroller.m_button.m_down[a_i].Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_hovered)
            {
                m_textures.m_scroller.m_button.m_hover[a_i].Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_scroller.m_button.m_normal[a_i].Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawComboBoxArrow(GameControl c_control, bool c_hovered, bool c_down, bool c_open, bool c_disabled)
        {
            if (c_disabled)
            {
                m_textures.m_input.m_comboBox.m_button.m_disabled.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_down || c_open)
            {
                m_textures.m_input.m_comboBox.m_button.m_down.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_hovered)
            {
                m_textures.m_input.m_comboBox.m_button.m_hover.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_input.m_comboBox.m_button.m_normal.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawNumericUpDownButton(GameControl c_control, bool c_depressed, bool c_up)
        {
            if (c_up)
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_input.m_upDown.m_up.m_disabled.DrawCenter(Renderer, c_control.RenderBounds);
                    return;
                }

                if (c_depressed)
                {
                    m_textures.m_input.m_upDown.m_up.m_down.DrawCenter(Renderer, c_control.RenderBounds);
                    return;
                }

                if (c_control.IsHovered)
                {
                    m_textures.m_input.m_upDown.m_up.m_hover.DrawCenter(Renderer, c_control.RenderBounds);
                    return;
                }

                m_textures.m_input.m_upDown.m_up.m_normal.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsDisabled)
            {
                m_textures.m_input.m_upDown.m_down.m_disabled.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_depressed)
            {
                m_textures.m_input.m_upDown.m_down.m_down.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsHovered)
            {
                m_textures.m_input.m_upDown.m_down.m_hover.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_input.m_upDown.m_down.m_normal.DrawCenter(Renderer, c_control.RenderBounds);
        }

        public void DrawStatusBar(GameControl c_control)
        {
            m_textures.m_statusBar.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawTreeButton(GameControl c_control, bool c_open)
        {
            var a_rect = c_control.RenderBounds;

            if (c_open)
            {
                m_textures.m_tree.m_minus.Draw(Renderer, a_rect);
            }
            else
            {
                m_textures.m_tree.m_plus.Draw(Renderer, a_rect);
            }
        }

        public void DrawTreeControl(GameControl c_control)
        {
            m_textures.m_tree.m_background.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawTreeNode(GameControl c_ctrl, bool c_open, bool c_selected, int c_labelHeight, int c_labelWidth, int c_halfWay, int c_lastBranch, bool c_isRoot)
        {
            if (c_selected)
            {
                m_textures.m_selection.Draw(Renderer, new Rectangle(17, 0, c_labelWidth + 2, c_labelHeight - 1));
            }

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

        public void DrawColorDisplay(GameControl c_control, Color c_color)
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

            Renderer.DrawColor = Color.Black;
            Renderer.DrawLinedRect(a_rect);
        }

        public void DrawModalControl(GameControl c_control)
        {
            if (!c_control.ShouldDrawBackground)
            {
                return;
            }
            var a_rect = c_control.RenderBounds;
            Renderer.DrawColor = m_colors.m_modalBackground;
            Renderer.DrawFilledRect(a_rect);
        }

        public void DrawMenuDivider(GameControl c_control)
        {
            var a_rect = c_control.RenderBounds;
            Renderer.DrawColor = Color.FromArgb(100, 0, 0, 0);
            Renderer.DrawFilledRect(a_rect);
        }

        public void DrawWindowCloseButton(GameControl c_control, bool c_depressed, bool c_hovered, bool c_disabled)
        {
            if (c_disabled)
            {
                m_textures.m_window.m_closeDisabled.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_depressed)
            {
                m_textures.m_window.m_closeDown.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_hovered)
            {
                m_textures.m_window.m_closeHover.Draw(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_window.m_close.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawSliderButton(GameControl c_control, bool c_depressed, bool c_horizontal)
        {
            if (!c_horizontal)
            {
                if (c_control.IsDisabled)
                {
                    m_textures.m_input.m_slider.m_v.m_disabled.DrawCenter(Renderer, c_control.RenderBounds);
                    return;
                }

                if (c_depressed)
                {
                    m_textures.m_input.m_slider.m_v.m_down.DrawCenter(Renderer, c_control.RenderBounds);
                    return;
                }

                if (c_control.IsHovered)
                {
                    m_textures.m_input.m_slider.m_v.m_hover.DrawCenter(Renderer, c_control.RenderBounds);
                    return;
                }

                m_textures.m_input.m_slider.m_v.m_normal.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsDisabled)
            {
                m_textures.m_input.m_slider.m_h.m_disabled.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_depressed)
            {
                m_textures.m_input.m_slider.m_h.m_down.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            if (c_control.IsHovered)
            {
                m_textures.m_input.m_slider.m_h.m_hover.DrawCenter(Renderer, c_control.RenderBounds);
                return;
            }

            m_textures.m_input.m_slider.m_h.m_normal.DrawCenter(Renderer, c_control.RenderBounds);
        }

        public void DrawCategoryHolder(GameControl c_control)
        {
            m_textures.m_categoryList.m_outer.Draw(Renderer, c_control.RenderBounds);
        }

        public void DrawCategoryInner(GameControl c_control, bool c_collapsed)
        {
            if (c_collapsed)
            {
                m_textures.m_categoryList.m_header.Draw(Renderer, c_control.RenderBounds);
            }
            else
            {
                m_textures.m_categoryList.m_inner.Draw(Renderer, c_control.RenderBounds);
            }
        }

        public void DrawDebugOutlines(GameControl c_control)
        {
            m_renderer.DrawColor = c_control.PaddingOutlineColor;
            var a_inner = new Rectangle(c_control.Bounds.Left + c_control.Padding.Left, c_control.Bounds.Top + c_control.Padding.Top, c_control.Bounds.Width - c_control.Padding.Right - c_control.Padding.Left, c_control.Bounds.Height - c_control.Padding.Bottom - c_control.Padding.Top);
            m_renderer.DrawLinedRect(a_inner);

            m_renderer.DrawColor = c_control.MarginOutlineColor;
            var a_outer = new Rectangle(c_control.Bounds.Left - c_control.Margin.m_left, c_control.Bounds.Top - c_control.Margin.m_top, c_control.Bounds.Width + c_control.Margin.m_right + c_control.Margin.m_left, c_control.Bounds.Height + c_control.Margin.m_bottom + c_control.Margin.m_top);
            m_renderer.DrawLinedRect(a_outer);

            m_renderer.DrawColor = c_control.BoundsOutlineColor;
            m_renderer.DrawLinedRect(c_control.Bounds);
        }

        public void DrawPropertyRow(GameControl c_control, int c_iWidth, bool c_bBeingEdited, bool c_hovered)
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

        public void DrawPropertyTreeNode(GameControl c_control, int c_borderLeft, int c_borderTop)
        {
            var a_rect = c_control.RenderBounds;

            m_renderer.DrawColor = m_colors.m_properties.m_border;

            m_renderer.DrawFilledRect(new Rectangle(a_rect.X, a_rect.Y, c_borderLeft, a_rect.Height));
            m_renderer.DrawFilledRect(new Rectangle(a_rect.X + c_borderLeft, a_rect.Y, a_rect.Width - c_borderLeft, c_borderTop));
        }

        public void DrawArrowDown(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 0.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 1.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 3.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 4.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 1.0f));
        }

        public void DrawArrowUp(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 0.0f, c_rect.Y + a_y * 3.0f, a_x, a_y * 1.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 2.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 1.0f, a_x, a_y * 3.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 2.0f, a_x, a_y * 2.0f));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 4.0f, c_rect.Y + a_y * 3.0f, a_x, a_y * 1.0f));
        }

        public void DrawArrowLeft(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 0.0f, a_x * 1.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 1.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 2.0f, a_x * 3.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 2.0f, c_rect.Y + a_y * 3.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 3.0f, c_rect.Y + a_y * 4.0f, a_x * 1.0f, a_y));
        }

        public void DrawArrowRight(Rectangle c_rect)
        {
            var a_x = (c_rect.Width / 5.0f);
            var a_y = (c_rect.Height / 5.0f);

            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 0.0f, a_x * 1.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 1.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 2.0f, a_x * 3.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 3.0f, a_x * 2.0f, a_y));
            m_renderer.DrawFilledRect(Util.FloatRect(c_rect.X + a_x * 1.0f, c_rect.Y + a_y * 4.0f, a_x * 1.0f, a_y));
        }

        public void DrawCheck(Rectangle c_rect)
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