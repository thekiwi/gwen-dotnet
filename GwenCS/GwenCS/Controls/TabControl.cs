using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class TabControl : Base
    {
        protected TabStrip m_TabStrip;
        protected TabButton m_pCurrentButton;
        protected ScrollBarButton[] m_pScroll;
        protected int m_iScrollOffset;

        public event ControlCallback OnAddTab;
        public event ControlCallback OnLoseTab;

        public bool AllowReorder { get { return m_TabStrip.AllowReorder; } set { m_TabStrip.AllowReorder = value; } }
        public TabButton CurrentButton { get { return m_pCurrentButton; } }
        public Pos TabStripPosition { get { return m_TabStrip.TabPosition; }set { m_TabStrip.TabPosition = value; } }

        public TabControl(Base parent)
            : base(parent)
        {
            m_pScroll = new ScrollBarButton[2];

            m_iScrollOffset = 0;

            m_TabStrip = new TabStrip(this);
            m_TabStrip.Dock = Pos.Top;
            m_TabStrip.Width = 100;
            m_TabStrip.Height = 20;

            // Make this some special control?
            m_pScroll[0] = new ScrollBarButton(this);
            m_pScroll[0].SetDirectionLeft();
            m_pScroll[0].OnPress += ScrollPressedLeft;
            m_pScroll[0].SetSize(14, 16);

            m_pScroll[1] = new ScrollBarButton(this);
            m_pScroll[1].SetDirectionRight();
            m_pScroll[1].OnPress += ScrollPressedRight;
            m_pScroll[1].SetSize(14, 16);

            m_InnerPanel = new TabControlInner(this);
            m_InnerPanel.Dock = Pos.Fill;

            IsTabable = false;
        }

        public TabButton AddPage(String strText, Base pPage = null)
        {
            if (null == pPage)
            {
                pPage = new Base(this);
            }
            else
            {
                pPage.Parent = this;
            }

            TabButton pButton = new TabButton(m_TabStrip);
            pButton.SetText(strText);
            pButton.Page = pPage;
            pButton.IsTabable = false;

            AddPage(pButton);
            return pButton;
        }

        public void AddPage(TabButton pButton)
        {
            Base pPage = pButton.Page;
            pPage.Parent = this;
            pPage.IsHidden = true;
            pPage.Margin = new Margin(6, 6, 6, 6);
            pPage.Dock = Pos.Fill;

            pButton.Parent = m_TabStrip;
            pButton.Dock = Pos.Left;
            pButton.SizeToContents();
            if (pButton.TabControl != null)
                pButton.TabControl.UnsubscribeTabEvent(pButton);
            pButton.TabControl = this;
            pButton.OnPress += onTabPressed;

            if (null == m_pCurrentButton)
            {
                pButton.onPress();
            }

            if (OnAddTab != null)
                OnAddTab.Invoke(this);

            Invalidate();
        }

        private void UnsubscribeTabEvent(TabButton button)
        {
            button.OnPress -= onTabPressed;
        }

        internal virtual void onTabPressed(Base control)
        {
            TabButton pButton = control as TabButton;
            if (null == pButton) return;

            Base pPage = pButton.Page;
            if (null == pPage) return;

            if (m_pCurrentButton == pButton)
                return;

            if (null != m_pCurrentButton)
            {
                Base page = m_pCurrentButton.Page;
                if (page != null)
                {
                    page.IsHidden = true;
                }
                m_pCurrentButton = null;
            }

            m_pCurrentButton = pButton;

            pPage.IsHidden = false;

            m_TabStrip.Invalidate();
            Invalidate();
        }

        protected override void PostLayout(Skin.Base skin)
        {
            base.PostLayout(skin);
            HandleOverflow();

            if (m_TabStrip.IsHidden)
            {
                (m_InnerPanel as TabControlInner).UpdateCurrentButton(Rectangle.Empty);
            }
            else if (m_pCurrentButton != null)
            {
                Rectangle rct;

                Point p = m_pCurrentButton.LocalPosToCanvas(Point.Empty);
                p = m_InnerPanel.CanvasPosToLocal(p);

                rct = new Rectangle(p.X + 1, p.Y + 1, m_pCurrentButton.Width - 2, m_pCurrentButton.Height - 2);
                (m_InnerPanel as TabControlInner).UpdateCurrentButton(rct);
            }
        }

        internal virtual void onLoseTab(TabButton button)
        {
            if (m_pCurrentButton == button)
                m_pCurrentButton = null;

            //TODO: Select a tab if any exist.

            if (OnLoseTab != null)
                OnLoseTab.Invoke(this);

            Invalidate();
        }

        public int TabCount { get { return m_TabStrip.ChildrenCount; } }

        protected void HandleOverflow()
        {
            Point TabsSize = m_TabStrip.ChildrenSize();

            // Only enable the scrollers if the tabs are at the top.
            // This is a limitation we should explore.
            // Really TabControl should have derivitives for tabs placed elsewhere where we could specialize 
            // some functions like this for each direction.
            bool bNeeded = TabsSize.X > Width && m_TabStrip.Dock == Pos.Top;

            m_pScroll[0].IsHidden = !bNeeded;
            m_pScroll[1].IsHidden = !bNeeded;

            if (!bNeeded) return;

            m_iScrollOffset = Global.Clamp(m_iScrollOffset, 0, TabsSize.X - Width + 32);

#if false
    //
    // This isn't frame rate independent. 
    // Could be better. Get rid of m_iScrollOffset and just use m_TabStrip.GetMargin().left ?
    // Then get a margin animation type and do it properly! 
    // TODO!
    //
        m_TabStrip.SetMargin( Margin( Gwen::Approach( m_TabStrip.GetMargin().left, m_iScrollOffset * -1, 2 ), 0, 0, 0 ) );
        InvalidateParent();
#else
            m_TabStrip.Margin = new Margin(m_iScrollOffset*-1, 0, 0, 0);
#endif

            m_pScroll[0].SetPos(Width - 30, 5);
            m_pScroll[1].SetPos(m_pScroll[0].Right, 5);
        }

        protected virtual void ScrollPressedLeft(Base control)
        {
            m_iScrollOffset -= 120;
        }

        protected virtual void ScrollPressedRight(Base control)
        {
            m_iScrollOffset += 120;
        }
    }
}
