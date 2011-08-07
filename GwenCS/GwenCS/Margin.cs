using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen
{
    public struct Margin : IEquatable<Margin>
    {
        // todo: add equality?
        public int top;
        public int bottom;
        public int left;
        public int right;

        public Margin(int t, int b, int l, int r)
        {
            top = t;
            bottom = b;
            left = l;
            right = r;
        }

        public bool Equals(Margin other)
        {
            return (top == other.top && bottom == other.bottom && left == other.left && right == other.right);
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
