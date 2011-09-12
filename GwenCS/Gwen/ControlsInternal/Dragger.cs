using System;
using System.Drawing;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class Dragger : Base
    {
        protected bool m_Depressed;
        protected Point m_HoldPos;
        protected Base m_Target;

        internal Base Target { get { return m_Target; } set { m_Target = value; } }
        public bool IsDepressed { get { return m_Depressed; } }

        public event ControlCallback OnDragged;

        public Dragger(Base parent) : base(parent)
        {
            MouseInputEnabled = true;
            m_Depressed = false;
        }

        protected override void onMouseClickLeft(int x, int y, bool down)
        {
            if (null == m_Target) return;

            if (down)
            {
                m_Depressed = true;
                m_HoldPos = m_Target.CanvasPosToLocal(new Point(x, y));
                Global.MouseFocus = this;
            }
            else
            {
                m_Depressed = false;

                Global.MouseFocus = null;
            }
        }

        protected override void onMouseMoved(int x, int y, int dx, int dy)
        {
            if (null == m_Target) return;
            if (!m_Depressed) return;

            Point p = new Point(x - m_HoldPos.X, y - m_HoldPos.Y);

            // Translate to parent
            if (m_Target.Parent != null)
                p = m_Target.Parent.CanvasPosToLocal(p);

            //m_Target->SetPosition( p.x, p.y );
            m_Target.MoveTo(p.X, p.Y);
            if (OnDragged != null)
                OnDragged.Invoke(this);
        }

        protected override void Render(Skin.Base skin)
        {
            
        }
    }
}
