using System;
using Gwen.DragDrop;

namespace Gwen.Control
{
    public class TabTitleBar : Label
    {
        public TabTitleBar(Base parent) : base(parent)
        {
            MouseInputEnabled = true;
            TextPadding = new Padding(5, 2, 5, 2);
            Padding = new Padding(1, 2, 1, 2);

            DragAndDrop_SetPackage(true, "TabWindowMove");
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawTabTitleBar(this);
        }

        public override void DragAndDrop_StartDragging(Package package, int x, int y)
        {
            DragAndDrop.SourceControl = Parent;
            DragAndDrop.SourceControl.DragAndDrop_StartDragging(package, x, y);
        }

        public void UpdateFromTab(TabButton button)
        {
            Text = button.Text;
            SizeToContents();
        }
    }
}
