using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Inner panel of tab control</summary>
    internal class TabControlInner : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="TabControlInner" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        internal TabControlInner(GameControl c_parentControl) : base(c_parentControl)
        {
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawTabControl(this);
        }
    }
}