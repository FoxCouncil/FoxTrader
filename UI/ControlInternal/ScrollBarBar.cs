using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Scrollbar bar</summary>
    internal class ScrollBarBar : Dragger
    {
        /// <summary>Initializes a new instance of the <see cref="ScrollBarBar" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ScrollBarBar(GameControl c_parentControl) : base(c_parentControl)
        {
            RestrictToParent = true;
            Target = this;
        }

        /// <summary>Indicates whether the bar is horizontal</summary>
        public bool IsHorizontal
        {
            get;
            set;
        }

        /// <summary>Indicates whether the bar is vertical</summary>
        public bool IsVertical
        {
            get
            {
                return !IsHorizontal;
            }
            set
            {
                IsHorizontal = !value;
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawScrollBarBar(this, m_held, IsHovered, IsHorizontal);
            base.Render(c_skin);
        }

        /// <summary>Handler invoked on mouse moved event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_dx">X change</param>
        /// <param name="c_dy">Y change</param>
        protected override void OnMouseMoved(int c_x, int c_y, int c_dx, int c_dy)
        {
            base.OnMouseMoved(c_x, c_y, c_dx, c_dy);
            if (!m_held)
            {
                return;
            }

            InvalidateParent();
        }

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            base.OnMouseClickedLeft(c_x, c_y, c_down);
            InvalidateParent();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            if (null == Parent)
            {
                return;
            }

            //Move to our current position to force clamping - is this a hack?
            MoveTo(X, Y);
        }
    }
}