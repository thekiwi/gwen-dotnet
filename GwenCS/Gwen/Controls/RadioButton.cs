using System;

namespace Gwen.Controls
{
    public class RadioButton : CheckBox
    {
        protected override bool AllowUncheck
        {
            get { return false; }
        }

        public RadioButton(Base parent) : base(parent)
        {
            SetSize(15, 15);
            MouseInputEnabled = true;
            IsTabable = false;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawRadioButton(this, IsChecked, IsDepressed);
        }
    }
}
