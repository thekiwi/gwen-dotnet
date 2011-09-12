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

        protected override void onMouseMoved(int x, int y, int dx, int dy)
        {
            if (null == m_Target) return;
            if (!m_Depressed) return;

            Rectangle oldBounds = m_Target.Bounds;
            Rectangle bounds = m_Target.Bounds;

            Point min = m_Target.MinimumSize;

            Point pCursorPos = m_Target.CanvasPosToLocal(new Point(x, y));

            Point delta = m_Target.LocalPosToCanvas(m_HoldPos);
            delta.X -= x;
            delta.Y -= y;

            if (m_ResizeDir.HasFlag(Pos.Left))
            {
                bounds.X -= delta.X;
                bounds.Width += delta.X;

                // Conform to minimum size here so we don't
                // go all weird when we snap it in the base conrt

                if (bounds.Width < min.X)
                {
                    int diff = min.X - bounds.Width;
                    bounds.Width += diff;
                    bounds.X -= diff;
                }
            }

            if (m_ResizeDir.HasFlag(Pos.Top))
            {
                bounds.Y -= delta.Y;
                bounds.Height += delta.Y;

                // Conform to minimum size here so we don't
                // go all weird when we snap it in the base conrt

                if (bounds.Height < min.Y)
                {
                    int diff = min.Y - bounds.Height;
                    bounds.Height += diff;
                    bounds.Y -= diff;
                }
            }

            if (m_ResizeDir.HasFlag(Pos.Right))
            {
                // This is complicated.
                // Basically we want to use the HoldPos, so it doesn't snap to the edge of the control
                // But we need to move the HoldPos with the window movement. Yikes.
                // I actually think this might be a big hack around the way this control works with regards
                // to the holdpos being on the parent panel.

                int woff = bounds.Width - m_HoldPos.X;
                int diff = bounds.Width;
                bounds.Width = pCursorPos.X + woff;
                if (bounds.Width < min.X) bounds.Width = min.X;
                diff -= bounds.Width;

                m_HoldPos.X -= diff;
            }

            if (m_ResizeDir.HasFlag(Pos.Bottom))
            {
                int hoff = bounds.Height - m_HoldPos.Y;
                int diff = bounds.Height;
                bounds.Height = pCursorPos.Y + hoff;
                if (bounds.Height < min.Y) bounds.Height = min.Y;
                diff -= bounds.Height;

                m_HoldPos.Y -= diff;
            }

            m_Target.SetBounds(bounds);

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
