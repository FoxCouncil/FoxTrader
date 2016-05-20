using System.Drawing;
using FoxTrader.UI.ControlInternal;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>ComboBox control</summary>
    internal class ComboBox : Button
    {
        private readonly GameControl m_button;
        private readonly Menu m_menu;
        private MenuItem m_selectedItem;

        /// <summary>Initializes a new instance of the <see cref="ComboBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ComboBox(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(100, 20);

            m_menu = new Menu(this);
            m_menu.IsHidden = true;
            m_menu.IconMarginDisabled = true;
            m_menu.IsTabable = false;

            var a_downArrow = new DownArrow(this);
            m_button = a_downArrow;

            Alignment = Pos.Left | Pos.CenterV;
            Text = string.Empty;
            Margin = new Margin(3, 0, 0, 0);

            IsTabable = true;
            KeyboardInputEnabled = true;
        }

        /// <summary>Indicates whether the combo menu is open</summary>
        public bool IsOpen => m_menu != null && !m_menu.IsHidden;

        /// <summary>Selected item</summary>
        /// <remarks>Not just String property, because items also have internal names</remarks>
        public Label SelectedItem => m_selectedItem;

        internal override bool IsMenuComponent => true;

        /// <summary>Invoked when the selected item has changed</summary>
        public event SelectionEventHandler ItemSelected;

        /// <summary>Adds a new item</summary>
        /// <param name="c_label">Item label (displayed)</param>
        /// <param name="c_name">Item name</param>
        /// <returns>Newly created control</returns>
        public virtual MenuItem AddItem(string c_label, string c_name = "")
        {
            var a_menuItem = m_menu.AddItem(c_label, string.Empty);
            a_menuItem.Name = c_name;
            a_menuItem.Selected += OnItemSelected;

            if (m_selectedItem == null)
            {
                OnItemSelected(a_menuItem);
            }

            return a_menuItem;
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawComboBox(this, IsDepressed, IsOpen);
            RenderText(c_skin.Renderer);
        }

        /// <summary>Internal Pressed implementation</summary>
        public override void OnClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (IsOpen)
            {
                GetCanvas().CloseMenus();

                return;
            }

            var a_wasMenuHidden = m_menu.IsHidden;

            GetCanvas().CloseMenus();

            if (a_wasMenuHidden)
            {
                Open();
            }
        }

        /// <summary>Removes all items</summary>
        public virtual void DeleteAll()
        {
            if (m_menu != null)
            {
                m_menu.DeleteAll();
            }
        }

        /// <summary>Internal handler for item selected event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnItemSelected(GameControl c_control)
        {
            //Convert selected to a menu item
            var a_menuItem = c_control as MenuItem;

            if (null == a_menuItem)
            {
                return;
            }

            m_selectedItem = a_menuItem;
            Text = m_selectedItem.Text;
            m_menu.IsHidden = true;
            Name = a_menuItem.Name;

            ItemSelected?.Invoke(this);

            OnFocus();
            Invalidate();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(Skin c_skin)
        {
            m_button.SetRelativePosition(Pos.Right | Pos.CenterV, 4, 0);
            base.OnLayout(c_skin);
        }

        /// <summary>Handler for gaining keyboard focus</summary>
        public override void OnFocus()
        {
            //Until we add the blue highlighting again
            TextColor = Color.Black;
        }

        /// <summary>Handler for losing keyboard focus</summary>
        public override void OnBlur()
        {
            TextColor = Color.Black;
        }

        /// <summary>Opens the combo</summary>
        public virtual void Open()
        {
            if (m_menu == null)
            {
                return;
            }

            m_menu.Parent = GetCanvas();
            m_menu.IsHidden = false;
            m_menu.BringToFront();

            var a_point = LocalPosToCanvas(Point.Empty);

            m_menu.SetBounds(new Rectangle(a_point.X, a_point.Y + Height, Width, m_menu.Height));
        }

        /// <summary>Closes the combo</summary>
        public virtual void Close()
        {
            if (m_menu == null)
            {
                return;
            }

            m_menu.Hide();
        }

        /// <summary>Handler for Down Arrow keyboard event</summary>
        /// <param name="c_down">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        public override void OnKeyDown(KeyboardKeyEventArgs c_keyboardKeyEventArgs)
        {
            switch (c_keyboardKeyEventArgs.Key)
            {
                case Key.Down:
                {
                    var a_idx = m_menu.Children.FindIndex(c_x => c_x == m_selectedItem);

                    if (a_idx + 1 < m_menu.Children.Count)
                    {
                        OnItemSelected(m_menu.Children[a_idx + 1]);
                    }
                }
                break;

                case Key.Up:
                {
                    var a_idx = m_menu.Children.FindLastIndex(c_x => c_x == m_selectedItem);

                    if (a_idx - 1 >= 0)
                    {
                        OnItemSelected(m_menu.Children[a_idx - 1]);
                    }
                }
                break;
            }

        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderFocus(Skin c_skin)
        {
        }
    }
}