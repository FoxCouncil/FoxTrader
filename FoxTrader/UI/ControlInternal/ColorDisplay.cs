using System.Drawing;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Color square</summary>
    internal class ColorDisplay : GameControl
    {
        //private bool m_DrawCheckers;

        /// <summary>Initializes a new instance of the <see cref="ColorDisplay" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ColorDisplay(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(32, 32);
            Color = Color.FromArgb(255, 255, 0, 0);
            //m_DrawCheckers = true;
        }

        /// <summary>Current color</summary>
        public Color Color
        {
            get;
            set;
        }

        //public bool DrawCheckers { get { return m_DrawCheckers; } set { m_DrawCheckers = value; } }
        public int R
        {
            get
            {
                return Color.R;
            }
            set
            {
                Color = Color.FromArgb(Color.A, value, Color.G, Color.B);
            }
        }

        public int G
        {
            get
            {
                return Color.G;
            }
            set
            {
                Color = Color.FromArgb(Color.A, Color.R, value, Color.B);
            }
        }

        public int B
        {
            get
            {
                return Color.B;
            }
            set
            {
                Color = Color.FromArgb(Color.A, Color.R, Color.G, value);
            }
        }

        public int A
        {
            get
            {
                return Color.A;
            }
            set
            {
                Color = Color.FromArgb(value, Color.R, Color.G, Color.B);
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawColorDisplay(this, Color);
        }
    }
}