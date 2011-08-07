using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls.Symbol
{
    public class Base : Controls.Base
    {
        public Base(Controls.Base parent) : base(parent)
        {
            MouseInputEnabled = false;
        }
    }
}
