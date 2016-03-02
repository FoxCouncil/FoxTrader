

using FoxTrader.Game.Utils;
using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    class Planetoid : ITickable
    {
        private readonly System m_system;
        private readonly int m_index;

        private PlanetoidType m_planetoidType;

        private string m_name;
        private Vector2 m_position;

        public Planetoid(System c_system, int c_index)
        {
            m_system = c_system;
            m_index = c_index;

            m_planetoidType = (PlanetoidType)Generator.RandomRange(0, (int)PlanetoidType.MAX);

            m_name = m_system.Name + "-" + (m_index + 1).ToString();
            m_position = Vector2.Random(kPlanetoidSizeMax);
        }

        public void Tick()
        {
            // Tick?
        }
    }
}
