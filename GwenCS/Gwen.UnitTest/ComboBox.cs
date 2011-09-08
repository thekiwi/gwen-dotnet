using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class ComboBox : GUnit
    {
        public ComboBox(Base parent)
            : base(parent)
        {
            {
                Controls.ComboBox combo = new Controls.ComboBox(this);
                combo.SetPos(50, 50);
                combo.Width = 200;

                combo.AddItem("Option One", "one");
                combo.AddItem("Number Two", "two");
                combo.AddItem("Door Three", "three");
                combo.AddItem("Four Legs", "four");
                combo.AddItem("Five Birds", "five");

                combo.OnSelection += OnComboSelect;
            }

            {
                // Empty..
                Controls.ComboBox combo = new Controls.ComboBox(this);
                combo.SetPos(50, 80);
                combo.Width = 200;
            }

            {
                // Empty..
                Controls.ComboBox combo = new Controls.ComboBox(this);
                combo.SetPos(50, 110);
                combo.Width = 200;

                for (int i = 0; i < 500; i++)
                    combo.AddItem(String.Format("Option {0}", i));

                combo.OnSelection += OnComboSelect;
            }
        }

        void OnComboSelect(Base control)
        {
            Controls.ComboBox combo = control as Controls.ComboBox;
            UnitPrint(String.Format("ComboBox: OnComboSelect: {0}", combo.SelectedItem.Text));
        }
    }
}
