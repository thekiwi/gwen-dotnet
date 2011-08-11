using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class ComboBoxButton : Button
    {
        public ComboBoxButton(Base parent) : base(parent)
        {}

        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBoxButton(this, m_bDepressed);
        }
    }

    public class ComboBox : Base
    {
        protected Menu m_Menu;
        protected MenuItem m_SelectedItem;
        protected ComboBoxButton m_OpenButton;
        protected Label m_SelectedText;

        public event ControlCallback OnSelection;

        public ComboBox(Base parent) : base(parent)
        {
            SetSize(100, 20);
            m_Menu = new Menu(this);
            m_Menu.IsHidden = true;
            m_Menu.IconMarginDisabled = true;
            m_Menu.IsTabable = false;

            m_OpenButton = new ComboBoxButton(this);
            m_OpenButton.OnPress += OpenButtonPressed;
            m_OpenButton.Dock = Pos.Right;
            m_OpenButton.Margin = new Margin(2, 2, 2, 2);
            m_OpenButton.Width = 16;
            m_OpenButton.IsTabable = false;

            m_SelectedText = new Label(this);
            m_SelectedText.Alignment = Pos.Left | Pos.CenterV;
            //m_SelectedText.Text = string.Empty;
            m_SelectedText.Margin = new Margin(3, 0, 0, 0);
            m_SelectedText.Dock = Pos.Fill;
            m_SelectedText.IsTabable = false;

            IsTabable = true;
        }

        public Label SelectedItem { get { return m_SelectedItem; } }

        public virtual MenuItem AddItem(String label, String name, ControlCallback handler = null)
        {
            MenuItem item = m_Menu.AddItem(label, String.Empty, handler);
            item.Name = name;
            item.OnMenuItemSelected += onItemSelected;

            if (m_SelectedText != null)
                onItemSelected(item);

            return item;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBox(this);
        }

        protected virtual void OpenButtonPressed(Base control)
        {
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
            m_SelectedText.Text = m_SelectedItem.Text;
            m_Menu.IsHidden = true;

            if (OnSelection != null)
                OnSelection.Invoke(this);

            Focus();
            Invalidate();
        }

        internal override void onLostKeyboardFocus()
        {
            m_SelectedText.TextColor = Color.Black;
        }

        internal override void onKeyboardFocus()
        {
            //Until we add the blue highlighting again
            m_SelectedText.TextColor = Color.Black;
            //m_SelectedText->SetTextColor( Color( 255, 255, 255, 255 ) );
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
            
        }

        internal override bool onKeyDown(bool bDown)
        {
            if (bDown)
            {
                var it = m_Menu.InnerChildren.FindIndex(x => x == m_SelectedItem);
                if (it + 1 < m_Menu.InnerChildren.Count)
                    onItemSelected(m_Menu.InnerChildren[it + 1]);
            }
            return true;
        }

        internal override bool onKeyUp(bool bDown)
        {
            if (bDown)
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
    }
}
