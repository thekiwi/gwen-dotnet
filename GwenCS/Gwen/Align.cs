using System;
using Gwen.Controls;

namespace Gwen
{
    public static class Align
    {
        public static void Center(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (parent == null) return;
            ctrl.SetPos(parent.Padding.Left + (((parent.Width - parent.Padding.Left - parent.Padding.Right) - ctrl.Width) / 2),
                                (parent.Height - ctrl.Height) / 2);
        }

        public static void AlignLeft(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;

            ctrl.SetPos(parent.Padding.Left, ctrl.Y);
        }

        public static void CenterHorizontally(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;


            ctrl.SetPos(parent.Padding.Left + (((parent.Width - parent.Padding.Left - parent.Padding.Right) - ctrl.Width) / 2), ctrl.Y);
        }

        public static void AlignRight(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;


            ctrl.SetPos(parent.Width - ctrl.Width - parent.Padding.Right, ctrl.Y);
        }

        public static void AlignTop(Base ctrl)
        {
            ctrl.SetPos(ctrl.X, 0);
        }

        public static void CenterVertically(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;

            ctrl.SetPos(ctrl.X, (parent.Height - ctrl.Height) / 2);
        }

        public static void AlignBottom(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;

            ctrl.SetPos(ctrl.X, parent.Height - ctrl.Height);
        }

        public static void PlaceBelow(Base ctrl, Base below, int iBorder = 0)
        {
            ctrl.SetPos(ctrl.X, below.Bottom + iBorder);
        }

    }
}
