using System;
using System.Drawing;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class ColorPickers : GUnit
    {
        public ColorPickers(Base parent) : base(parent)
        {
            ColorPicker rgbPicker = new ColorPicker(this);
            rgbPicker.SetPos(10, 10);

            rgbPicker.OnColorChanged += ColorChanged;

            HSVColorPicker hsvPicker = new HSVColorPicker(this);
            hsvPicker.SetPos(300, 10);
            hsvPicker.OnColorChanged += ColorChanged;
        }

        void ColorChanged(Base control)
        {
            IColorPicker picker = control as IColorPicker;
            Color c = picker.SelectedColor;
            HSV hsv = c.ToHSV();
            String text = String.Format("Color changed: RGB: {0:X2}{1:X2}{2:X2} HSV: {3:F1} {4:F2} {5:F2}",
                                        c.R, c.G, c.B, hsv.h, hsv.s, hsv.v);
            UnitPrint(text);
        }
    }
}
