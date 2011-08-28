using System;
using System.Linq;

namespace Gwen.Controls
{
    public class Menu : ScrollControl
    {
        protected bool m_bDisableIconMargin;

        public override bool IsMenuComponent { get { return true; } }
        public bool IconMarginDisabled { get { return m_bDisableIconMargin; } set { m_bDisableIconMargin = value; } }

        protected virtual bool ShouldHoverOpenMenu { get { return true; } }

        public Menu(Base parent)
            : base(parent)
        {
            SetBounds(0, 0, 10, 10);
            Padding = new Padding(2, 2, 2, 2);
            IconMarginDisabled = false;

            AutoHideBars = true;
            SetScroll(false, true);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenu(this, IconMarginDisabled);
        }

        protected override void RenderUnder(Skin.Base skin)
        {
            base.RenderUnder(skin);
            skin.DrawShadow(this);
        }

        protected override void Layout(Skin.Base skin)
        {
            int childrenHeight = m_InnerPanel.Children.Sum(child => child != null ? child.Height : 0);

            if (Y + childrenHeight > GetCanvas().Height)
                childrenHeight = GetCanvas().Height - Y;

            SetSize(Width, childrenHeight);

            base.Layout(skin);
        }

        public virtual MenuItem AddItem(String strName, ControlCallback handler = null)
        {
            return AddItem(strName, String.Empty, handler);
        }

        public virtual MenuItem AddItem(String strName, String strIconName, ControlCallback handler = null)
        {
            MenuItem item = new MenuItem(this);
            item.Padding = new Padding(4, 4, 4, 4);
            item.SetText(strName);
            item.SetImage(strIconName);

            if (handler != null)
                item.OnMenuItemSelected += handler;

            onAddItem(item);

            return item;
        }

        protected virtual void onAddItem(MenuItem item)
        {
            item.TextPadding = new Padding(IconMarginDisabled ? 0 : 24, 0, 16, 0);
            item.Dock = Pos.Top;
            item.SizeToContents();
            item.Alignment = Pos.CenterV | Pos.Left;
            item.OnHoverEnter += onHoverItem;

            // Do this here - after Top Docking these values mean nothing in layout
            int w = item.Width + 10 + 32;
            if (w < Width) w = Width;
            SetSize(w, Height);
        }

        public virtual void ClearItems()
        {
            foreach (Base child in m_InnerPanel.Children)
            {
                child.Dispose();
            }
        }

        public virtual void CloseAll()
        {
            m_InnerPanel.Children.ForEach(child => { if (child is MenuItem) (child as MenuItem).CloseMenu(); });
        }

        public virtual bool IsMenuOpen()
        {
            return m_InnerPanel.Children.Any(child => { if (child is MenuItem) return (child as MenuItem).IsMenuOpen; return false; });
        }

        protected virtual void onHoverItem(Base pControl)
        {
            if (!ShouldHoverOpenMenu) return;

            MenuItem pItem = pControl as MenuItem;
            if (null == pItem) return;
            if (pItem.IsMenuOpen) return;

            CloseAll();
            pItem.OpenMenu();
        }

        public virtual void Close()
        {
            IsHidden = true;
        }

        public override void CloseMenus()
        {
            base.CloseMenus();
            CloseAll();
            Close();
        }

        public virtual void AddDivider()
        {
            MenuDivider divider = new MenuDivider(this);
            divider.Dock = Pos.Top;
            divider.Margin = new Margin(IconMarginDisabled ? 0 : 24, 0, 4, 0);
        }
    }

    public class MenuDivider : Base
    {
        public MenuDivider(Base parent) : base(parent)
        {
            Height = 1;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuDivider(this);
        }
    }
}
