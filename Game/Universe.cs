using System.Collections.Generic;

namespace FoxTrader.Game
{
    class Universe : ITickable
    {
        private string m_name = "Fox Universe";

        private readonly List<Galaxy> m_galaxies;

        public Universe()
        {
            m_galaxies = new List<Galaxy>(Constants.kGalaxiesMax);
        }

        public void Tick()
        {
            m_galaxies.ForEach(c_galaxy => c_galaxy.Tick());
        }
    }
}
