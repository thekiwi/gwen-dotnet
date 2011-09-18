using System;

namespace Gwen.Control
{
    /// <summary>
    /// Single property row.
    /// </summary>
    public class PropertyRow : Base
    {
        protected Label m_Label;
        protected Property.Base m_Property;
        protected bool m_LastEditing;
        protected bool m_LastHover;

        /// <summary>
        /// Invoked when the property value has changed.
        /// </summary>
        public event ControlCallback OnChange;

        /// <summary>
        /// Indicates whether the property value is being edited.
        /// </summary>
        public bool IsEditing { get { return m_Property != null && m_Property.IsEditing; } }

        /// <summary>
        /// Property value.
        /// </summary>
        public String Value { get { return m_Property.Value; } set { m_Property.Value = value; } }

        /// <summary>
        /// Indicates whether the control is hovered by mouse pointer.
        /// </summary>
        public override bool IsHovered
        {
            get
            {
                return base.IsHovered || (m_Property != null && m_Property.IsHovered);
            }
        }

        /// <summary>
        /// Property name.
        /// </summary>
        public String Label { get { return m_Label.Text; } set { m_Label.Text = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRow"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        /// <param name="prop">Property control associated with this row.</param>
        public PropertyRow(Base parent, Property.Base prop)
            : base(parent)
        {
            PropertyRowLabel label = new PropertyRowLabel(this);
            label.Dock = Pos.Left;
            label.Alignment = Pos.Left | Pos.Top;
            label.Margin = new Margin(2, 2, 0, 0);
            m_Label = label;

            m_Property = prop;
            m_Property.Parent = this;
            m_Property.Dock = Pos.Fill;
            m_Property.OnChange += onPropertyValueChanged;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_Label.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            /* SORRY */
            if (IsEditing != m_LastEditing)
            {
                onEditingChanged();
                m_LastEditing = IsEditing;
            }

            if (IsHovered != m_LastHover)
            {
                onHoverChanged();
                m_LastHover = IsHovered;
            }
            /* SORRY */

            skin.DrawPropertyRow(this, m_Label.Right, IsEditing, IsHovered | m_Property.IsHovered);
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            Properties parent = Parent as Properties;
            if (null == parent) return;

            m_Label.Width = parent.SplitWidth;

            if (m_Property != null)
            {
                Height = m_Property.Height;
            }
        }

        protected virtual void onPropertyValueChanged(Base control)
        {
            if (OnChange != null)
                OnChange.Invoke(this);
        }

        private void onEditingChanged()
        {
            m_Label.Redraw();
        }

        private void onHoverChanged()
        {
            m_Label.Redraw();
        }
    }
}
