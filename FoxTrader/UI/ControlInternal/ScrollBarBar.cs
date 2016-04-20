using FoxTrader.UI.Control;
using OpenTK.Input;

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
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawScrollBarBar(this, m_held, IsHovered, IsHorizontal);
            base.Render(c_skin);
        }

        public override void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            base.OnMouseMoved(c_mouseEventArgs);

            if (!m_held)
            {
                return;
            }

            InvalidateParent();
        }

        public override void OnClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            base.OnClicked(c_mouseButtonEventArgs);
            InvalidateParent();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
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