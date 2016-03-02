namespace FoxTrader.UI.Control
{
    /// <summary>Text box with masked text</summary>
    /// <remarks>This class doesn't prevent programatic access to the text in any way</remarks>
    internal class TextBoxPassword : TextBox
    {
        private string m_mask;

        /// <summary>Initializes a new instance of the <see cref="TextBoxPassword" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TextBoxPassword(GameControl c_parentControl) : base(c_parentControl)
        {
            MaskCharacter = '*';
        }

        /// <summary>Character used in place of actual characters for display</summary>
        public char MaskCharacter
        {
            get;
            set;
        }

        /// <summary>Handler for text changed event</summary>
        protected override void OnTextChanged()
        {
            m_mask = new string(MaskCharacter, Text.Length);
            TextOverride = m_mask;
            base.OnTextChanged();
        }
    }
}