using System.Collections.Generic;
using System.Globalization;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    class OptionsView : GameView
    {
        private readonly GameControl m_menuControl;

        private readonly Label m_labelLanguage;
        private readonly ComboBox m_comboBoxLanguage;

        private readonly Button m_buttonBack;

        private readonly List<MenuItem> m_installedLanguages;

        public OptionsView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_installedLanguages = new List<MenuItem>();

            m_menuControl = new GameControl(this);
            m_menuControl.RestrictToParent = true;

            m_menuControl.DrawDebugOutlines = true;

            var a_controlsParent = m_menuControl;

            m_labelLanguage = new Label(a_controlsParent);
            m_labelLanguage.AutoSizeToContents = true;
            m_labelLanguage.Text = I18N.GetString("Language");
            m_labelLanguage.MakeColorBright();

            m_comboBoxLanguage = new ComboBox(a_controlsParent);
            m_comboBoxLanguage.SetSize(kMainMenuButtonWidth - m_labelLanguage.Width - 25, kMainMenuButtonHeight);

            foreach (var a_installedLanguage in I18N.InstalledLanguages)
            {
                m_installedLanguages.Add(m_comboBoxLanguage.AddItem(new CultureInfo(a_installedLanguage).NativeName, a_installedLanguage));
            }

            m_installedLanguages[I18N.CurrentLanguageIndex].Press();

            m_comboBoxLanguage.ItemSelected += (c_controlSender) =>
            {
                var a_languageSelection = c_controlSender as Label;

                I18N.SetUICulture(a_languageSelection.Name);

                Invalidate();
            };

            m_buttonBack = new Button(a_controlsParent);
            m_buttonBack.Text = I18N.GetString("Back");
            m_buttonBack.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonBack.Clicked += (c_senderControl) =>
            {
                FoxTraderGame.GameContextInstance.MarkStateComplete(ContextState.Options);
            };

            m_menuControl.SetSize(700, 700);

            m_comboBoxLanguage.X = m_labelLanguage.Width + 25;

            Align.PlaceDownLeft(m_buttonBack, m_labelLanguage, 25);
        }

        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);

            Align.CenterHorizontally(m_menuControl);
            Align.CenterVertically(m_menuControl);

            m_labelLanguage.Text = I18N.GetString("Language");
            m_buttonBack.Text = I18N.GetString("Back");
        }
    }
}
