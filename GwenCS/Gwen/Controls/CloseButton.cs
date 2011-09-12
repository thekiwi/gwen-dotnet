using System;

namespace Gwen.Controls
{
    /// <summary>
    /// Window close button.
    /// </summary>
    public class CloseButton : Button
    {
        private WindowControl m_Window; // [omeg] not ours, no disposing

        /// <summary>
        /// Window that owns this control.
        /// </summary>
        public WindowControl Window { set { m_Window = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseButton"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public CloseButton(Base parent) : base(parent)
        {
            
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawWindowCloseButton(this, IsDepressed && IsHovered, IsHovered && ShouldDrawHover, !m_Window.IsOnTop);
        }
    }
}
