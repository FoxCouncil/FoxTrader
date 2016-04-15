using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    /// <summary>Delegate used for all control's event handlers</summary>
    /// <param name="c_newState">Event source</param>
    public delegate void ContextStateChangeDelegate(ContextState c_newState);
}
