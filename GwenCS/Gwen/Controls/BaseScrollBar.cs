using System;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    /// <summary>
    /// Base class for scrollbars.
    /// </summary>
    public class BaseScrollBar : Base
    {
        protected ScrollBarButton[] m_ScrollButton;
        protected ScrollBarBar m_Bar;

        protected bool m_Depressed;
        protected float m_ScrollAmount;
        protected float m_ContentSize;
        protected float m_ViewableContentSize;
        protected float m_NudgeAmount;

        /// <summary>
        /// Invoked when the bar is moved.
        /// </summary>
        public event ControlCallback OnBarMoved;

        /// <summary>
        /// Bar size (in pixels).
        /// </summary>
        public virtual int BarSize { get; set; }

        /// <summary>
        /// Bar position (in pixels).
        /// </summary>
        public virtual int BarPos { get { return 0; } }
        
        /// <summary>
        /// Button size (in pixels).
        /// </summary>
        public virtual int ButtonSize { get { return 0; } }

        public virtual float NudgeAmount { get { return m_NudgeAmount / m_ContentSize; } set { m_NudgeAmount = value; } }
        public float ScrollAmount { get { return m_ScrollAmount; } }
        public float ContentSize { get { return m_ContentSize; } set { if (m_ContentSize != value) Invalidate(); m_ContentSize = value; } }
        public float ViewableContentSize { get { return m_ViewableContentSize; } set { if (m_ViewableContentSize != value) Invalidate(); m_ViewableContentSize = value; } }

        /// <summary>
        /// Indicates whether the bar is horizontal.
        /// </summary>
        public virtual bool IsHorizontal { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseScrollBar"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
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

        /// <summary>
        /// Internal handler invoked on mouse click (left) event.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="down">If set to <c>true</c> mouse button is down.</param>
        internal override void onMouseClickLeft(int x, int y, bool down)
        {

        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollBar(this, IsHorizontal, m_Depressed);
        }

        /// <summary>
        /// Internal handler for the OnBarMoved event.
        /// </summary>
        /// <param name="control">The control.</param>
        internal virtual void onBarMoved(Base control)
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_Bar.Dispose();
            m_ScrollButton[0].Dispose();
            m_ScrollButton[1].Dispose();
            base.Dispose();
        }
    }
}
