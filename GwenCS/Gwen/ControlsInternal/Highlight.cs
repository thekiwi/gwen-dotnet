using System;
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
