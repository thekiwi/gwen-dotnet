using System;
using Gwen.Controls;
using Gwen.Drawing;

namespace Gwen.UnitTest
{
    public class CrossSplitter : GUnit
    {
        private int m_CurZoom;
        private readonly Controls.CrossSplitter m_Splitter;

        public CrossSplitter(Control parent)
            : base(parent)
        {
            m_CurZoom = 0;

            m_Splitter = new Controls.CrossSplitter(this);
            m_Splitter.SetPosition(0, 0);
            m_Splitter.Dock = Pos.Fill;

            {
                VerticalSplitter splitter = new VerticalSplitter(m_Splitter);
                Controls.Button button1 = new Controls.Button(splitter);
                button1.SetText("Vertical left");
                Controls.Button button2 = new Controls.Button(splitter);
                button2.SetText("Vertical right");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(0, splitter);
            }

            {
                HorizontalSplitter splitter = new HorizontalSplitter(m_Splitter);
                Controls.Button button1 = new Controls.Button(splitter);
                button1.SetText("Horizontal up");
                Controls.Button button2 = new Controls.Button(splitter);
                button2.SetText("Horizontal down");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(1, splitter);
            }

            {
                HorizontalSplitter splitter = new HorizontalSplitter(m_Splitter);
                Controls.Button button1 = new Controls.Button(splitter);
                button1.SetText("Horizontal up");
                Controls.Button button2 = new Controls.Button(splitter);
                button2.SetText("Horizontal down");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(2, splitter);
            }

            {
                VerticalSplitter splitter = new VerticalSplitter(m_Splitter);
                Controls.Button button1 = new Controls.Button(splitter);
                button1.SetText("Vertical left");
                Controls.Button button2 = new Controls.Button(splitter);
                button2.SetText("Vertical right");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(3, splitter);
            }

            //Status bar to hold unit testing buttons
            Controls.StatusBar pStatus = new Controls.StatusBar(this);
            pStatus.Dock = Pos.Bottom;

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("Zoom");
                pButton.Clicked += ZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("UnZoom");
                pButton.Clicked += UnZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("CenterPanels");
                pButton.Clicked += CenterPanels;
                pStatus.AddControl(pButton, true);
            }

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("Splitters");
                pButton.Clicked += ToggleSplitters;
                pStatus.AddControl(pButton, true);
            }
        }

        void ZoomTest(Control control)
        {
            m_Splitter.Zoom(m_CurZoom);
            m_CurZoom++;
            if (m_CurZoom == 4)
                m_CurZoom = 0;
        }

        void UnZoomTest(Control control)
        {
            m_Splitter.UnZoom();
        }

        void CenterPanels(Control control)
        {
            m_Splitter.CenterPanels();
            m_Splitter.UnZoom();
        }

        void ToggleSplitters(Control control)
        {
            m_Splitter.SplittersVisible = !m_Splitter.SplittersVisible;
        }

        protected override void Layout(Skin skin)
        {
            
        }
    }
}
