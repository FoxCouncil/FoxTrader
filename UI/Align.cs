using FoxTrader.UI.Control;

namespace FoxTrader.UI
{
    /// <summary>Manipulating a control's position according to its parent</summary>
    internal static class Align
    {
        /// <summary>Centers the control inside its parent</summary>
        /// <param name="c_control">Control to center</param>
        internal static void Center(GameControl c_control)
        {
            var a_parentControl = c_control.Parent;

            if (a_parentControl == null)
            {
                return;
            }

            var a_newX = a_parentControl.Padding.m_left + (((a_parentControl.Width - a_parentControl.Padding.m_left - a_parentControl.Padding.m_right) - c_control.Width) / 2);
            var a_newY = (a_parentControl.Height - c_control.Height) / 2;
            c_control.SetPosition(a_newX, a_newY);
        }

        /// <summary>Moves the control to the left of its parent</summary>
        /// <param name="c_control">Control to align left</param>
        internal static void AlignLeft(GameControl c_control)
        {
            var a_parentControl = c_control.Parent;

            if (a_parentControl != null)
            {
                c_control.SetPosition(a_parentControl.Padding.m_left, c_control.Y);
            }
        }

        /// <summary>Centers the control horizontally inside its parent</summary>
        /// <param name="c_control">The control to center horizontally</param>
        internal static void CenterHorizontally(GameControl c_control)
        {
            var a_parentControl = c_control.Parent;

            if (a_parentControl != null)
            {
                c_control.SetPosition(a_parentControl.Padding.m_left + (((a_parentControl.Width - a_parentControl.Padding.m_left - a_parentControl.Padding.m_right) - c_control.Width) / 2), c_control.Y);
            }
        }

        /// <summary>Moves the control to the right of its parent</summary>
        /// <param name="c_control">The control to move to the right</param>
        internal static void AlignRight(GameControl c_control)
        {
            var a_parentControl = c_control.Parent;

            if (a_parentControl != null)
            {
                c_control.SetPosition(a_parentControl.Width - c_control.Width - a_parentControl.Padding.m_right, c_control.Y);
            }
        }

        /// <summary>Moves the control to the top of its parent</summary>
        /// <param name="c_control">The control to move to the top</param>
        internal static void AlignTop(GameControl c_control)
        {
            c_control.SetPosition(c_control.X, 0);
        }

        /// <summary>Centers the control vertically inside its parent</summary>
        /// <param name="c_control">The control to center vertically</param>
        internal static void CenterVertically(GameControl c_control)
        {
            var a_parentControl = c_control.Parent;

            if (a_parentControl != null)
            {
                c_control.SetPosition(c_control.X, (a_parentControl.Height - c_control.Height) / 2);
            }
        }

        /// <summary>Moves the control to the bottom of its parent</summary>
        /// <param name="c_control">The control to move to the bottom</param>
        internal static void AlignBottom(GameControl c_control)
        {
            var a_parentControl = c_control.Parent;

            if (a_parentControl != null)
            {
                c_control.SetPosition(c_control.X, a_parentControl.Height - c_control.Height);
            }
        }

        /// <summary>Margin sensitive control placement below left aligned other control</summary>
        /// <param name="c_control">Control to place</param>
        /// <param name="c_anchor">Anchor control to left align control against</param>
        /// <param name="c_spacing">Optional spacing</param>
        internal static void PlaceDownLeft(GameControl c_control, GameControl c_anchor, int c_spacing = 0)
        {
            c_control.SetPosition(c_anchor.X, c_anchor.Bottom + c_spacing);
        }

        /// <summary>Margin sensitive control placement below right aligned other control</summary>
        /// <param name="c_control">Control to place</param>
        /// <param name="c_anchor">Anchor control to right align control against</param>
        /// <param name="c_spacing">Optional spacing</param>
        internal static void PlaceRightBottom(GameControl c_control, GameControl c_anchor, int c_spacing = 0)
        {
            c_control.SetPosition(c_anchor.Right + c_spacing, c_anchor.Y - c_control.Height + c_anchor.Height);
        }
    }
}