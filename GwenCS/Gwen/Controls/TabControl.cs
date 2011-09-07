using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class TabControl : Base
    {
        protected TabStrip m_TabStrip;
        protected TabButton m_CurrentButton;
        protected ScrollBarButton[] m_Scroll;
        protected int m_ScrollOffset;

        public event ControlCallback OnAddTab;
        public event ControlCallback OnLoseTab;

        public bool AllowReorder { get { return m_TabStrip.AllowReorder; } set { m_TabStrip.AllowReorder = value; } }
        public TabButton CurrentButton { get { return m_CurrentButton; } }
        public Pos TabStripPosition { get { return m_TabStrip.TabPosition; }set { m_TabStrip.TabPosition = value; } }
        public TabStrip TabStrip { get { return m_TabStrip; } }

        public TabControl(Base parent)
            : base(parent)
        {
            m_Scroll = new ScrollBarButton[2];
            m_ScrollOffset = 0;

            m_TabStrip = new TabStrip(this);
            m_TabStrip.TabPosition = Pos.Top;

            // Make this some special control?
            m_Scroll[0] = new ScrollBarButton(this);
            m_Scroll[0].SetDirectionLeft();
            m_Scroll[0].OnPress += ScrollPressedLeft;
            m_Scroll[0].SetSize(14, 16);

            m_Scroll[1] = new ScrollBarButton(this);
            m_Scroll[1].SetDirectionRight();
            m_Scroll[1].OnPress += ScrollPressedRight;
            m_Scroll[1].SetSize(14, 16);

            m_InnerPanel = new TabControlInner(this);
            m_InnerPanel.Dock = Pos.Fill;
            m_InnerPanel.SendToBack();

            IsTabable = false;
        }

        public override void Dispose()
        {
            m_TabStrip.Dispose();
            m_Scroll[0].Dispose();
            m_Scroll[1].Dispose();
            // TabStrip disposes pages
            base.Dispose();
        }

        public TabButton AddPage(String text, Base page = null)
        {
            if (null == page)
            {
                page = new Base(this);
            }
            else
            {
                page.Parent = this;
            }

            TabButton button = new TabButton(m_TabStrip);
            button.SetText(text);
            button.Page = page;
            button.IsTabable = false;

            AddPage(button);
            return button;
        }

        public void AddPage(TabButton button)
        {
            Base page = button.Page;
            page.Parent = this;
            page.IsHidden = true;
            page.Margin = new Margin(6, 6, 6, 6);
            page.Dock = Pos.Fill;

            button.Parent = m_TabStrip;
            button.Dock = Pos.Left;
            button.SizeToContents();
            if (button.TabControl != null)
                button.TabControl.UnsubscribeTabEvent(button);
            button.TabControl = this;
            button.OnPress += onTabPressed;

            if (null == m_CurrentButton)
            {
                button.onPress();
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
            TabButton button = control as TabButton;
            if (null == button) return;

            Base page = button.Page;
            if (null == page) return;

            if (m_CurrentButton == button)
                return;

            if (null != m_CurrentButton)
            {
                Base page2 = m_CurrentButton.Page;
                if (page2 != null)
                {
                    page2.IsHidden = true;
                }
                m_CurrentButton.Redraw();
                m_CurrentButton = null;
            }

            m_CurrentButton = button;

            page.IsHidden = false;

            m_TabStrip.Invalidate();
            Invalidate();
        }

        protected override void PostLayout(Skin.Base skin)
        {
            base.PostLayout(skin);
            HandleOverflow();
        }

        internal virtual void onLoseTab(TabButton button)
        {
            if (m_CurrentButton == button)
                m_CurrentButton = null;

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
            bool needed = TabsSize.X > Width && m_TabStrip.Dock == Pos.Top;

            m_Scroll[0].IsHidden = !needed;
            m_Scroll[1].IsHidden = !needed;

            if (!needed) return;

            m_ScrollOffset = Global.Clamp(m_ScrollOffset, 0, TabsSize.X - Width + 32);

#if false
    //
    // This isn't frame rate independent. 
    // Could be better. Get rid of m_ScrollOffset and just use m_TabStrip.GetMargin().left ?
    // Then get a margin animation type and do it properly! 
    // TODO!
    //
        m_TabStrip.SetMargin( Margin( Gwen::Approach( m_TabStrip.GetMargin().left, m_iScrollOffset * -1, 2 ), 0, 0, 0 ) );
        InvalidateParent();
#else
            m_TabStrip.Margin = new Margin(m_ScrollOffset*-1, 0, 0, 0);
#endif

            m_Scroll[0].SetPos(Width - 30, 5);
            m_Scroll[1].SetPos(m_Scroll[0].Right, 5);
        }

        protected virtual void ScrollPressedLeft(Base control)
        {
            m_ScrollOffset -= 120;
        }

        protected virtual void ScrollPressedRight(Base control)
        {
            m_ScrollOffset += 120;
        }
    }
}
