using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Status bar</summary>
    internal class StatusBar : Label
    {
        /// <summary>Initializes a new instance of the <see cref="StatusBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public StatusBar(GameControl c_parentControl) : base(c_parentControl)
        {
            Height = 22;
            Dock = Pos.Bottom;
            Padding = Padding.kTwo;
            Alignment = Pos.Left | Pos.CenterV;
        }

        /// <summary>Adds a control to the bar</summary>
        /// <param name="c_control">Control to add</param>
        /// <param name="c_right">Determines whether the control should be added to the right side of the bar</param>
        public void AddControl(GameControl c_control, bool c_right)
        {
            c_control.Parent = this;
            c_control.Dock = c_right ? Pos.Right : Pos.Left;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawStatusBar(this);
        }
    }
}