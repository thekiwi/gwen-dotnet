using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class GroupBox : GUnit
    {
        public GroupBox(Base parent) : base(parent)
        {
            Controls.GroupBox gb = new Controls.GroupBox(this);
            gb.Text = "Group Box";
            gb.SetSize(300, 200);
        }
    }
}
