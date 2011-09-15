using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class Properties : GUnit
    {
        public Properties(Base parent)
            : base(parent)
        {
            Control.Label label = new Control.Label(this);
            label.Text = "Not implemented yet";
        }
    }
}
