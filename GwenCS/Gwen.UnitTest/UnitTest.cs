using System;
using Gwen.Controls;
using Gwen.Controls.Layout;

namespace Gwen.UnitTest
{
    public class UnitTest : DockBase
    {
        private Controls.Base m_LastControl;
        private readonly Controls.StatusBar m_StatusBar;
        private readonly Controls.ListBox m_TextOutput;
        private Controls.TabControl m_TabControl;
        private Controls.TabButton m_Button;
        private readonly Controls.CollapsibleList m_List;

        public double Fps; // set this in your rendering loop

        public UnitTest(Base parent) : base(parent)
        {
            Dock = Pos.Fill;
            SetSize(1024, 768);
            m_List = new Controls.CollapsibleList(this);

            LeftDock.TabControl.AddPage("Unit tests", m_List);
            LeftDock.Width = 150;

            m_TextOutput = new Controls.ListBox(BottomDock);
            m_Button = BottomDock.TabControl.AddPage("Output", m_TextOutput);
            BottomDock.Height = 200;

            m_StatusBar = new Controls.StatusBar(this);
            m_StatusBar.Dock = Pos.Bottom;

            Center center = new Center(this);
            center.Dock = Pos.Fill;
            GUnit test;

            {
                CollapsibleCategory cat = m_List.Add("Non-Interactive");
                {
                    test = new Label(center);
                    RegisterUnitTest("Label", cat, test);
                    test = new GroupBox(center);
                    RegisterUnitTest("GroupBox", cat, test);
                    test = new ProgressBar(center);
                    RegisterUnitTest("ProgressBar", cat, test);
                    test = new ImagePanel(center);
                    RegisterUnitTest("ImagePanel", cat, test);
                    test = new StatusBar(center);
                    RegisterUnitTest("StatusBar", cat, test);
                }
            }

            {
                CollapsibleCategory cat = m_List.Add("Standard");
                {
                    test = new Button(center);
                    RegisterUnitTest("Button", cat, test);
                    test = new TextBox(center);
                    RegisterUnitTest("TextBox", cat, test);
                    test = new CheckBox(center);
                    RegisterUnitTest("CheckBox", cat, test);
                    test = new RadioButton(center);
                    RegisterUnitTest("RadioButton", cat, test);
                    test = new ComboBox(center);
                    RegisterUnitTest("ComboBox", cat, test);
                    test = new ListBox(center);
                    RegisterUnitTest("ListBox", cat, test);
                    test = new NumericUpDown(center);
                    RegisterUnitTest("NumericUpDown", cat, test);
                    test = new Slider(center);
                    RegisterUnitTest("Slider", cat, test);
                    test = new MenuStrip(center);
                    RegisterUnitTest("MenuStrip", cat, test);
                    test = new CrossSplitter(center);
                    RegisterUnitTest("CrossSplitter", cat, test);
                }
            }
            
            {
                CollapsibleCategory cat = m_List.Add("Containers");
                {
                    test = new Window(center);
                    RegisterUnitTest("Window", cat, test);
                    test = new TreeControl(center);
                    RegisterUnitTest("TreeControl", cat, test);
                    test = new Properties(center);
                    RegisterUnitTest("Properties", cat, test);
                    test = new TabControl(center);
                    RegisterUnitTest("TabControl", cat, test);
                    test = new ScrollControl(center);
                    RegisterUnitTest("ScrollControl", cat, test);
                }
            }
            
            {
                CollapsibleCategory cat = m_List.Add("Non-standard");
                {
                    test = new CollapsibleList(center);
                    RegisterUnitTest("CollapsibleList", cat, test);
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
