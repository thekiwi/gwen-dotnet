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

        // common values
        public static Margin Zero = new Margin(0, 0, 0, 0);
        public static Margin One = new Margin(1, 1, 1, 1);
        public static Margin Two = new Margin(2, 2, 2, 2);
        public static Margin Three = new Margin(3, 3, 3, 3);
        public static Margin Four = new Margin(4, 4, 4, 4);
        public static Margin Five = new Margin(5, 5, 5, 5);

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
