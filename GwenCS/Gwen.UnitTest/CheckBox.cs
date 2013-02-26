using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class CheckBox : GUnit
    {
        public CheckBox(Control parent)
            : base(parent)
        {

            Controls.CheckBox check = new Controls.CheckBox(this);
            check.SetPosition(10, 10);
            check.Checked += OnChecked;
            check.UnChecked += OnUnchecked;
            check.CheckChanged += OnCheckChanged;

            LabeledCheckBox labeled = new LabeledCheckBox(this);
            labeled.Text = "Labeled CheckBox";
            labeled.Checked += OnChecked;
            labeled.UnChecked += OnUnchecked;
            labeled.CheckChanged += OnCheckChanged;
            Align.PlaceDownLeft(labeled, check, 10);

            LabeledCheckBox labeled2 = new LabeledCheckBox(this);
            labeled2.Text = "I'm autosized";
            labeled2.SizeToChildren();
            Align.PlaceDownLeft(labeled2, labeled, 10);

            Controls.CheckBox check2 = new Controls.CheckBox(this);
            check2.IsDisabled = true;
            Align.PlaceDownLeft(check2, labeled2, 20);
        }

        void OnChecked(Control control)
        {
            UnitPrint("CheckBox: Checked");
        }

        void OnCheckChanged(Control control)
        {
            UnitPrint("CheckBox: CheckChanged");
        }

        void OnUnchecked(Control control)
        {
            UnitPrint("CheckBox: UnChecked");
        }
    }
}
