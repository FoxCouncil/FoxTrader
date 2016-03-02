using System;

namespace FoxTrader.UI
{
    /// <summary>Represents inner spacing</summary>
    public struct Padding : IEquatable<Padding>
    {
        public readonly int m_top;
        public readonly int m_bottom;
        public readonly int m_left;
        public readonly int m_right;

        // common values
        public static Padding m_zero = new Padding(0, 0, 0, 0);
        public static Padding m_one = new Padding(1, 1, 1, 1);
        public static Padding m_two = new Padding(2, 2, 2, 2);
        public static Padding m_three = new Padding(3, 3, 3, 3);
        public static Padding m_four = new Padding(4, 4, 4, 4);
        public static Padding m_five = new Padding(5, 5, 5, 5);

        public Padding(int c_left, int c_top, int c_right, int c_bottom)
        {
            m_top = c_top;
            m_bottom = c_bottom;
            m_left = c_left;
            m_right = c_right;
        }

        public bool Equals(Padding c_other)
        {
            return c_other.m_top == m_top && c_other.m_bottom == m_bottom && c_other.m_left == m_left && c_other.m_right == m_right;
        }

        public static bool operator ==(Padding c_lhs, Padding c_rhs)
        {
            return c_lhs.Equals(c_rhs);
        }

        public static bool operator !=(Padding c_lhs, Padding c_rhs)
        {
            return !c_lhs.Equals(c_rhs);
        }

        public override bool Equals(object c_obj)
        {
            if (ReferenceEquals(null, c_obj))
            {
                return false;
            }

            if (c_obj.GetType() != typeof(Padding))
            {
                return false;
            }

            return Equals((Padding)c_obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var a_result = m_top;

                a_result = (a_result * 397) ^ m_bottom;
                a_result = (a_result * 397) ^ m_left;
                a_result = (a_result * 397) ^ m_right;

                return a_result;
            }
        }
    }
}