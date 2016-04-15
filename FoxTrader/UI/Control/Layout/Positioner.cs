using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control.Layout
{
    /// <summary>Helper control that positions its children in a specific way</summary>
    internal class Positioner : GameControl
    {
        /// <summary>Initializes a new instance of the <see cref="Positioner" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Positioner(GameControl c_parentControl) : base(c_parentControl)
        {
            Pos = Pos.Left | Pos.Top;
        }

        /// <summary>Children position</summary>
        public Pos Pos
        {
            get;
            set;
        }

        /// <summary>Function invoked after layout</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void PostLayout(SkinBase c_skin)
        {
            foreach (var a_child in Children) // ok?
            {
                a_child.SetRelativePosition(Pos);
            }
        }
    }

    /// <summary>Helper class that centers all its children</summary>
    internal class Center : Positioner
    {
        /// <summary>Initializes a new instance of the <see cref="Center" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Center(GameControl c_parentControl) : base(c_parentControl)
        {
            Pos = Pos.Center;
        }
    }
}