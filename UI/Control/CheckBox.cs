using FoxTrader.UI.Skin;
using OpenTK.Input;

namespace FoxTrader.UI.Control
{
    /// <summary>CheckBox control</summary>
    internal class CheckBox : Button
    {
        private bool m_checked;

        /// <summary>Initializes a new instance of the <see cref="CheckBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public CheckBox(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(15, 15);
        }

        /// <summary>Indicates whether the checkbox is checked</summary>
        public bool IsChecked
        {
            get
            {
                return m_checked;
            }
            set
            {
                if (m_checked == value)
                {
                    return;
                }

                m_checked = value;

                OnCheckChanged();
            }
        }

        /// <summary>Determines whether unchecking is allowed</summary>
        protected virtual bool AllowUncheck => true;

        /// <summary>Invoked when the checkbox has been checked</summary>
        public event CheckBoxEventHandler Checked;

        /// <summary>Invoked when the checkbox has been unchecked</summary>
        public event CheckBoxEventHandler UnChecked;

        /// <summary>Invoked when the checkbox state has been changed</summary>
        public event CheckBoxEventHandler CheckChanged;

        /// <summary>Toggles the checkbox</summary>
        public override void Toggle()
        {
            base.Toggle();

            IsChecked = !IsChecked;
        }

        /// <summary>Handler for CheckChanged event</summary>
        protected virtual void OnCheckChanged()
        {
            if (IsChecked)
            {
                if (Checked != null)
                {
                    Checked.Invoke(this);
                }
            }
            else
            {
                if (UnChecked != null)
                {
                    UnChecked.Invoke(this);
                }
            }

            if (CheckChanged != null)
            {
                CheckChanged.Invoke(this);
            }
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            c_skin.DrawCheckBox(this, m_checked, IsDepressed);
        }

        /// <summary>Internal OnPressed implementation</summary>
        public override void OnClicked(MouseButtonEventArgs c_mouseButtonEventArgs)
        {
            if (IsDisabled)
            {
                return;
            }

            if (IsChecked && !AllowUncheck)
            {
                return;
            }

            Toggle();
        }
    }
}