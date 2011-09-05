using System;
using System.Drawing;
using Gwen.ControlsInternal;
using Gwen.DragDrop;

namespace Gwen.Controls
{
    public class DockBase : Base
    {
        private DockBase m_Left;
        private DockBase m_Right;
        private DockBase m_Top;
        private DockBase m_Bottom;

        // Only CHILD dockpanels have a tabcontrol.
        private DockedTabControl m_DockedTabControl;

        private bool m_bDrawHover;
        private bool m_bDropFar;
        private Rectangle m_HoverRect;

        public virtual DockBase LeftDock { get { return GetChildDock(Pos.Left); } }
        public virtual DockBase RightDock { get { return GetChildDock(Pos.Right); } }
        public virtual DockBase TopDock { get { return GetChildDock(Pos.Top); } }
        public virtual DockBase BottomDock { get { return GetChildDock(Pos.Bottom); } }

        public TabControl TabControl { get { return m_DockedTabControl; } }

        public DockBase(Base parent)
            : base(parent)
        {
            Padding = new Padding(1, 1, 1, 1);
            SetSize(200, 200);
        }

        internal override bool onKeySpace(bool bDown)
        {
            // No action on space (default button action is to press)
            return false;
        }

        protected virtual void SetupChildDock(Pos pos)
        {
            if (m_DockedTabControl == null)
            {
                m_DockedTabControl = new DockedTabControl(this);
                m_DockedTabControl.OnLoseTab += onTabRemoved;
                m_DockedTabControl.TabStripPosition = Pos.Bottom;
                m_DockedTabControl.TitleBarVisible = true;
            }

            Dock = pos;

            Pos sizeDir = Pos.Left;
            if (pos == Pos.Left) sizeDir = Pos.Right;
            if (pos == Pos.Top) sizeDir = Pos.Bottom;
            if (pos == Pos.Bottom) sizeDir = Pos.Top;

            Resizer sizer = new Resizer(this);
            sizer.Dock = sizeDir;
            sizer.SetResizeDir(sizeDir);
            sizer.SetSize(2, 2);
            sizer.Target = this;
        }

        protected override void Render(Skin.Base skin)
        {

        }

        protected virtual DockBase GetChildDock(Pos pos)
        {
            // todo: verify
            DockBase dock = null;
            switch (pos)
            {
                case Pos.Left:
                    if (m_Left == null)
                    {
                        m_Left = new DockBase(this);
                        m_Left.SetupChildDock(pos);
                    }
                    dock = m_Left;
                    break;

                case Pos.Right:
                    if (m_Right == null)
                    {
                        m_Right = new DockBase(this);
                        m_Right.SetupChildDock(pos);
                    }
                    dock = m_Right;
                    break;

                case Pos.Top:
                    if (m_Top == null)
                    {
                        m_Top = new DockBase(this);
                        m_Top.SetupChildDock(pos);
                    }
                    dock = m_Top;
                    break;

                case Pos.Bottom:
                    if (m_Bottom == null)
                    {
                        m_Bottom = new DockBase(this);
                        m_Bottom.SetupChildDock(pos);
                    }
                    dock = m_Bottom;
                    break;
            }

            if (dock != null)
                dock.IsHidden = false;

            return dock;
        }

        protected virtual Pos GetDroppedTabDirection(int x, int y)
        {
            int w = Width;
            int h = Height;
            float top = y / (float)h;
            float left = x / (float)w;
            float right = (w - x) / (float)w;
            float bottom = (h - y) / (float)h;
            float minimum = Math.Min(Math.Min(Math.Min(top, left), right), bottom);

            m_bDropFar = (minimum < 0.2f);

            if (minimum > 0.3f)
                return Pos.Fill;

            if (top == minimum && (null == m_Top || m_Top.IsHidden))
                return Pos.Top;
            if (left == minimum && (null == m_Left || m_Left.IsHidden))
                return Pos.Left;
            if (right == minimum && (null == m_Right || m_Right.IsHidden))
                return Pos.Right;
            if (bottom == minimum && (null == m_Bottom || m_Bottom.IsHidden))
                return Pos.Bottom;

            return Pos.Fill;
        }

        public override bool DragAndDrop_CanAcceptPackage(Package p)
        {
            // A TAB button dropped 
            if (p.Name == "TabButtonMove")
                return true;

            // a TAB window dropped
            if (p.Name == "TabWindowMove")
                return true;

            return false;
        }

        public override bool DragAndDrop_HandleDrop(Package p, int x, int y)
        {
            Point pPos = CanvasPosToLocal(new Point(x, y));
            Pos dir = GetDroppedTabDirection(pPos.X, pPos.Y);

            DockedTabControl pAddTo = m_DockedTabControl;
            if (dir == Pos.Fill && pAddTo == null)
                return false;

            if (dir != Pos.Fill)
            {
                DockBase pDock = GetChildDock(dir);
                pAddTo = pDock.m_DockedTabControl;

                if (!m_bDropFar)
                    pDock.BringToFront();
                else
                    pDock.SendToBack();
            }

            if (p.Name == "TabButtonMove")
            {
                TabButton pTabButton = DragAndDrop.SourceControl as TabButton;
                if (null == pTabButton)
                    return false;

                pAddTo.AddPage(pTabButton);
            }

            if (p.Name == "TabWindowMove")
            {
                DockedTabControl pTabControl = DragAndDrop.SourceControl as DockedTabControl;
                if (null == pTabControl)
                    return false;
                if (pTabControl == pAddTo)
                    return false;

                pTabControl.MoveTabsTo(pAddTo);
            }

            Invalidate();

            return true;
        }

