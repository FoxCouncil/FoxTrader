using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>CheckBox with label</summary>
    internal class LabeledCheckBox : GameControl
    {
        private readonly CheckBox m_checkBox;
        private readonly LabelClickable m_label;

        /// <summary>Initializes a new instance of the <see cref="LabeledCheckBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public LabeledCheckBox(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(200, 19);
            m_checkBox = new CheckBox(this);
            m_checkBox.Dock = Pos.Left;
            m_checkBox.Margin = new Margin(0, 2, 2, 2);
            m_checkBox.IsTabable = false;
            m_checkBox.CheckChanged += OnCheckChanged;

            m_label = new LabelClickable(this);
            m_label.Dock = Pos.Fill;
            m_label.Clicked += (c_control, c_args) => m_checkBox.OnClicked(c_args);
            m_label.IsTabable = false;

            IsTabable = false;
        }

        /// <summary>Indicates whether the control is checked</summary>
        public bool IsChecked
        {
            get
            {
                return m_checkBox.IsChecked;
            }
            set
            {
                m_checkBox.IsChecked = value;
            }
        }

        /// <summary>Label text</summary>
        public string Text
        {
            get
            {
                return m_label.Text;
            }
            set
            {
                m_label.Text = value;
            }
        }

        /// <summary>Invoked when the control has been checked</summary>
        public event CheckBoxEventHandler Checked;

        /// <summary>Invoked when the control has been unchecked</summary>
        public event CheckBoxEventHandler UnChecked;

        /// <summary>Invoked when the control's check has been changed</summary>
        public event CheckBoxEventHandler CheckChanged;

        /// <summary>Handler for CheckChanged event</summary>
        protected virtual void OnCheckChanged(GameControl c_control)
        {
            if (m_checkBox.IsChecked)
            {
                Checked?.Invoke(this);
            }
            else
            {
                UnChecked?.Invoke(this);
            }

            CheckChanged?.Invoke(this);
        }
    }
}