using System;
using Gwen.Controls.Internal;

namespace Gwen.Controls
{
    /// <summary>
    /// Property table/tree.
    /// </summary>
    public class PropertyTree : TreeControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTree"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public PropertyTree(Control parent)
            : base(parent)
        {

        }

        /// <summary>
        /// Adds a new properties node.
        /// </summary>
        /// <param name="label">Node label.</param>
        /// <returns>Newly created control</returns>
        public PropertyTable Add(String label)
        {
            TreeNode node = new PropertyTreeNode(this);
            node.Text = label;
            node.Dock = Pos.Top;

            PropertyTable props = new PropertyTable(node);
            props.Dock = Pos.Top;

            return props;
        }
    }
}
