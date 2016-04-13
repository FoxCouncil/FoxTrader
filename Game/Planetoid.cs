using FoxTrader.Game.Utils;
using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class Planetoid : ITickable, IMapObject
    {
        public System System
        {
            get;
        }

        public int Index
        {
            get;
        }

        public PlanetoidType Type
        {
            get;
        }

        public string Name
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Planetoid(System c_system, int c_index)
        {
            System = c_system;
            Index = c_index;

            if (c_index == 0)
            {
                Type = (PlanetoidType)Generator.RandomDistribution(new[] { 1000, 1, 0, 1, 1, 0, 0, 0 });
            }
            else
            {
                Type = (PlanetoidType)Generator.RandomDistribution(new[] { 0, 0, 10, 10, 0, 100, 1000, 15 });
            }

            Name = System.Name + "-" + (Index + 1);
            Position = Vector2.Random(kSystemSizeMax);
        }

        public void Tick()
        {
            // Tick?
        }
    }
}
