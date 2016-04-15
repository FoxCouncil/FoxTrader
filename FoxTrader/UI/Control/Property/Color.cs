using FoxTrader.UI.ControlInternal;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control.Property
{
    /// <summary>Color property</summary>
    internal class Color : Text
    {
        protected readonly ColorButton m_button;

        /// <summary>Initializes a new instance of the <see cref="Color" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Color(GameControl c_parentControl) : base(c_parentControl)
        {
            m_button = new ColorButton(m_textBox);
            m_button.Dock = Pos.Right;
            m_button.Width = 20;
            m_button.Margin = new Margin(1, 1, 1, 2);
            m_button.Clicked += OnButtonPressed;
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
        public override bool IsEditing => GetCanvas().KeyboardFocus == m_textBox;

        /// <summary>Color-select button press handler</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnButtonPressed(GameControl c_control, MouseButtonEventArgs c_args)
        {
            var a_menu = new Menu(GetCanvas());
            a_menu.SetSize(256, 180);
            a_menu.DeleteOnClose = true;
            a_menu.IconMarginDisabled = true;

            var a_hsvPicker = new HSVColorPicker(a_menu);
            a_hsvPicker.Dock = Pos.Fill;
            a_hsvPicker.SetSize(256, 128);

            a_hsvPicker.SetColor(GetColorFromText(), false, true);
            a_hsvPicker.ColorChanged += OnColorChanged;

            a_menu.Open(Pos.Right | Pos.Top);
        }

        /// <summary>Color changed handler</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnColorChanged(GameControl c_control)
        {
            var a_hsvPicker = c_control as HSVColorPicker;

            SetTextFromColor(a_hsvPicker.SelectedColor);

            DoChanged();
        }

        /// <summary>Sets the property value</summary>
        /// <param name="c_value">Value to set</param>
        /// <param name="c_fireEvents">Determines whether to fire "value changed" event</param>
        public override void SetValue(string c_value, bool c_fireEvents = false)
        {
            m_textBox.SetText(c_value, c_fireEvents);
        }

        private void SetTextFromColor(System.Drawing.Color c_color)
        {
            m_textBox.Text = $"{c_color.R} {c_color.G} {c_color.B}";
        }

        private System.Drawing.Color GetColorFromText()
        {
            var a_splitString = m_textBox.Text.Split(' ');

            byte a_colorRed = 0;
            byte a_colorGreen = 0;
            byte a_colorBlue = 0;
            byte a_colorAlpha = 255;

            if (a_splitString.Length > 0 && a_splitString[0].Length > 0)
            {
                byte.TryParse(a_splitString[0], out a_colorRed);
            }

            if (a_splitString.Length > 1 && a_splitString[1].Length > 0)
            {
                byte.TryParse(a_splitString[1], out a_colorGreen);
            }

            if (a_splitString.Length > 2 && a_splitString[2].Length > 0)
            {
                byte.TryParse(a_splitString[2], out a_colorBlue);
            }

            return System.Drawing.Color.FromArgb(a_colorAlpha, a_colorRed, a_colorGreen, a_colorBlue);
        }

        protected override void DoChanged()
        {
            base.DoChanged();

            m_button.Color = GetColorFromText();
        }
    }
}