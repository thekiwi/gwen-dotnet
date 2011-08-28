using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class ScrollBarButton: Button
    {
        protected Pos m_iDirection;

        public ScrollBarButton(Base parent) : base(parent)
        {
            SetDirectionUp();
        }

        public virtual void SetDirectionUp()
        {
            m_iDirection = Pos.Top;
        }

        public virtual void SetDirectionDown()
        {
            m_iDirection = Pos.Bottom;
        }

        public virtual void SetDirectionLeft()
        {
            m_iDirection = Pos.Left;
        }

        public virtual void SetDirectionRight()
        {
            m_iDirection = Pos.Right;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollButton(this, m_iDirection, m_bDepressed, IsHovered, IsDisabled);
        }
    }
}
