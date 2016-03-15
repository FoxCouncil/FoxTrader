using System.Drawing;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Menu item</summary>
    internal class MenuItem : Button
    {
        private Label m_accelerator;
        private bool m_checked;
        private Menu m_menu;
        private GameControl m_submenuArrow;

        /// <summary>Initializes a new instance of the <see cref="MenuItem" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public MenuItem(GameControl c_parentControl) : base(c_parentControl)
        {
            IsOnStrip = false;
            IsTabable = false;
            IsCheckable = false;
            IsChecked = false;

            m_accelerator = new Label(this);
        }

        /// <summary>Indicates whether the item is on a menu strip</summary>
        public bool IsOnStrip
        {
            get;
            set;
        }

        /// <summary>Determines if the menu item is checkable</summary>
        public bool IsCheckable
        {
            get;
            set;
        }

        /// <summary>Indicates if the parent menu is open</summary>
        public bool IsMenuOpen
        {
            get
            {
                if (m_menu == null)
                {
                    return false;
                }
                return !m_menu.IsHidden;
            }
        }

        /// <summary>Gets or sets the check value</summary>
        public bool IsChecked
        {
            get
            {
                return m_checked;
            }
            set
            {
                if (value == m_checked)
                {
                    return;
                }

                m_checked = value;

                if (CheckChanged != null)
                {
                    CheckChanged.Invoke(this);
                }

                if (value)
                {
                    if (Checked != null)
                    {
                        Checked.Invoke(this);
                    }
                }
                else
                {
                    if (UnChecked != null)
                    {
                        UnChecked.Invoke(this);
                    }
                }
            }
        }

        /// <summary>Gets the parent menu</summary>
        public Menu Menu
        {
            get
            {
                if (null == m_menu)
                {
                    m_menu = new Menu(GetCanvas());
                    m_menu.IsHidden = true;

                    if (!IsOnStrip)
                    {
                        if (m_submenuArrow != null)
                        {
                            m_submenuArrow.Dispose();
                        }
                        m_submenuArrow = new RightArrow(this);
                        m_submenuArrow.SetSize(15, 15);
                    }

                    Invalidate();
                }

                return m_menu;
            }
        }

        /// <summary>Invoked when the item is selected</summary>
        public event GameControlEventHandler Selected;

        /// <summary>Invoked when the item is checked</summary>
        public event CheckBoxEventHandler Checked;

        /// <summary>Invoked when the item is unchecked</summary>
        public event CheckBoxEventHandler UnChecked;

        /// <summary>Invoked when the item's check value is changed</summary>
        public event CheckBoxEventHandler CheckChanged;

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawMenuItem(this, IsMenuOpen, IsCheckable ? m_checked : false);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            if (m_submenuArrow != null)
            {
                m_submenuArrow.SetRelativePosition(Pos.Right | Pos.CenterV, 4, 0);
            }
            base.Layout(c_skin);
        }

        /// <summary>Internal OnPressed implementation</summary>
        protected override void OnClicked()
        {
            if (m_menu != null)
            {
                ToggleMenu();
            }
            else if (!IsOnStrip)
            {
                IsChecked = !IsChecked;
                if (Selected != null)
                {
                    Selected.Invoke(this);
                }
                GetCanvas().CloseMenus();
            }
            base.OnClicked();
        }

        /// <summary>Toggles the menu open state</summary>
        public void ToggleMenu()
        {
            if (IsMenuOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        /// <summary>Opens the menu</summary>
        public void OpenMenu()
        {
            if (null == m_menu)
            {
                return;
            }

            m_menu.IsHidden = false;
            m_menu.BringToFront();

            var a_p = LocalPosToCanvas(Point.Empty);

            // Strip menus open downwards
            if (IsOnStrip)
            {
                m_menu.SetPosition(a_p.X, a_p.Y + Height + 1);
            }
            // Submenus open sidewards
            else
            {
                m_menu.SetPosition(a_p.X + Width, a_p.Y);
            }

            // TODO: Option this.
            // TODO: Make sure on screen, open the other side of the 
            // parent if it's better...
        }

        /// <summary>Closes the menu</summary>
        public void CloseMenu()
        {
            if (null == m_menu)
            {
                return;
            }
            m_menu.Close();
            m_menu.CloseAll();
        }

        public override void SizeToContents()
        {
            base.SizeToContents();
            if (m_accelerator != null)
            {
                m_accelerator.SizeToContents();
                Width = Width + m_accelerator.Width;
            }
        }

        public MenuItem SetAction(GameControlEventHandler c_handler)
        {
            if (m_accelerator != null)
            {
                AddAccelerator(m_accelerator.Text, c_handler);
            }

            Selected += c_handler;
            return this;
        }

        public void SetAccelerator(string c_acc)
        {
            if (m_accelerator != null)
            {
                //m_Accelerator.DelayedDelete(); // to prevent double disposing
                m_accelerator = null;
            }

            if (c_acc == string.Empty)
            {
                return;
            }

            m_accelerator = new Label(this);
            m_accelerator.Dock = Pos.Right;
            m_accelerator.Alignment = Pos.Right | Pos.CenterV;
            m_accelerator.Text = c_acc;
            m_accelerator.Margin = new Margin(0, 0, 16, 0);
        }
    }
}