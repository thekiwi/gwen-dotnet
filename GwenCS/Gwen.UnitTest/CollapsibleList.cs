using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class CollapsibleList : GUnit
    {
        public CollapsibleList(Base parent)
            : base(parent)
        {
            Control.CollapsibleList control = new Control.CollapsibleList(this);
            control.SetSize(100, 200);
            control.SetPos(10, 10);
            control.OnSelection += OnSelection;
            control.OnCollapsed += OnCollapsed;

            {
                Control.CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }

            {
                Control.CollapsibleCategory cat = control.Add("Shopping");
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
                Control.CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }
        }

        void OnSelection(Base control)
        {
            Control.CollapsibleList list = control as Control.CollapsibleList;
            UnitPrint(String.Format("CollapsibleList: Selected: {0}", list.Selected.Text));
        }

        void OnCollapsed(Base control)
        {
            Control.CollapsibleCategory cat = control as Control.CollapsibleCategory;
            UnitPrint(String.Format("CollapsibleCategory: Collapsed: {0} {1}", cat.Text, cat.IsCollapsed));
        }
    }
}
