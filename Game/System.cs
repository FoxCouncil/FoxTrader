using System.Collections.Generic;
using FoxTrader.Game.Utils;

namespace FoxTrader.Game
{
    class System : ITickable
    {
        private Galaxy m_galaxy;
        private int m_index;

        private Vector3 m_position;

        private readonly List<Planetoid> m_planetoids;

        public System(Galaxy c_galaxy, int c_index)
        {
            m_galaxy = c_galaxy;
            m_index = c_index;

            Name = Generator.Name();
            m_position = Vector3.Random(Constants.kSystemSizeMax);

            m_planetoids = new List<Planetoid>(Constants.kPlanetoidsMax);

            for (var a_idx = 0; a_idx <= Constants.kPlanetoidsMax; a_idx++)
            {
                m_planetoids.Add(new Planetoid(this, a_idx));
            }
        }

        public string Name
        {
            get;
        }

        public void Tick()
        {
            m_planetoids.ForEach(c_planet => c_planet.Tick());
        }
    }
}
