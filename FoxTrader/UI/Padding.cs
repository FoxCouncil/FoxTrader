//   !!  // FoxTrader - Padding.cs
// *.-". // Created: 01-02-2016 [9:35 PM]
//  | |  // ʇɟǝʃʎdoƆ 2016 FoxCouncil 

#region Usings

using System;

#endregion

namespace FoxTrader.UI
{
    /// <summary>Represents some kind of inner spacing</summary>
    public struct Padding : IEquatable<Padding>
    {
        public int Top
        {
            get;
        }

        public int Bottom
        {
            get;
        }

        public int Left
        {
            get;
        }

        public int Right
        {
            get;
        }

        // common values
        public static readonly Padding kZero = new Padding(0, 0, 0, 0);
        public static readonly Padding kOne = new Padding(1, 1, 1, 1);
        public static readonly Padding kTwo = new Padding(2, 2, 2, 2);
        public static readonly Padding kThree = new Padding(3, 3, 3, 3);
        public static readonly Padding kFour = new Padding(4, 4, 4, 4);
        public static readonly Padding kFive = new Padding(5, 5, 5, 5);

        public Padding(int c_left, int c_top, int c_right, int c_bottom)
        {
            Top = c_top;
            Bottom = c_bottom;
            Left = c_left;
            Right = c_right;
        }

        public bool Equals(Padding c_other)
        {
            return c_other.Top == Top && c_other.Bottom == Bottom && c_other.Left == Left && c_other.Right == Right;
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
                var a_result = Top;

                a_result = (a_result * 397) ^ Bottom;
                a_result = (a_result * 397) ^ Left;
                a_result = (a_result * 397) ^ Right;

                return a_result;
            }
        }
    }
}