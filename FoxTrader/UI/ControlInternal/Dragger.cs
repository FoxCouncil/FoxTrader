using System.Drawing;
using FoxTrader.UI.Control;
using OpenTK.Input;

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

        public override void OnMouseDown(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (m_target == null)
            {
                return;
            }

            m_held = true;
            m_holdPos = m_target.CanvasPosToLocal(c_mouseButtonEventArgs.Position);
            GetCanvas().MouseFocus = this;
        }

        public override void OnMouseUp(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (m_target == null)
            {
                return;
            }

            m_held = false;

            GetCanvas().MouseFocus = null;
        }

        public override void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            base.OnMouseMoved(c_mouseEventArgs);

            if (null == m_target)
            {
                return;
            }
            if (!m_held)
            {
                return;
            }

            var a_p = new Point(c_mouseEventArgs.X - m_holdPos.X, c_mouseEventArgs.Y - m_holdPos.Y);

            // Translate to parent
            if (m_target.Parent != null)
            {
                a_p = m_target.Parent.CanvasPosToLocal(a_p);
            }

            m_target.MoveTo(a_p.X, a_p.Y);

            Dragged?.Invoke(this);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
        }
    }
}