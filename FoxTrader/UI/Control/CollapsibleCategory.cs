using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>CollapsibleCategory control. Used in CollapsibleList</summary>
    internal class CollapsibleCategory : GameControl
    {
        private readonly Button m_headerButton;
        private readonly CollapsibleList m_list;

        /// <summary>Initializes a new instance of the <see cref="CollapsibleCategory" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public CollapsibleCategory(CollapsibleList c_parentControl) : base(c_parentControl)
        {
            m_headerButton = new CategoryHeaderButton(this) { Text = "Category Title", Dock = Pos.Top, Height = 20 };
            m_headerButton.Toggled += OnHeaderToggle;

            m_list = c_parentControl;

            Padding = new Padding(1, 0, 1, 5);
            SetSize(512, 512);
        }

        /// <summary>Header text</summary>
        public string Text
        {
            get
            {
                return m_headerButton.Text;
            }
            set
            {
                m_headerButton.Text = value;
            }
        }

        /// <summary>Determines whether the category is collapsed (closed)</summary>
        public bool IsCollapsed
        {
            get
            {
                return m_headerButton.ToggleState;
            }
            set
            {
                m_headerButton.ToggleState = value;
            }
        }

        /// <summary>Invoked when an entry has been selected</summary>
        public event CollapsibleCategoryEventHandler Selected;

        /// <summary>Invoked when the category collapsed state has been changed (header button has been pressed)</summary>
        public event CollapsibleCategoryEventHandler Collapsed;

        /// <summary>Gets the selected entry</summary>
        public Button GetSelectedButton()
        {
            foreach (var a_childControl in Children)
            {
                var a_button = a_childControl as CategoryButton;

                if (a_button == null)
                {
                    continue;
                }

                if (a_button.ToggleState)
                {
                    return a_button;
                }
            }

            return null;
        }

        /// <summary>Handler for header button toggle event</summary>
        /// <param name="c_control">Source control</param>
        protected virtual void OnHeaderToggle(GameControl c_control, MouseButtonEventArgs c_args)
        {
            Collapsed?.Invoke(this);
        }

        /// <summary>Handler for Selected event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnSelected(GameControl c_control, MouseButtonEventArgs c_args)
        {
            var a_childControl = c_control as CategoryButton;

            if (a_childControl == null)
            {
                return;
            }

            if (m_list != null)
            {
                m_list.UnselectAll();
            }
            else
            {
                UnselectAll();
            }

            a_childControl.ToggleState = true;

            if (Selected != null)
            {
                Selected.Invoke(this);
            }
        }

        /// <summary>Adds a new entry</summary>
        /// <param name="c_name">Entry name (displayed)</param>
        /// <returns>Newly created control</returns>
        public Button Add(string c_name)
        {
            var a_button = new CategoryButton(this);
            a_button.Text = c_name;
            a_button.Dock = Pos.Top;
            a_button.SizeToContents();
            a_button.SetSize(a_button.Width + 4, a_button.Height + 4);
            a_button.Padding = new Padding(5, 2, 2, 2);
            a_button.Clicked += OnSelected;

            return a_button;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawCategoryInner(this, m_headerButton.ToggleState);

            base.Render(c_skin);
        }

        /// <summary>Unselects all entries</summary>
        public void UnselectAll()
        {
            foreach (var a_childControl in Children)
            {
                var a_button = a_childControl as CategoryButton;

                if (a_button == null)
                {
                    continue;
                }

                a_button.ToggleState = false;
            }
        }

        /// <summary>Function invoked after layout</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void PostLayout(SkinBase c_skin)
        {
            if (IsCollapsed)
            {
                Height = m_headerButton.Height;
            }
            else
            {
                SizeToChildren(false, true);
            }

            // alternate row coloring
            var a_b = true;

            foreach (var a_childControl in Children)
            {
                var a_button = a_childControl as CategoryButton;

                if (a_button == null)
                {
                    continue;
                }

                a_button.m_alt = a_b;
                a_button.UpdateColors();
                a_b = !a_b;
            }
        }
    }
}