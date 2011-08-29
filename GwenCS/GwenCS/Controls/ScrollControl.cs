using System;
using System.Linq;

namespace Gwen.Controls
{
    public class ScrollControl : Base
    {
        protected bool m_bCanScrollH;
        protected bool m_bCanScrollV;
        protected bool m_bAutoHideBars;

        protected BaseScrollBar m_VerticalScrollBar;
        protected BaseScrollBar m_HorizontalScrollBar;

        public bool CanScrollH { get { return m_bCanScrollH; } }
        public bool CanScrollV { get { return m_bCanScrollV; } }
        public bool AutoHideBars { get { return m_bAutoHideBars; } set { m_bAutoHideBars = value; } }

        public ScrollControl(Base parent)
            : base(parent)
        {
            MouseInputEnabled = false;

            m_VerticalScrollBar = new VerticalScrollBar(this);
            m_VerticalScrollBar.Dock = Pos.Right;
            m_VerticalScrollBar.OnBarMoved += VBarMoved;
            m_bCanScrollV = true;
            m_VerticalScrollBar.NudgeAmount = 30;

            m_HorizontalScrollBar = new HorizontalScrollBar(this);
            m_HorizontalScrollBar.Dock = Pos.Bottom;
            m_HorizontalScrollBar.OnBarMoved += HBarMoved;
            m_bCanScrollH = true;
            m_HorizontalScrollBar.NudgeAmount = 30;

            m_InnerPanel = new Base(this);
            m_InnerPanel.SetPos(0, 0);
            m_InnerPanel.Margin = new Margin(5, 5, 5, 5);
            m_InnerPanel.SendToBack();
            m_InnerPanel.MouseInputEnabled = false;

            m_bAutoHideBars = false;
        }

        protected bool HScrollRequired
        {
            set
            {
                if (value)
                {
                    m_HorizontalScrollBar.SetScrollAmount(0, true);
                    m_HorizontalScrollBar.IsDisabled = true;
                    if (m_bAutoHideBars)
                        m_HorizontalScrollBar.IsHidden = true;
                }
                else
                {
                    m_HorizontalScrollBar.IsHidden = false;
                    m_HorizontalScrollBar.IsDisabled = false;
                }
            }
        }

        protected bool VScrollRequired
        {
            set
            {
                if (value)
                {
                    m_VerticalScrollBar.SetScrollAmount(0, true);
                    m_VerticalScrollBar.IsDisabled = true;
                    if (m_bAutoHideBars)
                        m_VerticalScrollBar.IsHidden = true;
                }
                else
                {
                    m_VerticalScrollBar.IsHidden = false;
                    m_VerticalScrollBar.IsDisabled = false;
                }
            }
        }

        public virtual void SetScroll(bool h, bool v)
        {
            m_bCanScrollV = v;
            m_bCanScrollH = h;
            m_VerticalScrollBar.IsHidden = !m_bCanScrollV;
            m_HorizontalScrollBar.IsHidden = !m_bCanScrollH;
        }

        protected virtual void SetInnerSize(int w, int h)
        {
            m_InnerPanel.SetSize(w, h);
        }

        protected virtual void VBarMoved(Base control)
        {
            Invalidate();
        }

        protected virtual void HBarMoved(Base control)
        {
            Invalidate();
        }

        internal override void onChildBoundsChanged(System.Drawing.Rectangle oldChildBounds, Base child)
        {
            UpdateScrollBars();
        }

        protected override void Layout(Skin.Base skin)
        {
            UpdateScrollBars();
            base.Layout(skin);
        }

        internal override bool onMouseWheeled(int iDelta)
        {
            if (CanScrollV && m_VerticalScrollBar.IsVisible)
            {
                if (m_VerticalScrollBar.SetScrollAmount(
                    m_VerticalScrollBar.ScrollAmount - m_VerticalScrollBar.NudgeAmount * (iDelta / 60.0f), true))
                    return true;
            }

            if (CanScrollH && m_HorizontalScrollBar.IsVisible)
            {
                if (m_HorizontalScrollBar.SetScrollAmount(
                    m_HorizontalScrollBar.ScrollAmount - m_HorizontalScrollBar.NudgeAmount * (iDelta / 60.0f), true))
                    return true;
            }

            return false;
        }

