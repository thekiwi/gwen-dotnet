using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class TreeControl : GUnit
    {
        public TreeControl(Base parent)
            : base(parent)
        {
            Controls.Label label = new Controls.Label(this);
            label.Text = "Not implemented yet";
        }
    }
}
