using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>Simple message box</summary>
    internal class MessageBox : WindowControl
    {
        private readonly Button m_button;
        private readonly Label m_label; // should be rich label with maxwidth = parent

        /// <summary>Initializes a new instance of the <see cref="MessageBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        /// <param name="c_text">Message to display</param>
        /// <param name="c_caption">Window caption</param>
        public MessageBox(GameControl c_parentControl, string c_text, string c_caption = "") : base(c_parentControl, c_caption, true)
        {
            DeleteOnClose = true;

            m_label = new Label(m_innerControl);
            m_label.Text = c_text;
            m_label.Margin = Margin.m_five;
            m_label.Dock = Pos.Top;
            m_label.Alignment = Pos.Center;
            m_label.AutoSizeToContents = true;

            m_button = new Button(m_innerControl);
            m_button.Text = "OK"; // TODO: parametrize buttons
            m_button.Clicked += CloseButtonPressed;
            m_button.Clicked += DismissedHandler;
            m_button.Margin = Margin.m_five;
            m_button.SetSize(50, 20);

            Align.Center(this);
        }

        /// <summary>Invoked when the message box has been dismissed</summary>
        public event MessageBoxEventHandler Dismissed;

        private void DismissedHandler(GameControl c_control)
        {
            if (Dismissed != null)
            {
                Dismissed.Invoke(this);
            }
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);

            Align.PlaceDownLeft(m_button, m_label, 10);
            Align.CenterHorizontally(m_button);
            m_innerControl.SizeToChildren();
            m_innerControl.Height += 10;
            SizeToChildren();
        }
    }
}