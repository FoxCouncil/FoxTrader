using FoxTrader.UI.Control;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Divider menu item</summary>
    internal class MenuDivider : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="MenuDivider" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public MenuDivider(GameControl c_parentControl) : base(c_parentControl)
        {
            Height = 1;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawMenuDivider(this);
        }
    }
}