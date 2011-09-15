using System;

namespace Gwen.Control
{
    /// <summary>
    /// Group box (container).
    /// </summary>
    public class GroupBox : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public GroupBox(Base parent) : base(parent)
        {
            // Set to true, because it's likely that our  
            // children will want mouse input, and they
            // can't get it without us..
            MouseInputEnabled = true;
            KeyboardInputEnabled = true;

            TextPadding = new Padding(10, 0, 0, 0);
            Alignment = Pos.Top | Pos.Left;
            Invalidate();

            m_InnerPanel = new Base(this);
            m_InnerPanel.Dock = Pos.Fill;
            m_InnerPanel.Padding = new Padding(0, 10, 0, 0); // [omeg] to prevent overlapping on label
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            m_InnerPanel.Margin = new Margin(TextHeight + 3, 6, 6, 6);
            base.Layout(skin);
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawGroupBox(this, TextX, TextHeight, TextWidth);
        }
    }
}
