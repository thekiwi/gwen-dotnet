using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class RadioButton : GUnit
    {
        public RadioButton(Base parent)
            : base(parent)
        {
            Control.RadioButtonController rbc = new Control.RadioButtonController(this);

            rbc.AddOption("Option 1");
            rbc.AddOption("Option 2");
            rbc.AddOption("Option 3");
            rbc.AddOption("\u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631");

            rbc.SetBounds(30, 30, 200, 200);

            rbc.OnSelectionChange += OnChange;
        }

        void OnChange(Base control)
        {
            RadioButtonController rbc = control as RadioButtonController;
            LabeledRadioButton rb = rbc.Selected;
            UnitPrint(String.Format("RadioButton: OnSelectionChange: {0}", rb.Text));
        }
    }
}
