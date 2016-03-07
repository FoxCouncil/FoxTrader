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

            if (c_index == 0)
            {
                m_planetoidType = (PlanetoidType)Generator.RandomDistribution(new[] { 1000, 1, 0, 1, 1, 0, 0, 0 });
            }
            else
            {
                m_planetoidType = (PlanetoidType)Generator.RandomDistribution(new[] { 0, 0, 10, 10, 0, 100, 1000, 15 });
            }

            m_name = m_system.Name + "-" + (m_index + 1).ToString();
            m_position = Vector2.Random(kPlanetoidSizeMax);
        }

        public void Tick()
        {
            // Tick?
        }
    }
}
