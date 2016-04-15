namespace FoxTrader.UI.Control.Property
{
    /// <summary>Checkable property</summary>
    internal class Check : PropertyBase
    {
        protected readonly CheckBox m_checkBox;

        /// <summary>Initializes a new instance of the <see cref="Check" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Check(GameControl c_parentControl) : base(c_parentControl)
        {
            m_checkBox = new CheckBox(this);
            m_checkBox.ShouldDrawBackground = false;
            m_checkBox.CheckChanged += OnValueChanged;
            m_checkBox.IsTabable = true;
            m_checkBox.KeyboardInputEnabled = true;
            m_checkBox.SetPosition(2, 1);

            Height = 18;
        }

        /// <summary>Property value</summary>
        public override string Value
        {
            get
            {
                return m_checkBox.IsChecked ? "1" : "0";
            }
            set
            {
                base.Value = value;
            }
        }

        /// <summary>Indicates whether the property value is being edited</summary>
        public override bool IsEditing => m_checkBox.HasFocus;

        /// <summary>Indicates whether the control is hovered by mouse pointer</summary>
        public override bool IsHovered => base.IsHovered || m_checkBox.IsHovered;

        /// <summary>Sets the property value</summary>
        /// <param name="c_value">Value to set</param>
        /// <param name="c_fireEvents">Determines whether to fire "value changed" event</param>
        public override void SetValue(string c_value, bool c_fireEvents = false)
        {
            if (c_value == "1" || c_value.ToLower() == "true" || c_value.ToLower() == "yes")
            {
                m_checkBox.IsChecked = true;
            }
            else
            {
                m_checkBox.IsChecked = false;
            }
        }
    }
}