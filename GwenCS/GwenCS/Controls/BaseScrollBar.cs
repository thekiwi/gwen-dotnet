using System;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class BaseScrollBar : Base
    {
        protected ScrollBarButton[] m_ScrollButton = new ScrollBarButton[2];
        protected ScrollBarBar m_Bar;

        protected bool m_bDepressed;
        protected float m_fScrollAmount;
        protected float m_fContentSize;
        protected float m_fViewableContentSize;
        protected float m_fNudgeAmount;

        public event ControlCallback OnBarMoved;

        public virtual int BarSize { get; set; }
        public virtual int BarPos { get { return 0; } }
        public virtual int ButtonSize { get { return 0; } }
        public virtual float NudgeAmount { get { return m_fNudgeAmount / m_fContentSize; } set { m_fNudgeAmount = value; } }
        public float ScrollAmount { get { return m_fScrollAmount; } }
        public float ContentSize { get { return m_fContentSize; } set { if (m_fContentSize != value) Invalidate(); m_fContentSize = value; } }
        public float ViewableContentSize { get { return m_fViewableContentSize; } set { if (m_fViewableContentSize != value) Invalidate(); m_fViewableContentSize = value; } }

        protected BaseScrollBar(Base parent) : base(parent)
        {
            m_ScrollButton[0] = new ScrollBarButton(this);
            m_ScrollButton[1] = new ScrollBarButton(this);

            m_Bar = new ScrollBarBar(this);

            SetBounds(0, 0, 15, 15);
            m_bDepressed = false;

            m_fScrollAmount = 0;
            m_fContentSize = 0;
            m_fViewableContentSize = 0;

            NudgeAmount = 20;
        }

        public virtual bool SetScrollAmount(float value, bool forceUpdate = true)
        {
            if (m_fScrollAmount == value) return false;
            m_fScrollAmount = value;
            Invalidate();
            onBarMoved(this);
            return true;
        }
        /*
        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
        }
        */
        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollBar(this, false, m_bDepressed); // [omeg] always horizontal?
        }

        protected virtual void onBarMoved(Base control)
        {
            if (OnBarMoved != null)
                OnBarMoved.Invoke(this);
        }

        protected virtual float CalculateScrolledAmount()
        {
            return 0;
        }

        protected virtual int CalculateBarSize()
        {
            return 0;
        } 

        public virtual void ScrollToLeft() { } 
        public virtual void ScrollToRight() { }
        public virtual void ScrollToTop() { }
        public virtual void ScrollToBottom() { }
    }
}
