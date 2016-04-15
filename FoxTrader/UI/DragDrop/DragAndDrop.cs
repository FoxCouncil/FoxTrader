using System;
using System.Drawing;
using System.Windows.Forms;
using FoxTrader.UI.Control;
using FoxTrader.UI.Platform;
using FoxTrader.UI.Skin;
using OpenTK.Input;
using MouseEventArgs = OpenTK.Input.MouseEventArgs;

namespace FoxTrader.UI.DragDrop
{
    /// <summary>Drag and drop handling</summary>
    internal static class DragAndDrop
    {
        public static Package m_currentPackage;
        public static GameControl m_hoveredControl;
        public static GameControl m_sourceControl;

        private static GameControl m_lastPressedControl;
        private static GameControl m_newHoveredControl;
        private static Point m_lastPressedPos;
        private static int m_mouseX;
        private static int m_mouseY;

        private static bool OnDrop(int c_x, int c_y)
        {
            var a_success = false;

            if (m_hoveredControl != null)
            {
                m_hoveredControl.DragAndDrop_HoverLeave(m_currentPackage);
                a_success = m_hoveredControl.DragAndDrop_HandleDrop(m_currentPackage, c_x, c_y);
            }

            // Report back to the source control, to tell it if we've been successful.
            m_sourceControl.DragAndDrop_EndDragging(a_success, c_x, c_y);

            m_currentPackage = null;
            m_sourceControl = null;

            return true;
        }

        private static bool ShouldStartDraggingControl(int c_x, int c_y)
        {
            // We're not holding a control down..
            if (m_lastPressedControl == null)
            {
                return false;
            }

            // Not been dragged far enough
            var a_length = Math.Abs(c_x - m_lastPressedPos.X) + Math.Abs(c_y - m_lastPressedPos.Y);

            if (a_length < 5)
            {
                return false;
            }

            // Create the dragging package
            m_currentPackage = m_lastPressedControl.DragAndDrop_GetPackage(m_lastPressedPos.X, m_lastPressedPos.Y);

            // We didn't create a package!
            if (m_currentPackage == null)
            {
                m_lastPressedControl = null;
                m_sourceControl = null;

                return false;
            }

            // Now we're dragging something!
            m_sourceControl = m_lastPressedControl;
            m_sourceControl.GetCanvas().MouseFocus = null;
            m_lastPressedControl = null;
            m_currentPackage.m_drawControl = null;

            // Some controls will want to decide whether they should be dragged at that moment.
            // This function is for them (it defaults to true)
            if (!m_sourceControl.DragAndDrop_ShouldStartDrag())
            {
                m_sourceControl = null;
                m_currentPackage = null;

                return false;
            }

            m_sourceControl.DragAndDrop_StartDragging(m_currentPackage, m_lastPressedPos.X, m_lastPressedPos.Y);

            return true;
        }

        private static void UpdateHoveredControl(GameControl c_control, int c_x, int c_y)
        {
            // We use this global variable to represent our hovered control
            // That way, if the new hovered control gets deleted in one of the
            // Hover callbacks, we won't be left with a hanging pointer.
            // This isn't ideal - but it's minimal.
            m_newHoveredControl = c_control;

            // Nothing to change..
            if (m_hoveredControl == m_newHoveredControl)
            {
                return;
            }

            // We changed - tell the old hovered control that it's no longer hovered.
            if (m_hoveredControl != null && m_hoveredControl != m_newHoveredControl)
            {
                m_hoveredControl.DragAndDrop_HoverLeave(m_currentPackage);
            }

            // If we're hovering where the control came from, just forget it.
            // By changing it to null here we're not going to show any error cursors
            // it will just do nothing if you drop it.
            if (m_newHoveredControl == m_sourceControl)
            {
                m_newHoveredControl = null;
            }

            // Check to see if the new potential control can accept this type of package.
            // If not, ignore it and show an error cursor.
            while (m_newHoveredControl != null && !m_newHoveredControl.DragAndDrop_CanAcceptPackage(m_currentPackage))
            {
                // We can't drop on this control, so lets try to drop
                // onto its parent..
                m_newHoveredControl = m_newHoveredControl.Parent;

                // Its parents are dead. We can't drop it here.
                // Show the NO WAY cursor.
                if (m_newHoveredControl == null)
                {
                    Neutral.SetCursor(Cursors.No);
                }
            }

            // Become out new hovered control
            m_hoveredControl = m_newHoveredControl;

            // If we exist, tell us that we've started hovering.
            m_hoveredControl?.DragAndDrop_HoverEnter(m_currentPackage, c_x, c_y);

            m_newHoveredControl = null;
        }

