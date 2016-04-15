using FoxTrader.UI.Skin;

namespace FoxTrader.UI.Control
{
    /// <summary>Radio button</summary>
    internal class RadioButton : CheckBox
    {
        /// <summary>Initializes a new instance of the <see cref="RadioButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public RadioButton(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(15, 15);
            MouseInputEnabled = true;
            IsTabable = false;
        }

        /// <summary>Determines whether unchecking is allowed</summary>
        protected override bool AllowUncheck => false;

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawRadioButton(this, IsChecked, IsDepressed);
        }
    }
}