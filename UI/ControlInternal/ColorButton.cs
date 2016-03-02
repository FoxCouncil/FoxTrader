using System.Drawing;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Property button</summary>
    internal class ColorButton : Button
    {
        /// <summary>Initializes a new instance of the <see cref="ColorButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ColorButton(GameControl c_parentControl) : base(c_parentControl)
        {
            Color = Color.Black;
            Text = string.Empty;
        }

        /// <summary>Current color value</summary>
        public Color Color
        {
            get;
            set;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.Renderer.DrawColor = Color;
            c_skin.Renderer.DrawFilledRect(RenderBounds);
        }
    }
}