using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Slider bar</summary>
    internal class SliderBar : Dragger
    {
        /// <summary>Initializes a new instance of the <see cref="SliderBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public SliderBar(GameControl c_parentControl) : base(c_parentControl)
        {
            Target = this;
            RestrictToParent = true;
        }

        /// <summary>Indicates whether the bar is horizontal</summary>
        public bool IsHorizontal
        {
            get;
            set;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawSliderButton(this, IsHeld, IsHorizontal);
        }
    }
}