using System;

namespace FoxTrader
{
    static class GameApp
    {
        private static FoxTraderWindow m_gameWindow;

        [STAThread]
        static void Main()
        {
            Log.Initialize();
            Log.Info("Game Started", "THEAPP");
            I18N.Initialize();
            Log.Info("I18N Started", "THEAPP");

            using (m_gameWindow = new FoxTraderWindow())
            {
                m_gameWindow.Icon = Properties.Resources.ico_FoxTrader;
                m_gameWindow.Title = "Fox Trader";
                m_gameWindow.VSync = OpenTK.VSyncMode.Off;
                Log.Info("Window Loading", "THEAPP");
                m_gameWindow.Run(60, 60);
            }
        }
    }
}
