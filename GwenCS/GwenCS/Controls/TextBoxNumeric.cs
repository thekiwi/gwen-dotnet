using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class TextBoxNumeric : TextBox
    {
        protected double m_Value;

        public TextBoxNumeric(Base parent) : base(parent)
        {
            Text = "0";
        }

        protected override bool IsTextAllowed(String str, int iPos)
        {
            String newText = Text.Insert(iPos, str);
            if (newText == "-")
                return true; // annoying if single - is not allowed
            double d;
            return double.TryParse(newText, out d);
        }

        public virtual double Value
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
            base.onTextChanged();
            if (String.IsNullOrEmpty(Text) || Text == "-")
            {
                m_Value = 0;
                Text = "0";
            }
            else
                m_Value = double.Parse(Text);
        }
    }
}
