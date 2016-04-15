using System.Collections.Generic;
using FoxTrader.Game.Utils;
using FoxTrader.Interfaces;
using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class Galaxy : ITickable, IMapObject
    {
        public Vector2 Position
        {
            get;
            set;
        }

        public int Index
        {
            get;
        }

        public string Name
        {
            get;
            set;
        }

        public List<System> Systems
        {
            get;
        }

        public Universe Universe
        {
            get;
        }

        public Galaxy(Universe c_universe, int c_index)
        {
            Universe = c_universe;
            Index = c_index;

            Name = Generator.Name();
            Position = Vector2.Random(kUniverseSizeMax);

            Systems = new List<System>(kSystemsMax);

            for (var a_idx = 0; a_idx <= kSystemsMax; a_idx++)
            {
                Systems.Add(new System(this, a_idx));
            }
        }

        public void Tick()
        {
            Systems.ForEach(c_system => c_system.Tick());
        }
    }
}
