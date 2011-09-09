using System;
using System.Diagnostics;
using System.Windows.Forms;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    /// <summary>
    /// Splitter control.
    /// </summary>
    public class CrossSplitter : Base
    {
        private SplitterBar m_VSplitter;
        private SplitterBar m_HSplitter;
        private SplitterBar m_CSplitter;

        private Base[] m_Sections;

        private float m_HVal;
        private float m_VVal;
        private int m_BarSize;

        private int m_ZoomedSection;

        public event ControlCallback OnZoomed;
        public event ControlCallback OnUnZoomed;
        public event ControlCallback OnZoomChange;

        public CrossSplitter(Base parent)
            : base(parent)
        {
            m_Sections = new Base[4];

            m_VSplitter = new SplitterBar(this);
            m_VSplitter.SetPos(0, 128);
            m_VSplitter.OnDragged += onVerticalMoved;
            m_VSplitter.Cursor = Cursors.SizeNS;

            m_HSplitter = new SplitterBar(this);
            m_HSplitter.SetPos(128, 0);
            m_HSplitter.OnDragged += onHorizontalMoved;
            m_HSplitter.Cursor = Cursors.SizeWE;

            m_CSplitter = new SplitterBar(this);
            m_CSplitter.SetPos(128, 128);
            m_CSplitter.OnDragged += onCenterMoved;
            m_CSplitter.Cursor = Cursors.SizeAll;

            m_HVal = 0.5f;
            m_VVal = 0.5f;

            SetPanel(0, null);
            SetPanel(1, null);
            SetPanel(2, null);
            SetPanel(3, null);

            SplitterSize = 5;
            SplittersVisible = false;

            m_ZoomedSection = -1;
        }

        public override void Dispose()
        {
            m_VSplitter.Dispose();
            m_HSplitter.Dispose();
            m_CSplitter.Dispose();
            for (int i = 0; i < 4; i++ )
                m_Sections[i].Dispose();
            base.Dispose();
        }

        public void CenterPanels()
        {
            m_HVal = 0.5f;
            m_VVal = 0.5f;
            Invalidate();
        }

        public bool IsZoomed { get { return m_ZoomedSection != -1; } }

        public bool SplittersVisible
        {
            get { return m_CSplitter.ShouldDrawBackground; }
            set 
            {
                m_CSplitter.ShouldDrawBackground = value;
                m_VSplitter.ShouldDrawBackground = value;
                m_HSplitter.ShouldDrawBackground = value;
            }
        }

        public int SplitterSize { get { return m_BarSize; } set { m_BarSize = value; } }

        protected void UpdateVSplitter()
        {
            m_VSplitter.MoveTo(m_VSplitter.X, (Height - m_VSplitter.Height) * (m_VVal));
        }

        protected void UpdateHSplitter()
        {
            m_HSplitter.MoveTo( ( Width - m_HSplitter.Width ) * ( m_HVal ), m_HSplitter.Y );
        }

        protected void UpdateCSplitter()
        {
            m_CSplitter.MoveTo((Width - m_CSplitter.Width) * (m_HVal), (Height - m_CSplitter.Height) * (m_VVal));
        }

        protected void onCenterMoved(Base control)
        {
            CalculateValueCenter();
            Invalidate();
        }

        protected void onVerticalMoved(Base control)
        {
            m_VVal = CalculateValueVertical();
            Invalidate();
        }

        protected void onHorizontalMoved(Base control)
        {
            m_HVal = CalculateValueHorizontal();
            Invalidate();
        }

        protected void CalculateValueCenter()
        {
            m_HVal = m_CSplitter.X / (float)(Width - m_CSplitter.Width);
            m_VVal = m_CSplitter.Y / (float)(Height - m_CSplitter.Height);
        }

        protected float CalculateValueVertical()
        {
            return m_VSplitter.Y / (float)(Height - m_VSplitter.Height);
        }

        protected float CalculateValueHorizontal()
        {
            return m_HSplitter.X / (float)(Width - m_HSplitter.Width);
        }

        protected override void Layout(Skin.Base skin)
        {
            m_VSplitter.SetSize(Width, m_BarSize);
            m_HSplitter.SetSize(m_BarSize, Height);
            m_CSplitter.SetSize(m_BarSize, m_BarSize);

            UpdateVSplitter();
            UpdateHSplitter();
            UpdateCSplitter();

            if (m_ZoomedSection == -1)
            {
                if (m_Sections[0] != null)
                    m_Sections[0].SetBounds(0, 0, m_HSplitter.X, m_VSplitter.Y);

                if (m_Sections[1] != null)
                    m_Sections[1].SetBounds(m_HSplitter.X + m_BarSize, 0, Width - (m_HSplitter.X + m_BarSize), m_VSplitter.Y);

                if (m_Sections[2] != null)
                    m_Sections[2].SetBounds(0, m_VSplitter.Y + m_BarSize, m_HSplitter.X, Height - (m_VSplitter.Y + m_BarSize));

                if (m_Sections[3] != null)
                    m_Sections[3].SetBounds(m_HSplitter.X + m_BarSize, m_VSplitter.Y + m_BarSize, Width - (m_HSplitter.X + m_BarSize), Height - (m_VSplitter.Y + m_BarSize));
            }
            else
            {
                //This should probably use Fill docking instead
                m_Sections[m_ZoomedSection].SetBounds(0, 0, Width, Height);
            }
        }

        public void SetPanel(int index, Base panel)
        {
            Debug.Assert(index >= 0 && index <= 3, "CrossSplitter::SetPanel out of range");

            m_Sections[index] = panel;

            if (panel != null)
            {
                panel.Dock = Pos.None;
                panel.Parent = this;
            }

            Invalidate();
        }

        public Base GetPanel(int index)
        {
            return m_Sections[index];
        }

        protected void ZoomChanged()
        {
            if (OnZoomChange != null)
                OnZoomChange.Invoke(this);
         
            if (m_ZoomedSection == -1)
            {
                if (OnUnZoomed != null)
                    OnUnZoomed.Invoke(this);
            }
            else
            {
                if (OnZoomed != null)
                    OnZoomed.Invoke(this);
            }
        }

        public void Zoom(int section)
        {
            UnZoom();

            if (m_Sections[section] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i != section && m_Sections[i] != null)
                        m_Sections[i].IsHidden = true;
                }
                m_ZoomedSection = section;

                Invalidate();
            }
            ZoomChanged();
        }

        public void UnZoom()
        {
            m_ZoomedSection = -1;

            for (int i = 0; i < 4; i++)
            {
                if (m_Sections[i] != null)
                    m_Sections[i].IsHidden = false;
            }

            Invalidate();
            ZoomChanged();
        }
    }
}
