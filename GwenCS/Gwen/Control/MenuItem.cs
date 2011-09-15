using System;
using System.Drawing;

namespace Gwen.Control
{
    /// <summary>
    /// Submenu indicator.
    /// </summary>
    public class RightArrow : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightArrow"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public RightArrow(Base parent) : base(parent)
        {
            MouseInputEnabled = false;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuRightArrow(this);
        }
    }

    /// <summary>
    /// Menu item.
    /// </summary>
    public class MenuItem : Button
    {
        protected Menu m_Menu;
        protected bool m_OnStrip;
        protected bool m_Checkable;
        protected bool m_Checked;
        protected Base m_SubmenuArrow;

        /// <summary>
        /// Indicates whether the item is on a menu strip.
        /// </summary>
        public bool IsOnStrip { get { return m_OnStrip; } set { m_OnStrip = value; } }

        /// <summary>
        /// Determines if the menu item is checkable.
        /// </summary>
        public bool IsCheckable { get { return m_Checkable; } set { m_Checkable = value; } }

        /// <summary>
        /// Indicates if the parent menu is open.
        /// </summary>
        public bool IsMenuOpen { get { if (m_Menu == null) return false; return !m_Menu.IsHidden; } }

        /// <summary>
        /// Gets or sets the check value.
        /// </summary>
        public bool Checked
        {
            get { return m_Checked; }
            set
            {
                if (value == m_Checked)
                    return;

                m_Checked = value;

                if (OnCheckChanged != null)
                    OnCheckChanged.Invoke(this);

                if (value)
                {
                    if (OnChecked != null)
                        OnChecked.Invoke(this);
                }
                else
                {
                    if (OnUnChecked != null)
                        OnUnChecked.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Gets the parent menu.
        /// </summary>
        public Menu Menu
        {
            get
            {
                if (null == m_Menu)
                {
                    m_Menu = new Menu(GetCanvas());
                    m_Menu.IsHidden = true;

                    if (!m_OnStrip)
                    {
                        if (m_SubmenuArrow != null)
                            m_SubmenuArrow.Dispose();
                        m_SubmenuArrow = new RightArrow(this);
                        m_SubmenuArrow.SetSize(15, 15);
                    }

                    Invalidate();
                }

                return m_Menu;
            }
        }

        /// <summary>
        /// Invoked when the item is selected.
        /// </summary>
        public event ControlCallback OnMenuItemSelected;

        /// <summary>
        /// Invoked when the item is checked.
        /// </summary>
        public event ControlCallback OnChecked;

        /// <summary>
        /// Invoked when the item is unchecked.
        /// </summary>
        public event ControlCallback OnUnChecked;

        /// <summary>
        /// Invoked when the item's check value is changed.
        /// </summary>
        public event ControlCallback OnCheckChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public MenuItem(Base parent)
            : base(parent)
        {
            m_OnStrip = false;
            IsTabable = false;
            IsCheckable = false;
            Checked = false;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            if (m_SubmenuArrow != null)
                m_SubmenuArrow.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuItem(this, IsMenuOpen, m_Checkable ? m_Checked : false);
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            if (m_SubmenuArrow != null)
            {
                m_SubmenuArrow.Position(Pos.Right | Pos.CenterV, 4, 0);
            }
            base.Layout(skin);
        }

        /// <summary>
        /// Internal OnPress implementation.
        /// </summary>
        protected override void onPress()
        {
            if (m_Menu != null)
            {
                ToggleMenu();
            }
            else if (!m_OnStrip)
            {
                Checked = !Checked;
                if (OnMenuItemSelected != null)
                    OnMenuItemSelected.Invoke(this);
                GetCanvas().CloseMenus();
            }
            base.onPress();
        }

        /// <summary>
        /// Toggles the menu open state.
        /// </summary>
        public void ToggleMenu()
        {
            if (IsMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }

        /// <summary>
        /// Opens the menu.
        /// </summary>
        public void OpenMenu()
        {
            if (null == m_Menu) return;

            m_Menu.IsHidden = false;
            m_Menu.BringToFront();

            Point p = LocalPosToCanvas(Point.Empty);

            // Strip menus open downwards
            if (m_OnStrip)
            {
                m_Menu.SetPos(p.X, p.Y + Height + 1);
            }
            // Submenus open sidewards
            else
            {
                m_Menu.SetPos(p.X + Width, p.Y);
            }

            // TODO: Option this.
            // TODO: Make sure on screen, open the other side of the 
            // parent if it's better...
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void CloseMenu()
        {
            if (null == m_Menu) return;
            m_Menu.Close();
            m_Menu.CloseAll();
        }
    }
}
