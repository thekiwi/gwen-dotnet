using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class Slider : Base
    {
        protected SliderBar m_SliderBar;
        protected bool m_ClampToNotches;
        protected int m_NumNotches;
        protected float m_Value;
        protected float m_Min;
        protected float m_Max;

        public float Value
        {
            get { return m_Min + (m_Value * (m_Max - m_Min)); }
            set
            {
                if (value < m_Min) value = m_Min;
                if (value > m_Max) value = m_Max;
                // Normalize Value
                value = (value - m_Min) / (m_Max - m_Min);
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

            m_Min = 0.0f;
            m_Max = 1.0f;

            m_ClampToNotches = false;
            m_NumNotches = 5;
            m_Value = 0.0f;

            IsTabable = true;
        }

        public override void Dispose()
        {
            m_SliderBar.Dispose();
            base.Dispose();
        }

        internal override bool onKeyRight(bool down)
        {
            if (down)
                Value = Value + 1;
            return true;
        }

        internal override bool onKeyUp(bool down)
        {
            if (down)
                Value = Value + 1;
            return true;
        }

        internal override bool onKeyLeft(bool down)
        {
            if (down)
                Value = Value - 1;
            return true;
        }

        internal override bool onKeyDown(bool down)
        {
            if (down)
                Value = Value - 1;
            return true;
        }

        // omeg
        internal override bool onKeyHome(bool down)
        {
            if (down)
                Value = m_Min;
            return true;
        }

        // omeg
        internal override bool onKeyEnd(bool down)
        {
            if (down)
                Value = m_Max;
            return true;
        }

        internal override void onMouseClickLeft(int x, int y, bool down)
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
            if (m_ClampToNotches)
            {
                val = (float)Math.Floor((val * m_NumNotches) + 0.5f);
                val /= m_NumNotches;
            }

            if (m_Value != val)
            {
                m_Value = val;
                if (OnValueChanged != null)
                    OnValueChanged.Invoke(this);
            }

            UpdateBarFromValue();
        }

        public void SetRange(float min, float max)
        {
            m_Min = min;
            m_Max = max;
        }

        protected override void RenderFocus(Skin.Base skin)
        {
            if (Global.KeyboardFocus != this) return;
            if (!IsTabable) return;
            
            skin.DrawKeyboardHighlight(this, RenderBounds, 0);
        }
    }
}
