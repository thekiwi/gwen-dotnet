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
            m_Label.OnPress += m_CheckBox.Press;
            m_Label.IsTabable = false;

            IsTabable = false;
        }

        public override void Dispose()
        {
            m_CheckBox.Dispose();
            m_Label.Dispose();
            base.Dispose();
        }

        public CheckBox CheckBox { get { return m_CheckBox; } }
        public LabelClickable Label { get { return m_Label; } }

        internal override bool onKeySpace(bool down)
        {
            base.onKeySpace(down);
            if (!down) 
                m_CheckBox.IsChecked = !m_CheckBox.IsChecked; 
            return true;
        }
    }
}
