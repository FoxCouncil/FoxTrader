using System.Drawing;
using System.Reflection;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    class MainMenuView : BaseGameView
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

            m_menuControl = new GameControl(this) { RestrictToParent = true, DrawDebugOutlines = true };

            var a_controlsParent = m_menuControl;

            m_imagePanel = new ImagePanel(a_controlsParent) { ImageName = "png_FoxTrader_256x256" };
            m_imagePanel.SetSize(256, 256);
            m_imagePanel.Y = 10;

            var a_assemblyVersion = Assembly.GetCallingAssembly().GetName().Version;

            m_label = new Label(a_controlsParent) { AutoSizeToContents = true, Text = string.Format(I18N.GetString("GameTrademark"), a_assemblyVersion.Major + "." + a_assemblyVersion.Minor), Font = FoxTraderWindow.Instance.Renderer.GetFont("Roboto Black", 32) };
            m_label.TextColor = Color.White;

            m_buttonNewGame = new Button(a_controlsParent) { Text = I18N.GetString("NewGame") };
            m_buttonNewGame.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonNewGame.Clicked += (c_senderControl, c_args) =>
            {
                GameContext.Instance.New();
            };

            m_buttonLoadGame = new Button(a_controlsParent) { Text = I18N.GetString("LoadGame") };
            m_buttonLoadGame.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonLoadGame.Disable();

            m_buttonSaveGame = new Button(a_controlsParent) { Text = I18N.GetString("SaveGame") };
            m_buttonSaveGame.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonSaveGame.Disable();

            m_buttonOptions = new Button(a_controlsParent) { Text = I18N.GetString("GameOptions") };
            m_buttonOptions.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonOptions.Clicked += (c_senderControl, a_args) =>
            {
                GameContext.Instance.ViewOptions();
            };

            m_buttonQuit = new Button(a_controlsParent) { Text = I18N.GetString("QuitGame") };
            m_buttonQuit.SetSize(kMainMenuButtonWidth, kMainMenuButtonHeight);
            m_buttonQuit.Clicked += (c_senderControl, c_args) =>
            {
                FoxTraderWindow.Instance.Exit();
            };

            m_menuControl.SetSize(550, 670);

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

        protected override void OnLayout(SkinBase c_skin)
        {
            base.OnLayout(c_skin);

            Align.CenterHorizontally(m_menuControl);
            Align.CenterVertically(m_menuControl);
        }
    }
}
