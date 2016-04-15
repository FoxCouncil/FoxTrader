using static FoxTrader.Constants;

namespace FoxTrader.UI.Control.Property
{
    /// <summary>Text property</summary>
    internal class Text : PropertyBase
    {
        protected readonly TextBox m_textBox;

        /// <summary>Initializes a new instance of the <see cref="Text" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Text(GameControl c_parentControl) : base(c_parentControl)
        {
            m_textBox = new TextBox(this);
            m_textBox.Dock = Pos.Fill;
            m_textBox.ShouldDrawBackground = false;
            m_textBox.TextChanged += OnValueChanged;
        }

        /// <summary>Property value</summary>
        public override string Value
        {
            get
            {
                return m_textBox.Text;
            }
            set
            {
                base.Value = value;
            }
        }

        /// <summary>Indicates whether the property value is being edited</summary>
        public override bool IsEditing => m_textBox.HasFocus;

        /// <summary>Indicates whether the control is hovered by mouse pointer</summary>
        public override bool IsHovered => base.IsHovered | m_textBox.IsHovered;

        /// <summary>Sets the property value</summary>
        /// <param name="c_value">Value to set</param>
        /// <param name="c_fireEvents">Determines whether to fire "value changed" event</param>
        public override void SetValue(string c_value, bool c_fireEvents = false)
        {
            m_textBox.SetText(c_value, c_fireEvents);
        }
    }
}