using System;

namespace Gwen.Controls
{
    /// <summary>
    /// Text box with masked text.
    /// </summary>
    /// <remarks>
    /// This class doesn't prevent programatic access to the text in any way.
    /// </remarks>
    public class TextBoxPassword : TextBox
    {
        private String m_Mask;

        /// <summary>
        /// Character used in place of actual characters for display.
        /// </summary>
        public char MaskCharacter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxPassword"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TextBoxPassword(Control parent)
            : base(parent)
        {
            MaskCharacter = '*';
        }

        /// <summary>
        /// Handler for text changed event.
        /// </summary>
        protected override void OnTextChanged()
        {
            m_Mask = new string(MaskCharacter, Text.Length);
            TextOverride = m_Mask;
            base.OnTextChanged();
        }
    }
}
