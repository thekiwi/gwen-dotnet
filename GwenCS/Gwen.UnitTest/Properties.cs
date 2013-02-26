using System;
using Gwen.Controls;
using Gwen.Controls.Properties;

namespace Gwen.UnitTest
{
    public class Properties : GUnit
    {
        public Properties(Control parent)
            : base(parent)
        {
            {
                var props = new PropertyTable(this);
                props.ValueChanged += OnChanged;

                props.SetBounds(10, 10, 150, 300);
                
                props.Add("First Name");
                props.Add("Middle Name");
                props.Add("Last Name");                
            }

            {
                PropertyTree ptree = new PropertyTree(this);
                ptree.SetBounds(200, 10, 200, 200);

                {
                    var props = ptree.Add("Item One");
                    props.ValueChanged += OnChanged;

                    props.Add("Middle Name");
                    props.Add("Last Name");
                    props.Add("Four");
                }

                {
                    PropertyTable props = ptree.Add("Item Two");
                    props.ValueChanged += OnChanged;
                    
                    props.Add("More Items");
                    props.Add("Bacon", new CheckProperty(props), "1");
                    props.Add("To Fill");
                    props.Add("Colour", new ColorProperty(props), "255 0 0");
                    props.Add("Out Here");
                }

                ptree.ExpandAll();
            }
        }

        void OnChanged(Control control)
        {
            PropertyRow row = control as PropertyRow;
            UnitPrint(String.Format("Property changed: {0}", row.Value));
        }
    }
}
