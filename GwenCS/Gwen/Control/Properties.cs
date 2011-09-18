using System;
using System.Windows.Forms;
using Gwen.ControlInternal;

namespace Gwen.Control
{
    public class Properties : Base
    {
        protected SplitterBar m_SplitterBar;

        /// <summary>
        /// Returns the width of the first column (property names).
        /// </summary>
        public int SplitWidth { get { return m_SplitterBar.X; } } // todo: rename?

        /// <summary>
        /// Invoked when a property value has been changed.
        /// </summary>
        public event ControlCallback OnChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="Properties"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public Properties(Base parent)
            : base(parent)
        {
            m_SplitterBar = new SplitterBar(this);
            m_SplitterBar.SetPos(80, 0);
            m_SplitterBar.Cursor = Cursors.SizeWE;
            m_SplitterBar.OnDragged += onSplitterMoved;
            m_SplitterBar.ShouldDrawBackground = false;
        }

        /// <summary>
        /// Function invoked after layout.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void PostLayout(Skin.Base skin)
        {
            m_SplitterBar.Height = 0;

            if (SizeToChildren(false, true))
            {
                InvalidateParent();
            }

            m_SplitterBar.SetSize(3, Height);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_SplitterBar.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Handles the splitter moved event.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onSplitterMoved(Base control)
        {
            InvalidateChildren();
        }

        /// <summary>
        /// Adds a new text property row.
        /// </summary>
        /// <param name="label">Property name.</param>
        /// <param name="value">Initial value.</param>
        /// <returns>Newly created row.</returns>
        public PropertyRow Add(String label, String value="")
        {
            return Add(label, new Property.Text(this), value);
        }

        /// <summary>
        /// Adds a new property row.
        /// </summary>
        /// <param name="label">Property name.</param>
        /// <param name="prop">Property control.</param>
        /// <param name="value">Initial value.</param>
        /// <returns>Newly created row.</returns>
        public PropertyRow Add(String label, Property.Base prop, String value="")
        {
            PropertyRow row = new PropertyRow(this, prop);
            row.Dock = Pos.Top;
            row.Label = label;
            row.OnChange += onRowValueChanged;

            prop.SetValue(value, true);

            m_SplitterBar.BringToFront();
            return row;
        }

        private void onRowValueChanged(Base control)
        {
            if (OnChange != null)
                OnChange.Invoke(control);
        }

        /// <summary>
        /// Deletes all rows.
        /// </summary>
        public void DeleteAll()
        {
            m_InnerPanel.DeleteAllChildren();
        }
    }
}
