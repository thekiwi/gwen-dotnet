using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class Slider : GUnit
    {
        public Slider(Base parent)
            : base(parent)
        {
            {
                Controls.HorizontalSlider slider = new Controls.HorizontalSlider(this);
                slider.SetPos(10, 10);
                slider.SetSize(150, 20);
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.OnValueChanged += SliderMoved;
            }

            {
                Controls.HorizontalSlider slider = new Controls.HorizontalSlider(this);
                slider.SetPos(10, 40);
                slider.SetSize(150, 20);
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.ClampToNotches = true;
                slider.OnValueChanged += SliderMoved;
            }

            {
                Controls.VerticalSlider slider = new Controls.VerticalSlider(this);
                slider.SetPos(160, 10);
                slider.SetSize(20, 200);
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.OnValueChanged += SliderMoved;
            }

            {
                Controls.VerticalSlider slider = new Controls.VerticalSlider(this);
                slider.SetPos(190, 10);
                slider.SetSize(20, 200);
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.ClampToNotches = true;
                slider.OnValueChanged += SliderMoved;
            }
        }

        void SliderMoved(Base control)
        {
            Controls.Slider slider = control as Controls.Slider;
            UnitPrint(String.Format("Slider moved: OnValueChanged: {0}", slider.Value));
        }
    }
}
