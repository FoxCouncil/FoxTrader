using System;
using FoxTrader.Game;

namespace FoxTrader
{
    static class FoxTraderGame
    {
        private static FoxTraderWindow m_gameWindow;
        private static Context m_gameContextInstance;
        private static readonly object m_gameContextLock = new object();

        internal static Context GameContextInstance
        {
            get
            {
                lock (m_gameContextLock)
                {
                    if (m_gameContextInstance == null)
                    {
                        m_gameContextInstance = new Context();
                    }
                }

                return m_gameContextInstance;
            }
        }

        [STAThread]
        static void Main()
        {
            I18N.Initialize();

            using (m_gameWindow = new FoxTraderWindow())
            {
                m_gameWindow.Icon = Properties.Resources.ico_FoxTrader;
                m_gameWindow.Title = "Fox Trader";
                m_gameWindow.VSync = OpenTK.VSyncMode.Off;
                m_gameWindow.Run(60, 60);
            }
        }
    }
}
