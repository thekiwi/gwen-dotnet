using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class MenuStrip : Menu
    {
        public MenuStrip(Base parent)
            : base(parent)
        {
            SetBounds(0, 0, 200, 22);
            Dock = Pos.Top;
            m_InnerPanel.Padding = new Padding(5, 0, 0, 0);
        }

        public override void Close()
        {
            
        }

        protected override void RenderUnder(Skin.Base skin)
        {
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuStrip(this);
        }

        protected override void Layout(Skin.Base skin)
        {
            //TODO: We don't want to do vertical sizing the same as Menu, do nothing for now
        }

        protected override bool ShouldHoverOpenMenu
        {
            get { return IsMenuOpen(); }
        }

        protected override void onAddItem(MenuItem item)
        {
            item.Dock = Pos.Left;
            item.TextPadding = new Padding(5, 0, 5, 0);
            item.Padding = new Padding(10, 0, 10, 0);
            item.SizeToContents();
            item.IsOnStrip = true;
            item.OnHoverEnter += onHoverItem;
        }
    }
}
