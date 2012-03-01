using System;
using System.Linq;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
    public class ListBox : GUnit
    {
        public ListBox(Base parent)
            : base(parent)
        {
            {
                Control.ListBox ctrl = new Control.ListBox(this);
                ctrl.SetBounds(10, 10, 100, 200);

                ctrl.AddRow("First");
                ctrl.AddRow("Blue");
                ctrl.AddRow("Yellow");
                ctrl.AddRow("Orange");
                ctrl.AddRow("Brown");
                ctrl.AddRow("Black");
                ctrl.AddRow("Green");
                ctrl.AddRow("Dog");
                ctrl.AddRow("Cat Blue");
                ctrl.AddRow("Shoes");
                ctrl.AddRow("Shirts");
                ctrl.AddRow("Chair");
                ctrl.AddRow("Last");

                ctrl.AllowMultiSelect = true;
                ctrl.SelectRowsByRegex("Bl.e|Dog");

                ctrl.RowSelected += RowSelected;
                ctrl.RowUnselected += RowUnSelected;
            }

            {
                Control.ListBox ctrl = new Control.ListBox(this);
                ctrl.SetBounds(120, 10, 200, 200);
                ctrl.ColumnCount = 3;
                //ctrl.AllowMultiSelect = true;
                ctrl.RowSelected += RowSelected;
                ctrl.RowUnselected += RowUnSelected;

                {
                    TableRow row = ctrl.AddRow("Baked Beans");
                    row.SetCellText(1, "Heinz");
                    row.SetCellText(2, "£3.50");
                }

                {
                    TableRow row = ctrl.AddRow("Bananas");
                    row.SetCellText(1, "Trees");
                    row.SetCellText(2, "£1.27");
                }

                {
                    TableRow row = ctrl.AddRow("Chicken");
                    row.SetCellText(1, "\u5355\u5143\u6D4B\u8BD5");
                    row.SetCellText(2, "£8.95");
                }
            }
        }

        void RowSelected(Base control)
        {
            Control.ListBox list = control as Control.ListBox;
            UnitPrint(String.Format("ListBox: RowSelected: {0} [{1}]", list.SelectedRows.Last().Text, list[list.SelectedRowIndex].Text));
        }

        void RowUnSelected(Base control)
        {
            // todo: how to determine which one was unselected (store somewhere)
            // or pass row as the event param?
            Control.ListBox list = control as Control.ListBox;
            UnitPrint(String.Format("ListBox: OnRowUnselected"));
        }
    }
}
