using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class RadioButton : GUnit
    {
        public RadioButton(Control parent)
            : base(parent)
        {
            RadioButtonGroup rbg = new RadioButtonGroup(this, "Sample radio group");
            rbg.SetPosition(10, 10);

            rbg.AddOption("Option 1");
            rbg.AddOption("Option 2");
            rbg.AddOption("Option 3");
            rbg.AddOption("\u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631");
            //rbg.SizeToContents(); // it's auto

            rbg.SelectionChanged += OnChange;

            LabeledRadioButton rb1 = new LabeledRadioButton(this);
            rb1.Text = "Option 1";
            rb1.SetPosition(300, 10);

            LabeledRadioButton rb2 = new LabeledRadioButton(this);
            rb2.Text = "Option 2222222222222222222222222222222222";
            rb2.SetPosition(300, 30);

            LabeledRadioButton rb3 = new LabeledRadioButton(this);
            rb3.Text = "\u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631";
            rb3.SetPosition(300, 50);

            //this.DrawDebugOutlines = true;
        }

        void OnChange(Control control)
        {
            RadioButtonGroup rbc = control as RadioButtonGroup;
            LabeledRadioButton rb = rbc.Selected;
            UnitPrint(String.Format("RadioButton: SelectionChanged: {0}", rb.Text));
        }
    }
}
