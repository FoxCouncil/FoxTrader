using System.Drawing;
using FoxTrader.UI.Control.Layout;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>List box row (selectable)</summary>
    internal class ListBoxRow : TableRow
    {
        private bool m_selected;

        /// <summary>Initializes a new instance of the <see cref="ListBoxRow" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ListBoxRow(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = true;
            IsSelected = false;
        }

        /// <summary>Indicates whether the control is selected</summary>
        public bool IsSelected
        {
            get
            {
                return m_selected;
            }
            set
            {
                m_selected = value;
                // TODO: Get these values from the skin
                SetTextColor(value ? Color.White : Color.Black);
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(Skin c_skin)
        {
            c_skin.DrawListBoxLine(this, IsSelected, EvenRow);
        }

        public override void OnClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (c_mouseButtonEventArgs.IsPressed)
            {
                OnRowSelected();
            }
        }
    }
}