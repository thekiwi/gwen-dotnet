using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Gwen.Controls;

namespace Gwen
{
    public static class Global
    {
        public static Base HoveredControl;
        public static Base KeyboardFocus;
        public static Base MouseFocus;

        public const int MaxCoord = 4096; // added here from various places in code

        public static int Round(double x)
        {
            return (int)Math.Round(x, MidpointRounding.AwayFromZero);
        }

        public static int Trunc(double x)
        {
            return (int)Math.Truncate(x);
        }

        public static int Ceil(double x)
        {
            return (int)Math.Ceiling(x);
        }

        public static Rectangle FloatRect(double x, double y, double w, double h)
        {
            return new Rectangle(Trunc(x), Trunc(y), Trunc(w), Trunc(h));
        }

        public static Rectangle ClampRectToRect(Rectangle inside, Rectangle outside, bool clampSize = false)
        {
            if (inside.X < outside.X)
                inside.X = outside.X;

            if (inside.Y < outside.Y)
                inside.Y = outside.Y;

            if (inside.Right > outside.Right)
            {
                if (clampSize)
                    inside.Width = outside.Width;
                else
                    inside.X = outside.Right - inside.Width;
            }
            if (inside.Bottom > outside.Bottom)
            {
                if (clampSize)
                    inside.Height = outside.Height;
                else
                    inside.Y = outside.Bottom - inside.Height;
            }

            return inside;
        }

    }
}
