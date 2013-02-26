using System;
using System.Linq;
using Gwen.Controls.Internal;
using Gwen.Drawing;

namespace Gwen.Controls
{
    /// <summary>
    /// Tree control node.
    /// </summary>
    public class TreeNode : Control
    {
        public const int TreeIndentation = 14;

        protected Button m_ToggleButton;
        protected Button m_Title;
        private bool m_Selected;

        /// <summary>
        /// Indicates whether this is a root node.
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// Parent tree control.
        /// </summary>
        public TreeControl TreeControl { get; set; }

        /// <summary>
        /// Determines whether the node is selectable.
        /// </summary>
        public bool IsSelectable { get; set; }

        /// <summary>
        /// Indicates whether the node is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return m_Selected; }
            set
            {
                if (!IsSelectable)
                    return;
                if (IsSelected == value)
                    return;

                m_Selected = value;

                if (m_Title != null)
                    m_Title.ToggleState = value;

                if (SelectionChanged != null)
                    SelectionChanged.Invoke(this);

                // propagate to root parent (tree)
                if (TreeControl != null && TreeControl.SelectionChanged != null)
                    TreeControl.SelectionChanged.Invoke(this);

                if (value)
                {
                    if (Selected != null)
                        Selected.Invoke(this);

                    if (TreeControl != null && TreeControl.Selected != null)
                        TreeControl.Selected.Invoke(this);
                }
                else
                {
                    if (Unselected != null)
                        Unselected.Invoke(this);

                    if (TreeControl != null && TreeControl.Unselected != null)
                        TreeControl.Unselected.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Node's label.
        /// </summary>
        public String Text { get { return m_Title.Text; } set { m_Title.Text = value; } }

        /// <summary>
        /// Invoked when the node label has been pressed.
        /// </summary>
        public event GwenEventHandler LabelPressed;

        /// <summary>
        /// Invoked when the node's selected state has changed.
        /// </summary>
        public event GwenEventHandler SelectionChanged;

        /// <summary>
        /// Invoked when the node has been selected.
        /// </summary>
        public event GwenEventHandler Selected;

        /// <summary>
        /// Invoked when the node has been unselected.
        /// </summary>
        public event GwenEventHandler Unselected;

        /// <summary>
        /// Invoked when the node has been expanded.
        /// </summary>
        public event GwenEventHandler Expanded;

        /// <summary>
        /// Invoked when the node has been collapsed.
        /// </summary>
        public event GwenEventHandler Collapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TreeNode(Control parent)
            : base(parent)
        {
            m_ToggleButton = new TreeToggleButton(this);
            m_ToggleButton.SetBounds(0, 0, 15, 15);
            m_ToggleButton.Toggled += OnToggleButtonPress;

            m_Title = new TreeNodeLabel(this);
            m_Title.Dock = Pos.Top;
            m_Title.Margin = new Margin(16, 0, 0, 0);
            m_Title.DoubleClickedLeft += OnDoubleClickName;
            m_Title.Clicked += OnClickName;

            m_InnerPanel = new Control(this);
            m_InnerPanel.Dock = Pos.Top;
            m_InnerPanel.Height = 100;
            m_InnerPanel.Margin = new Margin(TreeIndentation, 1, 0, 0);
            m_InnerPanel.Hide();

            IsRoot = false;
            m_Selected = false;
            IsSelectable = true;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin skin)
        {
            int bottom = 0;
            if (m_InnerPanel.Children.Count > 0)
            {
                bottom = m_InnerPanel.Children.Last().Y + m_InnerPanel.Y;
            }

            skin.DrawTreeNode(this, m_InnerPanel.IsVisible, IsSelected, m_Title.Height, m_Title.TextRight,
                (int)(m_ToggleButton.Y + m_ToggleButton.Height * 0.5f), bottom, TreeControl == Parent); // IsRoot
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin skin)
        {
            if (m_ToggleButton != null)
            {
                if (m_Title != null)
                {
                    m_ToggleButton.SetPosition(0, (m_Title.Height - m_ToggleButton.Height)*0.5f);
                }

                if (m_InnerPanel.Children.Count == 0)
                {
                    m_ToggleButton.Hide();
                    m_ToggleButton.ToggleState = false;
                    m_InnerPanel.Hide();
                }
                else
                {
                    m_ToggleButton.Show();
                    m_InnerPanel.SizeToChildren(false, true);
                }
            }

            base.Layout(skin);
        }

        /// <summary>
        /// Function invoked after layout.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void PostLayout(Skin skin)
        {
            if (SizeToChildren(false, true))
            {
                InvalidateParent();
            }
        }

        /// <summary>
        /// Adds a new child node.
        /// </summary>
        /// <param name="label">Node's label.</param>
        /// <returns>Newly created control.</returns>
        public TreeNode AddNode(string label)
        {
            TreeNode node = new TreeNode(this);
            node.Text = label;
            node.Dock = Pos.Top;
            node.IsRoot = this is TreeControl;
            node.TreeControl = TreeControl;

            if (TreeControl != null)
            {
                TreeControl.OnNodeAdded(node);
            }

            return node;
        }

        /// <summary>
        /// Opens the node.
        /// </summary>
        public void Open()
        {
            m_InnerPanel.Show();
            if (m_ToggleButton != null)
                m_ToggleButton.ToggleState = true;

            if (Expanded != null)
                Expanded.Invoke(this);
            if (TreeControl != null && TreeControl.Expanded != null)
                TreeControl.Expanded.Invoke(this);

            Invalidate();
        }

        /// <summary>
        /// Closes the node.
        /// </summary>
        public void Close()
        {
            m_InnerPanel.Hide();
            if (m_ToggleButton != null)
                m_ToggleButton.ToggleState = false;

            if (Collapsed != null)
                Collapsed.Invoke(this);
            if (TreeControl != null && TreeControl.Collapsed != null)
                TreeControl.Collapsed.Invoke(this);

            Invalidate();
        }

        /// <summary>
        /// Opens the node and all child nodes.
        /// </summary>
        public void ExpandAll()
        {
            Open();
            foreach (Control child in Children)
            {
                TreeNode node = child as TreeNode;
                if (node == null)
                    continue;
                node.ExpandAll();
            }
        }

        /// <summary>
        /// Clears the selection on the node and all child nodes.
        /// </summary>
        public void UnselectAll()
        {
            IsSelected = false;
            if (m_Title != null)
                m_Title.ToggleState = false;

            foreach (Control child in Children)
            {
                TreeNode node = child as TreeNode;
                if (node == null)
                    continue;
                node.UnselectAll();
            }
        }

        /// <summary>
        /// Handler for the toggle button.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void OnToggleButtonPress(Control control)
        {
            if (m_ToggleButton.ToggleState)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Handler for label double click.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void OnDoubleClickName(Control control)
        {
            if (!m_ToggleButton.IsVisible)
                return;
            m_ToggleButton.Toggle();
        }

        /// <summary>
        /// Handler for label click.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void OnClickName(Control control)
        {
            if (LabelPressed != null)
                LabelPressed.Invoke(this);
            IsSelected = !IsSelected;
        }
    }
}
