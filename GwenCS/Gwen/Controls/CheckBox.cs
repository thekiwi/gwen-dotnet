using System;

namespace Gwen.Controls
{
    /// <summary>
    /// CheckBox control.
    /// </summary>
    public class CheckBox : Button
    {
        private bool m_Checked;

        /// <summary>
        /// Indicates whether the checkbox is checked.
        /// </summary>
        public bool IsChecked
        {
            get { return m_Checked; } 
            set
            {
                if (m_Checked == value) return;
                m_Checked = value;
                onCheckChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public CheckBox(Base parent) : base(parent)
        {
            SetSize(15, 15);
            m_Checked = true; // [omeg] why?!
            Toggle();
        }

        /// <summary>
        /// Toggles the button.
        /// </summary>
        public override void Toggle()
        {
            base.Toggle();
            IsChecked = !IsChecked;
        }

        /// <summary>
        /// Invoked when the checkbox is checked.
        /// </summary>
        public event ControlCallback OnChecked;

        /// <summary>
        /// Invoked when the checkbox is unchecked.
        /// </summary>
        public event ControlCallback OnUnChecked;

        /// <summary>
        /// Invoked when the checkbox state changes.
        /// </summary>
        public event ControlCallback OnCheckChanged;

        /// <summary>
        /// Determines whether unchecking is allowed.
        /// </summary>
        protected virtual bool AllowUncheck { get { return true; } }

        /// <summary>
        /// Internal handler for OnCheckChanged event.
        /// </summary>
        internal void onCheckChanged()
        {
            if (IsChecked)
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
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);
            skin.DrawCheckBox(this, m_Checked, IsDepressed);
        }

        /// <summary>
        /// Internal OnPress implementation.
        /// </summary>
        internal override void onPress()
        {
            if (IsDisabled)
                return;
            
            if (IsChecked && !AllowUncheck)
            {
                return;
            }

            Toggle();
        }
    }
}
