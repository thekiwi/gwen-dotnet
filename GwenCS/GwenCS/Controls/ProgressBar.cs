using System;

namespace Gwen.Controls
{
    public class ProgressBar : Label
    {
        protected bool m_bHorizontal;
        protected bool m_bAutoLabel;
        protected float m_fProgress;

        public bool IsHorizontal { get { return m_bHorizontal; } set { m_bHorizontal = value; } }
        public float Value
        {
            get { return m_fProgress; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 1)
                    value = 1;

                m_fProgress = value;
                if (m_bAutoLabel)
                {
                    int displayVal = (int)(m_fProgress * 100);
                    Text = displayVal.ToString() + "%";
                }
            }
        }
        public bool AutoLabel { get { return m_bAutoLabel; } set { m_bAutoLabel = value; } }

        public ProgressBar(Base parent) : base(parent)
        {
            MouseInputEnabled = false; // [omeg] what? was true
            SetSize(128, 32);
            TextPadding = new Padding(3, 3, 3, 3);
            IsHorizontal = true;

            Alignment = Pos.Center;
            m_fProgress = 0;
            m_bAutoLabel = true;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawProgressBar(this, m_bHorizontal, m_fProgress);
        }
    }
}
