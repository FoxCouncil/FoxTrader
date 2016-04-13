using System.Collections.Generic;
using FoxTrader.Game.Utils;
using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class System : ITickable, IMapObject
    {
        public string Name
        {
            get;
            set;
        }

        public int Index
        {
            get;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Galaxy Galaxy
        {
            get;
        }

        public List<Planetoid> Planetoids
        {
            get;
        }

        public System(Galaxy c_galaxy, int c_index)
        {
            Galaxy = c_galaxy;
            Index = c_index;

            Name = Generator.Name();
            Position = Vector2.Random(kGalaxySizeMax);

            Planetoids = new List<Planetoid>(kPlanetoidsMax);

            for (var a_idx = 0; a_idx <= kPlanetoidsMax; a_idx++)
            {
                Planetoids.Add(new Planetoid(this, a_idx));
            }
        }


        public void Tick()
        {
            Planetoids.ForEach(c_planet => c_planet.Tick());
        }
    }
}
