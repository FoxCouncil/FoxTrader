using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Header of CollapsibleCategory</summary>
    internal class CategoryHeaderButton : Button
    {
        /// <summary>Initializes a new instance of the <see cref="CategoryHeaderButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public CategoryHeaderButton(GameControl c_parentControl) : base(c_parentControl)
        {
            ShouldDrawBackground = false;
            IsToggle = true;
            Alignment = Pos.Center;
            TextPadding = new Padding(3, 0, 3, 0);
        }

        /// <summary>Updates control colors</summary>
        public override void UpdateColors()
        {
            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.m_colors.m_category.m_headerClosed;
            }
            else
            {
                TextColor = Skin.m_colors.m_category.m_header;
            }
        }
    }
}