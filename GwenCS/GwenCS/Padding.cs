using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public struct Padding : IEquatable<Padding>
    {
        // todo: add equality?
        public int Top;
        public int Bottom;
        public int Left;
        public int Right;

        public Padding(int t, int b, int l, int r)
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
