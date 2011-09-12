using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class Properties : GUnit
    {
        public Properties(Base parent)
            : base(parent)
        {
            Controls.Label label = new Controls.Label(this);
            label.Text = "Not implemented yet";
        }
    }
}
