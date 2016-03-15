using System.Collections.Generic;

namespace FoxTrader.Game
{
    class Universe : ITickable
    {
        private string m_name = "Fox Universe";

        private readonly List<Galaxy> m_galaxies;
        public List<Galaxy> Galaxies => m_galaxies;

        public Universe()
        {
            m_galaxies = new List<Galaxy>(Constants.kGalaxiesMax);

            for (var a_idx = 0; a_idx <= Constants.kGalaxiesMax; a_idx++)
            {
                m_galaxies.Add(new Galaxy(this, a_idx));
            }
        }

        public void Tick()
        {
            m_galaxies.ForEach(c_galaxy => c_galaxy.Tick());
        }
    }
}
