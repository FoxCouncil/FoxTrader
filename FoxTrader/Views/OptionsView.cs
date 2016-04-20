using System.Drawing;
using System.Globalization;
using System.Linq;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    class OptionsView : BaseGameView
    {
        private readonly GameControl m_menuControl;

        private readonly Label m_labelLanguage;

        private readonly Button m_buttonBack;

        public OptionsView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_menuControl = new GameControl(this) { RestrictToParent = true, DrawDebugOutlines = true };

            var a_controlsParent = m_menuControl;

            m_labelLanguage = new Label(a_controlsParent) { AutoSizeToContents = true, Text = I18N.GetString("Language") };
            m_labelLanguage.TextColor = Color.White;
            m_labelLanguage.SetPosition(10, 10);

            var a_comboBoxLanguage = new ComboBox(a_controlsParent);
            a_comboBoxLanguage.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            a_comboBoxLanguage.Y = 10;

            Align.PlaceDownLeft(a_comboBoxLanguage, m_labelLanguage, 5);

            var a_installedLanguages = I18N.InstalledLanguages.Select(c_installedLanguage => a_comboBoxLanguage.AddItem(new CultureInfo(c_installedLanguage).NativeName, c_installedLanguage)).ToList();

            a_installedLanguages[I18N.CurrentLanguageIndex].OnClicked(null);

            a_comboBoxLanguage.ItemSelected += (c_controlSender) =>
            {
                var a_languageSelection = c_controlSender as Label;

                if (a_languageSelection != null)
                {
                    I18N.SetUICulture(a_languageSelection.Name);
                }

                Invalidate();
            };

            var a_testTextBox = new TextBox(a_controlsParent) { Y = 10 };
            Align.PlaceDownLeft(a_testTextBox, a_comboBoxLanguage, 5);

            var a_testNumericUpDown = new NumericUpDown(a_controlsParent) { Y = 10 };
            Align.PlaceDownLeft(a_testNumericUpDown, a_testTextBox, 5);

            m_buttonBack = new Button(a_controlsParent) { Text = I18N.GetString("Back") };
            m_buttonBack.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonBack.Clicked += (c_senderControl, c_args) =>
            {
                GameContext.Instance.MarkStateComplete(ContextState.Options);
            };

            Align.CenterHorizontally(m_buttonBack);

            m_buttonBack.Y = 700 - kMainMenuButtonHeight - 10;

            m_menuControl.SetSize(700, 700);
        }

        protected override void OnLayout(Skin c_skin)
        {
            base.OnLayout(c_skin);

            Align.CenterHorizontally(m_menuControl);
            Align.CenterVertically(m_menuControl);

            Align.CenterHorizontally(m_buttonBack);

            m_labelLanguage.Text = I18N.GetString("Language");
            m_buttonBack.Text = I18N.GetString("Back");
        }
    }
}
