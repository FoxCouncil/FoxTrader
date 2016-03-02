using FoxTrader.UI.DragDrop;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Tab header</summary>
    internal class TabButton : Button
    {
        private TabControl m_tabControl;

        /// <summary>Initializes a new instance of the <see cref="TabButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TabButton(GameControl c_parentControl) : base(c_parentControl)
        {
            DragAndDrop_SetPackage(true, "TabButtonMove");
            Alignment = Pos.Top | Pos.Left;
            TextPadding = new Padding(5, 3, 3, 3);
            Padding = Padding.m_two;
            KeyboardInputEnabled = true;
        }

        /// <summary>Indicates whether the tab is active</summary>
        public bool IsActive => Page != null && Page.IsVisible;

        // TODO: remove public access
        public TabControl TabControl
        {
            get
            {
                return m_tabControl;
            }

            set
            {
                if (m_tabControl == value)
                {
                    return;
                }

                m_tabControl?.OnLoseTab(this);

                m_tabControl = value;
            }
        }

        /// <summary>Interior of the tab</summary>
        public GameControl Page
        {
            get;
            set;
        }

        /// <summary>Determines whether the control should be clipped to its bounds while rendering</summary>
        protected override bool ShouldClip => false;

        public override void DragAndDrop_StartDragging(Package c_package, int c_x, int c_y)
        {
            IsHidden = true;
        }

        public override void DragAndDrop_EndDragging(bool c_success, int c_x, int c_y)
        {
            IsHidden = false;
            IsDepressed = false;
        }

        public override bool DragAndDrop_ShouldStartDrag()
        {
            return m_tabControl.AllowReorder;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawTabButton(this, IsActive, m_tabControl.TabStrip.Dock);
        }

        /// <summary>Handler for Down Arrow keyboard event</summary>
        /// <param name="c_down">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyDown(bool c_down)
        {
            OnKeyRight(c_down);

            return true;
        }

        /// <summary>Handler for Up Arrow keyboard event</summary>
        /// <param name="c_down">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyUp(bool c_down)
        {
            OnKeyLeft(c_down);

            return true;
        }

        /// <summary>Handler for Right Arrow keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyRight(bool c_isButtonDown)
        {
            if (c_isButtonDown)
            {
                var a_count = Parent.Children.Count;

                var a_idx = Parent.Children.IndexOf(this);

                if (a_idx + 1 < a_count)
                {
                    var a_nextTab = Parent.Children[a_idx + 1];

                    TabControl.OnTabPressed(a_nextTab);

                    FoxTraderWindow.Instance.KeyboardFocus = a_nextTab;
                }
            }

            return true;
        }

        /// <summary>Handler for Left Arrow keyboard event</summary>
        /// <param name="c_isButtonDown">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyLeft(bool c_isButtonDown)
        {
            if (c_isButtonDown)
            {
                var a_count = Parent.Children.Count;

                var a_idx = Parent.Children.IndexOf(this);

                if (a_idx - 1 >= 0)
                {
                    var a_prevTab = Parent.Children[a_idx - 1];

                    TabControl.OnTabPressed(a_prevTab);

                    FoxTraderWindow.Instance.KeyboardFocus = a_prevTab;
                }
            }

            return true;
        }

        /// <summary>Updates control colors</summary>
        public override void UpdateColors()
        {
            if (IsActive)
            {
                if (IsDisabled)
                {
                    TextColor = Skin.m_colors.m_tab.m_active.m_disabled;
                    return;
                }
                if (IsDepressed)
                {
                    TextColor = Skin.m_colors.m_tab.m_active.m_down;
                    return;
                }
                if (IsHovered)
                {
                    TextColor = Skin.m_colors.m_tab.m_active.m_hover;
                    return;
                }

                TextColor = Skin.m_colors.m_tab.m_active.m_normal;
            }

            if (IsDisabled)
            {
                TextColor = Skin.m_colors.m_tab.m_inactive.m_disabled;
                return;
            }

            if (IsDepressed)
            {
                TextColor = Skin.m_colors.m_tab.m_inactive.m_down;
                return;
            }

            if (IsHovered)
            {
                TextColor = Skin.m_colors.m_tab.m_inactive.m_hover;
                return;
            }

            TextColor = Skin.m_colors.m_tab.m_inactive.m_normal;
        }
    }
}