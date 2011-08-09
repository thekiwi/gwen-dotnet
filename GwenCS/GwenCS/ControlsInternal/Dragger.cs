using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class Dragger : Base
    {
        protected bool m_bDepressed;
        protected Point m_HoldPos;
        protected Base m_pTarget;

        internal Base Target { get { return m_pTarget; } set { m_pTarget = value; } }

        public event ControlCallback OnDragged;

        public Dragger(Base parent) : base(parent)
        {
            MouseInputEnabled = true;
            m_bDepressed = false;
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            if (null == m_pTarget) return;

            if (pressed)
            {
                m_bDepressed = true;
                m_HoldPos = m_pTarget.CanvasPosToLocal(new Point(x, y));
                Global.MouseFocus = this;
            }
            else
            {
                m_bDepressed = false;

                Global.MouseFocus = null;
            }
        }

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            if (null == m_pTarget) return;
            if (!m_bDepressed) return;

            Point p = new Point(x - m_HoldPos.X, y - m_HoldPos.Y);

            // Translate to parent
            if (m_pTarget.Parent != null)
                p = m_pTarget.Parent.CanvasPosToLocal(p);

            //m_pTarget->SetPosition( p.x, p.y );
            m_pTarget.MoveTo(p.X, p.Y);
            OnDragged.Invoke(this);
        }

        protected override void Render(Skin.Base skin)
        {
            
        }
    }
}
