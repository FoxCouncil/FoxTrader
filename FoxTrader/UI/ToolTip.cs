using System.Drawing;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;
using OpenTK.Input;

namespace FoxTrader.UI
{
    /// <summary>Tooltip handling</summary>
    internal static class ToolTip
    {
        private static GameControl m_toolTip;

        /// <summary>Enables tooltip display for the specified control</summary>
        /// <param name="c_control">Target control</param>
        public static void Enable(GameControl c_control)
        {
            if (c_control.ToolTip != null)
            {
                m_toolTip = c_control;
            }
        }

        /// <summary>Disables tooltip display for the specified control</summary>
        /// <param name="c_control">Target control</param>
        public static void Disable(GameControl c_control)
        {
            if (m_toolTip == c_control)
            {
                m_toolTip = null;
            }
        }

        /// <summary>Disables tooltip display for the specified control</summary>
        /// <param name="c_control">Target control</param>
        public static void ControlDeleted(GameControl c_control)
        {
            Disable(c_control);
        }

        /// <summary>Renders the currently visible tooltip</summary>
        /// <param name="c_skin">A skin to render in</param>
        public static void RenderToolTip(SkinBase c_skin)
        {
            if (m_toolTip != null)
            {
                var a_render = c_skin.Renderer;
                var a_oldRenderOffset = a_render.RenderOffset;
                var a_mousePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                var a_bounds = m_toolTip.ToolTip.Bounds;
                var a_offset = Util.FloatRect(a_mousePos.X - a_bounds.Width * 0.5f, a_mousePos.Y - a_bounds.Height - 10, a_bounds.Width, a_bounds.Height);

                a_offset = Util.ClampRectToRect(a_offset, m_toolTip.GetCanvas().Bounds);

                //Calculate offset on screen bounds
                a_render.AddRenderOffset(a_offset);
                a_render.EndClip();

                c_skin.DrawToolTip(m_toolTip.ToolTip);
                m_toolTip.ToolTip.DoRender(c_skin);

                a_render.RenderOffset = a_oldRenderOffset;
            }
        }
    }
}