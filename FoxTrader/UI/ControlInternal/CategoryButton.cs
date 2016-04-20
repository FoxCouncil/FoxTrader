using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Item in CollapsibleCategory</summary>
    internal class CategoryButton : Button
    {
        internal bool m_alt; // for alternate coloring

        /// <summary>Initializes a new instance of the <see cref="CategoryButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public CategoryButton(GameControl c_parentControl) : base(c_parentControl)
        {
            Alignment = Pos.Left | Pos.CenterV;
            m_alt = false;
            IsToggle = true;
            TextPadding = new Padding(3, 0, 3, 0);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            if (m_alt)
            {
                if (IsDepressed || ToggleState)
                {
                    Skin.Renderer.DrawColor = c_skin.m_colors.m_category.m_lineAlt.m_buttonSelected;
                }
                else if (IsHovered)
                {
                    Skin.Renderer.DrawColor = c_skin.m_colors.m_category.m_lineAlt.m_buttonHover;
                }
                else
                {
                    Skin.Renderer.DrawColor = c_skin.m_colors.m_category.m_lineAlt.m_button;
                }
            }
            else
            {
                if (IsDepressed || ToggleState)
                {
                    Skin.Renderer.DrawColor = c_skin.m_colors.m_category.m_line.m_buttonSelected;
                }
                else if (IsHovered)
                {
                    Skin.Renderer.DrawColor = c_skin.m_colors.m_category.m_line.m_buttonHover;
                }
                else
                {
                    Skin.Renderer.DrawColor = c_skin.m_colors.m_category.m_line.m_button;
                }
            }

            c_skin.Renderer.DrawFilledRect(RenderBounds);
        }

        /// <summary>Updates control colors</summary>
        public override void UpdateColors()
        {
            if (m_alt)
            {
                if (IsDepressed || ToggleState)
                {
                    TextColor = Skin.m_colors.m_category.m_lineAlt.m_textSelected;
                    return;
                }
                if (IsHovered)
                {
                    TextColor = Skin.m_colors.m_category.m_lineAlt.m_textHover;
                    return;
                }
                TextColor = Skin.m_colors.m_category.m_lineAlt.m_text;
                return;
            }

            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.m_colors.m_category.m_line.m_textSelected;
                return;
            }
            if (IsHovered)
            {
                TextColor = Skin.m_colors.m_category.m_line.m_textHover;
                return;
            }
            TextColor = Skin.m_colors.m_category.m_line.m_text;
        }
    }
}