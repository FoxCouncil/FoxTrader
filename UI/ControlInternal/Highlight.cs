using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Drag & drop highlight</summary>
    internal class Highlight : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="Highlight" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Highlight(GameControl c_parentControl) : base(c_parentControl)
        {
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawHighlight(this);
        }
    }
}