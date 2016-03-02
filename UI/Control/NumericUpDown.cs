using FoxTrader.UI.Control.Layout;
using FoxTrader.UI.ControlInternal;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Numeric up/down</summary>
    internal class NumericUpDown : TextBoxNumeric
    {
        private readonly UpDownButtonDown m_down;

        private readonly Splitter m_splitter;
        private readonly UpDownButtonUp m_up;

        /// <summary>Initializes a new instance of the <see cref="NumericUpDown" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public NumericUpDown(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(100, 20);

            m_splitter = new Splitter(this);
            m_splitter.Dock = Pos.Right;
            m_splitter.SetSize(13, 13);

            m_up = new UpDownButtonUp(m_splitter);
            m_up.Clicked += OnButtonUp;
            m_up.IsTabable = false;
            m_splitter.SetPanel(0, m_up, false);

            m_down = new UpDownButtonDown(m_splitter);
            m_down.Clicked += OnButtonDown;
            m_down.IsTabable = false;
            m_down.Padding = new Padding(0, 1, 1, 0);
            m_splitter.SetPanel(1, m_down, false);

            Max = 100;
            Min = 0;
            m_value = 0f;
            Text = "0";
        }

        /// <summary>Minimum value</summary>
        public int Min
        {
            get;
            set;
        }

        /// <summary>Maximum value</summary>
        public int Max
        {
            get;
            set;
        }

        /// <summary>Numeric value of the control</summary>
        public override float Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value < Min)
                {
                    value = Min;
                }
                if (value > Max)
                {
                    value = Max;
                }
                if (value == m_value)
                {
                    return;
                }

                base.Value = value;
            }
        }

        /// <summary>Invoked when the value has been changed</summary>
        public event ValueEventHandler ValueChanged;

        /// <summary>Handler for Up Arrow keyboard event</summary>
        /// <param name="c_down">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyUp(bool c_down)
        {
            if (c_down)
            {
                OnButtonUp(null);
            }
            return true;
        }

        /// <summary>Handler for Down Arrow keyboard event</summary>
        /// <param name="c_down">Indicates whether the key was pressed or released</param>
        /// <returns>True if handled</returns>
        protected override bool OnKeyDown(bool c_down)
        {
            if (c_down)
            {
                OnButtonDown(null);
            }
            return true;
        }

        /// <summary>Handler for the button up event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnButtonUp(GameControl c_control)
        {
            Value = m_value + 1;
        }

        /// <summary>Handler for the button down event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnButtonDown(GameControl c_control)
        {
            Value = m_value - 1;
        }

        /// <summary>Determines whether the text can be assighed to the control</summary>
        /// <param name="c_str">Text to evaluate</param>
        /// <returns>True if the text is allowed</returns>
        protected override bool IsTextAllowed(string c_str)
        {
            float a_d;
            if (!float.TryParse(c_str, out a_d))
            {
                return false;
            }
            if (a_d < Min)
            {
                return false;
            }
            if (a_d > Max)
            {
                return false;
            }
            return true;
        }

        /// <summary>Handler for the text changed event</summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(this);
            }
        }
    }
}