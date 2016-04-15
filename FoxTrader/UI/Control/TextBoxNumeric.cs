namespace FoxTrader.UI.Control
{
    /// <summary>Numeric text box - accepts only float numbers</summary>
    internal class TextBoxNumeric : TextBox
    {
        /// <summary>Current numeric value</summary>
        protected float m_value;

        /// <summary>Initializes a new instance of the <see cref="TextBoxNumeric" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TextBoxNumeric(GameControl c_parentControl) : base(c_parentControl)
        {
            SetText("0", false);
        }

        /// <summary>Current numerical value</summary>
        public virtual float Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
                Text = value.ToString();
            }
        }

        protected virtual bool IsTextAllowed(string c_str)
        {
            if (c_str == "" || c_str == "-")
            {
                return true; // annoying if single - is not allowed
            }
            float a_d;
            return float.TryParse(c_str, out a_d);
        }

        /// <summary>Determines whether the control can insert text at a given cursor position</summary>
        /// <param name="c_text">Text to check</param>
        /// <param name="c_position">Cursor position</param>
        /// <returns>True if allowed</returns>
        protected override bool IsTextAllowed(string c_text, int c_position)
        {
            var a_newText = Text.Insert(c_position, c_text);
            return IsTextAllowed(a_newText);
        }

        // text -> value
        /// <summary>Handler for text changed event</summary>
        protected override void OnTextChanged()
        {
            if (string.IsNullOrEmpty(Text) || Text == "-")
            {
                m_value = 0;
                //SetText("0");
            }
            else
            {
                m_value = float.Parse(Text);
            }
            base.OnTextChanged();
        }

        /// <summary>Sets the control text</summary>
        /// <param name="c_str">Text to set</param>
        /// <param name="c_doEvents">Determines whether to invoke "text changed" event</param>
        public override void SetText(string c_str, bool c_doEvents = true)
        {
            if (IsTextAllowed(c_str))
            {
                base.SetText(c_str, c_doEvents);
            }
        }
    }
}