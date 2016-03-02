using System.Linq;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Radio button group</summary>
    internal class RadioButtonGroup : GroupBox
    {
        /// <summary>Initializes a new instance of the <see cref="RadioButtonGroup" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        /// <param name="c_label">Label for the outlining GroupBox</param>
        public RadioButtonGroup(GameControl c_parentControl, string c_label) : base(c_parentControl)
        {
            IsTabable = false;
            KeyboardInputEnabled = true;
            Text = c_label;
            AutoSizeToContents = true;
        }

        /// <summary>Selected radio button</summary>
        public LabeledRadioButton Selected
        {
            get;
            private set;
        }

        /// <summary>Internal name of the selected radio button</summary>
        public string SelectedName => Selected.Name;

        /// <summary>Text of the selected radio button</summary>
        public string SelectedLabel => Selected.Text;

        /// <summary>Index of the selected radio button</summary>
        public int SelectedIndex => Children.IndexOf(Selected);

        /// <summary>Invoked when the selected option has changed</summary>
        public event SelectionEventHandler SelectionChanged;

        /// <summary>Adds a new option</summary>
        /// <param name="c_text">Option text</param>
        /// <returns>Newly created control</returns>
        public virtual LabeledRadioButton AddOption(string c_text)
        {
            return AddOption(c_text, string.Empty);
        }

        /// <summary>Adds a new option</summary>
        /// <param name="c_text">Option text</param>
        /// <param name="c_optionName">Internal name</param>
        /// <returns>Newly created control</returns>
        public virtual LabeledRadioButton AddOption(string c_text, string c_optionName)
        {
            var a_lrb = new LabeledRadioButton(this);
            a_lrb.Name = c_optionName;
            a_lrb.Text = c_text;
            a_lrb.RadioButton.Checked += OnRadioClicked;
            a_lrb.Dock = Pos.Top;
            a_lrb.Margin = new Margin(0, 0, 0, 1); // 1 bottom
            a_lrb.KeyboardInputEnabled = false; // TODO: true?
            a_lrb.IsTabable = true;

            Invalidate();
            return a_lrb;
        }

        /// <summary>Handler for the option change</summary>
        /// <param name="c_fromPanel">Event source</param>
        protected virtual void OnRadioClicked(GameControl c_fromPanel)
        {
            var a_checked = c_fromPanel as RadioButton;
            foreach (var a_rb in Children.OfType<LabeledRadioButton>()) // TODO: optimize
            {
                if (a_rb.RadioButton == a_checked)
                {
                    Selected = a_rb;
                }
                else
                {
                    a_rb.RadioButton.IsChecked = false;
                }
            }

            OnChanged();
        }

        /*
        /// <summary>
        /// Sizes to contents.
        /// </summary>
        public override void SizeToContents()
        {
            RecurseLayout(Skin); // options are docked so positions are not updated until layout runs
            //base.SizeToContents();
            int width = 0;
            int height = 0;
            foreach (Base child in Children)
            {
                width = Math.Max(child.Width, width);
                height += child.Height;
            }
            SetSize(width, height);
            InvalidateParent();
        }
        */

        protected virtual void OnChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged.Invoke(this);
            }
        }

        /// <summary>Selects the specified option</summary>
        /// <param name="c_index">Option to select</param>
        public void SetSelection(int c_index)
        {
            if (c_index < 0 || c_index >= Children.Count)
            {
                return;
            }

            (Children[c_index] as LabeledRadioButton).RadioButton.Press();
        }
    }
}