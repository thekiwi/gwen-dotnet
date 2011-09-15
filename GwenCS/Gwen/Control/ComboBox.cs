using System;
using System.Drawing;

namespace Gwen.Control
{
    /// <summary>
    /// ComboBox arrow.
    /// </summary>
    public class DownArrow : Base
    {
        private readonly ComboBox m_ComboBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownArrow"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public DownArrow(ComboBox parent) : base(parent) // or Base?
        {
            MouseInputEnabled = false;
            SetSize(15, 15);

            m_ComboBox = parent;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBoxArrow(this, m_ComboBox.IsHovered, m_ComboBox.IsDepressed, m_ComboBox.IsMenuOpen, m_ComboBox.IsDisabled);
        }
    }

    /// <summary>
    /// ComboBox control.
    /// </summary>
    public class ComboBox : Button
    {
        protected readonly Menu m_Menu;
        protected MenuItem m_SelectedItem;
        protected readonly Base m_Button;

        /// <summary>
        /// Invoked when the selected item has changed.
        /// </summary>
        public event ControlCallback OnSelection;

        /// <summary>
        /// Indicates whether the combo menu is open.
        /// </summary>
        public bool IsMenuOpen { get { return m_Menu != null && !m_Menu.IsHidden; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public ComboBox(Base parent) : base(parent)
        {
            SetSize(100, 20);
            m_Menu = new Menu(this);
            m_Menu.IsHidden = true;
            m_Menu.IconMarginDisabled = true;
            m_Menu.IsTabable = false;

            DownArrow arrow = new DownArrow(this);
            m_Button = arrow;

            Alignment = Pos.Left | Pos.CenterV;
            Text = String.Empty;
            Margin = new Margin(3, 0, 0, 0);

            IsTabable = true;
            KeyboardInputEnabled = true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            m_Button.Dispose();
            m_Menu.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Selected item.
        /// </summary>
        public Label SelectedItem { get { return m_SelectedItem; } }

        internal override bool IsMenuComponent
        {
            get { return true; }
        }

        /// <summary>
        /// Adds a new item.
        /// </summary>
        /// <param name="label">Item label (displayed).</param>
        /// <param name="name">Item name.</param>
        /// <param name="handler">Handler invoked when the item is selected.</param>
        /// <returns>Newly created control.</returns>
        public virtual MenuItem AddItem(String label, String name = "", ControlCallback handler = null)
        {
            MenuItem item = m_Menu.AddItem(label, String.Empty, handler);
            item.Name = name;
            item.OnMenuItemSelected += onItemSelected;

            if (m_SelectedItem != null)
                onItemSelected(item);

            return item;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawComboBox(this, IsDepressed, IsMenuOpen);
        }

        /// <summary>
        /// Internal OnPress implementation.
        /// </summary>
        protected override void onPress()
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

        /// <summary>
        /// Removes all items.
        /// </summary>
        public virtual void ClearItems()
        {
            if (m_Menu != null)
                m_Menu.ClearItems();
        }

        /// <summary>
        /// Internal handler for item selected event.
        /// </summary>
        /// <param name="control">Event source.</param>
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

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            m_Button.Position(Pos.Right|Pos.CenterV, 4, 0);
            base.Layout(skin);
        }

        /// <summary>
        /// Handler for losing keyboard focus.
        /// </summary>
        protected override void onLostKeyboardFocus()
        {
            TextColor = Color.Black;
        }

        /// <summary>
        /// Handler for gaining keyboard focus.
        /// </summary>
        protected override void onKeyboardFocus()
        {
            //Until we add the blue highlighting again
            TextColor = Color.Black;
        }

        /// <summary>
        /// Opens the combo list.
        /// </summary>
        public virtual void OpenList()
        {
            if (null == m_Menu) return;

            m_Menu.Parent = GetCanvas();
            m_Menu.IsHidden = false;
            m_Menu.BringToFront();

            Point p = LocalPosToCanvas(Point.Empty);

            m_Menu.SetBounds(new Rectangle(p.X, p.Y + Height, Width, m_Menu.Height));
        }

        /// <summary>
        /// Closes the combo list.
        /// </summary>
        public virtual void CloseList()
        {
            if (m_Menu == null) 
                return;

            m_Menu.Hide();
        }

        /// <summary>
        /// Handler for Down Arrow keyboard event.
        /// </summary>
        /// <param name="down">Indicates whether the key was pressed or released.</param>
        /// <returns>
        /// True if handled.
        /// </returns>
        protected override bool onKeyDown(bool down)
        {
            if (down)
            {
                var it = m_Menu.Children.FindIndex(x => x == m_SelectedItem);
                if (it + 1 < m_Menu.Children.Count)
                    onItemSelected(m_Menu.Children[it + 1]);
            }
            return true;
        }

        /// <summary>
        /// Handler for Up Arrow keyboard event.
        /// </summary>
        /// <param name="down">Indicates whether the key was pressed or released.</param>
        /// <returns>
        /// True if handled.
        /// </returns>
        protected override bool onKeyUp(bool down)
        {
            if (down)
            {
                var it = m_Menu.Children.FindLastIndex(x => x == m_SelectedItem);
                if (it - 1 >= 0)
                    onItemSelected(m_Menu.Children[it - 1]);
            }
            return true;
        }

        /// <summary>
        /// Renders the focus overlay.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void RenderFocus(Skin.Base skin)
        {
            
        }
    }
}
