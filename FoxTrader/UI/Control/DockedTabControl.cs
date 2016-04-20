using FoxTrader.UI.DragDrop;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Docked tab control</summary>
    internal class DockedTabControl : TabControl
    {
        private readonly TabTitleBar m_titleBar;

        /// <summary>Initializes a new instance of the <see cref="DockedTabControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public DockedTabControl(GameControl c_parentControl) : base(c_parentControl)
        {
            Dock = Pos.Fill;

            m_titleBar = new TabTitleBar(this);
            m_titleBar.Dock = Pos.Top;
            m_titleBar.IsHidden = true;
        }

        /// <summary>Determines whether the title bar is visible</summary>
        public bool TitleBarVisible
        {
            get
            {
                return !m_titleBar.IsHidden;
            }
            set
            {
                m_titleBar.IsHidden = !value;
            }
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            TabStrip.IsHidden = (TabCount <= 1);
            UpdateTitleBar();
            base.OnLayout(c_skin);
        }

        private void UpdateTitleBar()
        {
            if (CurrentButton == null)
            {
                return;
            }

            m_titleBar.UpdateFromTab(CurrentButton);
        }

        public override void DragAndDrop_StartDragging(Package c_package, int c_x, int c_y)
        {
            base.DragAndDrop_StartDragging(c_package, c_x, c_y);

            IsHidden = true;
            // This hiding our parent thing is kind of lousy.
            Parent.IsHidden = true;
        }

        public override void DragAndDrop_EndDragging(bool c_success, int c_x, int c_y)
        {
            IsHidden = false;
            if (!c_success)
            {
                Parent.IsHidden = false;
            }
        }

        public void MoveTabsTo(DockedTabControl c_target)
        {
            var a_children = TabStrip.Children.ToArray(); // copy because collection will be modified
            foreach (var a_child in a_children)
            {
                var a_button = a_child as TabButton;
                if (a_button == null)
                {
                    continue;
                }
                c_target.AddPage(a_button);
            }
            Invalidate();
        }
    }
}