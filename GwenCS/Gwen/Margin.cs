using System;

namespace Gwen
{
    public struct Margin : IEquatable<Margin>
    {
        // todo: add equality?
        public int Top;
        public int Bottom;
        public int Left;
        public int Right;

        public Margin(int l, int t, int r, int b)
        {
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }

        public bool Equals(Margin other)
        {
            return (Top == other.Top && Bottom == other.Bottom && Left == other.Left && Right == other.Right);
        }

        public static bool operator==(Margin lhs, Margin rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(Margin lhs, Margin rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
