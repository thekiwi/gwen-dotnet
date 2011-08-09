using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class HorizontalScrollBar : BaseScrollBar
    {
        public override int BarSize
        {
            get { return m_Bar.Width; }
            set { m_Bar.Width = value; }
        }

        public override int BarPos
        {
            get { return m_Bar.X - Height; }
        }

        public override int ButtonSize
        {
            get { return Height; }
        }

        public HorizontalScrollBar(Base parent)
            : base(parent)
        {
            m_Bar.IsHorizontal = true;

            m_ScrollButton[0].SetDirectionLeft();
            m_ScrollButton[0].OnPress += NudgeLeft;

            m_ScrollButton[1].SetDirectionRight();
            m_ScrollButton[1].OnPress += NudgeRight;

            m_Bar.OnDragged += onBarMoved;
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);

            m_ScrollButton[0].Width = Height;
            m_ScrollButton[0].Dock = Pos.Left;

            m_ScrollButton[1].Width = Height;
            m_ScrollButton[1].Dock = Pos.Right;

            m_Bar.Height = ButtonSize;
            m_Bar.Padding = new Padding(ButtonSize, 0, ButtonSize, 0);

            float barWidth = (m_fViewableContentSize / m_fContentSize) * (Width - (ButtonSize * 2));

            if (barWidth < ButtonSize * 0.5)
                barWidth = Global.Trunc(ButtonSize * 0.5);

            m_Bar.Width = Global.Trunc(barWidth);
            m_Bar.IsHidden = Width - (ButtonSize * 2) <= barWidth;

            //Based on our last scroll amount, produce a position for the bar
            if (!m_Bar.IsDepressed)
            {
                SetScrollAmount(ScrollAmount, true);
            }
        }

        public virtual void NudgeLeft(Base control)
        {
            if (!IsDisabled)
                SetScrollAmount(ScrollAmount - NudgeAmount, true);
        }

        public virtual void NudgeRight(Base control)
        {
            if (!IsDisabled)
                SetScrollAmount(ScrollAmount + NudgeAmount, true);
        }

        public override void ScrollToLeft()
        {
            SetScrollAmount(0, true);
        }

        public override void ScrollToRight()
        {
            SetScrollAmount(1, true);
        }

        public override float NudgeAmount
        {
            get
            {
                if (m_bDepressed)
                    return m_fViewableContentSize / m_fContentSize;
                else
                    return base.NudgeAmount;
            }
            set
            {
                base.NudgeAmount = value;
            }
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            if (pressed)
            {
                m_bDepressed = true;
                Global.MouseFocus = this;
            }
            else
            {
                Point clickPos = CanvasPosToLocal(new Point(x, y));
                if (clickPos.X < m_Bar.X)
                    NudgeLeft(this);
                else
                    if (clickPos.X > m_Bar.X + m_Bar.Width)
                        NudgeRight(this);

                m_bDepressed = false;
                Global.MouseFocus = null;
            }
        }

        protected override float CalculateScrolledAmount()
        {
            return (float)(m_Bar.X - ButtonSize) / (Width - m_Bar.Width - (ButtonSize * 2));
        }

        public override bool SetScrollAmount(float amount, bool forceUpdate)
        {
            amount = Global.Clamp(amount, 0, 1);

            if (!base.SetScrollAmount(amount, forceUpdate))
                return false;

            if (forceUpdate)
            {
                int newX = Global.Trunc(ButtonSize + (amount * ((Width - m_Bar.Width) - (ButtonSize * 2))));
                m_Bar.MoveTo(newX, m_Bar.Y);
            }

            return true;
        }

        protected override void onBarMoved(Base control)
        {
            if (m_Bar.IsDepressed)
            {
                SetScrollAmount(CalculateScrolledAmount(), false);
                base.onBarMoved(control);
            }
            else
                InvalidateParent();
        }
    }
}
