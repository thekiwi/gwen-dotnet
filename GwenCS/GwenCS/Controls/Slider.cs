using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class Slider : Base
    {
        protected SliderBar m_SliderBar;
        protected bool m_bClampToNotches;
        protected int m_iNumNotches;
        protected float m_fValue;
        protected float m_fMin;
        protected float m_fMax;

        public float Value
        {
            get { return m_fMin + (m_fValue * (m_fMax - m_fMin)); }
            set
            {
                if (value < m_fMin) value = m_fMin;
                if (value > m_fMax) value = m_fMax;
                // Normalize Value
                value = (value - m_fMin) / (m_fMax - m_fMin);
                SetValueInternal(value);
                Redraw();
            }
        }

        public event ControlCallback OnValueChanged;

        protected Slider(Base parent) : base(parent)
        {
            SetBounds(new Rectangle(0, 0, 32, 128));

            m_SliderBar = new SliderBar(this);
            m_SliderBar.OnDragged += onMoved;

            m_fMin = 0.0f;
            m_fMax = 1.0f;

            m_bClampToNotches = false;
            m_iNumNotches = 5;
            m_fValue = 0.0f;

            IsTabable = true;
        }

        internal override bool onKeyRight(bool bDown)
        {
            if (bDown)
                Value = Value + 1;
            return true;
        }

        internal override bool onKeyUp(bool bDown)
        {
            if (bDown)
                Value = Value + 1;
            return true;
        }

        internal override bool onKeyLeft(bool bDown)
        {
            if (bDown)
                Value = Value - 1;
            return true;
        }

        internal override bool onKeyDown(bool bDown)
        {
            if (bDown)
                Value = Value - 1;
            return true;
        }

        // omeg
        internal override bool onKeyHome(bool bDown)
        {
            if (bDown)
                Value = m_fMin;
            return true;
        }

        // omeg
        internal override bool onKeyEnd(bool bDown)
        {
            if (bDown)
                Value = m_fMax;
            return true;
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            
        }

        protected virtual void onMoved(Base control)
        {
            SetValueInternal(CalculateValue());
        }

        protected virtual float CalculateValue()
        {
            return 0;
        }

        protected virtual void UpdateBarFromValue()
        {
            
        }

        protected virtual void SetValueInternal(float val)
        {
            if (m_bClampToNotches)
            {
                val = (float)Math.Floor((val * m_iNumNotches) + 0.5f);
                val /= m_iNumNotches;
            }

            if (m_fValue != val)
            {
                m_fValue = val;
                if (OnValueChanged != null)
                    OnValueChanged.Invoke(this);
            }

            UpdateBarFromValue();
        }

        public void SetRange(float min, float max)
        {
            m_fMin = min;
            m_fMax = max;
        }

        protected override void RenderFocus(Skin.Base skin)
        {
            if (Global.KeyboardFocus != this) return;
            if (!IsTabable) return;
            
            skin.DrawKeyboardHighlight(this, RenderBounds, 0);
        }
    }
}
