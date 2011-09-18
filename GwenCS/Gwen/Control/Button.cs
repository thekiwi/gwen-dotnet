using System;

namespace Gwen.Control
{
    /// <summary>
    /// Button control.
    /// </summary>
    public class Button : Label
    {
        protected bool m_Depressed;
        protected bool m_Toggle;
        protected bool m_ToggleStatus;
        protected bool m_CenterImage;
        protected ImagePanel m_Image;

        /// <summary>
        /// Invoked when the button is released.
        /// </summary>
        public event ControlCallback OnPress;

        /// <summary>
        /// Invoked when the button is pressed down.
        /// </summary>
        public event ControlCallback OnDown;

        /// <summary>
        /// Invoked when the button is released.
        /// </summary>
        public event ControlCallback OnUp;

        /// <summary>
        /// Invoked when the button's toggle state has changed.
        /// </summary>
        public event ControlCallback OnToggle;

        /// <summary>
        /// Invoked when the button's toggle state has changed to On.
        /// </summary>
        public event ControlCallback OnToggleOn;

        /// <summary>
        /// Invoked when the button's toggle state has changed to Off.
        /// </summary>
        public event ControlCallback OnToggleOff;

        /// <summary>
        /// Invoked when the button has been double clicked.
        /// </summary>
        public event ControlCallback OnDoubleClickLeft;

        /// <summary>
        /// Indicates whether the button is depressed.
        /// </summary>
        public bool IsDepressed
        {
            get { return m_Depressed; }
            set
            {
                if (m_Depressed == value) return;
                m_Depressed = value; 
                Redraw();
            }
        }

        /// <summary>
        /// Indicates whether the button is toggleable.
        /// </summary>
        public bool IsToggle { get { return m_Toggle; } set { m_Toggle = value; } }

        /// <summary>
        /// Determines the button's toggle state.
        /// </summary>
        public bool ToggleState
        {
            get { return m_ToggleStatus; }
            set
            {
                if (!m_Toggle) return;
                if (m_ToggleStatus == value) return;

                m_ToggleStatus = value;

                if (OnToggle != null)
                    OnToggle.Invoke(this);

                if (m_ToggleStatus)
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

        /// <summary>
        /// Control constructor.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public Button(Base parent)
            : base(parent)
        {
            SetSize(100, 20);
            MouseInputEnabled = true;
            Alignment = Pos.Center;
            TextPadding = new Padding(3, 0, 3, 0);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            if (m_Image != null)
                m_Image.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Toggles the button.
        /// </summary>
        public virtual void Toggle()
        {
            ToggleState = !ToggleState;
        }

        /// <summary>
        /// Presses the button.
        /// </summary>
        public virtual void Press(Base control = null)
        {
            onPress();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);

            if (ShouldDrawBackground)
            {
                bool drawDepressed = IsDepressed && IsHovered;
                if (IsToggle)
                    drawDepressed = drawDepressed || ToggleState;

                bool bDrawHovered = IsHovered && ShouldDrawHover;

                skin.DrawButton(this, drawDepressed, bDrawHovered, IsDisabled);
            }
        }

        /// <summary>
        /// Handler invoked on mouse click (left) event.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="down">If set to <c>true</c> mouse button is down.</param>
        protected override void onMouseClickLeft(int x, int y, bool down)
        {
            base.onMouseClickLeft(x, y, down);
            if (down)
            {
                IsDepressed = true;
                Global.MouseFocus = this;
                if (OnDown != null)
                    OnDown.Invoke(this);
            }
            else
            {
                if (IsHovered && m_Depressed)
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

        /// <summary>
        /// Internal OnPress implementation.
        /// </summary>
        protected virtual void onPress()
        {
            if (IsToggle)
            {
                Toggle();
            }

            if (OnPress != null)
                OnPress.Invoke(this);
        }
        
        /// <summary>
        /// Sets the button's image.
        /// </summary>
        /// <param name="textureName">Texture name. Null to remove.</param>
        /// <param name="center">Determines whether the image should be centered.</param>
        public virtual void SetImage(String textureName, bool center = false)
        {
            if (String.IsNullOrEmpty(textureName))
            {
                if (m_Image != null)
                    m_Image.Dispose();
                m_Image = null;
                return;
            }

            if (m_Image == null)
            {
                m_Image = new ImagePanel(this);
            }

            m_Image.ImageName = textureName;
            m_Image.SizeToContents( );
            m_Image.SetPos(Math.Max(m_Padding.Left, 2), 2);
            m_CenterImage = center;

            m_TextPadding.Left = m_Image.Right + 2;
        }

        /// <summary>
        /// Sizes to contents.
        /// </summary>
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

        /// <summary>
        /// Handler for Space keyboard event.
        /// </summary>
        /// <param name="down">Indicates whether the key was pressed or released.</param>
        /// <returns>
        /// True if handled.
        /// </returns>
        protected override bool onKeySpace(bool down)
        {
            if (down)
                onPress();
            return true;
        }

        /// <summary>
        /// Default accelerator handler.
        /// </summary>
        protected override void AcceleratePressed()
        {
            onPress();
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);
            if (m_Image != null)
            {
                Align.CenterVertically(m_Image);

                if (m_CenterImage)
                    Align.CenterHorizontally(m_Image);
            }
        }

        /// <summary>
        /// Updates control colors.
        /// </summary>
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

        /// <summary>
        /// Handler invoked on mouse double click (left) event.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        protected override void onMouseDoubleClickLeft(int x, int y)
        {
            onMouseClickLeft(x, y, true);
            if (OnDoubleClickLeft != null)
                OnDoubleClickLeft.Invoke(this);
        }
    }
}
