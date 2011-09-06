using System;

namespace Gwen.Controls
{
    public class TextBoxNumeric : TextBox
    {
        protected float m_Value;

        public TextBoxNumeric(Base parent) : base(parent)
        {
            SetText("0", false);
        }

        // [omeg] added
        protected virtual bool IsTextAllowed(String str)
        {
            if (str == "" || str == "-")
                return true; // annoying if single - is not allowed
            float d;
            return float.TryParse(str, out d);
        }

        protected override bool IsTextAllowed(String str, int iPos)
        {
            String newText = Text.Insert(iPos, str);
            return IsTextAllowed(newText);
        }

        public virtual float Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                Text = value.ToString();
            }
        }
        
        // text -> value
        protected override void onTextChanged()
        {
            if (String.IsNullOrEmpty(Text) || Text == "-")
            {
                m_Value = 0;
                //SetText("0");
            }
            else
                m_Value = float.Parse(Text);
            base.onTextChanged();
        }
        
        public override void SetText(string str, bool doEvents = true)
        {
            if (IsTextAllowed(str))
                base.SetText(str, doEvents);
        }
    }
}
