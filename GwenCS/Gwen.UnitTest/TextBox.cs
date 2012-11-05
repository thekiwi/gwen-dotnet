using System;
using System.Drawing;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class TextBox : GUnit
    {
        private readonly Font m_Font1;
        private readonly Font m_Font2;
        private readonly Font m_Font3;

        public TextBox(Base parent)
            : base(parent)
        {
            int row = 0;

            m_Font1 = new Font(Skin.Renderer, "Consolas", 14); // fixed width font!
            m_Font2 = new Font(Skin.Renderer, "Impact", 50);
            m_Font3 = new Font(Skin.Renderer, "Arial", 14);

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Type something here");
                label.SetPosition(10, 10 + 25 * row);
                label.TextChanged += OnEdit;
                label.SubmitPressed += OnSubmit;
                row++;
            }

            {
                Control.TextBoxPassword label = new Control.TextBoxPassword(this);
                //label.MaskCharacter = '@';
                label.SetText("secret");
                label.TextChanged += OnEdit;
                label.SetPosition(10, 10 + 25 * row);
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Select All Text On Focus");
                label.SetPosition(10, 10 + 25 * row);
                label.SelectAllOnFocus = true;
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Different Coloured Text, for some reason");
                label.TextColor = Color.ForestGreen;
                label.SetPosition(10, 10 + 25 * row);
                row++;
            }

            {
                Control.TextBox label = new Control.TextBoxNumeric(this);
                label.SetText("200456698");
                label.TextColor = Color.LightCoral;
                label.SetPosition(10, 10 + 25 * row);
                row++;
            }

            row++;

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font3;
                label.SizeToContents();
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("..............................................................");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font3;
                label.SizeToContents();
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("public override void SetText(string str, bool doEvents = true)");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font3;
                label.SizeToContents();
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("あおい　うみから　やってきた");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font3;
                label.SizeToContents();
                row++;
            }

            row++;

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font1;
                label.SizeToContents();
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("..............................................................");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font1;
                label.SizeToContents();
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("public override void SetText(string str, bool doEvents = true)");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font1;
                label.SizeToContents();
                row++;
            }

            {
                Control.TextBox label = new Control.TextBox(this);
                label.SetText("あおい　うみから　やってきた");
                label.TextColor = Color.Black;
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font1;
                label.SizeToContents();
                row++;
            }

            row++;

            {

                Control.TextBox label = new Control.TextBox(this);
                label.SetText("Different Font (autosized)");
                label.SetPosition(10, 10 + 28 * row);
                label.Font = m_Font2;
                label.SizeToContents();

                row += 2;
            }
        }

        public override void Dispose()
        {
            m_Font1.Dispose();
            m_Font2.Dispose();
            m_Font3.Dispose();
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
