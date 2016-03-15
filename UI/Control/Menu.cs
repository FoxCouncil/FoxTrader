using System.Linq;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Popup menu</summary>
    internal class Menu : ScrollControl
    {
        /// <summary>Initializes a new instance of the <see cref="Menu" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Menu(GameControl c_parentControl) : base(c_parentControl)
        {
            SetBounds(0, 0, 10, 10);
            Padding = Padding.m_two;
            IconMarginDisabled = false;

            AutoHideBars = true;
            EnableScroll(false, true);
            DeleteOnClose = false;
        }

        internal override bool IsMenuComponent => true;

        public bool IconMarginDisabled
        {
            get;
            set;
        }

        /// <summary>Determines whether the menu should be disposed on close</summary>
        public bool DeleteOnClose
        {
            get;
            set;
        }

        /// <summary>Determines whether the menu should open on mouse hover</summary>
        protected virtual bool ShouldHoverOpenMenu => true;

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawMenu(this, IconMarginDisabled);
        }

        /// <summary>Renders under the actual control (shadows etc)</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderUnder(SkinBase c_skin)
        {
            base.RenderUnder(c_skin);

            c_skin.DrawShadow(this);
        }

        /// <summary>Opens the menu</summary>
        /// <param name="c_position">Unused</param>
        public void Open(Pos c_position)
        {
            IsHidden = false;

            BringToFront();

            var a_mousePosition = FoxTraderWindow.Instance.MousePosition;

            SetPosition(a_mousePosition.X, a_mousePosition.Y);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            var a_childrenHeight = Children.Sum(c_child => c_child != null ? c_child.Height : 0);

            if (Y + a_childrenHeight > GetCanvas().Height)
            {
                a_childrenHeight = GetCanvas().Height - Y;
            }

            SetSize(Width, a_childrenHeight);

            base.Layout(c_skin);
        }

        /// <summary>Adds a new menu item</summary>
        /// <param name="c_text">Item text</param>
        /// <returns>Newly created control</returns>
        public virtual MenuItem AddItem(string c_text)
        {
            return AddItem(c_text, string.Empty);
        }

        /// <summary>Adds a new menu item</summary>
        /// <param name="c_text">Item text</param>
        /// <param name="c_iconName">Icon texture name</param>
        /// <param name="c_accelerator">Accelerator for this item</param>
        /// <returns>Newly created control</returns>
        public virtual MenuItem AddItem(string c_text, string c_iconName, string c_accelerator = "")
        {
            var a_menuItem = new MenuItem(this);
            a_menuItem.Padding = Padding.m_four;
            a_menuItem.SetText(c_text);
            a_menuItem.SetImage(c_iconName);
            a_menuItem.SetAccelerator(c_accelerator);

            OnAddItem(a_menuItem);

            return a_menuItem;
        }

        /// <summary>Add item handler</summary>
        /// <param name="c_item">Item added</param>
        protected virtual void OnAddItem(MenuItem c_item)
        {
            c_item.TextPadding = new Padding(IconMarginDisabled ? 0 : 24, 0, 16, 0);
            c_item.Dock = Pos.Top;
            c_item.SizeToContents();
            c_item.Alignment = Pos.CenterV | Pos.Left;
            c_item.MouseIn += OnHoverItem;

            // Do this here - after Top Docking these values mean nothing in layout
            var a_width = c_item.Width + 10 + 32;

            if (a_width < Width)
            {
                a_width = Width;
            }

            SetSize(a_width, Height);
        }

        /// <summary>Closes all submenus</summary>
        public virtual void CloseAll()
        {
            Children.ForEach(c_child =>
            {
                if (c_child is MenuItem)
                {
                    (c_child as MenuItem).CloseMenu();
                }
            });
        }

        /// <summary>Indicates whether any (sub)menu is open</summary>
        /// <returns></returns>
        public virtual bool IsMenuOpen()
        {
            return Children.Any(c_child =>
            {
                if (c_child is MenuItem)
                {
                    return (c_child as MenuItem).IsMenuOpen;
                }
                return false;
            });
        }

        /// <summary>Mouse hover handler</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnHoverItem(GameControl c_control)
        {
            if (!ShouldHoverOpenMenu)
            {
                return;
            }

            var a_menuItem = c_control as MenuItem;

            if (null == a_menuItem || a_menuItem.IsMenuOpen)
            {
                return;
            }

            CloseAll();

            a_menuItem.OpenMenu();
        }

        /// <summary>Closes the current menu</summary>
        public virtual void Close()
        {
            IsHidden = true;

            if (DeleteOnClose)
            {
                DelayedDelete();
            }
        }

        /// <summary>Closes all submenus and the current menu</summary>
        public override void CloseMenus()
        {
            base.CloseMenus();

            CloseAll();
            Close();
        }

        /// <summary>Adds a divider menu item</summary>
        public virtual void AddDivider()
        {
            var a_menuDivider = new MenuDivider(this);

            a_menuDivider.Dock = Pos.Top;
            a_menuDivider.Margin = new Margin(IconMarginDisabled ? 0 : 24, 0, 4, 0);
        }
    }
}