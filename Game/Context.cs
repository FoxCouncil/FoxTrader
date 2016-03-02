using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class Context
    {
        public event ContextStateChangeDelegate StateChanged;

        public ContextState State
        {
            get; private set;
        }

        public Player Player
        {
            get; private set;
        }

        public Universe Universe
        {
            get; private set;
        }

        public Context()
        {
            State = ContextState.Bumpers;
        }

        public void MarkStateComplete(object c_completedState)
        {
            var a_completedState = (ContextState)c_completedState;

            switch (a_completedState)
            {
                case ContextState.Options:
                case ContextState.Bumpers:
                {
                    State = ContextState.MainMenu;

                    StateChanged?.Invoke(State);
                }
                break;
            }
        }

        public void Tick()
        {
            Player?.Tick();
            Universe?.Tick();
        }

        public void Start()
        {
            // TODO: Start a new game
        }

        public void Save(string c_saveName)
        {
            // TODO: Save the current game
        }

        public void Load(string c_saveName)
        {
            // TODO: Load an old game
        }

        public void ViewOptions()
        {
            State = ContextState.Options;
            StateChanged?.Invoke(State);
        }
    }
}
