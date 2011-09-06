using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    public class ResizableControl : Base
    {
        protected bool m_bClampMovement;
        protected Resizer[] m_Resizer;

        public bool ClampMovement { get { return m_bClampMovement; } set { m_bClampMovement = value; } }

        public event ControlCallback OnResized;

        public ResizableControl(Base parent) : base(parent)
        {
            m_Resizer = new Resizer[10];
            MinimumSize = new Point(5, 5);
            m_bClampMovement = false;

            m_Resizer[2] = new Resizer(this);
            m_Resizer[2].Dock = Pos.Bottom;
            m_Resizer[2].ResizeDir = Pos.Bottom;
            m_Resizer[2].OnResize += onResized;

            m_Resizer[1] = new Resizer(m_Resizer[2]);
            m_Resizer[1].Dock = Pos.Left;
            m_Resizer[1].ResizeDir = Pos.Bottom | Pos.Left;
            m_Resizer[1].OnResize += onResized;

            m_Resizer[3] = new Resizer(m_Resizer[2]);
            m_Resizer[3].Dock = Pos.Right;
            m_Resizer[3].ResizeDir = Pos.Bottom | Pos.Right;
            m_Resizer[3].OnResize += onResized;

            m_Resizer[8] = new Resizer(this);
            m_Resizer[8].Dock = Pos.Top;
            m_Resizer[8].ResizeDir = Pos.Top;
            m_Resizer[8].OnResize += onResized;

            m_Resizer[7] = new Resizer(m_Resizer[8]);
            m_Resizer[7].Dock = Pos.Left;
            m_Resizer[7].ResizeDir = Pos.Left | Pos.Top;
            m_Resizer[7].OnResize += onResized;

            m_Resizer[9] = new Resizer(m_Resizer[8]);
            m_Resizer[9].Dock = Pos.Right;
            m_Resizer[9].ResizeDir = Pos.Right | Pos.Top;
            m_Resizer[9].OnResize += onResized;

            m_Resizer[4] = new Resizer(this);
            m_Resizer[4].Dock = Pos.Left;
            m_Resizer[4].ResizeDir = Pos.Left;
            m_Resizer[4].OnResize += onResized;

            m_Resizer[6] = new Resizer(this);
            m_Resizer[6].Dock = Pos.Right;
            m_Resizer[6].ResizeDir = Pos.Right;
            m_Resizer[6].OnResize += onResized;
        }

        public override void Dispose()
        {
            foreach (Resizer resizer in m_Resizer)
            {
                if (resizer != null)
                    resizer.Dispose();
            }
            base.Dispose();
        }

        protected virtual void onResized(Base control)
        {
            if (OnResized != null)
                OnResized.Invoke(this);
        }

        public Resizer GetResizer(int i)
        {
            return m_Resizer[i];
        }

        public void DisableResizing()
        {
            for (int i = 0; i < 10; i++)
            {
                if (m_Resizer[i] == null)
                    continue;
                m_Resizer[i].MouseInputEnabled = false;
                m_Resizer[i].IsHidden = true;
                Padding = new Padding(m_Resizer[i].Width, m_Resizer[i].Width, m_Resizer[i].Width, m_Resizer[i].Width);
            }
        }

        public void EnableResizing()
        {
            for (int i = 0; i < 10; i++)
            {
                if (m_Resizer[i] == null)
                    continue;
                m_Resizer[i].MouseInputEnabled = true;
                m_Resizer[i].IsHidden = false;
                Padding = new Padding(0, 0, 0, 0); // todo: check if ok
            }
        }

        public override bool SetBounds(int x, int y, int w, int h)
        {
            Point minSize = MinimumSize;
            // Clamp Minimum Size
            if (w < minSize.X) w = minSize.X;
            if (h < minSize.Y) h = minSize.Y;

            // Clamp to parent's window
            Base pParent = Parent;
            if (pParent != null && m_bClampMovement)
            {
                if (x + w > pParent.Width) x = pParent.Width - w;
                if (x < 0) x = 0;
                if (y + h > pParent.Height) y = pParent.Height - h;
                if (y < 0) y = 0;
            }

            return base.SetBounds(x, y, w, h);
        }
    }
}
