using System;

namespace Gwen.Controls
{
    /// <summary>
    /// Tree control.
    /// </summary>
    public class TreeControl : TreeNode
    {
        protected ScrollControl m_ScrollControl;
        protected bool m_MultiSelect;

        /// <summary>
        /// Determines if multiple nodes can be selected at the same time.
        /// </summary>
        public bool AllowMultiSelect { get { return m_MultiSelect; } set { m_MultiSelect = value; } }

        //public ScrollControl Scroller { get { return m_ScrollControl; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControl"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TreeControl(Base parent)
            : base(parent)
        {
            m_TreeControl = this;

            m_ToggleButton.Hide();
            m_ToggleButton.Dispose(); // base constructor runs first
            m_ToggleButton = null;

            m_Title.Hide();
            m_Title.Dispose();
            m_Title = null;

            m_InnerPanel.Hide();
            m_InnerPanel.Dispose();
            m_InnerPanel = null;

            m_MultiSelect = false;

            m_ScrollControl = new ScrollControl(this);
            m_ScrollControl.Dock = Pos.Fill;
            m_ScrollControl.EnableScroll(false, true);
            m_ScrollControl.AutoHideBars = true;
            m_ScrollControl.Margin = new Margin(1, 1, 1, 1);

            m_InnerPanel = m_ScrollControl;

            m_ScrollControl.SetInnerSize(1000, 1000); // todo: why such arbitrary numbers?
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            if (ShouldDrawBackground)
                skin.DrawTreeControl(this);
        }

        /// <summary>
        /// Handler invoked when control children's bounds change.
        /// </summary>
        /// <param name="oldChildBounds"></param>
        /// <param name="child"></param>
        protected override void onChildBoundsChanged(System.Drawing.Rectangle oldChildBounds, Base child)
        {
            if (m_ScrollControl != null)
                m_ScrollControl.UpdateScrollBars();
        }

        /// <summary>
        /// Removes all child nodes.
        /// </summary>
        public virtual void RemoveAll()
        {
            m_ScrollControl.RemoveAll();
        }

        public virtual void onNodeAdded(TreeNode node)
        {
            node.OnLabelPress += onNodeSelected;
        }

        protected virtual void onNodeSelected(Base Control)
        {
            if (!m_MultiSelect /*|| Input.Input.IsKeyDown(Key.Control)*/)
                UnselectAll();
        }
    }
}
