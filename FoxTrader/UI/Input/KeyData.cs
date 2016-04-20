using FoxTrader.UI.Control;
using OpenTK.Input;

namespace FoxTrader.UI.Input
{
    /// <summary>Keyboard state</summary>
    internal class KeyData
    {
        public bool[] KeyState
        {
            get;
            set;
        }

        public float[] NextRepeat
        {
            get;
            set;
        }

        public GameControl Target
        {
            get;
            set;
        }

        public KeyboardKeyEventArgs[] KeyEventArgs
        {
            get;
            set;
        }

        public KeyData()
        {
            KeyState = new bool[1024];
            NextRepeat = new float[1024];
            KeyEventArgs = new KeyboardKeyEventArgs[1024];
            // everything is initialized to 0 by default
        }
    }
}