using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class ImagePanel : GUnit
    {
        public ImagePanel(Base parent)
            : base(parent)
        {
            // Normal
            {
                Controls.ImagePanel img = new Controls.ImagePanel(this);
                img.ImageName = "gwen.png";
                img.SetBounds(10, 10, 100, 100);
            }

            // Missing
            {
                Controls.ImagePanel img = new Controls.ImagePanel(this);
                img.ImageName = "missingimage.png";
                img.SetBounds(120, 10, 100, 100);
            }
        }
    }
}
