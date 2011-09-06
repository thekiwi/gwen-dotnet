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

        private Splitter m_Splitter;
        private NumericUpDownButton_Up m_Up;
        private NumericUpDownButton_Down m_Down;

        public int Min { get { return m_iMin; } set { m_iMin = value; } }
        public int Max { get { return m_iMax; } set { m_iMax = value; } }

        public NumericUpDown(Base parent)
            : base(parent)
        {
            SetSize(100, 20);

            m_Splitter = new Splitter(this);
            m_Splitter.Dock = Pos.Right;
            m_Splitter.SetSize(13, 13);

            m_Up = new NumericUpDownButton_Up(m_Splitter);
            m_Up.OnPress += onButtonUp;
            m_Up.IsTabable = false;
            m_Splitter.SetPanel(0, m_Up, false);

            m_Down = new NumericUpDownButton_Down(m_Splitter);
            m_Down.OnPress += onButtonDown;
            m_Down.IsTabable = false;
            m_Down.Padding = new Padding(0, 1, 1, 0);
            m_Splitter.SetPanel(1, m_Down, false);

            m_iMax = 100;
            m_iMin = 0;
            m_Value = 0f;
            Text = "0";
        }

        public event ControlCallback OnValueChanged;

        public override void Dispose()
        {
            m_Splitter.Dispose();
            m_Up.Dispose();
            m_Down.Dispose();
            base.Dispose();
        }

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
