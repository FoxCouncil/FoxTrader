using System.Windows.Forms;
using FoxTrader.UI.Control.Property;
using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;
using Text = FoxTrader.UI.Control.Property.Text;

namespace FoxTrader.UI.Control
{
    /// <summary>Properties table</summary>
    internal class Properties : GameControl
    {
        private readonly SplitterBar m_splitterBar;

        /// <summary>Initializes a new instance of the <see cref="Properties" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Properties(GameControl c_parentControl) : base(c_parentControl)
        {
            m_splitterBar = new SplitterBar(this);
            m_splitterBar.SetPosition(80, 0);
            m_splitterBar.Cursor = Cursors.SizeWE;
            m_splitterBar.Dragged += OnSplitterMoved;
            m_splitterBar.ShouldDrawBackground = false;
        }

        /// <summary>Returns the width of the first column (property names)</summary>
        public int SplitWidth => m_splitterBar.X;

        // TODO: rename?

        /// <summary>Invoked when a property value has been changed</summary>
        public event ValueEventHandler ValueChanged;

        /// <summary>Function invoked after layout</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void PostLayout(Skin c_skin)
        {
            m_splitterBar.Height = 0;

            if (SizeToChildren(false, true))
            {
                InvalidateParent();
            }

            m_splitterBar.SetSize(3, Height);
        }

        /// <summary>Handles the splitter moved event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnSplitterMoved(GameControl c_control)
        {
            InvalidateChildren();
        }

        /// <summary>Adds a new text property row</summary>
        /// <param name="c_label">Property name</param>
        /// <param name="c_value">Initial value</param>
        /// <returns>Newly created row</returns>
        public PropertyRow Add(string c_label, string c_value = "")
        {
            return Add(c_label, new Text(this), c_value);
        }

        /// <summary>Adds a new property row</summary>
        /// <param name="c_label">Property name</param>
        /// <param name="c_prop">Property control</param>
        /// <param name="c_value">Initial value</param>
        /// <returns>Newly created row</returns>
        public PropertyRow Add(string c_label, PropertyBase c_prop, string c_value = "")
        {
            var a_row = new PropertyRow(this, c_prop);
            a_row.Dock = Pos.Top;
            a_row.Label = c_label;
            a_row.ValueChanged += OnRowValueChanged;

            c_prop.SetValue(c_value, true);

            m_splitterBar.BringToFront();
            return a_row;
        }

        private void OnRowValueChanged(GameControl c_control)
        {
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(c_control);
            }
        }

        /// <summary>Deletes all rows</summary>
        public void DeleteAll()
        {
            m_innerControl.DeleteAllChildren();
        }
    }
}