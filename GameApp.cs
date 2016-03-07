using System;

namespace FoxTrader
{
    static class GameApp
    {
        private static FoxTraderWindow m_gameWindow;

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
