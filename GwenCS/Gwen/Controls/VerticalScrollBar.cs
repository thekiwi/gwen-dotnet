using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class VerticalScrollBar : BaseScrollBar
    {
        public override int BarSize
        {
            get { return m_Bar.Height; }
            set { m_Bar.Height = value; }
        }

        public override int BarPos
        {
            get { return m_Bar.Y - Width; }
        }

        public override int ButtonSize
        {
            get { return Width; }
        }

        public VerticalScrollBar(Base parent)
            : base(parent)
        {
            m_Bar.IsVertical = true;

            m_ScrollButton[0].SetDirectionUp();
            m_ScrollButton[0].OnPress += NudgeUp;

            m_ScrollButton[1].SetDirectionDown();
            m_ScrollButton[1].OnPress += NudgeDown;

            m_Bar.OnDragged += onBarMoved;
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);

            m_ScrollButton[0].Height = Width;
            m_ScrollButton[0].Dock = Pos.Top;

            m_ScrollButton[1].Height = Width;
            m_ScrollButton[1].Dock = Pos.Bottom;

            m_Bar.Width = ButtonSize;
            m_Bar.Padding = new Padding(0, ButtonSize, 0, ButtonSize);

            float barHeight = (m_ViewableContentSize / m_ContentSize) * (Height - (ButtonSize * 2));

            if (barHeight < ButtonSize * 0.5f)
                barHeight = Global.Trunc(ButtonSize * 0.5f);

            m_Bar.Height = Global.Trunc(barHeight);
            m_Bar.IsHidden = Height - (ButtonSize * 2) <= barHeight;

            //Based on our last scroll amount, produce a position for the bar
            if (!m_Bar.IsDepressed)
            {
                SetScrollAmount(ScrollAmount, true);
            }
        }

        public virtual void NudgeUp(Base control)
        {
            if (!IsDisabled)
                SetScrollAmount(ScrollAmount - NudgeAmount, true);
        }

        public virtual void NudgeDown(Base control)
        {
            if (!IsDisabled)
                SetScrollAmount(ScrollAmount + NudgeAmount, true);
        }

        public override void ScrollToTop()
        {
            SetScrollAmount(0, true);
        }

        public override void ScrollToBottom()
        {
            SetScrollAmount(1, true);
        }

        public override float NudgeAmount
        {
            get
            {
                if (m_Depressed)
                    return m_ViewableContentSize / m_ContentSize;
                else
                    return base.NudgeAmount;
            }
            set
            {
                base.NudgeAmount = value;
            }
        }

        internal override void onMouseClickLeft(int x, int y, bool down)
        {
            if (down)
            {
                m_Depressed = true;
                Global.MouseFocus = this;
            }
            else
            {
                Point clickPos = CanvasPosToLocal(new Point(x, y));
                if (clickPos.Y < m_Bar.Y)
                    NudgeUp(this);
                else if (clickPos.Y > m_Bar.Y + m_Bar.Height)
                    NudgeDown(this);

                m_Depressed = false;
                Global.MouseFocus = null;
            }
        }

        protected override float CalculateScrolledAmount()
        {
            return (float)(m_Bar.Y - ButtonSize) / (Height - m_Bar.Height - (ButtonSize * 2));
        }

        public override bool SetScrollAmount(float amount, bool forceUpdate)
        {
            amount = Global.Clamp(amount, 0, 1);

            if (!base.SetScrollAmount(amount, forceUpdate))
                return false;

            if (forceUpdate)
            {
                int newY = Global.Trunc(ButtonSize + (amount * ((Height - m_Bar.Height) - (ButtonSize * 2))));
                m_Bar.MoveTo(m_Bar.X, newY);
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
