using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class CollapsibleList : GUnit
    {
        public CollapsibleList(Base parent)
            : base(parent)
        {
            Controls.CollapsibleList control = new Controls.CollapsibleList(this);
            control.SetSize(100, 200);
            control.SetPos(10, 10);
            control.OnSelection += OnSelection;
            control.OnCollapsed += OnCollapsed;

            {
                Controls.CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }

            {
                Controls.CollapsibleCategory cat = control.Add("Shopping");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
            }

            {
                Controls.CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }
        }

        void OnSelection(Base control)
        {
            Controls.CollapsibleList list = control as Controls.CollapsibleList;
            UnitPrint(String.Format("CollapsibleList: Selected: {0}", list.Selected.Text));
        }

        void OnCollapsed(Base control)
        {
            Controls.CollapsibleCategory cat = control as Controls.CollapsibleCategory;
            UnitPrint(String.Format("CollapsibleCategory: Collapsed: {0} {1}", cat.Text, cat.IsCollapsed));
        }
    }
}
