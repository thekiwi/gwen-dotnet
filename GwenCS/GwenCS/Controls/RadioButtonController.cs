using System;
using System.Linq;

namespace Gwen.Controls
{
    public class RadioButtonController : Base
    {
        protected LabeledRadioButton m_Selected;

        public LabeledRadioButton Selected { get { return m_Selected; } }
        public String SelectedName { get { return m_Selected.Name; } }
        public String SelectedLabel { get { return m_Selected.Label.Text; } }

        public event ControlCallback OnSelectionChange;

        public RadioButtonController(Base parent) : base(parent)
        {
            IsTabable = false;
            KeyboardInputEnabled = false;
        }

        public virtual LabeledRadioButton AddOption(String text)
        {
            return AddOption(text, String.Empty);
        }

        public virtual LabeledRadioButton AddOption(String text, String optionName)
        {
            LabeledRadioButton lrb = new LabeledRadioButton(this);
            lrb.Name = optionName;
            lrb.Label.Text = text;
            lrb.RadioButton.OnChecked += onRadioClicked;
            lrb.Dock = Pos.Top;
            lrb.Margin = new Margin(0, 1, 0, 1);
            lrb.KeyboardInputEnabled = false;
            lrb.IsTabable = false;

            Invalidate();
            return lrb;
        }

        protected virtual void onRadioClicked(Base pFromPanel)
        {
            RadioButton pChecked = pFromPanel as RadioButton;
            foreach (LabeledRadioButton rb in Children.OfType<LabeledRadioButton>())
            {
                if (rb.RadioButton == pChecked)
                    m_Selected = rb;
                else
                    rb.RadioButton.IsChecked = false;
            }

            onChange();
        }

        protected virtual void onChange()
        {
            if (OnSelectionChange != null)
                OnSelectionChange.Invoke(this);
        }

        protected override void Render(Skin.Base skin)
        {

        }

        // [omeg] added
        public void SetSelection(int index)
        {
            if (index < 0 || index >= Children.Count)
                return;

            (Children[index] as LabeledRadioButton).RadioButton.onPress();
        }
    }
}
