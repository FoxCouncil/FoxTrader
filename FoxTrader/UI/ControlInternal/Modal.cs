using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Modal control for windows</summary>
    internal class Modal : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="Modal" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Modal(GameControl c_parentControl) : base(c_parentControl)
        {
            KeyboardInputEnabled = true;
            MouseInputEnabled = true;
            ShouldDrawBackground = true;
            SetBounds(0, 0, GetCanvas().Width, GetCanvas().Height);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            SetBounds(0, 0, GetCanvas().Width, GetCanvas().Height);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawModalControl(this);
        }
    }
}