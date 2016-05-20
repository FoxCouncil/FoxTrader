using System;
using System.Drawing;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.DragDrop;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Base for dockable containers</summary>
    internal class Panel : GameControl
    {
        private Panel m_bottom;

        // Only CHILD dockpanels have a tabcontrol.
        private DockedTabControl m_dockedTabControl;

        private bool m_drawHover;
        private bool m_dropFar;
        private Rectangle m_hoverRect;
        private Panel m_left;
        private Panel m_right;
        private Resizer m_sizer;
        private Panel m_top;

        /// <summary>Initializes a new instance of the <see cref="Panel" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Panel(GameControl c_parentControl) : base(c_parentControl)
        {
            Padding = Padding.kOne;
            SetSize(200, 200);
        }

        // TODO: dock events?

        /// <summary>Control docked on the left side</summary>
        public Panel LeftPanel => GetChildPanel(Pos.Left);

        /// <summary>Control docked on the right side</summary>
        public Panel RightPanel => GetChildPanel(Pos.Right);

        /// <summary>Control docked on the top side</summary>
        public Panel TopPanel => GetChildPanel(Pos.Top);

        /// <summary>Control docked on the bottom side</summary>
        public Panel BottomPanel => GetChildPanel(Pos.Bottom);

        public TabControl TabControl => m_dockedTabControl;

        /// <summary>Indicates whether the control contains any docked children</summary>
        public virtual bool IsEmpty
        {
            get
            {
                if (m_dockedTabControl != null && m_dockedTabControl.TabCount > 0)
                {
                    return false;
                }

                if (m_left != null && !m_left.IsEmpty)
                {
                    return false;
                }
                if (m_right != null && !m_right.IsEmpty)
                {
                    return false;
                }
                if (m_top != null && !m_top.IsEmpty)
                {
                    return false;
                }
                if (m_bottom != null && !m_bottom.IsEmpty)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>Initializes an inner docked control for the specified position</summary>
        /// <param name="c_pos">Dock position</param>
        protected virtual void SetupChildPanel(Pos c_pos)
        {
            if (m_dockedTabControl == null)
            {
                m_dockedTabControl = new DockedTabControl(this);
                m_dockedTabControl.TabRemoved += OnTabRemoved;
                m_dockedTabControl.TabStripPosition = Pos.Bottom;
                m_dockedTabControl.TitleBarVisible = true;
            }

            Dock = c_pos;

            Pos a_sizeDir;
            if (c_pos == Pos.Right)
            {
                a_sizeDir = Pos.Left;
            }
            else if (c_pos == Pos.Left)
            {
                a_sizeDir = Pos.Right;
            }
            else if (c_pos == Pos.Top)
            {
                a_sizeDir = Pos.Bottom;
            }
            else if (c_pos == Pos.Bottom)
            {
                a_sizeDir = Pos.Top;
            }
            else
            {
                throw new ArgumentException("Invalid dock", "c_pos");
            }

            if (m_sizer != null)
            {
                m_sizer.Dispose();
            }
            m_sizer = new Resizer(this);
            m_sizer.Dock = a_sizeDir;
            m_sizer.ResizeDir = a_sizeDir;
            m_sizer.SetSize(2, 2);
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
        }

        /// <summary>Gets an inner docked control for the specified position</summary>
        /// <param name="c_pos"></param>
        /// <returns></returns>
        protected virtual Panel GetChildPanel(Pos c_pos)
        {
            // TODO: verify
            Panel a_dock = null;
            switch (c_pos)
            {
                case Pos.Left:
                if (m_left == null)
                {
                    m_left = new Panel(this);
                    m_left.SetupChildPanel(c_pos);
                }
                a_dock = m_left;
                break;

                case Pos.Right:
                if (m_right == null)
                {
                    m_right = new Panel(this);
                    m_right.SetupChildPanel(c_pos);
                }
                a_dock = m_right;
                break;

                case Pos.Top:
                if (m_top == null)
                {
                    m_top = new Panel(this);
                    m_top.SetupChildPanel(c_pos);
                }
                a_dock = m_top;
                break;

                case Pos.Bottom:
                if (m_bottom == null)
                {
                    m_bottom = new Panel(this);
                    m_bottom.SetupChildPanel(c_pos);
                }
                a_dock = m_bottom;
                break;
            }

            if (a_dock != null)
            {
                a_dock.IsHidden = false;
            }

            return a_dock;
        }

        /// <summary>Calculates dock direction from dragdrop coordinates</summary>
        /// <param name="c_x">X coordinate</param>
        /// <param name="c_y">Y coordinate</param>
        /// <returns>Dock direction</returns>
        protected virtual Pos GetDroppedTabDirection(int c_x, int c_y)
        {
            var a_w = Width;
            var a_h = Height;
            var a_top = c_y / (float)a_h;
            var a_left = c_x / (float)a_w;
            var a_right = (a_w - c_x) / (float)a_w;
            var a_bottom = (a_h - c_y) / (float)a_h;
            var a_minimum = Math.Min(Math.Min(Math.Min(a_top, a_left), a_right), a_bottom);

            m_dropFar = (a_minimum < 0.2f);

            if (a_minimum > 0.3f)
            {
                return Pos.Fill;
            }

            if (a_top == a_minimum && (null == m_top || m_top.IsHidden))
            {
                return Pos.Top;
            }
            if (a_left == a_minimum && (null == m_left || m_left.IsHidden))
            {
                return Pos.Left;
            }
            if (a_right == a_minimum && (null == m_right || m_right.IsHidden))
            {
                return Pos.Right;
            }
            if (a_bottom == a_minimum && (null == m_bottom || m_bottom.IsHidden))
            {
                return Pos.Bottom;
            }

            return Pos.Fill;
        }

        public override bool DragAndDrop_CanAcceptPackage(Package c_p)
        {
            // A TAB button dropped 
            if (c_p.m_name == "TabButtonMove")
            {
                return true;
            }

            // a TAB window dropped
            if (c_p.m_name == "TabWindowMove")
            {
                return true;
            }

            return false;
        }

        public override bool DragAndDrop_HandleDrop(Package c_p, int c_x, int c_y)
        {
            var a_pos = CanvasPosToLocal(new Point(c_x, c_y));
            var a_dir = GetDroppedTabDirection(a_pos.X, a_pos.Y);

            var a_addTo = m_dockedTabControl;
            if (a_dir == Pos.Fill && a_addTo == null)
            {
                return false;
            }

            if (a_dir != Pos.Fill)
            {
                var a_dock = GetChildPanel(a_dir);
                a_addTo = a_dock.m_dockedTabControl;

                if (!m_dropFar)
                {
                    a_dock.BringToFront();
                }
                else
                {
                    a_dock.SendToBack();
                }
            }

            if (c_p.m_name == "TabButtonMove")
            {
                var a_tabButton = DragAndDrop.m_sourceControl as TabButton;
                if (null == a_tabButton)
                {
                    return false;
                }

                a_addTo.AddPage(a_tabButton);
            }

            if (c_p.m_name == "TabWindowMove")
            {
                var a_tabControl = DragAndDrop.m_sourceControl as DockedTabControl;
                if (null == a_tabControl)
                {
                    return false;
                }
                if (a_tabControl == a_addTo)
                {
                    return false;
                }

                a_tabControl.MoveTabsTo(a_addTo);
            }

            Invalidate();

            return true;
        }

        protected virtual void OnTabRemoved(GameControl c_control)
        {
            DoRedundancyCheck();
            DoConsolidateCheck();
        }

        protected virtual void DoRedundancyCheck()
        {
            if (!IsEmpty)
            {
                return;
            }

            var a_pDockParent = Parent as Panel;
            if (null == a_pDockParent)
            {
                return;
            }

            a_pDockParent.OnRedundantChildPanel(this);
        }

        protected virtual void DoConsolidateCheck()
        {
            if (IsEmpty)
            {
                return;
            }
            if (null == m_dockedTabControl)
            {
                return;
            }
            if (m_dockedTabControl.TabCount > 0)
            {
                return;
            }

            if (m_bottom != null && !m_bottom.IsEmpty)
            {
                m_bottom.m_dockedTabControl.MoveTabsTo(m_dockedTabControl);
                return;
            }

            if (m_top != null && !m_top.IsEmpty)
            {
                m_top.m_dockedTabControl.MoveTabsTo(m_dockedTabControl);
                return;
            }

            if (m_left != null && !m_left.IsEmpty)
            {
                m_left.m_dockedTabControl.MoveTabsTo(m_dockedTabControl);
                return;
            }

            if (m_right != null && !m_right.IsEmpty)
            {
                m_right.m_dockedTabControl.MoveTabsTo(m_dockedTabControl);
            }
        }

        protected virtual void OnRedundantChildPanel(Panel c_panel)
        {
            c_panel.IsHidden = true;
            DoRedundancyCheck();
            DoConsolidateCheck();
        }

        public override void DragAndDrop_HoverEnter(Package c_p, int c_x, int c_y)
        {
            m_drawHover = true;
        }

        public override void DragAndDrop_HoverLeave(Package c_p)
        {
            m_drawHover = false;
        }

        public override void DragAndDrop_Hover(Package c_p, int c_x, int c_y)
        {
            var a_pos = CanvasPosToLocal(new Point(c_x, c_y));
            var a_dir = GetDroppedTabDirection(a_pos.X, a_pos.Y);

            if (a_dir == Pos.Fill)
            {
                if (null == m_dockedTabControl)
                {
                    m_hoverRect = Rectangle.Empty;
                    return;
                }

                m_hoverRect = InnerBounds;
                return;
            }

            m_hoverRect = RenderBounds;

            var a_helpBarWidth = 0;

            if (a_dir == Pos.Left)
            {
                a_helpBarWidth = (int)(m_hoverRect.Width * 0.25f);
                m_hoverRect.Width = a_helpBarWidth;
            }

            if (a_dir == Pos.Right)
            {
                a_helpBarWidth = (int)(m_hoverRect.Width * 0.25f);
                m_hoverRect.X = m_hoverRect.Width - a_helpBarWidth;
                m_hoverRect.Width = a_helpBarWidth;
            }

            if (a_dir == Pos.Top)
            {
                a_helpBarWidth = (int)(m_hoverRect.Height * 0.25f);
                m_hoverRect.Height = a_helpBarWidth;
            }

            if (a_dir == Pos.Bottom)
            {
                a_helpBarWidth = (int)(m_hoverRect.Height * 0.25f);
                m_hoverRect.Y = m_hoverRect.Height - a_helpBarWidth;
                m_hoverRect.Height = a_helpBarWidth;
            }

            if ((a_dir == Pos.Top || a_dir == Pos.Bottom) && !m_dropFar)
            {
                if (m_left != null && m_left.IsVisible)
                {
                    m_hoverRect.X += m_left.Width;
                    m_hoverRect.Width -= m_left.Width;
                }

                if (m_right != null && m_right.IsVisible)
                {
                    m_hoverRect.Width -= m_right.Width;
                }
            }

            if ((a_dir == Pos.Left || a_dir == Pos.Right) && !m_dropFar)
            {
                if (m_top != null && m_top.IsVisible)
                {
                    m_hoverRect.Y += m_top.Height;
                    m_hoverRect.Height -= m_top.Height;
                }

                if (m_bottom != null && m_bottom.IsVisible)
                {
                    m_hoverRect.Height -= m_bottom.Height;
                }
            }
        }

        /// <summary>Renders over the actual control (overlays)</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderOver(Skin c_skin)
        {
            if (!m_drawHover)
            {
                return;
            }

            var a_render = c_skin.Renderer;
            a_render.DrawColor = Color.FromArgb(20, 255, 200, 255);
            a_render.DrawFilledRect(RenderBounds);

            if (m_hoverRect.Width == 0)
            {
                return;
            }

            a_render.DrawColor = Color.FromArgb(100, 255, 200, 255);
            a_render.DrawFilledRect(m_hoverRect);

            a_render.DrawColor = Color.FromArgb(200, 255, 200, 255);
            a_render.DrawLinedRect(m_hoverRect);
        }
    }
}