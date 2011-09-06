using System;

namespace Gwen.Controls
{
    public class LabeledRadioButton : Base
    {
        protected RadioButton m_RadioButton;
        protected LabelClickable m_Label;

        public LabeledRadioButton(Base parent) : base(parent)
        {
            SetSize(200, 19);

            m_RadioButton = new RadioButton(this);
            m_RadioButton.Dock = Pos.Left;
            m_RadioButton.Margin = new Margin(0, 2, 2, 2);
            m_RadioButton.IsTabable = false;
            m_RadioButton.KeyboardInputEnabled = false;

            m_Label = new LabelClickable(this);
            m_Label.Alignment = Pos.CenterV | Pos.Left;
            m_Label.Text = "Radio Button";
            m_Label.Dock = Pos.Fill;
            m_Label.OnPress += m_RadioButton.ReceiveEventPress;
            m_Label.IsTabable = false;
            m_Label.KeyboardInputEnabled = false;
        }

        public override void Dispose()
        {
            m_RadioButton.Dispose();
            m_Label.Dispose();
            base.Dispose();
        }

        protected override void RenderFocus(Skin.Base skin)
        {
            if (Global.KeyboardFocus != this) return;
            if (!IsTabable) return;

            skin.DrawKeyboardHighlight(this, RenderBounds, 0);
        }

        public RadioButton RadioButton { get { return m_RadioButton; } }
        public LabelClickable Label { get { return m_Label; } }

        internal override bool onKeySpace(bool bDown)
        {
            if (bDown)
                m_RadioButton.IsChecked = !m_RadioButton.IsChecked;
            return true;
        }

        public virtual void Select()
        {
            m_RadioButton.IsChecked = true;
        }
    }
}
