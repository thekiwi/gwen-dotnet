using System;
using System.Drawing;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class TextBox : GUnit
    {
        private readonly Font m_Font;

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
                Control.TextBoxPassword label = new Control.TextBoxPassword(this);
                //label.MaskCharacter = '@';
                label.SetText("secret");
                label.TextChanged += OnEdit;
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
                Control.TextBox label = new Control.TextBoxNumeric(this);
                label.SetText("2004");
                label.TextColor = Color.LightCoral;
                label.SetPosition(10, 10 + 25*4);
            }

            {
                m_Font = new Font(Skin.Renderer, "Impact", 50);

                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Different Font (autosized)");
                label.SetPosition(10, 10 + 25*5);
                label.Font = m_Font;
                label.SizeToContents();
            }
        }

        public override void Dispose()
        {
            m_Font.Dispose();
            base.Dispose();
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
