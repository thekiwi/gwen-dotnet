using System;

namespace Gwen.Controls
{
    public class GroupBox : Label
    {
        public GroupBox(Base parent) : base(parent)
        {
            // Set to true, because it's likely that our  
            // children will want mouse input, and they
            // can't get it without us..
            MouseInputEnabled = true;

            TextPadding = new Padding(10, 0, 0, 0);
            Alignment = Pos.Top | Pos.Left;
            Invalidate();

            m_InnerPanel = new Base(this);
            m_InnerPanel.Dock = Pos.Fill;
            m_InnerPanel.Padding = new Padding(0, 10, 0, 0); // [omeg] to prevent overlapping on label
        }

        protected override void Layout(Skin.Base skin)
        {
            m_InnerPanel.Margin = new Margin(TextHeight + 3, 6, 6, 6);
            base.Layout(skin);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawGroupBox(this, TextX, TextHeight, TextWidth);
        }
    }
}
