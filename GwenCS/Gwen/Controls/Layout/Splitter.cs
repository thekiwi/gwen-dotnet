using System;

namespace Gwen.Controls.Layout
{
    public class Splitter : Base
    {
        protected Base[] m_Panel;
        protected bool[] m_Scale;

        public Splitter(Base parent) : base(parent)
        {
            m_Panel = new Base[2];
            m_Scale = new bool[2];
            m_Scale[0] = true;
            m_Scale[1] = true;
        }

        public void SetPanel(int i, Base panel, bool noScale = false)
        {
            if (i < 0 || i > 1) return;

            m_Panel[i] = panel;
            m_Scale[i] = !noScale;

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
                if (m_Scale[0])
                    m_Panel[0].SetBounds(m.left, m.top, w - m.left - m.right, (h*0.5f) - m.top - m.bottom);
                else
                    m_Panel[0].Position(Pos.Center, 0, (int) (h*-0.25f));
            }

            if (m_Panel[1] != null)
            {
                Margin m = m_Panel[1].Margin;
                if (m_Scale[1])
                    m_Panel[1].SetBounds(m.left, m.top + (h*0.5f), w - m.left - m.right, (h*0.5f) - m.top - m.bottom);
                else
                    m_Panel[1].Position(Pos.Center, 0, (int) (h*0.25f));
            }
        }

        protected virtual void LayoutHorizontal(Skin.Base skin)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            m_Panel[0].Dispose();
            m_Panel[1].Dispose();
            base.Dispose();
        }
    }
}
