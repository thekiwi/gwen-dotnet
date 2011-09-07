using System;

namespace Gwen
{
    public struct Padding : IEquatable<Padding>
    {
        public int Top;
        public int Bottom;
        public int Left;
        public int Right;

        public Padding(int l, int t, int r, int b)
        {
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }

        public bool Equals(Padding other)
        {
            return (Top == other.Top && Bottom == other.Bottom && Left == other.Left && Right == other.Right);
        }

        public static bool operator ==(Padding lhs, Padding rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Padding lhs, Padding rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
