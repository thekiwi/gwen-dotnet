using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class Slider : GUnit
    {
        public Slider(Base parent)
            : base(parent)
        {
            {
                Control.HorizontalSlider slider = new Control.HorizontalSlider(this);
                slider.SetPos(10, 10);
                slider.SetSize(150, 20);
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.OnValueChanged += SliderMoved;
            }

            {
                Control.HorizontalSlider slider = new Control.HorizontalSlider(this);
                slider.SetPos(10, 40);
                slider.SetSize(150, 20);
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.ClampToNotches = true;
                slider.OnValueChanged += SliderMoved;
            }

            {
                Control.VerticalSlider slider = new Control.VerticalSlider(this);
                slider.SetPos(160, 10);
                slider.SetSize(20, 200);
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.OnValueChanged += SliderMoved;
            }

            {
                Control.VerticalSlider slider = new Control.VerticalSlider(this);
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
            Control.Slider slider = control as Control.Slider;
            UnitPrint(String.Format("Slider moved: OnValueChanged: {0}", slider.Value));
        }
    }
}
