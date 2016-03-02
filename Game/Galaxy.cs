using System.Collections.Generic;
using FoxTrader.Game.Utils;

namespace FoxTrader.Game
{
    class Galaxy : ITickable
    {
        private Universe m_universe;
        private int m_index;

        private string m_name;

        private Vector3 m_position;

        private readonly List<System> m_systems;

        public Galaxy(Universe c_universe, int c_index)
        {
            m_universe = c_universe;
            m_index = c_index;

            m_name = Generator.Name();
            m_position = Vector3.Random(Constants.kGalaxySizeMax);

            m_systems = new List<System>(Constants.kSystemsMax);
        }

        public void Tick()
        {
            m_systems.ForEach(c_system => c_system.Tick());
        }
    }
}
