using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class RadioButton : GUnit
    {
        public RadioButton(Base parent)
            : base(parent)
        {
            Control.RadioButtonGroup rbc = new Control.RadioButtonGroup(this);

            rbc.AddOption("Option 1");
            rbc.AddOption("Option 2");
            rbc.AddOption("Option 3");
            rbc.AddOption("\u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631");

            rbc.SetBounds(30, 30, 200, 200);

            rbc.SelectionChanged += OnChange;
        }

        void OnChange(Base control)
        {
            RadioButtonGroup rbc = control as RadioButtonGroup;
            LabeledRadioButton rb = rbc.Selected;
            UnitPrint(String.Format("RadioButton: SelectionChanged: {0}", rb.Text));
        }
    }
}
