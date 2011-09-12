using System;
using System.Drawing;
using Gwen.ControlsInternal;
using Gwen.DragDrop;

namespace Gwen.Controls
{
    public class TabStrip : Base
    {
        protected Base m_TabDragControl;
        protected bool m_AllowReorder;

        public bool AllowReorder { get { return m_AllowReorder; } set { m_AllowReorder = value; } }

        protected override bool ShouldClip
        {
            get { return false; }
        }

        public TabStrip(Base parent) : base(parent)
        {
            m_AllowReorder = false;
        }

        public override void Dispose()
        {
            foreach (var child in Children)
            {
                TabButton button = child as TabButton;
                if (null == button) continue;
                button.Dispose(); // this also disposes whole page
            }
            base.Dispose();
        }

        public Pos TabPosition
        {
            get { return Dock; }
            set
            {
                Dock = value;
                if (m_Dock == Pos.Top)
                    Padding = new Padding(5, 0, 0, 0);
                if (m_Dock == Pos.Left)
                    Padding = new Padding(0, 5, 0, 0);
                if (m_Dock == Pos.Bottom)
                    Padding = new Padding(5, 0, 0, 0);
                if (m_Dock == Pos.Right)
                    Padding = new Padding(0, 5, 0, 0);
            }
        }

        public override bool DragAndDrop_HandleDrop(Package p, int x, int y)
        {
            Point LocalPos = CanvasPosToLocal(new Point(x, y));

            TabButton button = DragAndDrop.SourceControl as TabButton;
            TabControl tabControl = Parent as TabControl;
            if (tabControl != null && button != null)
            {
                if (button.TabControl != tabControl)
                {
                    // We've moved tab controls!
                    tabControl.AddPage(button);
                }
            }

            Base droppedOn = GetControlAt(LocalPos.X, LocalPos.Y);
            if (droppedOn != null)
            {
                Point dropPos = droppedOn.CanvasPosToLocal(new Point(x, y));
                DragAndDrop.SourceControl.BringNextToControl(droppedOn, dropPos.X > droppedOn.Width/2);
            }
            else
            {
                DragAndDrop.SourceControl.BringToFront();
            }
            return true;
        }

        public override bool DragAndDrop_CanAcceptPackage(Package p)
        {
            if (!m_AllowReorder)
                return false;

            if (p.Name == "TabButtonMove")
                return true;

            return false;
        }

        protected override void Layout(Skin.Base skin)
        {
            Point largestTab = new Point(5, 5);

            int num = 0;
            foreach (var child in Children)
            {
                TabButton button = child as TabButton;
                if (null == button) continue;

                button.SizeToContents();

                Margin m = new Margin();
                int notFirst = num > 0 ? -1 : 0;

                if (m_Dock == Pos.Top)
                {
                    m.Left = notFirst;
                    button.Dock = Pos.Left;
                }

                if (m_Dock == Pos.Left)
                {
                    m.Top = notFirst;
                    button.Dock = Pos.Top;
                }

                if (m_Dock == Pos.Right)
                {
                    m.Top = notFirst;
                    button.Dock = Pos.Top;
                }

                if (m_Dock == Pos.Bottom)
                {
                    m.Left = notFirst;
                    button.Dock = Pos.Left;
                }

                largestTab.X = Math.Max(largestTab.X, button.Width);
                largestTab.Y = Math.Max(largestTab.Y, button.Height);

                button.Margin = m;
                num++;
            }

            if (m_Dock == Pos.Top || m_Dock == Pos.Bottom)
                SetSize(Width, largestTab.Y);

            if (m_Dock == Pos.Left || m_Dock == Pos.Right)
                SetSize(largestTab.X, Height);

            base.Layout(skin);
        }

        public override void DragAndDrop_HoverEnter(Package p, int x, int y)
        {
            if (m_TabDragControl != null)
            {
                throw new InvalidOperationException("ERROR! TabStrip::DragAndDrop_HoverEnter");
            }

            m_TabDragControl = new Highlight(this);
            m_TabDragControl.MouseInputEnabled = false;
            m_TabDragControl.SetSize(3, Height);
        }

        public override void DragAndDrop_HoverLeave(Package p)
        {
            if (m_TabDragControl != null)
            {
                RemoveChild(m_TabDragControl); // [omeg] need to do that explicitely
                m_TabDragControl.Dispose();
            }
            m_TabDragControl = null;
        }

        public override void DragAndDrop_Hover(Package p, int x, int y)
        {
            Point localPos = CanvasPosToLocal(new Point(x, y));

            Base droppedOn = GetControlAt(localPos.X, localPos.Y);
            if (droppedOn != null && droppedOn != this)
            {
                Point dropPos = droppedOn.CanvasPosToLocal(new Point(x, y));
                m_TabDragControl.SetBounds(new Rectangle(0, 0, 3, Height));
                m_TabDragControl.BringToFront();
                m_TabDragControl.SetPos(droppedOn.X - 1, 0);

                if (dropPos.X > droppedOn.Width/2)
                {
                    m_TabDragControl.MoveBy(droppedOn.Width - 1, 0);
                }
                m_TabDragControl.Dock = Pos.None;
            }
            else
            {
                m_TabDragControl.Dock = Pos.Left;
                m_TabDragControl.BringToFront();
            }
        }
    }
}
