using System.Drawing;
using FoxTrader.UI.ControlInternal;
using FoxTrader.UI.Skin;

namespace FoxTrader.UI.Control
{
    /// <summary>RGBA color picker</summary>
    internal class ColorPicker : GameControl, IColorPicker
    {
        private Color m_color;

        /// <summary>Initializes a new instance of the <see cref="ColorPicker" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ColorPicker(GameControl c_parentControl) : base(c_parentControl)
        {
            MouseInputEnabled = true;

            SetSize(256, 150);

            CreateControls();

            SelectedColor = Color.FromArgb(255, 50, 60, 70);
        }

        /// <summary>Red value of the selected color</summary>
        public int R
        {
            get
            {
                return m_color.R;
            }
            set
            {
                m_color = Color.FromArgb(m_color.A, value, m_color.G, m_color.B);
            }
        }

        /// <summary>Green value of the selected color</summary>
        public int G
        {
            get
            {
                return m_color.G;
            }
            set
            {
                m_color = Color.FromArgb(m_color.A, m_color.R, value, m_color.B);
            }
        }

        /// <summary>Blue value of the selected color</summary>
        public int B
        {
            get
            {
                return m_color.B;
            }
            set
            {
                m_color = Color.FromArgb(m_color.A, m_color.R, m_color.G, value);
            }
        }

        /// <summary>Alpha value of the selected color</summary>
        public int A
        {
            get
            {
                return m_color.A;
            }
            set
            {
                m_color = Color.FromArgb(value, m_color.R, m_color.G, m_color.B);
            }
        }

        /// <summary>Determines whether the Alpha control is visible</summary>
        public bool AlphaVisible
        {
            get
            {
                var a_groupBox = FindChildByName("Alphagroupbox", true) as GroupBox;
                return !a_groupBox.IsHidden;
            }
            set
            {
                var a_groupBox = FindChildByName("Alphagroupbox", true) as GroupBox;
                a_groupBox.IsHidden = !value;
                Invalidate();
            }
        }

