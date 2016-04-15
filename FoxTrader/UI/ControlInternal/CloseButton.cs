using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Window close button</summary>
    internal class CloseButton : Button
    {
        private readonly WindowControl m_window;

        /// <summary>Initializes a new instance of the <see cref="CloseButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        /// <param name="c_owner">Window that owns this button</param>
        public CloseButton(GameControl c_parentControl, WindowControl c_owner) : base(c_parentControl)
        {
            m_window = c_owner;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawWindowCloseButton(this, IsDepressed && IsHovered, IsHovered && ShouldDrawHover, !m_window.IsOnTop);
        }
    }
}