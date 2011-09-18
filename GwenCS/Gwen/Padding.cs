using System;

namespace Gwen
{
    public struct Padding : IEquatable<Padding>
    {
        public int Top;
        public int Bottom;
        public int Left;
        public int Right;

        // common values
        public static Padding Zero = new Padding(0, 0, 0, 0);
        public static Padding One = new Padding(1, 1, 1, 1);
        public static Padding Two = new Padding(2, 2, 2, 2);
        public static Padding Three = new Padding(3, 3, 3, 3);
        public static Padding Four = new Padding(4, 4, 4, 4);
        public static Padding Five = new Padding(5, 5, 5, 5);

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
