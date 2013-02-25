using System;
using Gwen.Control;
using Gwen.Skin;

namespace Gwen.ControlInternal
{
    /// <summary>
    /// Inner panel of tab control.
    /// </summary>
    public class TabControlInner : ControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlInner"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        internal TabControlInner(ControlBase parent) : base(parent)
        {
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(SkinBase skin)
        {
            skin.DrawTabControl(this);
        }
    }
}
