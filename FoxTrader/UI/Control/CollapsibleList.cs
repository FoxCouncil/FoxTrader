using System.Linq;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>CollapsibleList control. Groups CollapsibleCategory controls</summary>
    internal class CollapsibleList : ScrollControl
    {
        /// <summary>Initializes a new instance of the <see cref="CollapsibleList" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public CollapsibleList(GameControl c_parentControl) : base(c_parentControl)
        {
            EnableScroll(false, true);
            AutoHideBars = true;
        }

        /// <summary>Invoked when an entry has been selected</summary>
        public event CollapsibleListEventHandler ItemSelected;

        /// <summary>Invoked when a category collapsed state has been changed (header button has been pressed)</summary>
        public event CollapsibleListEventHandler CategoryCollapsed;

        /// <summary>Selected entry</summary>
        public Button GetSelectedButton()
        {
            return Children.OfType<CollapsibleCategory>().Select(c_category => c_category.GetSelectedButton()).FirstOrDefault(c_button => c_button != null);
        }

        /// <summary>Adds a category to the list</summary>
        /// <param name="c_category">Category control to add</param>
        protected virtual void Add(CollapsibleCategory c_category)
        {
            c_category.Parent = this;
            c_category.Dock = Pos.Top;
            c_category.Margin = new Margin(1, 0, 1, 1);
            c_category.Selected += OnCategorySelected;
            c_category.Collapsed += OnCategoryCollapsed;
            // this relies on fact that category.m_List is set to its parent
        }

        /// <summary>Adds a new category to the list</summary>
        /// <param name="c_categoryName">Name of the category</param>
        /// <returns>Newly created control</returns>
        public virtual CollapsibleCategory Add(string c_categoryName)
        {
            var a_category = new CollapsibleCategory(this);

            a_category.Text = c_categoryName;

            Add(a_category);

            return a_category;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawCategoryHolder(this);

            base.Render(c_skin);
        }

        /// <summary>Unselects all entriess</summary>
        public virtual void UnselectAll()
        {
            foreach (var a_child in Children)
            {
                var a_category = a_child as CollapsibleCategory;

                if (a_category == null)
                {
                    continue;
                }

                a_category.UnselectAll();
            }
        }

        /// <summary>Handler for ItemSelected event</summary>
        /// <param name="c_control">Event source: <see cref="CollapsibleList" /></param>
        protected virtual void OnCategorySelected(GameControl c_control)
        {
            var a_category = c_control as CollapsibleCategory;

            if (a_category == null && ItemSelected != null)
            {
                ItemSelected.Invoke(this);
            }
        }

        /// <summary>Handler for category collapsed event</summary>
        /// <param name="c_control">Event source: <see cref="CollapsibleCategory" /></param>
        protected virtual void OnCategoryCollapsed(GameControl c_control)
        {
            var a_category = c_control as CollapsibleCategory;

            if (a_category != null && CategoryCollapsed != null)
            {
                CategoryCollapsed.Invoke(c_control);
            }
        }
    }
}