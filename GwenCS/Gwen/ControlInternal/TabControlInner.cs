using System;
using Gwen.Control;

namespace Gwen.ControlInternal
{
    internal class TabControlInner : Base
    {
        internal TabControlInner(Base parent) : base(parent)
        {
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawTabControl(this);
        }
    }
}
