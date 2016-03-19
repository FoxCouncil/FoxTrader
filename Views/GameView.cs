using FoxTrader.UI;
using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    internal sealed class GameView : BaseGameView
    {
        private MapControl m_mapControl;

        public GameView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_mapControl = new MapControl(this);
        }
    }
}