using System;
using System.Drawing;
using System.Linq;
using Gwen.ControlInternal;

namespace Gwen.Control
{
    /// <summary>
    /// Popup menu.
    /// </summary>
    public class Menu : ScrollControl
    {
        private bool m_DisableIconMargin;
        private bool m_DeleteOnClose;

        internal override bool IsMenuComponent { get { return true; } }
        
        public bool IconMarginDisabled { get { return m_DisableIconMargin; } set { m_DisableIconMargin = value; } }
        
        /// <summary>
        /// Determines whether the menu should be disposed on close.
        /// </summary>
        public bool DeleteOnClose { get { return m_DeleteOnClose; } set { m_DeleteOnClose = value; } }

        /// <summary>
        /// Determines whether the menu should open on mouse hover.
        /// </summary>
        protected virtual bool ShouldHoverOpenMenu { get { return true; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public Menu(Base parent)
            : base(parent)
        {
            SetBounds(0, 0, 10, 10);
            Padding = Padding.Two;
            IconMarginDisabled = false;

            AutoHideBars = true;
            EnableScroll(false, true);
            DeleteOnClose = false;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            DeleteAll();
            base.Dispose();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenu(this, IconMarginDisabled);
        }

        /// <summary>
        /// Renders under the actual control (shadows etc).
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void RenderUnder(Skin.Base skin)
        {
            base.RenderUnder(skin);
            skin.DrawShadow(this);
        }

        /// <summary>
        ///  Opens the menu.
        /// </summary>
        /// <param name="pos">Unused.</param>
        public void Open(Pos pos)
        {
            IsHidden = false;
            BringToFront();
            Point mouse = Input.InputHandler.MousePosition;
            SetPosition(mouse.X, mouse.Y);
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            int childrenHeight = Children.Sum(child => child != null ? child.Height : 0);

            if (Y + childrenHeight > GetCanvas().Height)
                childrenHeight = GetCanvas().Height - Y;

            SetSize(Width, childrenHeight);

            base.Layout(skin);
        }

        /// <summary>
        /// Adds a new menu item.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="handler">Handler invoked on item selection.</param>
        /// <returns>Newly created control.</returns>
        public virtual MenuItem AddItem(String text, GwenEventHandler handler = null)
        {
            return AddItem(text, String.Empty, handler);
        }

        /// <summary>
        /// Adds a new menu item.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="handler">Handler invoked on item selection.</param>
        /// <param name="iconName">Icon texture name.</param>
        /// <returns>Newly created control.</returns>
        public virtual MenuItem AddItem(String text, String iconName, GwenEventHandler handler = null)
        {
            MenuItem item = new MenuItem(this);
            item.Padding = Padding.Four;
            item.SetText(text);
            item.SetImage(iconName);

            if (handler != null)
                item.Selected += handler;

            OnAddItem(item);

            return item;
        }

        /// <summary>
        /// Add item handler.
        /// </summary>
        /// <param name="item">Item added.</param>
        protected virtual void OnAddItem(MenuItem item)
        {
            item.TextPadding = new Padding(IconMarginDisabled ? 0 : 24, 0, 16, 0);
            item.Dock = Pos.Top;
            item.SizeToContents();
            item.Alignment = Pos.CenterV | Pos.Left;
            item.HoverEnter += OnHoverItem;

            // Do this here - after Top Docking these values mean nothing in layout
            int w = item.Width + 10 + 32;
            if (w < Width) w = Width;
            SetSize(w, Height);
        }

        /// <summary>
        /// Closes all submenus.
        /// </summary>
        public virtual void CloseAll()
        {
            Children.ForEach(child => { if (child is MenuItem) (child as MenuItem).CloseMenu(); });
        }

        /// <summary>
        /// Indicates whether any (sub)menu is open.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMenuOpen()
        {
            return Children.Any(child => { if (child is MenuItem) return (child as MenuItem).IsMenuOpen; return false; });
        }

        /// <summary>
        /// Mouse hover handler.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void OnHoverItem(Base control)
        {
            if (!ShouldHoverOpenMenu) return;

            MenuItem item = control as MenuItem;
            if (null == item) return;
            if (item.IsMenuOpen) return;

            CloseAll();
            item.OpenMenu();
        }

        /// <summary>
        /// Closes the current menu.
        /// </summary>
        public virtual void Close()
        {
            IsHidden = true;
            if (DeleteOnClose)
                Dispose();
        }

        /// <summary>
        /// Closes all submenus and the current menu.
        /// </summary>
        public override void CloseMenus()
        {
            base.CloseMenus();
            CloseAll();
            Close();
        }

        /// <summary>
        /// Adds a divider menu item.
        /// </summary>
        public virtual void AddDivider()
        {
            MenuDivider divider = new MenuDivider(this);
            divider.Dock = Pos.Top;
            divider.Margin = new Margin(IconMarginDisabled ? 0 : 24, 0, 4, 0);
        }
    }
}
