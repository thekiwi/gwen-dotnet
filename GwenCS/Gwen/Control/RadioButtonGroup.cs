using System;
using System.Linq;

namespace Gwen.Control
{
    /// <summary>
    /// Radio button group.
    /// </summary>
    public class RadioButtonGroup : Base
    {
        private LabeledRadioButton m_Selected;

        /// <summary>
        /// Selected radio button.
        /// </summary>
        public LabeledRadioButton Selected { get { return m_Selected; } }

        /// <summary>
        /// Internal name of the selected radio button.
        /// </summary>
        public String SelectedName { get { return m_Selected.Name; } }

        /// <summary>
        /// Text of the selected radio button.
        /// </summary>
        public String SelectedLabel { get { return m_Selected.Text; } }

        /// <summary>
        /// Invoked when the selected option has changed.
        /// </summary>
        public event GwenEventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonGroup"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public RadioButtonGroup(Base parent)
            : base(parent)
        {
            IsTabable = false;
            KeyboardInputEnabled = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            foreach (Base child in Children)
            {
                if (child is LabeledRadioButton)
                    child.Dispose();
            }
            base.Dispose();
        }

        /// <summary>
        /// Adds a new option.
        /// </summary>
        /// <param name="text">Option text.</param>
        /// <returns>Newly created control.</returns>
        public virtual LabeledRadioButton AddOption(String text)
        {
            return AddOption(text, String.Empty);
        }

        /// <summary>
        /// Adds a new option.
        /// </summary>
        /// <param name="text">Option text.</param>
        /// <param name="optionName">Internal name.</param>
        /// <returns>Newly created control.</returns>
        public virtual LabeledRadioButton AddOption(String text, String optionName)
        {
            LabeledRadioButton lrb = new LabeledRadioButton(this);
            lrb.Name = optionName;
            lrb.Text = text;
            lrb.RadioButton.Checked += OnRadioClicked;
            lrb.Dock = Pos.Top;
            lrb.Margin = new Margin(0, 1, 0, 1);
            lrb.KeyboardInputEnabled = false; // todo: true?
            lrb.IsTabable = false;

            Invalidate();
            return lrb;
        }

        /// <summary>
        /// Handler for the option change.
        /// </summary>
        /// <param name="fromPanel">Event source.</param>
        protected virtual void OnRadioClicked(Base fromPanel)
        {
            RadioButton @checked = fromPanel as RadioButton;
            foreach (LabeledRadioButton rb in Children.OfType<LabeledRadioButton>()) // todo: optimize
            {
                if (rb.RadioButton == @checked)
                    m_Selected = rb;
                else
                    rb.RadioButton.IsChecked = false;
            }

            OnChanged();
        }

        protected virtual void OnChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged.Invoke(this);
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {

        }

        /// <summary>
        /// Selects the specified option.
        /// </summary>
        /// <param name="index">Option to select.</param>
        public void SetSelection(int index)
        {
            if (index < 0 || index >= Children.Count)
                return;

            (Children[index] as LabeledRadioButton).RadioButton.Press();
        }
    }
}
