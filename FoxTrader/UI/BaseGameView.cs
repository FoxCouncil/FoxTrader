using FoxTrader.UI.Control;

namespace FoxTrader.UI
{
    internal class BaseGameView : Panel
    {
        protected BaseGameView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Constants.Pos.Fill;
        }
    }
}