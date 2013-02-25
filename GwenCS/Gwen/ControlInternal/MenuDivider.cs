using System;
using Gwen.Control;
using Gwen.Skin;

namespace Gwen.ControlInternal
{
    /// <summary>
    /// Divider menu item.
    /// </summary>
    public class MenuDivider : ControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuDivider"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public MenuDivider(ControlBase parent)
            : base(parent)
        {
            Height = 1;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(SkinBase skin)
        {
            skin.DrawMenuDivider(this);
        }
    }
}
