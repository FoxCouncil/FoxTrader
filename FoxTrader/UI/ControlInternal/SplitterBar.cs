using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Splitter bar</summary>
    internal class SplitterBar : Dragger
    {
        /// <summary>Initializes a new instance of the <see cref="SplitterBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public SplitterBar(GameControl c_parentControl) : base(c_parentControl)
        {
            Target = this;
            RestrictToParent = true;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            if (ShouldDrawBackground)
            {
                c_skin.DrawButton(this, true, false, IsDisabled);
            }
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            MoveTo(X, Y);
        }
    }
}