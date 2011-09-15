using System;
using System.Linq;

namespace Gwen.Control
{
    /// <summary>
    /// Tree node toggle button (the little plus sign).
    /// </summary>
    public class TreeToggleButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeToggleButton"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TreeToggleButton(Base parent) : base(parent)
        {
            IsToggle = true;
            IsTabable = false;
        }

        /// <summary>
        /// Renders the focus overlay.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void RenderFocus(Skin.Base skin)
        {
            
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawTreeButton(this, ToggleState);
        }
    }

    /// <summary>
    /// Tree node label.
    /// </summary>
    public class TreeNodeText : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNodeText"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TreeNodeText(Base parent) : base(parent)
        {
            Alignment = Pos.Left | Pos.CenterV;
            ShouldDrawBackground = false;
            Height = 16;
        }

        /// <summary>
        /// Updates control colors.
        /// </summary>
        public override void UpdateColors()
        {
            if (IsDisabled)
            {
                TextColor = Skin.Colors.Button.Disabled;
                return;
            }

            if (IsDepressed || ToggleState)
            {
                TextColor = Skin.Colors.Tree.Selected;
                return;
            }

            if (IsHovered)
            {
                TextColor = Skin.Colors.Tree.Hover;
                return;
            }

            TextColor = Skin.Colors.Tree.Normal;
        }
    }

    /// <summary>
    /// Tree control node.
    /// </summary>
    public class TreeNode : Base
    {
        public const int TreeIndentation = 14;

        protected TreeControl m_TreeControl;
        protected Button m_ToggleButton;
        protected Button m_Title;
        protected bool m_Root;
        protected bool m_Selected;
        protected bool m_Selectable;

        /// <summary>
        /// Indicates whether this is a root node.
        /// </summary>
        public bool IsRoot { get { return m_Root; } set { m_Root = value; } }

        /// <summary>
        /// Parent tree control.
        /// </summary>
        public TreeControl TreeControl { get { return m_TreeControl; } set { m_TreeControl = value; } }

        /// <summary>
        /// Determines whether the node is selectable.
        /// </summary>
        public bool IsSelectable { get { return m_Selectable; } set { m_Selectable = value; } }

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

                if (OnSelectionChange != null)
                    OnSelectionChange.Invoke(this);

                // propagate to root parent (tree)
                if (m_TreeControl.OnSelectionChange != null)
                    m_TreeControl.OnSelectionChange.Invoke(this);


                if (value)
                {
                    if (OnSelect != null)
                        OnSelect.Invoke(this);

                    if (m_TreeControl.OnSelect != null)
                        m_TreeControl.OnSelect.Invoke(this);
                }
                else
                {
                    if (OnUnselect != null)
                        OnUnselect.Invoke(this);

                    if (m_TreeControl.OnUnselect != null)
                        m_TreeControl.OnUnselect.Invoke(this);
                }
            }
        }

        public String Text { get { return m_Title.Text; } set { m_Title.Text = value; } }

        /// <summary>
        /// Invoked when the node label has been pressed.
        /// </summary>
        public event ControlCallback OnLabelPress;

        /// <summary>
        /// Invoked when the node's selected state has changed.
        /// </summary>
        public event ControlCallback OnSelectionChange;

        /// <summary>
        /// Invoked when the node has been selected.
        /// </summary>
        public event ControlCallback OnSelect;

        /// <summary>
        /// Invoked when the node has been unselected.
        /// </summary>
        public event ControlCallback OnUnselect;

        /// <summary>
        /// Invoked when the node has been expanded.
        /// </summary>
        public event ControlCallback OnExpanded;

        /// <summary>
        /// Invoked when the node has been collapsed.
        /// </summary>
        public event ControlCallback OnCollapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TreeNode(Base parent)
            : base(parent)
        {
            m_ToggleButton = new TreeToggleButton(this);
            m_ToggleButton.SetBounds(0, 0, 15, 15);
            m_ToggleButton.OnToggle += onToggleButtonPress;

            m_Title = new TreeNodeText(this);
            m_Title.Dock = Pos.Top;
            m_Title.Margin = new Margin(16, 0, 0, 0);
            m_Title.OnDoubleClickLeft += onDoubleClickName;
            m_Title.OnDown += onClickName;

            m_InnerPanel = new Base(this);
            m_InnerPanel.Dock = Pos.Top;
            m_InnerPanel.Height = 100;
            m_InnerPanel.Margin = new Margin(TreeIndentation, 1, 0, 0);
            m_InnerPanel.Hide();

            m_Root = false;
            m_Selected = false;
            m_Selectable = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_ToggleButton.Dispose();
            m_Title.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            int bottom = 0;
            if (m_InnerPanel.ChildrenCount > 0)
            {
                bottom = m_InnerPanel.Children.Last().Y + m_InnerPanel.Y;
            }

            skin.DrawTreeNode(this, m_InnerPanel.IsVisible, IsSelected, m_Title.Height, m_Title.TextRight,
                (int)(m_ToggleButton.Y + m_ToggleButton.Height * 0.5f), bottom, m_TreeControl == Parent); // IsRoot
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            if (m_ToggleButton != null)
            {
                if (m_Title != null)
                {
                    m_ToggleButton.SetPos(0, (m_Title.Height - m_ToggleButton.Height)*0.5f);
                }

                if (m_InnerPanel.ChildrenCount == 0)
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
        protected override void PostLayout(Skin.Base skin)
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
            node.TreeControl = m_TreeControl;

            if (m_TreeControl != null)
            {
                m_TreeControl.onNodeAdded(node);
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

            if (OnExpanded != null)
                OnExpanded.Invoke(this);
            if (m_TreeControl.OnExpanded != null)
                m_TreeControl.OnExpanded.Invoke(this);

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

            if (OnCollapsed != null)
                OnCollapsed.Invoke(this);
            if (m_TreeControl.OnCollapsed != null)
                m_TreeControl.OnCollapsed.Invoke(this);

            Invalidate();
        }

        /// <summary>
        /// Opens the node and all child nodes.
        /// </summary>
        public void ExpandAll()
        {
            Open();
            foreach (Base child in Children)
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

            foreach (Base child in Children)
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
        protected virtual void onToggleButtonPress(Base control)
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
        protected virtual void onDoubleClickName(Base control)
        {
            if (!m_ToggleButton.IsVisible)
                return;
            m_ToggleButton.Toggle();
        }

        /// <summary>
        /// Handler for label click.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onClickName(Base control)
        {
            if (OnLabelPress != null)
                OnLabelPress.Invoke(this);
            IsSelected = !IsSelected;
        }
    }
}
