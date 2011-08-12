using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class CheckBox : Button
    {
        private bool m_bChecked;

        public bool IsChecked
        {
            get { return m_bChecked; } 
            set
            {
                if (m_bChecked == value) return;
                m_bChecked = value;
                OnCheckStatusChanged();
            }
        }

        public CheckBox(Base parent) : base(parent)
        {
            SetSize(13, 13);
            m_bChecked = true; // [omeg] why?!
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

        protected void OnCheckStatusChanged()
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
            skin.DrawCheckBox(this, m_bChecked, IsDepressed);
        }

        protected override void onPress()
        {
            base.onPress();
            if (IsChecked && !AllowUncheck)
            {
                return;
            }

            Toggle();
        }
    }
}
