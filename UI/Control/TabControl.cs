using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Control with multiple tabs that can be reordered and dragged</summary>
    internal class TabControl : GameControl
    {
        private readonly ScrollBarButton[] m_scroll;
        private int m_scrollOffset;

        /// <summary>Initializes a new instance of the <see cref="TabControl" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TabControl(GameControl c_parentControl) : base(c_parentControl)
        {
            m_scroll = new ScrollBarButton[2];
            m_scrollOffset = 0;

            TabStrip = new TabStrip(this);
            TabStrip.StripPosition = Pos.Top;

            // Make this some special control?
            m_scroll[0] = new ScrollBarButton(this);
            m_scroll[0].SetDirectionLeft();
            m_scroll[0].Clicked += ScrollPressedLeft;
            m_scroll[0].SetSize(14, 16);

            m_scroll[1] = new ScrollBarButton(this);
            m_scroll[1].SetDirectionRight();
            m_scroll[1].Clicked += ScrollPressedRight;
            m_scroll[1].SetSize(14, 16);

            m_innerControl = new TabControlInner(this);
            m_innerControl.Dock = Pos.Fill;
            m_innerControl.SendToBack();

            IsTabable = false;
        }

        /// <summary>Determines if tabs can be reordered by dragging</summary>
        public bool AllowReorder
        {
            get
            {
                return TabStrip.AllowReorder;
            }
            set
            {
                TabStrip.AllowReorder = value;
            }
        }

        /// <summary>Currently active tab button</summary>
        public TabButton CurrentButton
        {
            get;
            private set;
        }

        /// <summary>Current tab strip position</summary>
        public Pos TabStripPosition
        {
            get
            {
                return TabStrip.StripPosition;
            }
            set
            {
                TabStrip.StripPosition = value;
            }
        }

        /// <summary>Tab strip</summary>
        public TabStrip TabStrip
        {
            get;
        }

        /// <summary>Number of tabs in the control</summary>
        public int TabCount => TabStrip.Children.Count;

        /// <summary>Invoked when a tab has been added</summary>
        public event TabEventHandler TabAdded;

        /// <summary>Invoked when a tab has been removed</summary>
        public event TabEventHandler TabRemoved;

        /// <summary>Adds a new page/tab</summary>
        /// <param name="c_label">Tab label</param>
        /// <param name="c_page">Page contents</param>
        /// <returns>Newly created control</returns>
        public TabButton AddPage(string c_label, GameControl c_page = null)
        {
            if (null == c_page)
            {
                c_page = new GameControl(this);
            }
            else
            {
                c_page.Parent = this;
            }

            var a_button = new TabButton(TabStrip);
            a_button.SetText(c_label);
            a_button.Page = c_page;
            a_button.IsTabable = false;

            AddPage(a_button);
            return a_button;
        }

        /// <summary>Adds a page/tab</summary>
        /// <param name="c_button">Page to add. (well, it's a TabButton which is a parent to the page)</param>
        public void AddPage(TabButton c_button)
        {
            var a_page = c_button.Page;
            a_page.Parent = this;
            a_page.IsHidden = true;
            a_page.Margin = new Margin(6, 6, 6, 6);
            a_page.Dock = Pos.Fill;

            c_button.Parent = TabStrip;
            c_button.Dock = Pos.Left;
            c_button.SizeToContents();
            if (c_button.TabControl != null)
            {
                c_button.TabControl.UnsubscribeTabEvent(c_button);
            }
            c_button.TabControl = this;
            c_button.Clicked += OnTabPressed;

            if (null == CurrentButton)
            {
                c_button.Press();
            }

            if (TabAdded != null)
            {
                TabAdded.Invoke(this);
            }

            Invalidate();
        }

        private void UnsubscribeTabEvent(TabButton c_button)
        {
            c_button.Clicked -= OnTabPressed;
        }

        /// <summary>Handler for tab selection</summary>
        /// <param name="c_control">Event source (TabButton)</param>
        internal virtual void OnTabPressed(GameControl c_control)
        {
            var a_button = c_control as TabButton;
            if (null == a_button)
            {
                return;
            }

            var a_page = a_button.Page;
            if (null == a_page)
            {
                return;
            }

            if (CurrentButton == a_button)
            {
                return;
            }

            if (null != CurrentButton)
            {
                var a_page2 = CurrentButton.Page;
                if (a_page2 != null)
                {
                    a_page2.IsHidden = true;
                }
                CurrentButton.Redraw();
                CurrentButton = null;
            }

            CurrentButton = a_button;

            a_page.IsHidden = false;

            TabStrip.Invalidate();
            Invalidate();
        }

        /// <summary>Function invoked after layout</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void PostLayout(SkinBase c_skin)
        {
            base.PostLayout(c_skin);
            HandleOverflow();
        }

        /// <summary>Handler for tab removing</summary>
        /// <param name="c_button"></param>
        internal virtual void OnLoseTab(TabButton c_button)
        {
            if (CurrentButton == c_button)
            {
                CurrentButton = null;
            }

            //TODO: Select a tab if any exist.

            if (TabRemoved != null)
            {
                TabRemoved.Invoke(this);
            }

            Invalidate();
        }

        private void HandleOverflow()
        {
            var a_tabsSize = TabStrip.GetChildrenSize();

            // Only enable the scrollers if the tabs are at the top.
            // This is a limitation we should explore.
            // Really TabControl should have derivitives for tabs placed elsewhere where we could specialize 
            // some functions like this for each direction.
            var a_needed = a_tabsSize.X > Width && TabStrip.Dock == Pos.Top;

            m_scroll[0].IsHidden = !a_needed;
            m_scroll[1].IsHidden = !a_needed;

            if (!a_needed)
            {
                return;
            }

            m_scrollOffset = Util.Clamp(m_scrollOffset, 0, a_tabsSize.X - Width + 32);

            TabStrip.Margin = new Margin(m_scrollOffset * -1, 0, 0, 0);

            m_scroll[0].SetPosition(Width - 30, 5);
            m_scroll[1].SetPosition(m_scroll[0].Right, 5);
        }

        protected virtual void ScrollPressedLeft(GameControl c_control)
        {
            m_scrollOffset -= 120;
        }

        protected virtual void ScrollPressedRight(GameControl c_control)
        {
            m_scrollOffset += 120;
        }
    }
}