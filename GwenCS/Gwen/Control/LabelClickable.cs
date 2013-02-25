using System;
using Gwen.Skin;

namespace Gwen.Control
{
    /// <summary>
    /// Clickable label (for checkboxes etc).
    /// </summary>
    public class LabelClickable : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelClickable"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public LabelClickable(ControlBase parent)
            : base(parent)
        {
            IsToggle = false;
            Alignment = Pos.Left | Pos.CenterV;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(SkinBase skin)
        {
            // no button look
        }
    }
}
