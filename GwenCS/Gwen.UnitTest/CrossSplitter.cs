using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class CrossSplitter : GUnit
    {
        private int m_CurZoom;
        private readonly Controls.CrossSplitter m_Splitter;

        public CrossSplitter(Base parent)
            : base(parent)
        {
            m_CurZoom = 0;

            m_Splitter = new Controls.CrossSplitter(this);
            m_Splitter.SetPos(0, 0);
            m_Splitter.Dock = Pos.Fill;

            {
                Controls.Button testButton = new Controls.Button(m_Splitter);
                testButton.SetText("TOPLEFT");
                m_Splitter.SetPanel(0, testButton);
            }

            {
                Controls.Button testButton = new Controls.Button(m_Splitter);
                testButton.SetText("TOPRIGHT");
                m_Splitter.SetPanel(1, testButton);
            }

            {
                Controls.Button testButton = new Controls.Button(m_Splitter);
                testButton.SetText("BOTTOMRIGHT");
                m_Splitter.SetPanel(2, testButton);
            }

            {
                Controls.Button testButton = new Controls.Button(m_Splitter);
                testButton.SetText("BOTTOMLEFT");
                m_Splitter.SetPanel(3, testButton);
            }

            //Status bar to hold unit testing buttons
            Controls.StatusBar pStatus = new Controls.StatusBar(this);
            pStatus.Dock = Pos.Bottom;

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("Zoom");
                pButton.OnPress += ZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("UnZoom");
                pButton.OnPress += UnZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("CenterPanels");
                pButton.OnPress += CenterPanels;
                pStatus.AddControl(pButton, true);
            }

            {
                Controls.Button pButton = new Controls.Button(pStatus);
                pButton.SetText("Splitters");
                pButton.OnPress += ToggleSplitters;
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
