using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class CrossSplitter : GUnit
    {
        private int m_CurZoom;
        private readonly Control.CrossSplitter m_Splitter;

        public CrossSplitter(Base parent)
            : base(parent)
        {
            m_CurZoom = 0;

            m_Splitter = new Control.CrossSplitter(this);
            m_Splitter.SetPosition(0, 0);
            m_Splitter.Dock = Pos.Fill;

            {
                Control.Button testButton = new Control.Button(m_Splitter);
                testButton.SetText("TOPLEFT");
                m_Splitter.SetPanel(0, testButton);
            }

            {
                Control.Button testButton = new Control.Button(m_Splitter);
                testButton.SetText("TOPRIGHT");
                m_Splitter.SetPanel(1, testButton);
            }

            {
                Control.Button testButton = new Control.Button(m_Splitter);
                testButton.SetText("BOTTOMLEFT");
                m_Splitter.SetPanel(2, testButton);
            }

            {
                Control.Button testButton = new Control.Button(m_Splitter);
                testButton.SetText("BOTTOMRIGHT");
                m_Splitter.SetPanel(3, testButton);
            }

            //Status bar to hold unit testing buttons
            Control.StatusBar pStatus = new Control.StatusBar(this);
            pStatus.Dock = Pos.Bottom;

            {
                Control.Button pButton = new Control.Button(pStatus);
                pButton.SetText("Zoom");
                pButton.Clicked += ZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
                Control.Button pButton = new Control.Button(pStatus);
                pButton.SetText("UnZoom");
                pButton.Clicked += UnZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
                Control.Button pButton = new Control.Button(pStatus);
                pButton.SetText("CenterPanels");
                pButton.Clicked += CenterPanels;
                pStatus.AddControl(pButton, true);
            }

            {
                Control.Button pButton = new Control.Button(pStatus);
                pButton.SetText("Splitters");
                pButton.Clicked += ToggleSplitters;
                pStatus.AddControl(pButton, true);
            }
        }

        void ZoomTest(Base control)
        {
            m_Splitter.Zoom(m_CurZoom);
            m_CurZoom++;
            if (m_CurZoom == 4)
                m_CurZoom = 0;
        }

        void UnZoomTest(Base control)
        {
            m_Splitter.UnZoom();
        }

        void CenterPanels(Base control)
        {
            m_Splitter.CenterPanels();
            m_Splitter.UnZoom();
        }

        void ToggleSplitters(Base control)
        {
            m_Splitter.SplittersVisible = !m_Splitter.SplittersVisible;
        }

        protected override void Layout(Skin.Base skin)
        {
            
        }
    }
}
