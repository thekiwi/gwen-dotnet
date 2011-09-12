using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class RightArrow : Base
    {
        public RightArrow(Base parent) : base(parent)
        {
            MouseInputEnabled = false;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuRightArrow(this);
        }
    }

    public class MenuItem : Button
    {
        protected Menu m_Menu;
        protected bool m_OnStrip;
        protected bool m_Checkable;
        protected bool m_Checked;
        protected Base m_SubmenuArrow;

        public bool IsOnStrip { get { return m_OnStrip; } set { m_OnStrip = value; } }
        public bool IsCheckable { get { return m_Checkable; } set { m_Checkable = value; } }
        public bool IsMenuOpen { get { if (m_Menu == null) return false; return !m_Menu.IsHidden; } }
        
        public bool Checked
        {
            get { return m_Checked; }
            set
            {
                if (value == m_Checked)
                    return;

                m_Checked = value;

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

        public event ControlCallback OnMenuItemSelected;
        public event ControlCallback OnChecked;
        public event ControlCallback OnUnChecked;
        public event ControlCallback OnCheckChanged;

        public MenuItem(Base parent) : base(parent)
        {
            m_OnStrip = false;
            IsTabable = false;
            IsCheckable = false;
            Checked = false;
        }

        public override void Dispose()
        {
            if (m_SubmenuArrow != null)
                m_SubmenuArrow.Dispose();
            
            base.Dispose();
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuItem(this, IsMenuOpen, m_Checkable ? m_Checked : false);
        }

        protected override void Layout(Skin.Base skin)
        {
            if (m_SubmenuArrow != null)
            {
                m_SubmenuArrow.Position(Pos.Right|Pos.CenterV, 4, 0);
            }
            base.Layout(skin);
        }

        protected override void onPress()
        {
            if (m_Menu != null)
            {
                ToggleMenu();
            }
            else if (!m_OnStrip)
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

        public void CloseMenu()
        {
            if (null==m_Menu) return;
            m_Menu.Close();
            m_Menu.CloseAll();
        }
    }
}
