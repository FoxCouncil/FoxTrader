using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Tree node label</summary>
    internal class TreeNodeLabel : Button
    {
        /// <summary>Initializes a new instance of the <see cref="TreeNodeLabel" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TreeNodeLabel(GameControl c_parentControl) : base(c_parentControl)
        {
            Alignment = Pos.Left | Pos.CenterV;
            ShouldDrawBackground = false;
            Height = 16;
            TextPadding = new Padding(3, 0, 3, 0);
        }

        /// <summary>Updates control colors</summary>
        public override void UpdateColors()
        {
            if (IsDisabled)
            {
                TextColor = Skin.m_colors.m_button.m_disabled;
                return;
            }

            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.m_colors.m_tree.m_selected;
                return;
            }

            if (IsHovered)
            {
                TextColor = Skin.m_colors.m_tree.m_hover;
                return;
            }

            TextColor = Skin.m_colors.m_tree.m_normal;
        }
    }
}