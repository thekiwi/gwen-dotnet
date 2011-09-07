using System;

namespace Gwen.Controls
{
    public class ProgressBar : Label
    {
        protected bool m_Horizontal;
        protected bool m_AutoLabel;
        protected float m_Progress;

        public bool IsHorizontal { get { return m_Horizontal; } set { m_Horizontal = value; } }
        public float Value
        {
            get { return m_Progress; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 1)
                    value = 1;

                m_Progress = value;
                if (m_AutoLabel)
                {
                    int displayVal = (int)(m_Progress * 100);
                    Text = displayVal.ToString() + "%";
                }
            }
        }
        public bool AutoLabel { get { return m_AutoLabel; } set { m_AutoLabel = value; } }

        public ProgressBar(Base parent) : base(parent)
        {
            MouseInputEnabled = false; // [omeg] what? was true
            SetSize(128, 32);
            TextPadding = new Padding(3, 3, 3, 3);
            IsHorizontal = true;

            Alignment = Pos.Center;
            m_Progress = 0;
            m_AutoLabel = true;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawProgressBar(this, m_Horizontal, m_Progress);
        }
    }
}
