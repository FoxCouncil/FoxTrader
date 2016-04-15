using System.Drawing;

namespace FoxTrader.UI.Skin
{
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