using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class Window : GUnit
    {
        private int m_WindowCount;
        private readonly Random rand;

        public Window(Base parent)
            : base(parent)
        {
            rand = new Random();

            Controls.Button button = new Controls.Button(this);
            button.SetText("Open a Window");
            button.OnPress += OpenWindow;

            m_WindowCount = 1;
        }

        void OpenWindow(Base control)
        {
            Controls.WindowControl pWindow = new Controls.WindowControl(GetCanvas());
            pWindow.Title = String.Format("Window {0}", m_WindowCount);
            pWindow.SetSize(rand.Next(200, 400), rand.Next(200, 400));
            pWindow.SetPos(rand.Next(700), rand.Next(400));

            m_WindowCount++;
        }
    }
}
