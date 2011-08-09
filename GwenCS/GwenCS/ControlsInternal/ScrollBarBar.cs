using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class ScrollBarBar : Dragger
    {
        protected bool m_bHorizontal;

        public bool IsHorizontal { get { return m_bHorizontal; } set { m_bHorizontal = value; } }
        public bool IsVertical { get { return !m_bHorizontal; } set { m_bHorizontal = !value; } }
        public bool IsDepressed { get { return m_bDepressed; } }

        public ScrollBarBar(Base parent) : base(parent)
        {
            RestrictToParent = true;
            Target = this;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollBarBar(this, m_bDepressed, IsHovered, m_bHorizontal);
            base.Render(skin);
        }

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            base.onMouseMoved(x, y, dx, dy);
            if (!m_bDepressed)
                return;

            InvalidateParent();
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            base.onMouseClickLeft(x, y, pressed);
            InvalidateParent();
        }

        protected override void Layout(Skin.Base skin)
        {
            if (null==Parent)
                return;

            //Move to our current position to force clamping - is this a hack?
            MoveTo(X, Y);
        }
        /*
        public override void MoveTo(int x, int y)
        {
            base.MoveTo(x, y);
        }
        */
    }
}
