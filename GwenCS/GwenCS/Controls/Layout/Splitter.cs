using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls.Layout
{
    public class Splitter : Base
    {
        protected Base[] m_Panel;

        public Splitter(Base parent) : base(parent)
        {
            m_Panel = new Base[2];
        }

        public void SetPanel(int i, Base panel)
        {
            if (i < 0 || i > 1) return;

            m_Panel[i] = panel;

            if (null!=m_Panel[i])
            {
                m_Panel[i].Parent = this;
            }
        }

        Base GetPanel(int i)
        {
            if (i < 0 || i > 1) return null;
            return m_Panel[i];
        }

        protected override void Layout(Skin.Base skin)
        {
            LayoutVertical(skin);
        }

        protected virtual void LayoutVertical(Skin.Base skin)
        {
            int w = Width;
            int h = Height;

            if (m_Panel[0] != null)
            {
                Margin m = m_Panel[0].Margin;
                m_Panel[0].SetBounds(m.left, m.top, w - m.left - m.right, (h*0.5f) - m.top - m.bottom);
            }

            if (m_Panel[1] != null)
            {
                Margin m = m_Panel[1].Margin;
                m_Panel[1].SetBounds(m.left, m.top + (h*0.5f), w - m.left - m.right, (h*0.5f) - m.top - m.bottom);
            }
        }

        protected virtual void LayoutHorizontal(Skin.Base skin)
        {
            throw new NotImplementedException();
        }
    }
}
