using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Numeric up arrow</summary>
    internal class UpDownButtonUp : Button
    {
        /// <summary>Initializes a new instance of the <see cref="UpDownButtonUp" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public UpDownButtonUp(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(7, 7);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawNumericUpDownButton(this, IsDepressed, true);
        }
    }
}