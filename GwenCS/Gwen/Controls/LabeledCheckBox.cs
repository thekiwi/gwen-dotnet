using System;

namespace Gwen.Controls
{
    /// <summary>
    /// CheckBox with label.
    /// </summary>
    public class LabeledCheckBox : Base
    {
        private readonly CheckBox m_CheckBox;
        private readonly LabelClickable m_Label;

        /// <summary>
        /// Invoked when the control is checked.
        /// </summary>
        public event ControlCallback OnChecked;

        /// <summary>
        /// Invoked when the control is unchecked.
        /// </summary>
        public event ControlCallback OnUnChecked;

        /// <summary>
        /// Invoked when the control's check is changed.
        /// </summary>
        public event ControlCallback OnCheckChanged;

        /// <summary>
        /// Indicates whether the control is checked.
        /// </summary>
        public bool IsChecked { get { return m_CheckBox.IsChecked; } set { m_CheckBox.IsChecked = value; } }

        /// <summary>
        /// Label text.
        /// </summary>
        public String Text { get { return m_Label.Text; } set { m_Label.Text = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledCheckBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public LabeledCheckBox(Base parent) : base(parent)
        {
            SetSize(200, 19);
            m_CheckBox = new CheckBox(this);
            m_CheckBox.Dock = Pos.Left;
            m_CheckBox.Margin = new Margin(0, 2, 2, 2);
            m_CheckBox.IsTabable = false;
            m_CheckBox.OnCheckChanged += onCheckChanged;

            m_Label = new LabelClickable(this);
            m_Label.Dock = Pos.Fill;
            m_Label.OnPress += m_CheckBox.Press;
            m_Label.IsTabable = false;

            IsTabable = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_CheckBox.Dispose();
            m_Label.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Handler for OnCheckChanged event.
        /// </summary>
        protected virtual void onCheckChanged(Base control)
        {
            if (m_CheckBox.IsChecked)
            {
                if (OnChecked != null)
                    OnChecked.Invoke(this);
            }
            else
            {
                if (OnUnChecked != null)
                    OnUnChecked.Invoke(this);
            }

            if (OnCheckChanged != null)
                OnCheckChanged.Invoke(this);
        }

        /// <summary>
        /// Handler for Space keyboard event.
        /// </summary>
        /// <param name="down">Indicates whether the key was pressed or released.</param>
        /// <returns>
        /// True if handled.
        /// </returns>
        protected override bool onKeySpace(bool down)
        {
            base.onKeySpace(down);
            if (!down) 
                m_CheckBox.IsChecked = !m_CheckBox.IsChecked; 
            return true;
        }
    }
}
