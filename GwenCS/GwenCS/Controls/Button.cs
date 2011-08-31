using System;

namespace Gwen.Controls
{
    public class Button : Label
    {
        protected bool m_bDepressed;
        protected bool m_bToggle;
        protected bool m_bToggleStatus;
        protected bool m_bCenterImage;
        protected ImagePanel m_Image;

        public event ControlCallback OnPress;
        public event ControlCallback OnDown;
        public event ControlCallback OnUp;
        public event ControlCallback OnDoubleClick;
        public event ControlCallback OnToggle;
        public event ControlCallback OnToggleOn;
        public event ControlCallback OnToggleOff;

        public bool IsDepressed
        {
            get { return m_bDepressed; }
            set
            {
                if (m_bDepressed == value) return;
                m_bDepressed = value; 
                Redraw();
            }
        }
        public bool IsToggle { get { return m_bToggle; } set { m_bToggle = value; } }
        public bool ToggleState
        {
            get { return m_bToggleStatus; }
            set
            {
                if (m_bToggleStatus == value) return;

                m_bToggleStatus = value;

                if (OnToggle != null)
                    OnToggle.Invoke(this);

                if (m_bToggleStatus)
                {
                    if (OnToggleOn != null)
                        OnToggleOn.Invoke(this);
                }
                else
                {
                    if (OnToggleOff != null)
                        OnToggleOff.Invoke(this);
                }

                Redraw();
            }
        }

        public Button(Base parent)
            : base(parent)
        {
            SetSize(100, 20);
            MouseInputEnabled = true;
            Alignment = Pos.Center;
            TextPadding = new Padding(3, 0, 3, 0);
        }

        public virtual void Toggle()
        {
            ToggleState = !ToggleState;
        }

        public virtual void ReceiveEventPress(Base control)
        {
            onPress();
        }

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);

            if (ShouldDrawBackground)
            {
                bool bDrawDepressed = IsDepressed && IsHovered;
                if (IsToggle)
                    bDrawDepressed = bDrawDepressed || ToggleState;

                bool bDrawHovered = IsHovered && ShouldDrawHover;

                skin.DrawButton(this, bDrawDepressed, bDrawHovered, IsDisabled);
            }
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            base.onMouseClickLeft(x, y, pressed);
            if (pressed)
            {
                IsDepressed = true;
                Global.MouseFocus = this;
                if (OnDown != null)
                    OnDown.Invoke(this);
            }
            else
            {
                if (IsHovered && m_bDepressed)
                {
                    onPress();
                }

                IsDepressed = false;
                Global.MouseFocus = null;
                if (OnUp != null)
                    OnUp.Invoke(this);
            }

            Redraw();
        }

        internal virtual void onPress()
        {
            if (IsToggle)
            {
                Toggle();
            }

            if (OnPress != null)
                OnPress.Invoke(this);
        }
        
        public virtual void SetImage(String name, bool center = false)
        {
            if (String.IsNullOrEmpty(name))
            {
                m_Image = null;
                return;
            }

            if (m_Image == null)
            {
                m_Image = new ImagePanel(this);
            }

            m_Image.ImageName = name;
            m_Image.SizeToContents( );
            m_Image.SetPos(Math.Max(m_Padding.Left, 2), 2);
            m_bCenterImage = center;

            m_rTextPadding.Left = m_Image.Right + 2;
        }

        public override void SizeToContents()
        {
            base.SizeToContents();
            if (m_Image != null)
            {
                int height = m_Image.Height + 4;
                if (Height < height)
                {
                    Height = height;
                }
            }
        }

        internal override bool onKeySpace(bool bDown)
        {
            if (bDown)
                onPress();
            return true;
        }

        protected override void AcceleratePressed()
        {
            onPress();
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);
            if (m_Image != null)
            {
                Align.CenterVertically(m_Image);

                if (m_bCenterImage)
                    Align.CenterHorizontally(m_Image);
            }
        }

        public override void UpdateColors()
        {
            if (IsDisabled)
            {
                TextColor = Skin.Colors.Button.Disabled;
                return;
            }

            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.Colors.Button.Down;
                return;
            }

            if (IsHovered)
            {
                TextColor = Skin.Colors.Button.Hover;
                return;
            }

            TextColor = Skin.Colors.Button.Normal;
        }
    }
}
