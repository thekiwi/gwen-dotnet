using System;
using Gwen.Control;
using Gwen.Skin;

namespace Gwen.ControlInternal
{
    /// <summary>
    /// Submenu indicator.
    /// </summary>
    public class RightArrow : ControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightArrow"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public RightArrow(ControlBase parent)
            : base(parent)
        {
            MouseInputEnabled = false;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(SkinBase skin)
        {
            skin.DrawMenuRightArrow(this);
        }
    }
}
