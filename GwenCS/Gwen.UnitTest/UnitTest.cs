using System;
using Gwen.Controls;
using Gwen.Controls.Layout;

namespace Gwen.UnitTest
{
    public class UnitTest : DockBase
    {
        private Controls.Base m_LastControl;
        private Controls.StatusBar m_StatusBar;
        private Controls.ListBox m_TextOutput;
        private Controls.TabControl m_TabControl;
        private Controls.TabButton m_Button;
        private Controls.CollapsibleList m_List;

        public double Fps; // set this in your rendering loop

        public UnitTest(Base parent) : base(parent)
        {
            Dock = Pos.Fill;
            SetSize(1024, 768);
            m_List = new CollapsibleList(this);

            LeftDock.TabControl.AddPage("CollapsibleList", m_List);
            LeftDock.Width = 150;

            m_TextOutput = new ListBox(BottomDock);
            m_Button = BottomDock.TabControl.AddPage("Output", m_TextOutput);
            BottomDock.Height = 200;

            m_StatusBar = new Controls.StatusBar(this);
            m_StatusBar.Dock = Pos.Bottom;

            Center center = new Center(this);
            center.Dock = Pos.Fill;

            {
                CollapsibleCategory cat = m_List.Add("Basic");
                GUnit test;
                {
                    test = new Button(center);
                    RegisterUnitTest("Button", cat, test);
                    test = new Label(center);
                    RegisterUnitTest("Label", cat, test);
                }
            }

            {
                CollapsibleCategory cat = m_List.Add("Non-Interactive");
                GUnit test;
                {
                    test = new ProgressBar(center);
                    RegisterUnitTest("ProgressBar", cat, test);
                    test = new GroupBox(center);
                    RegisterUnitTest("GroupBox", cat, test);
                    test = new ImagePanel(center);
                    RegisterUnitTest("ImagePanel", cat, test);
                    test = new StatusBar(center);
                    RegisterUnitTest("StatusBar", cat, test);
                }
            }

            {
                CollapsibleCategory cat = m_List.Add("Standard");
                GUnit test;
                {
                    test = new ComboBox(center);
                    RegisterUnitTest("ComboBox", cat, test);
                    test = new TextBox(center);
                    RegisterUnitTest("TextBox", cat, test);
                }
            }

            m_StatusBar.SendToBack();
            PrintText("Unit Test started!");
        }

        public void RegisterUnitTest(String name, CollapsibleCategory cat, GUnit test)
        {
            Controls.Button btn = cat.Add(name);
            test.Dock = Pos.Fill;
            test.Hide();
            test.UnitTest = this;
            btn.UserData = test;
            btn.OnPress += onCategorySelect;
        }

        private void onCategorySelect(Base control)
        {
            if (m_LastControl != null)
            {
                m_LastControl.Hide();
            }
            Base test = control.UserData as Base;
            test.Show();
            m_LastControl = test;
        }

        public void PrintText(String str)
        {
            m_TextOutput.AddItem(str);
            m_TextOutput.ScrollToBottom();
        }

        protected override void Render(Skin.Base skin)
        {
            m_StatusBar.Text = String.Format("GWEN.Net Unit Test - {0:F0} fps", Fps);

            base.Render(skin);
        }
    }
}
