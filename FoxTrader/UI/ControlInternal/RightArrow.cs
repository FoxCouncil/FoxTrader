using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Submenu indicator</summary>
    internal class RightArrow : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="RightArrow" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public RightArrow(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = false;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawMenuRightArrow(this);
        }
    }
}