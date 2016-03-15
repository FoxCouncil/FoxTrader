﻿using System.Collections.Generic;
using FoxTrader.Game.Utils;

namespace FoxTrader.Game
{
    class Galaxy : ITickable
    {
        private Universe m_universe;
        private readonly int m_index;

        private readonly string m_name;

        private readonly Vector3 m_position;

        private readonly List<System> m_systems;

        public Galaxy(Universe c_universe, int c_index)
        {
            m_universe = c_universe;
            m_index = c_index;

            m_name = Generator.Name();
            m_position = Vector3.Random(Constants.kGalaxySizeMax);

            m_systems = new List<System>(Constants.kSystemsMax);

            for (var a_idx = 0; a_idx <= Constants.kSystemsMax; a_idx++)
            {
                m_systems.Add(new System(this, a_idx));
            }
        }

        public Vector3 Position => m_position;

        public int Index => m_index;

        public string Name => m_name;

        public void Tick()
        {
            m_systems.ForEach(c_system => c_system.Tick());
        }
    }
}
