using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    class MainMenuView : GameView
    {
        private readonly GameControl m_menuControl;
        private readonly ImagePanel m_imagePanel;
        private readonly Label m_label;

        private readonly Button m_buttonNewGame;
        private readonly Button m_buttonLoadGame;
        private readonly Button m_buttonSaveGame;
        private readonly Button m_buttonOptions;
        private readonly Button m_buttonQuit;

        public MainMenuView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_menuControl = new GameControl(this);
            m_menuControl.RestrictToParent = true;
            m_menuControl.DrawDebugOutlines = true;

            var a_controlsParent = m_menuControl;

            m_imagePanel = new ImagePanel(a_controlsParent);
            m_imagePanel.ImageName = "png_FoxTrader_256x256";
            m_imagePanel.SetSize(256, 256);
            m_imagePanel.Y = 25;

            m_label = new Label(a_controlsParent) { AutoSizeToContents = true, Text = I18N.GetString("GameTrademark"), Font = FoxTraderWindow.Instance.GetFont("pix Chicago", 26) };
            m_label.MakeColorBright();

            m_buttonNewGame = new Button(a_controlsParent) { Text = I18N.GetString("NewGame") };
            m_buttonNewGame.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);

            m_buttonLoadGame = new Button(a_controlsParent) { Text = I18N.GetString("LoadGame") };
            m_buttonLoadGame.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonLoadGame.Disable();

            m_buttonSaveGame = new Button(a_controlsParent) { Text = I18N.GetString("SaveGame") };
            m_buttonSaveGame.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonSaveGame.Disable();

            m_buttonOptions = new Button(a_controlsParent) { Text = I18N.GetString("GameOptions") };
            m_buttonOptions.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonOptions.Clicked += (c_senderControl) =>
            {
                FoxTraderGame.GameContextInstance.ViewOptions();
            };

            m_buttonQuit = new Button(a_controlsParent) { Text = I18N.GetString("QuitGame") };
            m_buttonQuit.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonQuit.Clicked += (c_senderControl) =>
            {
                FoxTraderWindow.Instance.Exit();
            };

            m_menuControl.SetSize(700, 768);

            Align.CenterHorizontally(m_imagePanel);

            Align.PlaceDownLeft(m_label, m_imagePanel, 25);
            Align.CenterHorizontally(m_label);

            Align.PlaceDownLeft(m_buttonNewGame, m_label, 50);
            Align.CenterHorizontally(m_buttonNewGame);

            Align.PlaceDownLeft(m_buttonLoadGame, m_buttonNewGame, 25);

            Align.PlaceDownLeft(m_buttonSaveGame, m_buttonLoadGame, 25);

            Align.PlaceDownLeft(m_buttonOptions, m_buttonSaveGame, 25);

            Align.PlaceDownLeft(m_buttonQuit, m_buttonOptions, 25);
        }

        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);

            Align.CenterHorizontally(m_menuControl);
            Align.CenterVertically(m_menuControl);
        }
    }
}
