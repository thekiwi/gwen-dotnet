using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class GroupBox : GUnit
    {
        public GroupBox(Base parent) : base(parent)
        {
            Control.GroupBox gb = new Control.GroupBox(this);
            gb.Text = "Group Box";
            gb.SetSize(300, 200);
        }
    }
}
