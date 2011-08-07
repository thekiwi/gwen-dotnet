using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Controls.Symbol
{
    public class Arrow : Base
    {
        public Arrow(Controls.Base parent) : base(parent)
        {
        
        }

        protected override void Render(Skin.Base skin)
        {
            Rectangle r = new Rectangle(Width/2 - 2, Height/2 - 2, 5, 5);
            skin.DrawArrowRight(r);
        }
    }
}
