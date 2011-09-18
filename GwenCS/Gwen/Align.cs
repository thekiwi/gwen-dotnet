using System;
using Gwen.Control;

namespace Gwen
{
    public static class Align
    {
        public static void Center(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (parent == null) return;
            ctrl.SetPosition(parent.Padding.Left + (((parent.Width - parent.Padding.Left - parent.Padding.Right) - ctrl.Width) / 2),
                                (parent.Height - ctrl.Height) / 2);
        }

        public static void AlignLeft(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;

            ctrl.SetPosition(parent.Padding.Left, ctrl.Y);
        }

        public static void CenterHorizontally(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;


            ctrl.SetPosition(parent.Padding.Left + (((parent.Width - parent.Padding.Left - parent.Padding.Right) - ctrl.Width) / 2), ctrl.Y);
        }

        public static void AlignRight(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;


            ctrl.SetPosition(parent.Width - ctrl.Width - parent.Padding.Right, ctrl.Y);
        }

        public static void AlignTop(Base ctrl)
        {
            ctrl.SetPosition(ctrl.X, 0);
        }

        public static void CenterVertically(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;

            ctrl.SetPosition(ctrl.X, (parent.Height - ctrl.Height) / 2);
        }

        public static void AlignBottom(Base ctrl)
        {
            Base parent = ctrl.Parent;
            if (null == parent) return;

            ctrl.SetPosition(ctrl.X, parent.Height - ctrl.Height);
        }

        public static void PlaceBelow(Base ctrl, Base below, int iBorder = 0)
        {
            ctrl.SetPosition(ctrl.X, below.Bottom + iBorder);
        }

    }
}
