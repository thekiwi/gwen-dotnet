using System;
using System.Drawing;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class TextBox : GUnit
    {
        private Font m_Font;

        public TextBox(Base parent)
            : base(parent)
        {
            {
                Controls.TextBox label = new Controls.TextBox(this);
                label.SetText("Type something here");
                label.SetPos(10, 10);
                label.OnTextChanged += OnEdit;
                label.OnReturnPressed += OnSubmit;
            }

            {
                Controls.TextBox label = new Controls.TextBox(this);
                label.SetText("Normal Everyday TextBox");
                label.SetPos(10, 10 + 25);
            }

            {
                Controls.TextBox label = new Controls.TextBox(this);
                label.SetText("Select All Text On Focus");
                label.SetPos(10, 10 + 25*2);
                label.SelectAllOnFocus = true;
            }

            {
                Controls.TextBox label = new Controls.TextBox(this);
                label.SetText("Different Coloured Text, for some reason");
                label.TextColor = Color.ForestGreen;
                label.SetPos(10, 10 + 25*3);
            }

            {
                Controls.TextBoxNumeric label = new Controls.TextBoxNumeric(this);
                label.SetText("2004");
                label.TextColor = Color.LightCoral;
                label.SetPos(10, 10 + 25*4);
            }

            {
                m_Font = new Font("Impact", 50);

                Controls.TextBox label = new Controls.TextBox(this);
                label.SetText("Different Font");
                label.SetPos(10, 10 + 25*5);
                label.Font = m_Font;
                label.SetSize(200, 55);
            }
        }

        void OnEdit(Base control)
        {
            Controls.TextBox box = control as Controls.TextBox;
            UnitPrint(String.Format("TextBox: OnEdit: {0}", box.Text));
        }

        void OnSubmit(Base control)
        {
            Controls.TextBox box = control as Controls.TextBox;
            UnitPrint(String.Format("TextBox: OnSubmit: {0}", box.Text));
        }
    }
}
