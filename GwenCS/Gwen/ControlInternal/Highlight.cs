using System;
using Gwen.Control;

namespace Gwen.ControlInternal
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
