using System;
using System.Drawing;
using Gwen.ControlInternal;

namespace Gwen.Control
{
    /// <summary>
    /// Base resizable control.
    /// </summary>
    public class ResizableControl : Base
    {
        protected bool m_ClampMovement;
        protected readonly Resizer[] m_Resizer;

        /// <summary>
        /// Determines whether control's position should be restricted to its parent bounds.
        /// </summary>
        public bool ClampMovement { get { return m_ClampMovement; } set { m_ClampMovement = value; } }

        /// <summary>
        /// Invoked when the control has been resized.
        /// </summary>
        public event ControlCallback OnResized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableControl"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public ResizableControl(Base parent) : base(parent)
        {
            m_Resizer = new Resizer[10];
            MinimumSize = new Point(5, 5);
            m_ClampMovement = false;

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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            foreach (Resizer resizer in m_Resizer)
            {
                if (resizer != null)
                    resizer.Dispose();
            }
            base.Dispose();
        }

        /// <summary>
        /// Handler for the resized event.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onResized(Base control)
        {
            if (OnResized != null)
                OnResized.Invoke(this);
        }

        protected Resizer GetResizer(int i)
        {
            return m_Resizer[i];
        }

        /// <summary>
        /// Disables resizing.
        /// </summary>
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

        /// <summary>
        /// Enables resizing.
        /// </summary>
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

        /// <summary>
        /// Sets the control bounds.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <returns>
        /// True if bounds changed.
        /// </returns>
        public override bool SetBounds(int x, int y, int width, int height)
        {
            Point minSize = MinimumSize;
            // Clamp Minimum Size
            if (width < minSize.X) width = minSize.X;
            if (height < minSize.Y) height = minSize.Y;

            // Clamp to parent's window
            Base parent = Parent;
            if (parent != null && m_ClampMovement)
            {
                if (x + width > parent.Width) x = parent.Width - width;
                if (x < 0) x = 0;
                if (y + height > parent.Height) y = parent.Height - height;
                if (y < 0) y = 0;
            }

            return base.SetBounds(x, y, width, height);
        }
    }
}
