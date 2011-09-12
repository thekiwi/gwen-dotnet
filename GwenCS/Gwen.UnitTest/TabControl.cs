using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class TabControl : GUnit
    {
        private readonly Controls.TabControl m_DockControl;

        public TabControl(Base parent)
            : base(parent)
        {
            {
                m_DockControl = new Controls.TabControl(this);
                m_DockControl.SetBounds(10, 10, 200, 200);

                {
                    Controls.TabButton button = m_DockControl.AddPage("Controls");
                    Base page = button.Page;

                    {
                        Controls.RadioButtonController radio = new Controls.RadioButtonController(page);
                        radio.SetBounds(10, 10, 100, 100);

                        radio.AddOption("Top").Select();
                        radio.AddOption("Bottom");
                        radio.AddOption("Left");
                        radio.AddOption("Right");

                        radio.OnSelectionChange += OnDockChange;

                    }
                }

                m_DockControl.AddPage("Red");
                m_DockControl.AddPage("Green");
                m_DockControl.AddPage("Blue");
            }

            {
                Controls.TabControl dragMe = new Controls.TabControl(this);
                dragMe.SetBounds(220, 10, 200, 200);

                dragMe.AddPage("You");
                dragMe.AddPage("Can");
                dragMe.AddPage("Reorder").SetImage("test16.png");
                dragMe.AddPage("These");
                dragMe.AddPage("Tabs");

                dragMe.AllowReorder = true;
            }
        }

        void OnDockChange(Base control)
        {
            RadioButtonController rc = (RadioButtonController)control;

            if (rc.SelectedLabel == "Top") m_DockControl.TabStripPosition = Pos.Top;
            if (rc.SelectedLabel == "Bottom") m_DockControl.TabStripPosition = Pos.Bottom;
            if (rc.SelectedLabel == "Left") m_DockControl.TabStripPosition = Pos.Left;
            if (rc.SelectedLabel == "Right") m_DockControl.TabStripPosition = Pos.Right;
        }
    }
}
