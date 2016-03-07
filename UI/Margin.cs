using System;

namespace FoxTrader.UI
{
    /// <summary>Represents outer spacing</summary>
    public struct Margin : IEquatable<Margin>
    {
        public int m_top;
        public int m_bottom;
        public int m_left;
        public int m_right;

        // common values
        public static readonly Margin kZero = new Margin(0, 0, 0, 0);
        public static readonly Margin kOne = new Margin(1, 1, 1, 1);
        public static readonly Margin kTwo = new Margin(2, 2, 2, 2);
        public static readonly Margin kThree = new Margin(3, 3, 3, 3);
        public static readonly Margin kFour = new Margin(4, 4, 4, 4);
        public static readonly Margin kFive = new Margin(5, 5, 5, 5);
        public static readonly Margin kSix = new Margin(6, 6, 6, 6);
        public static readonly Margin kSeven = new Margin(7, 7, 7, 7);
        public static readonly Margin kEight = new Margin(8, 8, 8, 8);
        public static readonly Margin kNine = new Margin(9, 9, 9, 9);
        public static readonly Margin kTen = new Margin(10, 10, 10, 10);

        public Margin(int c_left, int c_top, int c_right, int c_bottom)
        {
            m_top = c_top;
            m_bottom = c_bottom;
            m_left = c_left;
            m_right = c_right;
        }

        public bool Equals(Margin c_other)
        {
            return c_other.m_top == m_top && c_other.m_bottom == m_bottom && c_other.m_left == m_left && c_other.m_right == m_right;
        }

        public static bool operator ==(Margin c_lhs, Margin c_rhs)
        {
            return c_lhs.Equals(c_rhs);
        }

        public static bool operator !=(Margin c_lhs, Margin c_rhs)
        {
            return !c_lhs.Equals(c_rhs);
        }

        public override bool Equals(object c_obj)
        {
            if (ReferenceEquals(null, c_obj))
            {
                return false;
            }

            if (c_obj.GetType() != typeof(Margin))
            {
                return false;
            }

            return Equals((Margin)c_obj);
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