using System.Drawing;
using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>HSV color picker with "before" and "after" color boxes</summary>
    internal class HSVColorPicker : GameControl, IColorPicker
    {
        private readonly ColorDisplay m_after;
        private readonly ColorDisplay m_before;
        private readonly ColorSlider m_colorSlider;
        private readonly ColorLerpBox m_lerpBox;

        /// <summary>Initializes a new instance of the <see cref="HSVColorPicker" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public HSVColorPicker(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = true;
            SetSize(256, 128);
            //ShouldCacheToTexture = true;

            m_lerpBox = new ColorLerpBox(this);
            m_lerpBox.ColorChanged += ColorBoxChanged;
            m_lerpBox.Dock = Pos.Left;

            m_colorSlider = new ColorSlider(this);
            m_colorSlider.SetPosition(m_lerpBox.Width + 15, 5);
            m_colorSlider.ColorChanged += ColorSliderChanged;
            m_colorSlider.Dock = Pos.Left;

            m_after = new ColorDisplay(this);
            m_after.SetSize(48, 24);
            m_after.SetPosition(m_colorSlider.X + m_colorSlider.Width + 15, 5);

            m_before = new ColorDisplay(this);
            m_before.SetSize(48, 24);
            m_before.SetPosition(m_after.X, 28);

            var a_x = m_before.X;
            var a_y = m_before.Y + 30;

            {
                var a_label = new Label(this);
                a_label.SetText("R:");
                a_label.SizeToContents();
                a_label.SetPosition(a_x, a_y);

                var a_numericTextBox = new TextBoxNumeric(this);
                a_numericTextBox.Name = "RedBox";
                a_numericTextBox.SetPosition(a_x + 15, a_y - 1);
                a_numericTextBox.SetSize(26, 16);
                a_numericTextBox.SelectAllOnFocus = true;
                a_numericTextBox.TextChanged += NumericTyped;
            }

            a_y += 20;

            {
                var a_label = new Label(this);
                a_label.SetText("G:");
                a_label.SizeToContents();
                a_label.SetPosition(a_x, a_y);

                var a_numericTextBox = new TextBoxNumeric(this);
                a_numericTextBox.Name = "GreenBox";
                a_numericTextBox.SetPosition(a_x + 15, a_y - 1);
                a_numericTextBox.SetSize(26, 16);
                a_numericTextBox.SelectAllOnFocus = true;
                a_numericTextBox.TextChanged += NumericTyped;
            }

            a_y += 20;

            {
                var a_label = new Label(this);
                a_label.SetText("B:");
                a_label.SizeToContents();
                a_label.SetPosition(a_x, a_y);

                var a_numericTextBox = new TextBoxNumeric(this);
                a_numericTextBox.Name = "BlueBox";
                a_numericTextBox.SetPosition(a_x + 15, a_y - 1);
                a_numericTextBox.SetSize(26, 16);
                a_numericTextBox.SelectAllOnFocus = true;
                a_numericTextBox.TextChanged += NumericTyped;
            }

            SetColor(DefaultColor);
        }

        /// <summary>The "before" color</summary>
        public Color DefaultColor
        {
            get
            {
                return m_before.Color;
            }
            set
            {
                m_before.Color = value;
            }
        }

        /// <summary>Selected color</summary>
        public Color SelectedColor => m_lerpBox.SelectedColor;

        /// <summary>Invoked when the selected color has changed</summary>
        public event ColorEventHandler ColorChanged;

        private void NumericTyped(GameControl c_control)
        {
            var a_numericTextBox = c_control as TextBoxNumeric;

            if (a_numericTextBox == null || a_numericTextBox.Text == string.Empty)
            {
                return;
            }

            var a_textValue = (int)a_numericTextBox.Value;

            if (a_textValue < 0)
            {
                a_textValue = 0;
            }

            if (a_textValue > 255)
            {
                a_textValue = 255;
            }

            var a_newColor = SelectedColor;

            if (a_numericTextBox.Name.Contains("Red"))
            {
                a_newColor = Color.FromArgb(SelectedColor.A, a_textValue, SelectedColor.G, SelectedColor.B);
            }
            else if (a_numericTextBox.Name.Contains("Green"))
            {
                a_newColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, a_textValue, SelectedColor.B);
            }
            else if (a_numericTextBox.Name.Contains("Blue"))
            {
                a_newColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, a_textValue);
            }
            else if (a_numericTextBox.Name.Contains("Alpha"))
            {
                a_newColor = Color.FromArgb(a_textValue, SelectedColor.R, SelectedColor.G, SelectedColor.B);
            }

            SetColor(a_newColor);
        }

        private void UpdateControls(Color c_color)
        {
            var a_redBox = FindChildByName("RedBox", false) as TextBoxNumeric;
            if (a_redBox != null)
            {
                a_redBox.SetText(c_color.R.ToString(), false);
            }

            var a_greenBox = FindChildByName("GreenBox", false) as TextBoxNumeric;
            if (a_greenBox != null)
            {
                a_greenBox.SetText(c_color.G.ToString(), false);
            }

            var a_blueBox = FindChildByName("BlueBox", false) as TextBoxNumeric;
            if (a_blueBox != null)
            {
                a_blueBox.SetText(c_color.B.ToString(), false);
            }

            m_after.Color = c_color;

            if (ColorChanged != null)
            {
                ColorChanged.Invoke(this);
            }
        }

        /// <summary>Sets the selected color</summary>
        /// <param name="c_color">Color to set</param>
        /// <param name="c_onlyHue">Determines whether only the hue should be set</param>
        /// <param name="c_reset">Determines whether the "before" color should be set as well</param>
        public void SetColor(Color c_color, bool c_onlyHue = false, bool c_reset = false)
        {
            UpdateControls(c_color);

            if (c_reset)
            {
                m_before.Color = c_color;
            }

            m_colorSlider.SelectedColor = c_color;
            m_lerpBox.SetColor(c_color, c_onlyHue);
            m_after.Color = c_color;
        }

        private void ColorBoxChanged(GameControl c_control)
        {
            UpdateControls(SelectedColor);
            Invalidate();
        }

        private void ColorSliderChanged(GameControl c_control)
        {
            if (m_lerpBox != null)
            {
                m_lerpBox.SetColor(m_colorSlider.SelectedColor, true);
            }

            Invalidate();
        }
    }
}