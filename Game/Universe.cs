using System.Collections.Generic;
using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class Universe : ITickable
    {
        public List<Galaxy> Galaxies
        {
            get;
        }

        public string Name
        {
            get;
        } = $"{kDefaultPlayerName}'s Universe";

        public Universe()
        {
            Galaxies = new List<Galaxy>(kGalaxiesMax);

            for (var a_idx = 0; a_idx <= kGalaxiesMax; a_idx++)
            {
                Galaxies.Add(new Galaxy(this, a_idx));
            }


            var a_testGalaxy = new Galaxy(this, kGalaxiesMax + 1);
            a_testGalaxy.Position = new Vector2(0, 0);
            a_testGalaxy.Name = "Test Galaxy";

            Galaxies.Add(a_testGalaxy);
        }

        public void Tick()
        {
            Galaxies.ForEach(c_galaxy => c_galaxy.Tick());
        }
    }
}
