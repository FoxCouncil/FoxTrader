using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>ComboBox arrow</summary>
    internal class DownArrow : GameControl
    {
        private readonly ComboBox m_comboBox;

        /// <summary>Initializes a new instance of the <see cref="DownArrow" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public DownArrow(ComboBox c_parentControl) : base(c_parentControl) // or Base?
        {
            MouseInputEnabled = false;
            SetSize(15, 15);

            m_comboBox = c_parentControl;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawComboBoxArrow(this, m_comboBox.IsHovered, m_comboBox.IsDepressed, m_comboBox.IsOpen, m_comboBox.IsDisabled);
        }
    }
}