using System;
using System.Drawing;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class TextBox : GUnit
    {
        private Font m_Font;

        public TextBox(Base parent)
            : base(parent)
        {
            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Type something here");
                label.SetPosition(10, 10);
                label.TextChanged += OnEdit;
                label.SubmitPressed += OnSubmit;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Normal Everyday TextBox");
                label.SetPosition(10, 10 + 25);
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Select All Text On Focus");
                label.SetPosition(10, 10 + 25*2);
                label.SelectAllOnFocus = true;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Different Coloured Text, for some reason");
                label.TextColor = Color.ForestGreen;
                label.SetPosition(10, 10 + 25*3);
            }

            {
                Control.TextBoxNumeric label = new Control.TextBoxNumeric(this);
                label.SetText("2004");
                label.TextColor = Color.LightCoral;
                label.SetPosition(10, 10 + 25*4);
            }

            {
                m_Font = new Font("Impact", 50);

                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Different Font");
                label.SetPosition(10, 10 + 25*5);
                label.Font = m_Font;
                label.SetSize(200, 55);
            }
        }

        void OnEdit(Base control)
        {
            Control.TextBox box = control as Control.TextBox;
            UnitPrint(String.Format("TextBox: OnEdit: {0}", box.Text));
        }

        void OnSubmit(Base control)
        {
            Control.TextBox box = control as Control.TextBox;
            UnitPrint(String.Format("TextBox: OnSubmit: {0}", box.Text));
        }
    }
}
