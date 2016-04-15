using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class GameContext
    {
        #region Singleton
        private static GameContext m_gameContextInstance;
        private static readonly object m_gameContextLock = new object();

        internal static GameContext Instance
        {
            get
            {
                lock (m_gameContextLock)
                {
                    if (m_gameContextInstance == null)
                    {
                        m_gameContextInstance = new GameContext();
                    }
                }

                return m_gameContextInstance;
            }
        }
        #endregion

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

        public GameContext()
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
                }
                break;

                case ContextState.NewGame:
                {
                    // TODO: Intro State!
                    State = ContextState.Game;
                }
                break;
            }

            StateChanged?.Invoke(State);
        }

        public void Tick()
        {
            Player?.Tick();
            Universe?.Tick();
        }

        public void New()
        {
            if (State != ContextState.MainMenu)
            {
                return;
            }

            Universe = new Universe();
            Player = new Player();

            State = ContextState.NewGame;
            StateChanged?.Invoke(State);
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
