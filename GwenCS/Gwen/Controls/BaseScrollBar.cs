using System;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class BaseScrollBar : Base
    {
        protected ScrollBarButton[] m_ScrollButton;
        protected ScrollBarBar m_Bar;

        protected bool m_Depressed;
        protected float m_ScrollAmount;
        protected float m_ContentSize;
        protected float m_ViewableContentSize;
        protected float m_NudgeAmount;

        public event ControlCallback OnBarMoved;

        public virtual int BarSize { get; set; }
        public virtual int BarPos { get { return 0; } }
        public virtual int ButtonSize { get { return 0; } }
        public virtual float NudgeAmount { get { return m_NudgeAmount / m_ContentSize; } set { m_NudgeAmount = value; } }
        public float ScrollAmount { get { return m_ScrollAmount; } }
        public float ContentSize { get { return m_ContentSize; } set { if (m_ContentSize != value) Invalidate(); m_ContentSize = value; } }
        public float ViewableContentSize { get { return m_ViewableContentSize; } set { if (m_ViewableContentSize != value) Invalidate(); m_ViewableContentSize = value; } }
        public virtual bool IsHorizontal { get { return false; } }

        protected BaseScrollBar(Base parent) : base(parent)
        {
            m_ScrollButton = new ScrollBarButton[2];
            m_ScrollButton[0] = new ScrollBarButton(this);
            m_ScrollButton[1] = new ScrollBarButton(this);

            m_Bar = new ScrollBarBar(this);

            SetBounds(0, 0, 15, 15);
            m_Depressed = false;

            m_ScrollAmount = 0;
            m_ContentSize = 0;
            m_ViewableContentSize = 0;

            NudgeAmount = 20;
        }

        public virtual bool SetScrollAmount(float value, bool forceUpdate = true)
        {
            if (m_ScrollAmount == value && !forceUpdate)
                return false;
            m_ScrollAmount = value;
            Invalidate();
            onBarMoved(this);
            return true;
        }
        
        internal override void onMouseClickLeft(int x, int y, bool down)
        {

        }
        
        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollBar(this, IsHorizontal, m_Depressed);
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

        public override void Dispose()
        {
            m_Bar.Dispose();
            m_ScrollButton[0].Dispose();
            m_ScrollButton[1].Dispose();
            base.Dispose();
        }
    }
}
