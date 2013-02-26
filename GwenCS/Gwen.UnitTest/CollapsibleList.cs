using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class CollapsibleList : GUnit
    {
        public CollapsibleList(Control parent)
            : base(parent)
        {
            Controls.CollapsibleList control = new Controls.CollapsibleList(this);
            control.SetSize(100, 200);
            control.SetPosition(10, 10);
            control.ItemSelected += OnSelection;
            control.CategoryCollapsed += OnCollapsed;

            {
                CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }

            {
                CollapsibleCategory cat = control.Add("Shopping");
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
                CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }
        }

        void OnSelection(Control control)
        {
            Controls.CollapsibleList list = control as Controls.CollapsibleList;
            UnitPrint(String.Format("CollapsibleList: Selected: {0}", list.GetSelectedButton().Text));
        }

        void OnCollapsed(Control control)
        {
            CollapsibleCategory cat = control as CollapsibleCategory;
            UnitPrint(String.Format("CollapsibleCategory: CategoryCollapsed: {0} {1}", cat.Text, cat.IsCollapsed));
        }
    }
}
