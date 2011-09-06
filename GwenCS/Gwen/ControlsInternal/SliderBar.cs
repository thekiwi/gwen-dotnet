using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class SliderBar : Dragger
    {
        protected bool m_bHorizontal;

        public bool IsHorizontal { get { return m_bHorizontal; } set { m_bHorizontal = value; } }

        internal SliderBar(Base parent) : base(parent)
        {
            Target = this;
            RestrictToParent = true;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawSliderButton(this, IsDepressed, IsHorizontal);
        }
    }
}
