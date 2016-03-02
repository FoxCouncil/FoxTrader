using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Numeric down arrow</summary>
    internal class UpDownButtonDown : Button
    {
        /// <summary>Initializes a new instance of the <see cref="UpDownButtonDown" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public UpDownButtonDown(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(7, 7);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawNumericUpDownButton(this, IsDepressed, false);
        }
    }
}