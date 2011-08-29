using System;
using Gwen.Controls.Layout;

namespace Gwen.Controls
{
    internal class NumericUpDownButton_Up : Button
    {
        public NumericUpDownButton_Up(Base parent)
            : base(parent)
        {
            SetSize(7, 7);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawNumericUpDownButton(this, IsDepressed, true);
        }
    }

    internal class NumericUpDownButton_Down : Button
    {
        public NumericUpDownButton_Down(Base parent)
            : base(parent)
        {
            SetSize(7, 7);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawNumericUpDownButton(this, IsDepressed, false);
        }
    }

    public class NumericUpDown : TextBoxNumeric
    {
        protected int m_iMax;
        protected int m_iMin;

        public int Min { get { return m_iMin; } set { m_iMin = value; } }
        public int Max { get { return m_iMax; } set { m_iMax = value; } }

        public NumericUpDown(Base parent)
            : base(parent)
        {
            SetSize(100, 20);

            Splitter splitter = new Splitter(this);
            splitter.Dock = Pos.Right;
            splitter.SetSize(13, 13);

            NumericUpDownButton_Up up = new NumericUpDownButton_Up(splitter);
            up.OnPress += onButtonUp;
            up.IsTabable = false;
            splitter.SetPanel(0, up, false);

            NumericUpDownButton_Down down = new NumericUpDownButton_Down(splitter);
            down.OnPress += onButtonDown;
            down.IsTabable = false;
            down.Padding = new Padding(0, 1, 1, 0);
            splitter.SetPanel(1, down, false);

            m_iMax = 100;
            m_iMin = 0;
            m_Value = 0f;
            SetText("0");
        }

        public event ControlCallback OnValueChanged;

        internal override bool onKeyUp(bool bDown)
        {
            if (bDown) onButtonUp(null);
            return true;
        }

        internal override bool onKeyDown(bool bDown)
        {
            if (bDown) onButtonDown(null);
            return true;
        }
        
        protected virtual void onButtonUp(Base control)
        {
            Value = m_Value + 1;
        }

        protected virtual void onButtonDown(Base control)
        {
            Value = m_Value - 1;
        }

        protected override bool IsTextAllowed(string str)
        {
            float d;
            if (!float.TryParse(str, out d))
                return false;
            if (d < m_iMin) return false;
            if (d > m_iMax) return false;
            return true;
        }

        public override float Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value < m_iMin) value = m_iMin;
                if (value > m_iMax) value = m_iMax;
                if (value == m_Value) return;

                base.Value = value;
            }
        }
    }
}
