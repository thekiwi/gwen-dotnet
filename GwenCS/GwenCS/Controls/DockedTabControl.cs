using System;

namespace Gwen.Controls
{
    public class DockedTabControl : TabControl
    {
        private TabTitleBar m_TitleBar;

        public bool TitleBarVisible { get { return !m_TitleBar.IsHidden; } set { m_TitleBar.IsHidden = !value; } }

        public DockedTabControl(Base parent) : base(parent)
        {
            Dock = Pos.Fill;

            m_TitleBar = new TabTitleBar(this);
            m_TitleBar.Dock = Pos.Top;
            m_TitleBar.IsHidden = true;
        }

        protected override void Layout(Skin.Base skin)
        {
            TabStrip.IsHidden = (TabCount <= 1);
            UpdateTitleBar();
            base.Layout(skin);
        }

        protected void UpdateTitleBar()
        {
            if (CurrentButton == null)
                return;

            m_TitleBar.UpdateFromTab(CurrentButton);
        }

        public override void DragAndDrop_StartDragging(DragDrop.Package package, int x, int y)
        {
            base.DragAndDrop_StartDragging(package, x, y);

            IsHidden = true;
            // This hiding our parent thing is kind of lousy.
            Parent.IsHidden = true;
        }

        public void MoveTabsTo(DockedTabControl target)
        {
            var children = TabStrip.Children;
            foreach (Base child in children)
            {
                TabButton button = child as TabButton;
                if (button == null)
                    continue;
                target.AddPage(button);
            }
        }
    }
}
