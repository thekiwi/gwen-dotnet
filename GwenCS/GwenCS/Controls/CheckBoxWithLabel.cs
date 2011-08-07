using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class CheckBoxWithLabel : Base
    {
        private CheckBox m_CheckBox;
        private LabelClickable m_Label;

        public CheckBoxWithLabel(Base parent) : base(parent)
        {
            SetSize(200, 19);
            m_CheckBox = new CheckBox(this);
            m_CheckBox.Dock = Pos.Left;
            m_CheckBox.Margin = new Margin(0, 3, 3, 3);
            m_CheckBox.IsTabable = false;

            m_Label = new LabelClickable(this);
            m_Label.Dock = Pos.Fill;
            m_Label.OnPress += m_CheckBox.ReceiveEventPress;
            m_Label.IsTabable = false;

            IsTabable = false;
        }

        public virtual CheckBox CheckBox { get { return m_CheckBox; } }
        public virtual LabelClickable Label { get { return m_Label; } }

        public override bool OnKeySpace(bool bDown)
        {
            if (bDown) 
                m_CheckBox.IsChecked = !m_CheckBox.IsChecked; 
            return true;
        }
    }
}
