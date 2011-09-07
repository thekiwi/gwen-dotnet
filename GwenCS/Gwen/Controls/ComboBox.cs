using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class DownArrow : Base
    {
        protected ComboBox m_ComboBox;

        public DownArrow(Base parent) : base(parent)
        {
            MouseInputEnabled = false;
            SetSize(15, 15);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBoxArrow(this, m_ComboBox.IsHovered, m_ComboBox.IsDepressed, m_ComboBox.IsMenuOpen, m_ComboBox.IsDisabled);
        }

        internal ComboBox ComboBox { set { m_ComboBox = value; } }
    }

    public class ComboBox : Button
    {
        protected Menu m_Menu;
        protected MenuItem m_SelectedItem;
        protected Base m_Button;

        public event ControlCallback OnSelection;

        public bool IsMenuOpen { get { return m_Menu != null && !m_Menu.IsHidden; } }

        public ComboBox(Base parent) : base(parent)
        {
            SetSize(100, 20);
            m_Menu = new Menu(this);
            m_Menu.IsHidden = true;
            m_Menu.IconMarginDisabled = true;
            m_Menu.IsTabable = false;

            DownArrow arrow = new DownArrow(this);
            arrow.ComboBox = this;
            m_Button = arrow;

            Alignment = Pos.Left | Pos.CenterV;
            Text = String.Empty;
            Margin = new Margin(3, 0, 0, 0);

            IsTabable = true;
            KeyboardInputEnabled = true;
        }

        public Label SelectedItem { get { return m_SelectedItem; } }

        public override bool IsMenuComponent
        {
            get { return true; }
        }

        public virtual MenuItem AddItem(String label, String name, ControlCallback handler = null)
        {
            MenuItem item = m_Menu.AddItem(label, String.Empty, handler);
            item.Name = name;
            item.OnMenuItemSelected += onItemSelected;

            if (m_SelectedItem != null)
                onItemSelected(item);

            return item;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBox(this, IsDepressed, IsMenuOpen);
        }

        internal override void onPress()
        {
            if (IsMenuOpen)
            {
                GetCanvas().CloseMenus();
                return;
            }

            bool bWasMenuHidden = m_Menu.IsHidden;

            GetCanvas().CloseMenus();

            if (bWasMenuHidden)
            {
                OpenList();
            }
        }

        public virtual void ClearItems()
        {
            if (m_Menu != null)
                m_Menu.ClearItems();
        }

        protected virtual void onItemSelected(Base control)
        {
            //Convert selected to a menu item
            MenuItem pItem = control as MenuItem;
            if (null==pItem) return;

            m_SelectedItem = pItem;
            Text = m_SelectedItem.Text;
            m_Menu.IsHidden = true;

            if (OnSelection != null)
                OnSelection.Invoke(this);

            Focus();
            Invalidate();
        }

        protected override void Layout(Skin.Base skin)
        {
            m_Button.Position(Pos.Right|Pos.CenterV, 4, 0);
            base.Layout(skin);
        }

        internal override void onLostKeyboardFocus()
        {
            TextColor = Color.Black;
        }

        internal override void onKeyboardFocus()
        {
            //Until we add the blue highlighting again
            TextColor = Color.Black;
        }

        public virtual void OpenList()
        {
            if (null == m_Menu) return;

            m_Menu.Parent = GetCanvas();
            m_Menu.IsHidden = false;
            m_Menu.BringToFront();

            Point p = LocalPosToCanvas(Point.Empty);

            m_Menu.SetBounds(new Rectangle(p.X, p.Y + Height, Width, m_Menu.Height));
        }

        public virtual void CloseList()
        {
            if (m_Menu == null) 
                return;

            m_Menu.Hide();
        }

        internal override bool onKeyDown(bool down)
        {
            if (down)
            {
                var it = m_Menu.InnerChildren.FindIndex(x => x == m_SelectedItem);
                if (it + 1 < m_Menu.InnerChildren.Count)
                    onItemSelected(m_Menu.InnerChildren[it + 1]);
            }
            return true;
        }

        internal override bool onKeyUp(bool down)
        {
            if (down)
            {
                var it = m_Menu.InnerChildren.FindLastIndex(x => x == m_SelectedItem);
                if (it - 1 >= 0)
                    onItemSelected(m_Menu.InnerChildren[it - 1]);
            }
            return true;
        }

        protected override void RenderFocus(Skin.Base skin)
        {
            
        }

        public override void Dispose()
        {
            m_Button.Dispose();
            m_Menu.Dispose();
            base.Dispose();
        }
    }
}
