using System;
using System.Drawing;
using System.Windows.Forms;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class Resizer : Dragger
    {
        protected Pos m_ResizeDir;

        public event ControlCallback OnResize;

        public Resizer(Base parent) : base(parent)
        {
            m_ResizeDir = Pos.Left;
            MouseInputEnabled = true;
            SetSize(6, 6);
            Target = parent;
        }

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            if (null == m_pTarget) return;
            if (!m_bDepressed) return;

            Rectangle oldBounds = m_pTarget.Bounds;
            Rectangle pBounds = m_pTarget.Bounds;

            Point pntMin = m_pTarget.MinimumSize;

            Point pCursorPos = m_pTarget.CanvasPosToLocal(new Point(x, y));

            Point pDelta = m_pTarget.LocalPosToCanvas(m_HoldPos);
            pDelta.X -= x;
            pDelta.Y -= y;

            if (m_ResizeDir.HasFlag(Pos.Left))
            {
                pBounds.X -= pDelta.X;
                pBounds.Width += pDelta.X;

                // Conform to minimum size here so we don't
                // go all weird when we snap it in the base conrt

                if (pBounds.Width < pntMin.X)
                {
                    int diff = pntMin.X - pBounds.Width;
                    pBounds.Width += diff;
                    pBounds.X -= diff;
                }

            }

            if (m_ResizeDir.HasFlag(Pos.Top))
            {
                pBounds.Y -= pDelta.Y;
                pBounds.Height += pDelta.Y;

                // Conform to minimum size here so we don't
                // go all weird when we snap it in the base conrt

                if (pBounds.Height < pntMin.Y)
                {
                    int diff = pntMin.Y - pBounds.Height;
                    pBounds.Height += diff;
                    pBounds.Y -= diff;
                }

            }

            if (m_ResizeDir.HasFlag(Pos.Right))
            {
                // This is complicated.
                // Basically we want to use the HoldPos, so it doesn't snap to the edge of the control
                // But we need to move the HoldPos with the window movement. Yikes.
                // I actually think this might be a big hack around the way this control works with regards
                // to the holdpos being on the parent panel.

                int woff = pBounds.Width - m_HoldPos.X;
                int diff = pBounds.Width;
                pBounds.Width = pCursorPos.X + woff;
                if (pBounds.Width < pntMin.X) pBounds.Width = pntMin.X;
                diff -= pBounds.Width;

                m_HoldPos.X -= diff;
            }

            if (m_ResizeDir.HasFlag(Pos.Bottom))
            {
                int hoff = pBounds.Height - m_HoldPos.Y;
                int diff = pBounds.Height;
                pBounds.Height = pCursorPos.Y + hoff;
                if (pBounds.Height < pntMin.Y) pBounds.Height = pntMin.Y;
                diff -= pBounds.Height;

                m_HoldPos.Y -= diff;
            }

            m_pTarget.SetBounds(pBounds);

            if (OnResize != null)
                OnResize.Invoke(this);
        }

        public Pos ResizeDir
        {
            set
            {
                if ((value.HasFlag(Pos.Left) && value.HasFlag(Pos.Top)) || (value.HasFlag(Pos.Right) && value.HasFlag(Pos.Bottom)))
                {
                    Cursor = Cursors.SizeNWSE;
                    return;
                }
                if ((value.HasFlag(Pos.Right) && value.HasFlag(Pos.Top)) || (value.HasFlag(Pos.Left) && value.HasFlag(Pos.Bottom)))
                {
                    Cursor = Cursors.SizeNESW;
                    return;
                }
                if (value.HasFlag(Pos.Right) && value.HasFlag(Pos.Left))
                {
                    Cursor = Cursors.SizeWE;
                    return;
                }
                if (value.HasFlag(Pos.Top) && value.HasFlag(Pos.Bottom))
                {
                    Cursor = Cursors.SizeNS;
                    return;
                }
            }
        }
    }
}
