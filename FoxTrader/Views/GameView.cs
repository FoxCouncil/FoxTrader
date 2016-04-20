using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    internal sealed class GameView : BaseGameView
    {
        private readonly Label m_timeDisplay;
        private readonly Label m_moneyDisplay;

        private readonly MapControl m_mapControl;
        private readonly StatusBar m_statusBar;

        public GameView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_statusBar = new StatusBar(this) { Dock = Pos.Top };
            {
                m_timeDisplay = new Label(m_statusBar) { AutoSizeToContents = true };

                m_statusBar.AddControl(m_timeDisplay, true);

                m_moneyDisplay = new Label(m_statusBar) { AutoSizeToContents = true };

                m_statusBar.AddControl(m_moneyDisplay, false);
            }


            m_mapControl = new MapControl(this);
        }

        protected override void Render(Skin c_skin)
        {
            base.Render(c_skin);

            m_timeDisplay.Text = Time.SpaceTime.ToString();

            if (GameContext.Instance.Player.Money.ToString() != m_moneyDisplay.Text)
            {
                m_moneyDisplay.Text = $"Credits: {GameContext.Instance.Player.Money.ToString("N0")}";
            }
        }
    }
}