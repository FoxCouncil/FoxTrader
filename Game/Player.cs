using FoxTrader.Interfaces;
using static FoxTrader.Constants;

namespace FoxTrader.Game
{
    public class Player : ITickable
    {
        public string Name
        {
            get; set;
        }

        public long Money
        {
            get; set;
        }

        public SpaceShipType SpaceShip
        {
            get; set;
        }

        public Vector2 Position
        {
            get; private set;
        }

        public Player()
        {
            Setup();
        }

        public Player(string c_playerName)
        {
            Setup(c_playerName);
        }

        public Player(string c_playerName, long c_playerMoney)
        {
            Setup(c_playerName, c_playerMoney);
        }

        private void Setup(string c_playerName = kDefaultPlayerName, long c_playerMoney = kDefaultPlayerMoney)
        {
            Name = c_playerName;
            Money = c_playerMoney;
            SpaceShip = SpaceShipType.Shuttle;
        }

        public void Tick()
        {
            // TODO
        }
    }
}
