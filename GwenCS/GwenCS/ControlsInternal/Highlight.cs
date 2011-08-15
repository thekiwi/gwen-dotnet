using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    internal class Highlight : Base
    {
        public Highlight(Base parent) : base(parent)
        {
            
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawHighlight(this);
        }
    }
}