        public static bool Start(GameControl c_control, Package c_package)
        {
            if (m_currentPackage != null)
            {
                return false;
            }

            m_currentPackage = c_package;
            m_sourceControl = c_control;
            return true;
        }

        public static bool OnMouseButton(GameControl c_hoveredControl, MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (!c_mouseButtonEventArgs.IsPressed)
            {
                m_lastPressedControl = null;

                // Not carrying anything, allow normal actions
                if (m_currentPackage == null)
                {
                    return false;
                }

                // We were carrying something, drop it.
                OnDrop(c_mouseButtonEventArgs.X, c_mouseButtonEventArgs.Y);

                return true;
            }

            if (c_hoveredControl == null || !c_hoveredControl.DragAndDrop_Draggable())
            {
                return false;
            }

            // Store the last clicked on control. Don't do anything yet, 
            // we'll check it in OnMouseMoved, and if it moves further than
            // x pixels with the mouse down, we'll start to drag.
            m_lastPressedPos = new Point(c_mouseButtonEventArgs.X, c_mouseButtonEventArgs.Y);
            m_lastPressedControl = c_hoveredControl;

            return false;
        }

        public static void OnMouseMoved(GameControl c_hoveredControl, MouseEventArgs c_mouseEventArgs)
        {
            // Always keep these up to date, they're used to draw the dragged control.
            m_mouseX = c_mouseEventArgs.X;
            m_mouseY = c_mouseEventArgs.Y;

            // If we're not carrying anything, then check to see if we should
            // pick up from a control that we're holding down. If not, then forget it.
            if (m_currentPackage == null && !ShouldStartDraggingControl(m_mouseX, m_mouseY))
            {
                return;
            }

            // Swap to this new hovered control and notify them of the change.
            UpdateHoveredControl(c_hoveredControl, m_mouseX, m_mouseY);

            if (m_hoveredControl == null)
            {
                return;
            }

            // Update the hovered control every mouse move, so it can show where
            // the dropped control will land etc..
            m_hoveredControl.DragAndDrop_Hover(m_currentPackage, m_mouseX, m_mouseY);

            // Override the cursor - since it might have been set my underlying controls
            // Ideally this would show the 'being dragged' control. TODO
            Neutral.SetCursor(Cursors.Default);

            c_hoveredControl.Redraw();
        }

        internal static void RenderOverlay(Canvas c_canvas, SkinBase c_skin)
        {
            if (m_currentPackage == null || m_currentPackage.m_drawControl == null)
            {
                return;
            }

            var a_oldPosition = c_skin.Renderer.RenderOffset;

            c_skin.Renderer.AddRenderOffset(new Rectangle(m_mouseX - m_sourceControl.X - m_currentPackage.m_holdOffset.X, m_mouseY - m_sourceControl.Y - m_currentPackage.m_holdOffset.Y, 0, 0));

            m_currentPackage.m_drawControl.DoRender(c_skin);

            c_skin.Renderer.RenderOffset = a_oldPosition;
        }

        public static void ControlDeleted(GameControl c_control)
        {
            if (m_sourceControl == c_control)
            {
                m_sourceControl = null;
                m_currentPackage = null;
                m_hoveredControl = null;
                m_lastPressedControl = null;
            }

            if (m_lastPressedControl == c_control)
            {
                m_lastPressedControl = null;
            }

            if (m_hoveredControl == c_control)
            {
                m_hoveredControl = null;
            }

            if (m_newHoveredControl == c_control)
            {
                m_newHoveredControl = null;
            }
        }
    }
}