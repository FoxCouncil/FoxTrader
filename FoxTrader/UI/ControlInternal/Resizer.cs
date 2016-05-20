using System.Windows.Forms;
using FoxTrader.UI.Control;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.ControlInternal
{
    /// <summary>Grab point for resizing</summary>
    internal class Resizer : Dragger
    {
        private Pos m_resizeDir;

        /// <summary>Initializes a new instance of the <see cref="Resizer" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Resizer(GameControl c_parentControl) : base(c_parentControl)
        {
            m_resizeDir = Pos.Left;
            MouseInputEnabled = true;
            SetSize(6, 6);
            Target = c_parentControl;
        }

        /// <summary>Gets or sets the sizing direction</summary>
        public Pos ResizeDir
        {
            set
            {
                m_resizeDir = value;

                if ((0 != (value & Pos.Left) && 0 != (value & Pos.Top)) || (0 != (value & Pos.Right) && 0 != (value & Pos.Bottom)))
                {
                    Cursor = Cursors.SizeNWSE;
                    return;
                }
                if ((0 != (value & Pos.Right) && 0 != (value & Pos.Top)) || (0 != (value & Pos.Left) && 0 != (value & Pos.Bottom)))
                {
                    Cursor = Cursors.SizeNESW;
                    return;
                }
                if (0 != (value & Pos.Right) || 0 != (value & Pos.Left))
                {
                    Cursor = Cursors.SizeWE;
                    return;
                }
                if (0 != (value & Pos.Top) || 0 != (value & Pos.Bottom))
                {
                    Cursor = Cursors.SizeNS;
                }
            }
        }

        /// <summary>Invoked when the control has been resized</summary>
        public event SizeEventHandler Resized;

        public override void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            if (m_target == null)
            {
                return;
            }

            if (!m_held)
            {
                return;
            }

            var a_bounds = m_target.Bounds;
            var a_min = m_target.MinimumSize;
            var a_pCursorPos = m_target.CanvasPosToLocal(c_mouseEventArgs.Position);
            var a_delta = m_target.LocalPosToCanvas(m_holdPos);

            a_delta.X -= c_mouseEventArgs.X;
            a_delta.Y -= c_mouseEventArgs.Y;

            if (0 != (m_resizeDir & Pos.Left))
            {
                a_bounds.X -= a_delta.X;
                a_bounds.Width += a_delta.X;

                // Conform to minimum size here so we don't
                // go all weird when we snap it in the base conrt

                if (a_bounds.Width < a_min.Width)
                {
                    var a_diff = a_min.Width - a_bounds.Width;
                    a_bounds.Width += a_diff;
                    a_bounds.X -= a_diff;
                }
            }

            if (0 != (m_resizeDir & Pos.Top))
            {
                a_bounds.Y -= a_delta.Y;
                a_bounds.Height += a_delta.Y;

                // Conform to minimum size here so we don't
                // go all weird when we snap it in the base conrt

                if (a_bounds.Height < a_min.Height)
                {
                    var a_diff = a_min.Height - a_bounds.Height;
                    a_bounds.Height += a_diff;
                    a_bounds.Y -= a_diff;
                }
            }

            if (0 != (m_resizeDir & Pos.Right))
            {
                // This is complicated.
                // Basically we want to use the HoldPos, so it doesn't snap to the edge of the control
                // But we need to move the HoldPos with the window movement. Yikes.
                // I actually think this might be a big hack around the way this control works with regards
                // to the holdpos being on the parent panel.

                var a_woff = a_bounds.Width - m_holdPos.X;
                var a_diff = a_bounds.Width;

                a_bounds.Width = a_pCursorPos.X + a_woff;

                if (a_bounds.Width < a_min.Width)
                {
                    a_bounds.Width = a_min.Width;
                }

                a_diff -= a_bounds.Width;

                m_holdPos.X -= a_diff;
            }

            if (0 != (m_resizeDir & Pos.Bottom))
            {
                var a_hoff = a_bounds.Height - m_holdPos.Y;
                var a_diff = a_bounds.Height;

                a_bounds.Height = a_pCursorPos.Y + a_hoff;

                if (a_bounds.Height < a_min.Height)
                {
                    a_bounds.Height = a_min.Height;
                }

                a_diff -= a_bounds.Height;

                m_holdPos.Y -= a_diff;
            }

            m_target.SetBounds(a_bounds);

            Resized?.Invoke(this);
        }
    }
}