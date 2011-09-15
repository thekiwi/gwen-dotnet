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
                check.SetPos(10, 10);
                check.OnChecked += OnChecked;
                check.OnUnChecked += OnUnchecked;
                check.OnCheckChanged += OnCheckChanged;

                Control.LabeledCheckBox labeled = new Control.LabeledCheckBox(this);
                labeled.SetPos(10, 30);
                labeled.Text = "Labeled CheckBox";
                labeled.OnChecked += OnChecked;
                labeled.OnUnChecked += OnUnchecked;
                labeled.OnCheckChanged += OnCheckChanged;
            }

            {
                Control.CheckBox check = new Control.CheckBox(this);
                check.SetPos(10, 54);
                check.IsDisabled = true;
            }
        }

        void OnChecked(Base control)
        {
            UnitPrint("CheckBox: OnChecked");
        }

        void OnCheckChanged(Base control)
        {
            UnitPrint("CheckBox: OnCheckChanged");
        }

        void OnUnchecked(Base control)
        {
            UnitPrint("CheckBox: OnUnchecked");
        }
    }
}
