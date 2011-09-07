using System;
using Gwen.Controls;
using Gwen.Controls.Layout;

namespace Gwen.UnitTest
{
    public class UnitTest : DockBase
    {
        private Base m_LastControl;
        private float m_LastSecond;
        private uint m_Frames;
        private StatusBar m_StatusBar;
        private ListBox m_TextOutput;
        private TabControl m_TabControl;
        private TabButton m_Button;
        private CollapsibleList m_List;

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

            m_StatusBar = new StatusBar(this);
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

            m_StatusBar.SendToBack();
            PrintText("Unit Test started!");
            m_LastSecond = Platform.Windows.GetTimeInSeconds();
            m_Frames = 0;
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
            m_Frames++;
            if (m_LastSecond < Platform.Windows.GetTimeInSeconds() + 0.5f)
            {
                m_StatusBar.Text = String.Format("GWEN Unit Test - {0} fps", m_Frames*2);
                m_Frames = 0;
            }

            base.Render(skin);
        }
    }
}
