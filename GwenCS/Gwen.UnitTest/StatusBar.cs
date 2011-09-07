using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class StatusBar : GUnit
    {
        public StatusBar(Base parent)
            : base(parent)
        {
            Controls.StatusBar sb = new Controls.StatusBar(this);
            Controls.Label left = new Controls.Label(sb);
            left.Text = "Label added to left";
            sb.AddControl(left, false);

            Controls.Label right = new Controls.Label(sb);
            right.Text = "Label added to right";
            sb.AddControl(right, true);

            Controls.Button bl = new Controls.Button(sb);
            bl.Text = "Left button";
            sb.AddControl(bl, false);

            Controls.Button br = new Controls.Button(sb);
            br.Text = "Right button";
            sb.AddControl(br, true);
        }
    }
}
