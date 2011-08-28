using System;
using System.Drawing;
using Gwen.Controls;
using Gwen.DragDrop;

namespace Gwen.ControlsInternal
{
    public class TabStrip : Base
    {
        protected Base m_TabDragControl;
        protected bool m_bAllowReorder;

        public bool AllowReorder { get { return m_bAllowReorder; } set { m_bAllowReorder = value; } }

        public TabStrip(Base parent) : base(parent)
        {
            m_bAllowReorder = false;
        }

        public Pos TabPosition
        {
            get { return Dock; }
            set
            {
                Dock = value;
                if (m_iDock == Pos.Top)
                    Padding = new Padding(5, 0, 0, 0);
                if (m_iDock == Pos.Left)
                    Padding = new Padding(0, 5, 0, 0);
                if (m_iDock == Pos.Bottom)
                    Padding = new Padding(5, 0, 0, 0);
                if (m_iDock == Pos.Right)
                    Padding = new Padding(0, 5, 0, 0);
            }
        }

        public override bool DragAndDrop_HandleDrop(Package p, int x, int y)
        {
            Point LocalPos = CanvasPosToLocal(new Point(x, y));

            TabButton pButton = DragAndDrop.SourceControl as TabButton;
            TabControl pTabControl = Parent as TabControl;
            if (pTabControl!=null && pButton!=null)
            {
                if (pButton.TabControl != pTabControl)
                {
                    // We've moved tab controls!
                    pTabControl.AddPage(pButton);
                }
            }

            Base DroppedOn = GetControlAt(LocalPos.X, LocalPos.Y);
            if (DroppedOn != null)
            {
                Point DropPos = DroppedOn.CanvasPosToLocal(new Point(x, y));
                DragAndDrop.SourceControl.BringNextToControl(DroppedOn, DropPos.X > DroppedOn.Width/2);
            }
            else
            {
                DragAndDrop.SourceControl.BringToFront();
            }
            return true;
        }

        public override bool DragAndDrop_CanAcceptPackage(Package p)
        {
            if (!m_bAllowReorder)
                return false;

            if (p.Name == "TabButtonMove")
                return true;

            return false;
        }

        protected override void Layout(Skin.Base skin)
        {
            Point pLargestTab = new Point(5, 5);

            int iNum = 0;
            foreach (var child in Children)
            {
                TabButton pButton = child as TabButton;
                if (null == pButton) continue;

                pButton.SizeToContents();

                Margin m = new Margin();
                int iNotFirst = iNum > 0 ? -1 : 0;

                if (m_iDock == Pos.Top)
                {
                    m.left = iNotFirst;
                    pButton.Dock = Pos.Left;
                }

                if (m_iDock == Pos.Left)
                {
                    m.top = iNotFirst;
                    pButton.Dock = Pos.Top;
                }

                if (m_iDock == Pos.Right)
                {
                    m.top = iNotFirst;
                    pButton.Dock = Pos.Top;
                }

                if (m_iDock == Pos.Bottom)
                {
                    m.left = iNotFirst;
                    pButton.Dock = Pos.Left;
                }

                pLargestTab.X = Math.Max(pLargestTab.X, pButton.Width);
                pLargestTab.Y = Math.Max(pLargestTab.Y, pButton.Height);

                pButton.Margin = m;
                iNum++;
            }

            if (m_iDock == Pos.Top || m_iDock == Pos.Bottom)
                SetSize(Width, pLargestTab.Y);

            if (m_iDock == Pos.Left || m_iDock == Pos.Right)
                SetSize(pLargestTab.X, Height);

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
            m_TabDragControl = null;
        }

        public override void DragAndDrop_Hover(Package p, int x, int y)
        {
            Point LocalPos = CanvasPosToLocal(new Point(x, y));

            Base DroppedOn = GetControlAt(LocalPos.X, LocalPos.Y);
            if (DroppedOn != null && DroppedOn != this)
            {
                Point DropPos = DroppedOn.CanvasPosToLocal(new Point(x, y));
                m_TabDragControl.SetBounds(new Rectangle(0, 0, 3, Height));
                m_TabDragControl.BringToFront();
                m_TabDragControl.SetPos(DroppedOn.X - 1, 0);

                if (DropPos.X > DroppedOn.Width/2)
                {
                    m_TabDragControl.MoveBy(DroppedOn.Width - 1, 0);
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
