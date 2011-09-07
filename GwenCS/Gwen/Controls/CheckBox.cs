using System;

namespace Gwen.Controls
{
    public class CheckBox : Button
    {
        private bool m_Checked;

        public bool IsChecked
        {
            get { return m_Checked; } 
            set
            {
                if (m_Checked == value) return;
                m_Checked = value;
                onCheckStatusChanged();
            }
        }

        public CheckBox(Base parent) : base(parent)
        {
            SetSize(15, 15);
            m_Checked = true; // [omeg] why?!
            Toggle();
        }
        
        public override void Toggle()
        {
            base.Toggle();
            IsChecked = !IsChecked;
        }

        public event ControlCallback OnChecked;
        public event ControlCallback OnUnChecked;
        public event ControlCallback OnCheckChanged;

        // For derived controls
        protected virtual bool AllowUncheck { get { return true; } }

        protected void onCheckStatusChanged()
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

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);
            skin.DrawCheckBox(this, m_Checked, IsDepressed);
        }

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
