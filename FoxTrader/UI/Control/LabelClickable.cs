using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Clickable label (for checkboxes etc)</summary>
    internal class LabelClickable : Button
    {
        /// <summary>Initializes a new instance of the <see cref="LabelClickable" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public LabelClickable(GameControl c_parentControl) : base(c_parentControl)
        {
            IsToggle = false;
            Alignment = Pos.Left | Pos.CenterV;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            // no button look
        }
    }
}