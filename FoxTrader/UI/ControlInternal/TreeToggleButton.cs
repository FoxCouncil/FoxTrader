using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Tree node toggle button (the little plus sign)</summary>
    internal class TreeToggleButton : Button
    {
        /// <summary>Initializes a new instance of the <see cref="TreeToggleButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TreeToggleButton(GameControl c_parentControl) : base(c_parentControl)
        {
            IsToggle = true;
            IsTabable = false;
        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderFocus(SkinBase c_skin)
        {
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawTreeButton(this, ToggleState);
        }
    }
}