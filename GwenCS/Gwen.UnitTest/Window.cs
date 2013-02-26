using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class Window : GUnit
    {
        private int m_WindowCount;
        private readonly Random rand;

        public Window(Control parent)
            : base(parent)
        {
            rand = new Random();

            Controls.Button button1 = new Controls.Button(this);
            button1.SetText("Open a Window");
            button1.Clicked += OpenWindow;

            Controls.Button button2 = new Controls.Button(this);
            button2.SetText("Open a MessageBox");
            button2.Clicked += OpenMsgbox;
            Align.PlaceRightBottom(button2, button1, 10);

            m_WindowCount = 1;
        }

        void OpenWindow(Control control)
        {
            WindowControl window = new WindowControl(GetCanvas());
            window.Caption = String.Format("Window {0}", m_WindowCount);
            window.SetSize(rand.Next(200, 400), rand.Next(200, 400));
            window.SetPosition(rand.Next(700), rand.Next(400));

            m_WindowCount++;
        }

        void OpenMsgbox(Control control)
        {
            MessageBox window = new MessageBox(GetCanvas(), String.Format("Window {0}   MessageBox window = new MessageBox(GetCanvas(), String.Format(  MessageBox window = new MessageBox(GetCanvas(), String.Format(", m_WindowCount));
            window.SetPosition(rand.Next(700), rand.Next(400));

            m_WindowCount++;
        }
    }
}
