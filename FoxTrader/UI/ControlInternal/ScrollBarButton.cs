using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Scrollbar button</summary>
    internal class ScrollBarButton : Button
    {
        private Pos m_direction;

        /// <summary>Initializes a new instance of the <see cref="ScrollBarButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ScrollBarButton(GameControl c_parentControl) : base(c_parentControl)
        {
            SetDirectionUp();
        }

        public virtual void SetDirectionUp()
        {
            m_direction = Pos.Top;
        }

        public virtual void SetDirectionDown()
        {
            m_direction = Pos.Bottom;
        }

        public virtual void SetDirectionLeft()
        {
            m_direction = Pos.Left;
        }

        public virtual void SetDirectionRight()
        {
            m_direction = Pos.Right;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawScrollButton(this, m_direction, IsDepressed, IsHovered, IsDisabled);
        }
    }
}