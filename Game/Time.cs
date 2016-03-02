using System;

namespace FoxTrader.Game
{
    public static class Time
    {
        private static DateTime m_startTime;
        private static DateTime m_updateTime;
        private static DateTime m_spaceTime;
        private static bool m_isEnabled;
        private static bool m_isInitialized;
        private static readonly object m_staticLock = new object();

        public static void Initialize()
        {
            lock (m_staticLock)
            {
                if (!m_isInitialized)
                {
                    m_startTime = DateTime.Now;
                    m_updateTime = DateTime.Now;
                    m_spaceTime = m_startTime.AddYears(Constants.kDefaultFluxCapacitance);

                    m_isEnabled = true;

                    m_isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("You cannot init Time yet again!");
                }
            }
        }

        public static void Tick()
        {
            if (!m_isEnabled)
            {
                return;
            }

            var a_timeDifference = DateTime.Now - m_updateTime;
            m_spaceTime = m_spaceTime.AddTicks(a_timeDifference.Ticks * Constants.kDefaultTimeDilation);
            m_updateTime = DateTime.Now;
        }

        public static DateTime SpaceTime => m_spaceTime;

        public static bool IsEnabled => m_isEnabled;
    }
}
