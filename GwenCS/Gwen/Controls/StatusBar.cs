using Gwen.Drawing;

namespace Gwen.Controls
{
    /// <summary>
    /// Status bar.
    /// </summary>
    public class StatusBar : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public StatusBar(Control parent)
            : base(parent)
        {
            Height = 22;
            Dock = Pos.Bottom;
            Padding = Padding.Two;
            //Text = "Status Bar"; // [omeg] todo i18n
            Alignment = Pos.Left | Pos.CenterV;
        }

        /// <summary>
        /// Adds a control to the bar.
        /// </summary>
        /// <param name="control">Control to add.</param>
        /// <param name="right">Determines whether the control should be added to the right side of the bar.</param>
        public void AddControl(Control control, bool right)
        {
            control.Parent = this;
            control.Dock = right ? Pos.Right : Pos.Left;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin skin)
        {
            skin.DrawStatusBar(this);
        }
    }
}