        protected override void Render(Skin.Base skin)
        {
#if false

    // Debug render - this shouldn't render ANYTHING REALLY - it should be up to the parent!

    Gwen::Rect rect = GetRenderBounds();
    Gwen::Renderer::Base* render = skin->GetRender();

    render->SetDrawColor( Gwen::Color( 255, 255, 0, 100 ) );
    render->DrawFilledRect( rect );

    render->SetDrawColor( Gwen::Color( 255, 0, 0, 100 ) );
    render->DrawFilledRect( m_InnerPanel->GetBounds() );

    render->RenderText( skin->GetDefaultFont(), Gwen::Point( 0, 0 ), Utility::Format( L"Offset: %i %i", m_InnerPanel->X(), m_InnerPanel->Y() ) );
#endif
        }

        internal virtual void UpdateScrollBars()
        {
            if (null == m_InnerPanel)
                return;

            //Get the max size of all our children together
            int childrenWidth = m_InnerPanel.Children.Count > 0 ? m_InnerPanel.Children.Max(x => x.Right) : 0;
            int childrenHeight = m_InnerPanel.Children.Count > 0 ? m_InnerPanel.Children.Max(x => x.Bottom) : 0;


            m_InnerPanel.SetSize(Math.Max(Width, childrenWidth), Math.Max(Height, childrenHeight));

            float wPercent = Width/
                             (float) (childrenWidth + (m_VerticalScrollBar.IsHidden ? 0 : m_VerticalScrollBar.Width));
            float hPercent = Height/
                             (float)
                             (childrenHeight + (m_HorizontalScrollBar.IsHidden ? 0 : m_HorizontalScrollBar.Height));

            if (m_bCanScrollV)
                VScrollRequired = hPercent >= 1;
            else
                m_VerticalScrollBar.IsHidden = true;

            if (m_bCanScrollH)
                HScrollRequired = wPercent >= 1;
            else
                m_HorizontalScrollBar.IsHidden = true;


            m_VerticalScrollBar.ContentSize = m_InnerPanel.Height;
            m_VerticalScrollBar.ViewableContentSize = Height -
                                                      (m_HorizontalScrollBar.IsHidden ? 0 : m_HorizontalScrollBar.Height);


            m_HorizontalScrollBar.ContentSize = m_InnerPanel.Width;
            m_HorizontalScrollBar.ViewableContentSize = Width -
                                                        (m_VerticalScrollBar.IsHidden ? 0 : m_VerticalScrollBar.Width);

            int newInnerPanelPosX = 0;
            int newInnerPanelPosY = 0;

            if (CanScrollV && !m_VerticalScrollBar.IsHidden)
            {
                newInnerPanelPosY =
                    Global.Trunc(
                        -((m_InnerPanel.Height) - Height + (m_HorizontalScrollBar.IsHidden ? 0 : m_HorizontalScrollBar.Height))*
                        m_VerticalScrollBar.ScrollAmount);
            }
            if (CanScrollH && !m_HorizontalScrollBar.IsHidden)
            {
                newInnerPanelPosX =
                    Global.Trunc(
                        -((m_InnerPanel.Width) - Width + (m_VerticalScrollBar.IsHidden ? 0 : m_VerticalScrollBar.Width))*
                        m_HorizontalScrollBar.ScrollAmount);
            }

            m_InnerPanel.SetPos(newInnerPanelPosX, newInnerPanelPosY);
        }

        public virtual void ScrollToBottom()
        {
            if (!CanScrollV)
                return;

            UpdateScrollBars();
            m_VerticalScrollBar.ScrollToBottom();
        }

        public virtual void ScrollToTop()
        {
            if (CanScrollV)
            {
                UpdateScrollBars();
                m_VerticalScrollBar.ScrollToTop();
            }
        }

        public virtual void ScrollToLeft()
        {
            if (CanScrollH)
            {
                UpdateScrollBars();
                m_VerticalScrollBar.ScrollToLeft();
            }
        }

        public virtual void ScrollToRight()
        {
            if (CanScrollH)
            {
                UpdateScrollBars();
                m_VerticalScrollBar.ScrollToRight();
            }
        }

        public virtual void Clear()
        {
            m_InnerPanel.RemoveAllChildren();
        }
    }
}
