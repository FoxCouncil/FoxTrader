using System;
using System.Drawing;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.DragDrop;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Tab strip - groups TabButtons and allows reordering</summary>
    internal class TabStrip : GameControl
    {
        private GameControl m_tabDragControl;

        /// <summary>Initializes a new instance of the <see cref="TabStrip" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TabStrip(GameControl c_parentControl) : base(c_parentControl)
        {
            AllowReorder = false;
        }

        /// <summary>Determines whether it is possible to reorder tabs by mouse dragging</summary>
        public bool AllowReorder
        {
            get;
            set;
        }

        /// <summary>Determines whether the control should be clipped to its bounds while rendering</summary>
        protected override bool ShouldClip => false;

        /// <summary>Strip position (top/left/right/bottom)</summary>
        public Pos StripPosition
        {
            get
            {
                return Dock;
            }
            set
            {
                Dock = value;
                if (Dock == Pos.Top)
                {
                    Padding = new Padding(5, 0, 0, 0);
                }
                if (Dock == Pos.Left)
                {
                    Padding = new Padding(0, 5, 0, 0);
                }
                if (Dock == Pos.Bottom)
                {
                    Padding = new Padding(5, 0, 0, 0);
                }
                if (Dock == Pos.Right)
                {
                    Padding = new Padding(0, 5, 0, 0);
                }
            }
        }

        public override bool DragAndDrop_HandleDrop(Package c_p, int c_x, int c_y)
        {
            var a_localPos = CanvasPosToLocal(new Point(c_x, c_y));

            var a_button = DragAndDrop.m_sourceControl as TabButton;
            var a_tabControl = Parent as TabControl;
            if (a_tabControl != null && a_button != null)
            {
                if (a_button.TabControl != a_tabControl)
                {
                    // We've moved tab controls!
                    a_tabControl.AddPage(a_button);
                }
            }

            var a_droppedOn = GetControlAt(a_localPos.X, a_localPos.Y);
            if (a_droppedOn != null)
            {
                var a_dropPos = a_droppedOn.CanvasPosToLocal(new Point(c_x, c_y));
                DragAndDrop.m_sourceControl.BringNextToControl(a_droppedOn, a_dropPos.X > a_droppedOn.Width / 2);
            }
            else
            {
                DragAndDrop.m_sourceControl.BringToFront();
            }
            return true;
        }

        public override bool DragAndDrop_CanAcceptPackage(Package c_p)
        {
            if (!AllowReorder)
            {
                return false;
            }

            if (c_p.m_name == "TabButtonMove")
            {
                return true;
            }

            return false;
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            var a_largestTab = new Point(5, 5);

            var a_num = 0;
            foreach (var a_child in Children)
            {
                var a_button = a_child as TabButton;
                if (null == a_button)
                {
                    continue;
                }

                a_button.SizeToContents();

                var a_m = new Margin();
                var a_notFirst = a_num > 0 ? -1 : 0;

                if (Dock == Pos.Top)
                {
                    a_m.m_left = a_notFirst;
                    a_button.Dock = Pos.Left;
                }

                if (Dock == Pos.Left)
                {
                    a_m.m_top = a_notFirst;
                    a_button.Dock = Pos.Top;
                }

                if (Dock == Pos.Right)
                {
                    a_m.m_top = a_notFirst;
                    a_button.Dock = Pos.Top;
                }

                if (Dock == Pos.Bottom)
                {
                    a_m.m_left = a_notFirst;
                    a_button.Dock = Pos.Left;
                }

                a_largestTab.X = Math.Max(a_largestTab.X, a_button.Width);
                a_largestTab.Y = Math.Max(a_largestTab.Y, a_button.Height);

                a_button.Margin = a_m;
                a_num++;
            }

            if (Dock == Pos.Top || Dock == Pos.Bottom)
            {
                SetSize(Width, a_largestTab.Y);
            }

            if (Dock == Pos.Left || Dock == Pos.Right)
            {
                SetSize(a_largestTab.X, Height);
            }

            base.OnLayout(c_skin);
        }

        public override void DragAndDrop_HoverEnter(Package c_p, int c_x, int c_y)
        {
            if (m_tabDragControl != null)
            {
                throw new InvalidOperationException("ERROR! TabStrip::DragAndDrop_HoverEnter");
            }

            m_tabDragControl = new Highlight(this);
            m_tabDragControl.MouseInputEnabled = false;
            m_tabDragControl.SetSize(3, Height);
        }

        public override void DragAndDrop_HoverLeave(Package c_p)
        {
            if (m_tabDragControl != null)
            {
                RemoveChild(m_tabDragControl, false);
                m_tabDragControl.Dispose();
            }
            m_tabDragControl = null;
        }

        public override void DragAndDrop_Hover(Package c_p, int c_x, int c_y)
        {
            var a_localPos = CanvasPosToLocal(new Point(c_x, c_y));

            var a_droppedOn = GetControlAt(a_localPos.X, a_localPos.Y);
            if (a_droppedOn != null && a_droppedOn != this)
            {
                var a_dropPos = a_droppedOn.CanvasPosToLocal(new Point(c_x, c_y));
                m_tabDragControl.SetBounds(new Rectangle(0, 0, 3, Height));
                m_tabDragControl.BringToFront();
                m_tabDragControl.SetPosition(a_droppedOn.X - 1, 0);

                if (a_dropPos.X > a_droppedOn.Width / 2)
                {
                    m_tabDragControl.MoveBy(a_droppedOn.Width - 1, 0);
                }
                m_tabDragControl.Dock = Pos.None;
            }
            else
            {
                m_tabDragControl.Dock = Pos.Left;
                m_tabDragControl.BringToFront();
            }
        }
    }
}