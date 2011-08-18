using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class SliderBar : Dragger
    {
        internal SliderBar(Base parent) : base(parent)
        {
            Target = this;
            RestrictToParent = true;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawButton(this, m_bDepressed, IsHovered);
        }
    }
}
