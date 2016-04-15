using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>RadioButton with label</summary>
    internal class LabeledRadioButton : GameControl
    {
        private readonly LabelClickable m_label;

        /// <summary>Initializes a new instance of the <see cref="LabeledRadioButton" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public LabeledRadioButton(GameControl c_parentControl) : base(c_parentControl)
        {
            SetSize(100, 20);

            RadioButton = new RadioButton(this);
            RadioButton.IsTabable = false;
            RadioButton.KeyboardInputEnabled = false;

            m_label = new LabelClickable(this);
            m_label.Alignment = Pos.Bottom | Pos.Left;
            m_label.Text = "Radio Button";
            m_label.Clicked += (c_control, c_args) => RadioButton.OnClicked(c_args);
            m_label.IsTabable = false;
            m_label.KeyboardInputEnabled = false;
            m_label.AutoSizeToContents = true;
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

        // TODO: would be nice to remove that
        internal RadioButton RadioButton
        {
            get;
        }

        protected override void OnLayout(SkinBase c_skin)
        {
            // ugly stuff because we don't have anchoring without docking (docking resizes children)
            if (m_label.Height > RadioButton.Height) // usually radio is smaller than label so it gets repositioned to avoid clipping with negative Y
            {
                RadioButton.Y = (m_label.Height - RadioButton.Height) / 2;
            }

            Align.PlaceRightBottom(m_label, RadioButton);

            SizeToChildren();

            base.OnLayout(c_skin);
        }

        /// <summary>Renders the focus overlay</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void RenderFocus(SkinBase c_skin)
        {
            if (GetCanvas().KeyboardFocus != this)
            {
                return;
            }

            if (!IsTabable)
            {
                return;
            }

            c_skin.DrawKeyboardHighlight(this, RenderBounds, 0);
        }

        /// <summary>Selects the radio button</summary>
        public virtual void Select()
        {
            RadioButton.IsChecked = true;
        }
    }
}