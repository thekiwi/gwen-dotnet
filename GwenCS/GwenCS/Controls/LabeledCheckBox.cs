using System;

namespace Gwen.Controls
{
    public class LabeledCheckBox : Base
    {
        private CheckBox m_CheckBox;
        private LabelClickable m_Label;

        public LabeledCheckBox(Base parent) : base(parent)
        {
            SetSize(200, 19);
            m_CheckBox = new CheckBox(this);
            m_CheckBox.Dock = Pos.Left;
            m_CheckBox.Margin = new Margin(0, 2, 2, 2);
            m_CheckBox.IsTabable = false;

            m_Label = new LabelClickable(this);
            m_Label.Dock = Pos.Fill;
            m_Label.OnPress += m_CheckBox.ReceiveEventPress;
            m_Label.IsTabable = false;

            IsTabable = false;
        }

        public CheckBox CheckBox { get { return m_CheckBox; } }
        public LabelClickable Label { get { return m_Label; } }

        internal override bool onKeySpace(bool bDown)
        {
            base.onKeySpace(bDown);
            if (!bDown) 
                m_CheckBox.IsChecked = !m_CheckBox.IsChecked; 
            return true;
        }
    }
}
