using System;

namespace Gwen.Controls
{
    public class CloseButton : Button
    {
        protected WindowControl m_Window; // [omeg] not ours, no disposing

        public WindowControl Window { set { m_Window = value; } }

        public CloseButton(Base parent) : base(parent)
        {
            
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawWindowCloseButton(this, IsDepressed && IsHovered, IsHovered && ShouldDrawHover, !m_Window.IsOnTop);
        }
    }
}
