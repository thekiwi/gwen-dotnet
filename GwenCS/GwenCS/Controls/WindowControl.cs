using System;
using System.Drawing;
using System.Linq;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class WindowControl : ResizableControl
    {
        protected Dragger m_TitleBar;
        protected Label m_Title;
        protected CloseButton m_CloseButton;
        protected bool m_bDeleteOnClose;
        protected Modal m_Modal;

        public String Title { get { return m_Title.Text; } set { m_Title.Text = value; } }
        public bool IsClosable { get { return !m_CloseButton.IsHidden; } set { m_CloseButton.IsHidden = !value; } }
        public override bool IsHidden
        {
            get { return base.IsHidden; }
            set
            {
                if (!value)
                    BringToFront();
                base.IsHidden = value;
            }
        }

        public WindowControl(Base parent) : base(parent)
        {
            m_TitleBar = new Dragger(this);
            m_TitleBar.Height = 24;
            m_TitleBar.Padding = new Padding(0, 0, 0, 0);
            m_TitleBar.Margin = new Margin(0, 0, 0, 4);
            m_TitleBar.Target = this;
            m_TitleBar.Dock = Pos.Top;

            m_Title = new Label(m_TitleBar);
            m_Title.Alignment = Pos.Left | Pos.CenterV;
            m_Title.Text = "Window"; // [omeg] todo: i18n
            m_Title.Dock = Pos.Fill;
            m_Title.Padding = new Padding(8, 0, 0, 0);
            m_Title.TextColor = Skin.Colors.Window.TitleInactive;

            m_CloseButton = new CloseButton(m_TitleBar);
            m_CloseButton.Text = String.Empty;
            m_CloseButton.SetSize(24, 24);
            m_CloseButton.Dock = Pos.Right;
            m_CloseButton.OnPress += CloseButtonPressed;
            m_CloseButton.IsTabable = false;
            m_CloseButton.Window = this;

            //Create a blank content control, dock it to the top - Should this be a ScrollControl?
            m_InnerPanel = new Base(this);
            m_InnerPanel.Dock = Pos.Fill;
            GetResizer(8).Hide();
            BringToFront();
            IsTabable = false;
            Focus();
            MinimumSize = new Point(100, 40);
            ClampMovement = true;
            KeyboardInputEnabled = false;
        }

        public override void Dispose()
        {
            if (m_Modal != null)
                m_Modal.Dispose();
            m_TitleBar.Dispose();
            m_Title.Dispose();
            m_CloseButton.Dispose();
            base.Dispose();
        }

        protected virtual void CloseButtonPressed(Base control)
        {
            IsHidden = true;

            if (m_bDeleteOnClose)
                Dispose();
        }

        public void MakeModal(bool invisible = false)
        {
            if (m_Modal != null)
                return;

            m_Modal = new Modal(GetCanvas());
            Parent = m_Modal;

            if (invisible)
                m_Modal.ShouldDrawBackground = false;
        }

        public override bool IsOnTop
        {
            get { return Parent.Children.Where(x => x is WindowControl).Last() == this; }
        }

        protected override void Render(Skin.Base skin)
        {
            bool hasFocus = IsOnTop;

            if (hasFocus)
                m_Title.TextColor = Skin.Colors.Window.TitleActive;
            else
                m_Title.TextColor = Skin.Colors.Window.TitleInactive;

            skin.DrawWindow(this, m_TitleBar.Bottom, hasFocus);
        }

        protected override void RenderUnder(Skin.Base skin)
        {
            base.RenderUnder(skin);
            skin.DrawShadow(this);
        }

        public override void Touch()
        {
            base.Touch();
            BringToFront();
        }

        protected override void RenderFocus(Skin.Base skin)
        {
            
        }
    }
}
