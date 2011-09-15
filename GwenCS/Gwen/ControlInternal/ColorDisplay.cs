using System;
using System.Drawing;
using Gwen.Control;

namespace Gwen.ControlInternal
{
    public class ColorDisplay : Base
    {
        protected Color m_Color;
        protected bool m_DrawCheckers;

        public ColorDisplay(Base parent) : base(parent)
        {
            SetSize(32, 32);
            m_Color = Color.FromArgb(255, 255, 0, 0);
            m_DrawCheckers = true;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawColorDisplay(this, m_Color);
        }

        public Color Color { get { return m_Color; } set { m_Color = value; } }
        public bool DrawCheckers { get { return m_DrawCheckers; } set { m_DrawCheckers = value; } }
        public int R { get { return m_Color.R; } set { m_Color = Color.FromArgb(m_Color.A, value, m_Color.G, m_Color.B); } }
        public int G { get { return m_Color.G; } set { m_Color = Color.FromArgb(m_Color.A, m_Color.R, value, m_Color.B); } }
        public int B { get { return m_Color.B; } set { m_Color = Color.FromArgb(m_Color.A, m_Color.R, m_Color.G, value); } }
        public int A { get { return m_Color.A; } set { m_Color = Color.FromArgb(value, m_Color.R, m_Color.G, m_Color.B); } }
    }
}