        /// <summary>Selected color</summary>
        public Color SelectedColor
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
                UpdateControls();
            }
        }

        /// <summary>Invoked when the selected color has been changed</summary>
        public event ColorEventHandler ColorChanged;

        private void CreateColorControl(string c_name, int c_y)
        {
            const int colorSize = 12;

            var a_colorGroup = new GroupBox(this);
            a_colorGroup.SetPosition(10, c_y);
            a_colorGroup.SetText(c_name);
            a_colorGroup.SetSize(160, 35);
            a_colorGroup.Name = c_name + "groupbox";

            var a_display = new ColorDisplay(a_colorGroup);
            a_display.Name = c_name;
            a_display.SetBounds(0, 10, colorSize, colorSize);

            var a_numericTextBox = new TextBoxNumeric(a_colorGroup);
            a_numericTextBox.Name = c_name + "Box";
            a_numericTextBox.SetPosition(105, 7);
            a_numericTextBox.SetSize(26, 16);
            a_numericTextBox.SelectAllOnFocus = true;
            a_numericTextBox.TextChanged += NumericTyped;

            var a_slider = new HorizontalSlider(a_colorGroup);
            a_slider.SetPosition(colorSize + 5, 10);
            a_slider.SetRange(0, 255);
            a_slider.SetSize(80, colorSize);
            a_slider.Name = c_name + "Slider";
            a_slider.ValueChanged += SlidersMoved;
        }

        private void NumericTyped(GameControl c_control)
        {
            var a_numericTextBox = c_control as TextBoxNumeric;

            if (null == a_numericTextBox)
            {
                return;
            }

            if (a_numericTextBox.Text == string.Empty)
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

            if (a_numericTextBox.Name.Contains("Red"))
            {
                R = a_textValue;
            }

            if (a_numericTextBox.Name.Contains("Green"))
            {
                G = a_textValue;
            }

            if (a_numericTextBox.Name.Contains("Blue"))
            {
                B = a_textValue;
            }

            if (a_numericTextBox.Name.Contains("Alpha"))
            {
                A = a_textValue;
            }

            UpdateControls();
        }

        private void CreateControls()
        {
            const int startY = 5;
            const int height = 35;

            CreateColorControl("Red", startY);
            CreateColorControl("Green", startY + height);
            CreateColorControl("Blue", startY + height * 2);
            CreateColorControl("Alpha", startY + height * 3);

            var a_finalGroup = new GroupBox(this);
            a_finalGroup.SetPosition(180, 40);
            a_finalGroup.SetSize(60, 60);
            a_finalGroup.SetText("Result");
            a_finalGroup.Name = "ResultGroupBox";

            var a_display = new ColorDisplay(a_finalGroup);
            a_display.Name = "Result";
            a_display.SetBounds(0, 10, 32, 32);
            //a_display.DrawCheckers = true;

            //UpdateControls();
        }

        private void UpdateColorControls(string c_name, Color c_color, int c_sliderValue)
        {
            var a_display = FindChildByName(c_name, true) as ColorDisplay;
            a_display.Color = c_color;

            var a_slider = FindChildByName(c_name + "Slider", true) as HorizontalSlider;
            a_slider.Value = c_sliderValue;

            var a_numericTextBox = FindChildByName(c_name + "Box", true) as TextBoxNumeric;
            a_numericTextBox.Value = c_sliderValue;
        }

        private void UpdateControls()
        {
            // TODO: Clean this up, as it's 'weird'...
            UpdateColorControls("Red", Color.FromArgb(255, SelectedColor.R, 0, 0), SelectedColor.R);
            UpdateColorControls("Green", Color.FromArgb(255, 0, SelectedColor.G, 0), SelectedColor.G);
            UpdateColorControls("Blue", Color.FromArgb(255, 0, 0, SelectedColor.B), SelectedColor.B);
            UpdateColorControls("Alpha", Color.FromArgb(SelectedColor.A, 255, 255, 255), SelectedColor.A);

            var a_display = FindChildByName("Result", true) as ColorDisplay;
            a_display.Color = SelectedColor;

            if (ColorChanged != null)
            {
                ColorChanged.Invoke(this);
            }
        }

        private void SlidersMoved(GameControl c_control)
        {
            var a_slider = c_control as HorizontalSlider;

            if (a_slider != null)
            {
                SetColorByName(GetColorFromName(a_slider.Name), (int)a_slider.Value);
            }

            UpdateControls();
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void OnLayout(SkinBase c_skin)
        {
            base.OnLayout(c_skin);

            SizeToChildren(false, true);
            SetSize(Width, Height + 5);

            var a_groupbox = FindChildByName("ResultGroupBox", true) as GroupBox;
            if (a_groupbox != null)
            {
                a_groupbox.SetPosition(a_groupbox.X, Height * 0.5f - a_groupbox.Height * 0.5f);
            }

            //UpdateControls(); // this spams events continuously every tick
        }

        private int GetColorByName(string c_colorName)
        {
            if (c_colorName == "Red")
            {
                return SelectedColor.R;
            }

            if (c_colorName == "Green")
            {
                return SelectedColor.G;
            }

            if (c_colorName == "Blue")
            {
                return SelectedColor.B;
            }

            if (c_colorName == "Alpha")
            {
                return SelectedColor.A;
            }

            return 0;
        }

        private static string GetColorFromName(string c_name)
        {
            if (c_name.Contains("Red"))
            {
                return "Red";
            }

            if (c_name.Contains("Green"))
            {
                return "Green";
            }

            if (c_name.Contains("Blue"))
            {
                return "Blue";
            }

            if (c_name.Contains("Alpha"))
            {
                return "Alpha";
            }

            return string.Empty;
        }

        private void SetColorByName(string c_colorName, int c_colorValue)
        {
            if (c_colorName == "Red")
            {
                R = c_colorValue;
            }
            else if (c_colorName == "Green")
            {
                G = c_colorValue;
            }
            else if (c_colorName == "Blue")
            {
                B = c_colorValue;
            }
            else if (c_colorName == "Alpha")
            {
                A = c_colorValue;
            }
        }
    }
}