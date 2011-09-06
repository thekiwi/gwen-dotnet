using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
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
