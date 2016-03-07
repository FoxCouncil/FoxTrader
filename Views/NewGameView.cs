using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    class NewGameView : GameView
    {
        public NewGameView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            GameContext.Instance.MarkStateComplete(ContextState.NewGame);
        }
    }
}
