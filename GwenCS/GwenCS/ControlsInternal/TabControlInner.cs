using System;
using System.Drawing;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    internal class TabControlInner : Base
    {
        protected Rectangle m_ButtonRect;

        internal TabControlInner(Base parent) : base(parent)
        {
            m_ButtonRect = Rectangle.Empty;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawTabControl(this, m_ButtonRect);
        }

        internal void UpdateCurrentButton(Rectangle rect)
        {
            m_ButtonRect = rect;
        }
    }
}
