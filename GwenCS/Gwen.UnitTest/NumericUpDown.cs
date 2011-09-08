using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class NumericUpDown : GUnit
    {
        public NumericUpDown(Base parent)
            : base(parent)
        {
            Controls.NumericUpDown ctrl = new Controls.NumericUpDown(this);
            ctrl.SetBounds(10, 10, 50, 20);
            ctrl.Value = 50;
            ctrl.Max = 100;
            ctrl.Min = -100;
            ctrl.OnValueChanged += OnValueChanged;
        }

        void OnValueChanged(Base control)
        {
            UnitPrint(String.Format("NumericUpDown: OnValueChanged: {0}", ((Controls.NumericUpDown)control).Value));
        }
    }
}