        public virtual bool IsEmpty
        {
            get
            {
                if (m_DockedTabControl != null && m_DockedTabControl.TabCount > 0) return false;

                if (m_Left != null && !m_Left.IsEmpty) return false;
                if (m_Right != null && !m_Right.IsEmpty) return false;
                if (m_Top != null && !m_Top.IsEmpty) return false;
                if (m_Bottom != null && !m_Bottom.IsEmpty) return false;

                return true;
            }
        }

        protected virtual void onTabRemoved(Base control)
        {
            DoRedundancyCheck();
            DoConsolidateCheck();
        }

        protected virtual void DoRedundancyCheck()
        {
            if (!IsEmpty) return;

            DockBase pDockParent = Parent as DockBase;
            if (null == pDockParent) return;

            pDockParent.onRedundantChildDock(this);
        }

        protected virtual void DoConsolidateCheck()
        {
            if (IsEmpty) return;
            if (null == m_DockedTabControl) return;
            if (m_DockedTabControl.TabCount > 0) return;

            if (m_Bottom != null && !m_Bottom.IsEmpty)
            {
                m_Bottom.m_DockedTabControl.MoveTabsTo(m_DockedTabControl);
                return;
            }

            if (m_Top != null && !m_Top.IsEmpty)
            {
                m_Top.m_DockedTabControl.MoveTabsTo(m_DockedTabControl);
                return;
            }

            if (m_Left != null && !m_Left.IsEmpty)
            {
                m_Left.m_DockedTabControl.MoveTabsTo(m_DockedTabControl);
                return;
            }

            if (m_Right != null && !m_Right.IsEmpty)
            {
                m_Right.m_DockedTabControl.MoveTabsTo(m_DockedTabControl);
                return;
            }
        }

        protected virtual void onRedundantChildDock(DockBase dock)
        {
            dock.IsHidden = true;
            DoRedundancyCheck();
            DoConsolidateCheck();
        }

        public override void DragAndDrop_HoverEnter(Package p, int x, int y)
        {
            m_bDrawHover = true;
        }

        public override void DragAndDrop_HoverLeave(Package p)
        {
            m_bDrawHover = false;
        }

        public override void DragAndDrop_Hover(Package p, int x, int y)
        {
            Point pPos = CanvasPosToLocal(new Point(x, y));
            Pos dir = GetDroppedTabDirection(pPos.X, pPos.Y);

            if (dir == Pos.Fill)
            {
                if (null == m_DockedTabControl)
                {
                    m_HoverRect = Rectangle.Empty;
                    return;
                }

                m_HoverRect = InnerBounds;
                return;
            }

            m_HoverRect = RenderBounds;

            int HelpBarWidth = 0;

            if (dir == Pos.Left)
            {
                HelpBarWidth = (int)(m_HoverRect.Width * 0.25f);
                m_HoverRect.Width = HelpBarWidth;
            }

            if (dir == Pos.Right)
            {
                HelpBarWidth = (int)(m_HoverRect.Width * 0.25f);
                m_HoverRect.X = m_HoverRect.Width - HelpBarWidth;
                m_HoverRect.Width = HelpBarWidth;
            }

            if (dir == Pos.Top)
            {
                HelpBarWidth = (int)(m_HoverRect.Height * 0.25f);
                m_HoverRect.Height = HelpBarWidth;
            }

            if (dir == Pos.Bottom)
            {
                HelpBarWidth = (int)(m_HoverRect.Height * 0.25f);
                m_HoverRect.Y = m_HoverRect.Height - HelpBarWidth;
                m_HoverRect.Height = HelpBarWidth;
            }

            if ((dir == Pos.Top || dir == Pos.Bottom) && !m_bDropFar)
            {
                if (m_Left != null && m_Left.IsVisible)
                {
                    m_HoverRect.X += m_Left.Width;
                    m_HoverRect.Width -= m_Left.Width;
                }

                if (m_Right != null && m_Right.IsVisible)
                {
                    m_HoverRect.Width -= m_Right.Width;
                }
            }

            if ((dir == Pos.Left || dir == Pos.Right) && !m_bDropFar)
            {
                if (m_Top != null && m_Top.IsVisible)
                {
                    m_HoverRect.Y += m_Top.Height;
                    m_HoverRect.Height -= m_Top.Height;
                }

                if (m_Bottom != null && m_Bottom.IsVisible)
                {
                    m_HoverRect.Height -= m_Bottom.Height;
                }
            }
        }

        protected override void RenderOver(Skin.Base skin)
        {
            if (!m_bDrawHover)
                return;

            Renderer.Base render = skin.Renderer;
            render.DrawColor = Color.FromArgb(20, 255, 200, 255);
            render.DrawFilledRect(RenderBounds);

            if (m_HoverRect.Width == 0)
                return;

            render.DrawColor = Color.FromArgb(100, 255, 200, 255);
            render.DrawFilledRect(m_HoverRect);

            render.DrawColor = Color.FromArgb(200, 255, 200, 255);
            render.DrawLinedRect(m_HoverRect);
        }
    }
}
