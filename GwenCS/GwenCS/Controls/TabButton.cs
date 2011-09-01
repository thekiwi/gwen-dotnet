using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class TabButton : Button
    {
        protected Base m_Page;
        protected TabControl m_Control;

        public bool IsActive { get { return m_Page != null && m_Page.IsVisible; } }
        public TabControl TabControl
        {
            get { return m_Control; }
            set
            {
                if (value == m_Control) return;
                if (m_Control != null)
                    m_Control.onLoseTab(this);
                m_Control = value;
            }
        }

        public Base Page { get { return m_Page; } set { m_Page = value; } }
        public override bool ShouldClip
        {
            get { return false; }
        }

        public TabButton(Base parent) : base(parent)
        {
            DragAndDrop_SetPackage(true, "TabButtonMove");
            Alignment = Pos.Top | Pos.Left;
            TextPadding = new Padding(5, 3, 3, 3);
            Padding = new Padding(2, 2, 2, 2);
        }

        public override void DragAndDrop_StartDragging(DragDrop.Package package, int x, int y)
        {
            IsHidden = true;
        }

        public override void DragAndDrop_EndDragging(bool success, int x, int y)
        {
            IsHidden = false;
            IsDepressed = false;
        }

        public override bool DragAndDrop_ShouldStartDrag()
        {
            return m_Control.AllowReorder;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawTabButton(this, IsActive, m_Control.TabStrip.Dock);
        }

        internal override bool onKeyDown(bool bDown)
        {
            onKeyRight(bDown);
            return true;
        }

        internal override bool onKeyUp(bool bDown)
        {
            onKeyLeft(bDown);
            return true;
        }

        internal override bool onKeyRight(bool bDown)
        {
            if (bDown)
            {
                var count = Parent.ChildrenCount;
                int me = Parent.Children.IndexOf(this);
                if (me + 1 < count)
                {
                    var nextTab = Parent.Children[me + 1];
                    TabControl.onTabPressed(nextTab);
                    Global.KeyboardFocus = nextTab;
                }
            }

            return true;
        }

        internal override bool onKeyLeft(bool bDown)
        {
            if (bDown)
            {
                var count = Parent.ChildrenCount;
                int me = Parent.Children.IndexOf(this);
                if (me - 1 >= 0)
                {
                    var prevTab = Parent.Children[me - 1];
                    TabControl.onTabPressed(prevTab);
                    Global.KeyboardFocus = prevTab;
                }
            }

            return true;
        }

        public override void UpdateColors()
        {
            if (IsActive)
            {
                if (IsDisabled)
                {
                    TextColor = Skin.Colors.Tab.Active.Disabled;
                    return;
                }
                if (IsDepressed)
                {
                    TextColor = Skin.Colors.Tab.Active.Down;
                    return;
                }
                if (IsHovered)
                {
                    TextColor = Skin.Colors.Tab.Active.Hover;
                    return;
                }

                TextColor = Skin.Colors.Tab.Active.Normal;
            }

            if (IsDisabled)
            {
                TextColor = Skin.Colors.Tab.Inactive.Disabled;
                return;
            }
            if (IsDepressed)
            {
                TextColor = Skin.Colors.Tab.Inactive.Down;
                return;
            }
            if (IsHovered)
            {
                TextColor = Skin.Colors.Tab.Inactive.Hover;
                return;
            }

            TextColor = Skin.Colors.Tab.Inactive.Normal;
        }
    }
}
