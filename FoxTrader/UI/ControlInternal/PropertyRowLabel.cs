using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Label for PropertyRow</summary>
    internal class PropertyRowLabel : Label
    {
        private readonly PropertyRow m_propertyRow;

        /// <summary>Initializes a new instance of the <see cref="PropertyRowLabel" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public PropertyRowLabel(PropertyRow c_parentControl) : base(c_parentControl)
        {
            Alignment = Pos.Left | Pos.CenterV;
            m_propertyRow = c_parentControl;
        }

        /// <summary>Updates control colors</summary>
        public override void UpdateColors()
        {
            if (IsDisabled)
            {
                TextColor = Skin.m_colors.m_button.m_disabled;
                return;
            }

            if (m_propertyRow != null && m_propertyRow.IsEditing)
            {
                TextColor = Skin.m_colors.m_properties.m_labelSelected;
                return;
            }

            if (m_propertyRow != null && m_propertyRow.IsHovered)
            {
                TextColor = Skin.m_colors.m_properties.m_labelHover;
                return;
            }

            TextColor = Skin.m_colors.m_properties.m_labelNormal;
        }
    }
}