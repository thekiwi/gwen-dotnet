using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class VerticalSlider : Slider
    {
        public VerticalSlider(Base parent) : base(parent)
        {
            m_SliderBar.IsHorizontal = false;
        }

        protected override float CalculateValue()
        {
            return 1 - m_SliderBar.Y / (float)(Height - m_SliderBar.Height);
        }

        protected override void UpdateBarFromValue()
        {
            m_SliderBar.MoveTo(m_SliderBar.X, (int)((Height - m_SliderBar.Height) * (1 - m_Value)));
        }

        protected override void onMouseClickLeft(int x, int y, bool down)
        {
            m_SliderBar.MoveTo(m_SliderBar.X, (int) (CanvasPosToLocal(new Point(x, y)).Y - m_SliderBar.Height*0.5));
            m_SliderBar.InputMouseClickLeft(x, y, down);
            onMoved(m_SliderBar);
        }

        protected override void Layout(Skin.Base skin)
        {
            m_SliderBar.SetSize(Width, 15);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawSlider(this, false, m_ClampToNotches ? m_NotchCount : 0, m_SliderBar.Height);
        }
    }
}
