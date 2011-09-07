using System;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class CategoryHeaderButton : Button
    {
        public CategoryHeaderButton(Base parent) : base(parent)
        {
            ShouldDrawBackground = false;
            IsToggle = true;
            Alignment = Pos.Center;
        }

        public override void UpdateColors()
        {
            if (IsDepressed || ToggleState)
                TextColor = Skin.Colors.Category.Header_Closed;
            else
                TextColor = Skin.Colors.Category.Header;
        }
    }
}
