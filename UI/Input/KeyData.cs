using FoxTrader.UI.Control;

namespace FoxTrader.UI.Input
{
    /// <summary>Keyboard state</summary>
    internal class KeyData
    {
        public readonly bool[] m_keyState;
        public readonly float[] m_nextRepeat;
        public bool m_leftMouseDown;
        public bool m_rightMouseDown;
        public GameControl m_target;

        public KeyData()
        {
            m_keyState = new bool[(int)Key.Count];
            m_nextRepeat = new float[(int)Key.Count];
            // everything is initialized to 0 by default
        }
    }
}