using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class CheckBox : GUnit
    {
        public CheckBox(Base parent)
            : base(parent)
        {
            {
                Control.CheckBox check = new Control.CheckBox(this);
                check.SetPosition(10, 10);
                check.Checked += OnChecked;
                check.UnChecked += OnUnchecked;
                check.CheckChanged += OnCheckChanged;

                Control.LabeledCheckBox labeled = new Control.LabeledCheckBox(this);
                labeled.SetPosition(10, 30);
                labeled.Text = "Labeled CheckBox";
                labeled.Checked += OnChecked;
                labeled.UnChecked += OnUnchecked;
                labeled.CheckChanged += OnCheckChanged;
            }

            {
                Control.CheckBox check = new Control.CheckBox(this);
                check.SetPosition(10, 54);
                check.IsDisabled = true;
            }
        }

        void OnChecked(Base control)
        {
            UnitPrint("CheckBox: Checked");
        }

        void OnCheckChanged(Base control)
        {
            UnitPrint("CheckBox: CheckChanged");
        }

        void OnUnchecked(Base control)
        {
            UnitPrint("CheckBox: UnChecked");
        }
    }
}
