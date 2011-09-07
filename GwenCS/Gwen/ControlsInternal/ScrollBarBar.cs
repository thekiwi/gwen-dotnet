using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class ScrollBarBar : Dragger
    {
        protected bool m_Horizontal;

        public bool IsHorizontal { get { return m_Horizontal; } set { m_Horizontal = value; } }
        public bool IsVertical { get { return !m_Horizontal; } set { m_Horizontal = !value; } }

        public ScrollBarBar(Base parent) : base(parent)
        {
            RestrictToParent = true;
            Target = this;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawScrollBarBar(this, m_Depressed, IsHovered, m_Horizontal);
            base.Render(skin);
        }

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            base.onMouseMoved(x, y, dx, dy);
            if (!m_Depressed)
                return;

            InvalidateParent();
        }

        internal override void onMouseClickLeft(int x, int y, bool down)
        {
            base.onMouseClickLeft(x, y, down);
            InvalidateParent();
        }

        protected override void Layout(Skin.Base skin)
        {
            if (null == Parent)
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
