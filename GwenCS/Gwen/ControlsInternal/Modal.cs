using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class Modal : Base
    {
        public Modal(Base parent) : base(parent)
        {
            KeyboardInputEnabled = true;
            MouseInputEnabled = true;
            ShouldDrawBackground = true;
        }

        protected override void Layout(Skin.Base skin)
        {
            SetBounds(0, 0, GetCanvas().Width, GetCanvas().Height);
        }

        protected override void Render(Skin.Base skin)
        {
            if (!ShouldDrawBackground)
                return;
            skin.DrawModalControl(this);
        }
    }
}
