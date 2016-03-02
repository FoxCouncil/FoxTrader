using System.Drawing;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Base for controls that can be dragged by mouse</summary>
    internal class Dragger : GameControl
    {
        protected bool m_held;
        protected Point m_holdPos;
        protected GameControl m_target;

        /// <summary>Initializes a new instance of the <see cref="Dragger" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Dragger(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = true;
            m_held = false;
        }

        internal GameControl Target
        {
            get
            {
                return m_target;
            }
            set
            {
                m_target = value;
            }
        }

        /// <summary>Indicates if the control is being dragged</summary>
        public bool IsHeld => m_held;

        /// <summary>Event invoked when the control position has been changed</summary>
        public event DragEventHandler Dragged;

        /// <summary>Handler invoked on mouse click (left) event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_down">If set to <c>true</c> mouse button is down</param>
        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            if (null == m_target)
            {
                return;
            }

            if (c_down)
            {
                m_held = true;
                m_holdPos = m_target.CanvasPosToLocal(new Point(c_x, c_y));
                FoxTraderWindow.Instance.MouseFocus = this;
            }
            else
            {
                m_held = false;

                FoxTraderWindow.Instance.MouseFocus = null;
            }
        }

        /// <summary>Handler invoked on mouse moved event</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <param name="c_dx">X change</param>
        /// <param name="c_dy">Y change</param>
        protected override void OnMouseMoved(int c_x, int c_y, int c_dx, int c_dy)
        {
            if (null == m_target)
            {
                return;
            }
            if (!m_held)
            {
                return;
            }

            var a_p = new Point(c_x - m_holdPos.X, c_y - m_holdPos.Y);

            // Translate to parent
            if (m_target.Parent != null)
            {
                a_p = m_target.Parent.CanvasPosToLocal(a_p);
            }

            //m_Target->SetPosition( p.x, p.y );
            m_target.MoveTo(a_p.X, a_p.Y);
            if (Dragged != null)
            {
                Dragged.Invoke(this);
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
        }
    }
}