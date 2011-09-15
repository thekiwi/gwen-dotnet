using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class TreeControl : GUnit
    {
        public TreeControl(Base parent)
            : base(parent)
        {
            {
                Control.TreeControl ctrl = new Control.TreeControl(this);

                ctrl.AddNode("Node One");
                Control.TreeNode node = ctrl.AddNode("Node Two");
                node.AddNode("Node Two Inside");
                node.AddNode("Eyes");
                node.AddNode("Brown").AddNode("Node Two Inside").AddNode("Eyes").AddNode("Brown");
                node.AddNode("More");
                node.AddNode("Nodes");
                ctrl.AddNode("Node Three");

                ctrl.SetBounds(30, 30, 200, 200);
                ctrl.ExpandAll();

                ctrl.OnSelect += NodeSelected;
                ctrl.OnExpanded += NodeExpanded;
                ctrl.OnCollapsed += NodeCollapsed;
            }

            {
                Control.TreeControl ctrl = new Control.TreeControl(this);

                ctrl.AllowMultiSelect = true;

                ctrl.AddNode("Node One");
                Control.TreeNode node = ctrl.AddNode("Node Two");
                node.AddNode("Node Two Inside");
                node.AddNode("Eyes");
                Control.TreeNode nodeTwo = node.AddNode("Brown").AddNode("Node Two Inside").AddNode("Eyes");
                nodeTwo.AddNode("Brown");
                nodeTwo.AddNode("Green");
                nodeTwo.AddNode("Slime");
                nodeTwo.AddNode("Grass");
                nodeTwo.AddNode("Pipe");
                node.AddNode("More");
                node.AddNode("Nodes");

                ctrl.AddNode("Node Three");

                ctrl.SetBounds(240, 30, 200, 200);
                ctrl.ExpandAll();

                ctrl.OnSelect += NodeSelected;
                ctrl.OnExpanded += NodeExpanded;
                ctrl.OnCollapsed += NodeCollapsed;
            }
        }

        void NodeCollapsed(Base control)
        {
            TreeNode node = control as TreeNode;
            UnitPrint(String.Format("Node collapsed: {0}", node.Text));
        }

        void NodeExpanded(Base control)
        {
            TreeNode node = control as TreeNode;
            UnitPrint(String.Format("Node expanded: {0}", node.Text));
        }

        void NodeSelected(Base control)
        {
            TreeNode node = control as TreeNode;
            UnitPrint(String.Format("Node selected: {0}", node.Text));
        }
    }
}
