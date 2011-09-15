using System;
using Gwen.Control;

namespace Gwen.ControlInternal
{
    internal class SplitterBar : Dragger
    {
        internal SplitterBar(Base parent) : base(parent)
        {
            Target = this;
            RestrictToParent = true;
        }

        protected override void Render(Skin.Base skin)
        {
            if (ShouldDrawBackground)
                skin.DrawButton(this, true, false, IsDisabled);
        }

        protected override void Layout(Skin.Base skin)
        {
            MoveTo(X, Y);
        }
    }
}
