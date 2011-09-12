using System;
using System.Linq;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class ListBox : GUnit
    {
        public ListBox(Base parent)
            : base(parent)
        {
            {
                Controls.ListBox ctrl = new Controls.ListBox(this);
                ctrl.SetBounds(10, 10, 100, 200);

                ctrl.AddItem("First");
                ctrl.AddItem("Blue");
                ctrl.AddItem("Yellow");
                ctrl.AddItem("Orange");
                ctrl.AddItem("Brown");
                ctrl.AddItem("Black");
                ctrl.AddItem("Green");
                ctrl.AddItem("Dog");
                ctrl.AddItem("Cat Blue");
                ctrl.AddItem("Shoes");
                ctrl.AddItem("Shirts");
                ctrl.AddItem("Chair");
                ctrl.AddItem("Last");

                ctrl.AllowMultiSelect = true;
                ctrl.SelectRowsByRegex("Bl.e|Dog");

                ctrl.OnRowSelected += RowSelected;
                ctrl.OnRowUnselected += RowUnSelected;
            }

            {
                Controls.ListBox ctrl = new Controls.ListBox(this);
                ctrl.SetBounds(120, 10, 200, 200);
                ctrl.ColumnCount = 3;
                ctrl.AllowMultiSelect = true;
                ctrl.OnRowSelected += RowSelected;
                ctrl.OnRowUnselected += RowUnSelected;

                {
                    Controls.Layout.TableRow pRow = ctrl.AddItem("Baked Beans");
                    pRow.SetCellText(1, "Heinz");
                    pRow.SetCellText(2, "£3.50");
                }

                {
                    Controls.Layout.TableRow pRow = ctrl.AddItem("Bananas");
                    pRow.SetCellText(1, "Trees");
                    pRow.SetCellText(2, "£1.27");
                }

                {
                    Controls.Layout.TableRow pRow = ctrl.AddItem("Chicken");
                    pRow.SetCellText(1, "\u5355\u5143\u6D4B\u8BD5");
                    pRow.SetCellText(2, "£8.95");
                }
            }
        }

        void RowSelected(Base control)
        {
            Controls.ListBox list = control as Controls.ListBox;
            UnitPrint(String.Format("ListBox: OnRowSelected: {0}", list.SelectedRows.Last().Text));
        }

        void RowUnSelected(Base control)
        {
            // todo: how to determine which one was unselected (store somewhere)
            // or pass row as the event param?
            Controls.ListBox list = control as Controls.ListBox;
            UnitPrint(String.Format("ListBox: OnRowUnselected"));
        }
    }
}
