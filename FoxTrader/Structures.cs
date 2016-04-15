using System.Runtime.InteropServices;
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

        internal static Vector2 Zero => new Vector2(0, 0);
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

        public Vector2 ToVec2()
        {
            return new Vector2(X, Y);
        }

        internal static Vector3 Random(int c_maxRange)
        {
            var a_randomX = Generator.RandomRange(0, c_maxRange);
            var a_randomY = Generator.RandomRange(0, c_maxRange);
            var a_randomZ = Generator.RandomRange(0, c_maxRange);

            return new Vector3(a_randomX, a_randomY, a_randomZ);
        }

        internal static Vector3 Zero => new Vector3(0, 0, 0);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        public short X;
        public short Y;

        public float U;
        public float V;

        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }
}