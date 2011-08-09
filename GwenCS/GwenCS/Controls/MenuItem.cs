using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class MenuItem : Button
    {
        protected Menu m_Menu;
        protected bool m_bOnStrip;
        protected bool m_bCheckable;
        protected bool m_bChecked;
        protected Symbol.Arrow m_SubmenuArrow;

        public bool IsOnStrip { get { return m_bOnStrip; } set { m_bOnStrip = value; } }
        public bool IsCheckable { get { return m_bCheckable; } set { m_bCheckable = value; } }
        public bool IsMenuOpen { get { if (m_Menu == null) return false; return !m_Menu.IsHidden; } }
        
        public bool Checked
        {
            get { return m_bChecked; }
            set
            {
                if (value == m_bChecked)
                    return;

                m_bChecked = value;

                if (OnCheckChanged!=null)
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

        public Menu Menu
        {
            get
            {
                if (null == m_Menu)
                {
                    m_Menu = new Menu(GetCanvas());
                    m_Menu.IsHidden = true;

                    if (!m_bOnStrip)
                    {
                        m_SubmenuArrow = new Symbol.Arrow(this);
                        m_SubmenuArrow.Dock = Pos.Right;
                        m_SubmenuArrow.SetSize(20, 20);
                    }

                    Invalidate();
                }

                return m_Menu;
            }
        }

        public event ControlCallback OnMenuItemSelected;
        public event ControlCallback OnChecked;
        public event ControlCallback OnUnChecked;
        public event ControlCallback OnCheckChanged;

        public MenuItem(Base parent) : base(parent)
        {
            m_bOnStrip = false;
            IsTabable = false;
            IsCheckable = false;
            Checked = false;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuItem(this, IsMenuOpen, m_bCheckable ? m_bChecked : false);
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);
        }

        protected override void onPress()
        {
            if (m_Menu != null)
            {
                ToggleMenu();
            }
            else if (!m_bOnStrip)
            {
                Checked = !Checked;
                if (OnMenuItemSelected!=null)
                    OnMenuItemSelected.Invoke(this);
                GetCanvas().CloseMenus();
            }
            base.onPress();
        }

        public void ToggleMenu()
        {
            if (IsMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }

        public void OpenMenu()
        {
            if (null == m_Menu) return;

            m_Menu.IsHidden = false;
            m_Menu.BringToFront();

            Point p = LocalPosToCanvas(Point.Empty);

            // Strip menus open downwards
            if (m_bOnStrip)
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

        public void CloseMenu()
        {
            if (null==m_Menu) return;
            m_Menu.Close();
            m_Menu.CloseAll();
        }
    }
}
