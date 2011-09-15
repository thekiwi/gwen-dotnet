using System;
using Gwen.Control;

namespace Gwen.ControlInternal
{
    public class ScrollBarButton : Button
    {
        protected Pos m_Direction;

        public ScrollBarButton(Base parent)
            : base(parent)
        {
            SetDirectionUp();
        }

        public virtual void SetDirectionUp()
        {
            m_Direction = Pos.Top;
        }

        public virtual void SetDirectionDown()
        {
            m_Direction = Pos.Bottom;
        }

        public virtual void SetDirectionLeft()
        {
            m_Direction = Pos.Left;
        }

        public virtual void SetDirectionRight()
        {
            m_Direction = Pos.Right;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollButton(this, m_Direction, m_Depressed, IsHovered, IsDisabled);
        }
    }
}
