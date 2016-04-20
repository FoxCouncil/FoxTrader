using System.Drawing;
using System.Runtime.InteropServices;
using FoxTrader.Game.Utils;
using FoxTrader.UI.Texturing;

namespace FoxTrader
{
    public struct Vector2
    {
        public Vector2(int c_x, int c_y)
        {
            X = c_x;
            Y = c_y;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        internal static Vector2 Random(int c_maxRange)
        {
            var a_randomX = Generator.RandomRange(0, c_maxRange);
            var a_randomY = Generator.RandomRange(0, c_maxRange);

            return new Vector2(a_randomX, a_randomY);
        }

        internal static Vector2 Zero => new Vector2(0, 0);
    };

    public struct Vector3
    {
        public Vector3(int c_x, int c_y, int c_z)
        {
            X = c_x;
            Y = c_y;
            Z = c_z;
        }

        public int X
        {
            get;

            set;
        }

        public int Y
        {
            get;

            set;
        }

        public int Z
        {
            get;

            set;
        }

        public Vector2 ToVec2()
        {
            return new Vector2(X, Y);
        }

        internal static Vector3 Random(int c_maxRange)
        {
            var a_randomX = Generator.RandomRange(0, c_maxRange);
            var a_randomY = Generator.RandomRange(0, c_maxRange);
            var a_randomZ = Generator.RandomRange(0, c_maxRange);

            return new Vector3(a_randomX, a_randomY, a_randomZ);
        }

        internal static Vector3 Zero => new Vector3(0, 0, 0);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        public short X;
        public short Y;

        public float U;
        public float V;

        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }

    public struct SkinTextures
    {
        public Bordered m_statusBar;
        public Bordered m_selection;
        public Bordered m_shadow;
        public Bordered m_tooltip;

        public struct PanelTexture
        {
            public Bordered m_normal;
            public Bordered m_bright;
            public Bordered m_dark;
            public Bordered m_highlight;
        }

        public struct WindowTexture
        {
            public Bordered m_normal;
            public Bordered m_inactive;
            public Single m_close;
            public Single m_closeHover;
            public Single m_closeDown;
            public Single m_closeDisabled;
        }

        public struct CheckBoxTexture
        {
            public struct Active
            {
                public Single m_normal;
                public Single m_checked;
            }

            public struct Disabled
            {
                public Single m_normal;
                public Single m_checked;
            }

            public Active m_active;
            public Disabled m_disabled;
        }

        public struct RadioButtonTexture
        {
            public struct Active
            {
                public Single m_normal;
                public Single m_checked;
            }

            public struct Disabled
            {
                public Single m_normal;
                public Single m_checked;
            }

            public Active m_active;
            public Disabled m_disabled;
        }

        public struct TextBoxTexture
        {
            public Bordered m_normal;
            public Bordered m_focus;
            public Bordered m_disabled;
        }

        public struct TreeTexture
        {
            public Bordered m_background;
            public Single m_minus;
            public Single m_plus;
        }

        public struct ProgressBarTexture
        {
            public Bordered m_back;
            public Bordered m_front;
        }

        public struct ScrollerTexture
        {
            public Bordered m_trackV;
            public Bordered m_trackH;
            public Bordered m_buttonVNormal;
            public Bordered m_buttonVHover;
            public Bordered m_buttonVDown;
            public Bordered m_buttonVDisabled;
            public Bordered m_buttonHNormal;
            public Bordered m_buttonHHover;
            public Bordered m_buttonHDown;
            public Bordered m_buttonHDisabled;

            public struct ButtonTexture
            {
                public Bordered[] m_normal;
                public Bordered[] m_hover;
                public Bordered[] m_down;
                public Bordered[] m_disabled;
            }

            public ButtonTexture m_button;
        }

        public struct MenuTexture
        {
            public Single m_rightArrow;
            public Single m_check;

            public Bordered m_strip;
            public Bordered m_background;
            public Bordered m_backgroundWithMargin;
            public Bordered m_hover;
        }

        public struct InputTexture
        {
            public struct ButtonTexture
            {
                public Bordered m_normal;
                public Bordered m_hovered;
                public Bordered m_disabled;
                public Bordered m_pressed;
            }

            public struct ComboBoxTexture
            {
                public Bordered m_normal;
                public Bordered m_hover;
                public Bordered m_down;
                public Bordered m_disabled;

                public struct ButtonTexture
                {
                    public Single m_normal;
                    public Single m_hover;
                    public Single m_down;
                    public Single m_disabled;
                }

                public ButtonTexture m_button;
            }

            public struct SliderTexture
            {
                public struct HTexture
                {
                    public Single m_normal;
                    public Single m_hover;
                    public Single m_down;
                    public Single m_disabled;
                }

