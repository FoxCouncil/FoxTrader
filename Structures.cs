using FoxTrader.Game.Utils;

namespace FoxTrader
{
    public struct Vector2
    {
        public Vector2(int c_x, int c_y)
        {
            X = c_x;
            Y = c_y;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        internal static Vector2 Random(int c_maxRange)
        {
            var a_randomX = Generator.RandomRange(0, c_maxRange);
            var a_randomY = Generator.RandomRange(0, c_maxRange);

            return new Vector2(a_randomX, a_randomY);
        }
    };

    public struct Vector3
    {
        public Vector3(int c_x, int c_y, int c_z)
        {
            X = c_x;
            Y = c_y;
            Z = c_z;
        }

        public int X
        {
            get;

            set;
        }

        public int Y
        {
            get;

            set;
        }

        public int Z
        {
            get;

            set;
        }

        internal static Vector3 Random(int c_maxRange)
        {
            var a_randomX = Generator.RandomRange(0, c_maxRange);
            var a_randomY = Generator.RandomRange(0, c_maxRange);
            var a_randomZ = Generator.RandomRange(0, c_maxRange);

            return new Vector3(a_randomX, a_randomY, a_randomZ);
        }
    }
}