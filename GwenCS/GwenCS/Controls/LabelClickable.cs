using System;

namespace Gwen.Controls
{
    public class LabelClickable : Button
    {
        public LabelClickable(Base parent) : base(parent)
        {
            IsToggle = false;
            Alignment = Pos.Left | Pos.CenterV;
        }
        
        protected override void Render(Skin.Base skin)
        {
            // [omeg] no button look
        }
        
    }
}