                public struct VTexture
                {
                    public Single m_normal;
                    public Single m_hover;
                    public Single m_down;
                    public Single m_disabled;
                }

                public HTexture m_h;
                public VTexture m_v;
            }

            public struct ListBoxTexture
            {
                public Bordered m_background;
                public Bordered m_hovered;
                public Bordered m_evenLine;
                public Bordered m_oddLine;
                public Bordered m_evenLineSelected;
                public Bordered m_oddLineSelected;
            }

            public struct UpDownTexture
            {
                public struct UpTexture
                {
                    public Single m_normal;
                    public Single m_hover;
                    public Single m_down;
                    public Single m_disabled;
                }

                public struct DownTexture
                {
                    public Single m_normal;
                    public Single m_hover;
                    public Single m_down;
                    public Single m_disabled;
                }

                public UpTexture m_up;
                public DownTexture m_down;
            }

            public ButtonTexture m_button;
            public ComboBoxTexture m_comboBox;
            public SliderTexture m_slider;
            public ListBoxTexture m_listBox;
            public UpDownTexture m_upDown;
        }

        public struct TabTexture
        {
            public struct BottomTexture
            {
                public Bordered m_inactive;
                public Bordered m_active;
            }

            public struct TopTexture
            {
                public Bordered m_inactive;
                public Bordered m_active;
            }

            public struct LeftTexture
            {
                public Bordered m_inactive;
                public Bordered m_active;
            }

            public struct RightTexture
            {
                public Bordered m_inactive;
                public Bordered m_active;
            }

            public BottomTexture m_bottom;
            public TopTexture m_top;
            public LeftTexture m_left;
            public RightTexture m_right;

            public Bordered m_control;
            public Bordered m_headerBar;
        }

        public struct CategoryListTexture
        {
            public Bordered m_outer;
            public Bordered m_inner;
            public Bordered m_header;
        }

        public PanelTexture m_panel;
        public WindowTexture m_window;
        public CheckBoxTexture m_checkBox;
        public RadioButtonTexture m_radioButton;
        public TextBoxTexture m_textBox;
        public TreeTexture m_tree;
        public ProgressBarTexture m_progressBar;
        public ScrollerTexture m_scroller;
        public MenuTexture m_menu;
        public InputTexture m_input;
        public TabTexture m_tab;
        public CategoryListTexture m_categoryList;
    }

    /// <summary>UI colors used by skins</summary>
    public struct SkinColors
    {
        public struct WindowColors
        {
            public Color m_titleActive;
            public Color m_titleInactive;
        }

        public struct ButtonColors
        {
            public Color m_normal;
            public Color m_hover;
            public Color m_down;
            public Color m_disabled;
        }

        public struct TabColors
        {
            public struct InactiveColors
            {
                public Color m_normal;
                public Color m_hover;
                public Color m_down;
                public Color m_disabled;
            }

            public struct ActiveColors
            {
                public Color m_normal;
                public Color m_hover;
                public Color m_down;
                public Color m_disabled;
            }

            public InactiveColors m_inactive;
            public ActiveColors m_active;
        }

        public struct LabelColors
        {
            public Color m_default;
            public Color m_bright;
            public Color m_dark;
            public Color m_highlight;
        }

        public struct TreeColors
        {
            public Color m_lines;
            public Color m_normal;
            public Color m_hover;
            public Color m_selected;
        }

        public struct PropertyColors
        {
            public Color m_lineNormal;
            public Color m_lineSelected;
            public Color m_lineHover;
            public Color m_columnNormal;
            public Color m_columnSelected;
            public Color m_columnHover;
            public Color m_labelNormal;
            public Color m_labelSelected;
            public Color m_labelHover;
            public Color m_border;
            public Color m_title;
        }

        public struct CategoryColors
        {
            public Color m_header;
            public Color m_headerClosed;

            public struct LineColors
            {
                public Color m_text;
                public Color m_textHover;
                public Color m_textSelected;
                public Color m_button;
                public Color m_buttonHover;
                public Color m_buttonSelected;
            }

            public struct LineAltColors
            {
                public Color m_text;
                public Color m_textHover;
                public Color m_textSelected;
                public Color m_button;
                public Color m_buttonHover;
                public Color m_buttonSelected;
            }

            public LineColors m_line;
            public LineAltColors m_lineAlt;
        }

        public Color m_modalBackground;
        public Color m_tooltipText;

        public WindowColors m_window;
        public ButtonColors m_button;
        public TabColors m_tab;
        public LabelColors m_label;
        public TreeColors m_tree;
        public PropertyColors m_properties;
        public CategoryColors m_category;
    }
}