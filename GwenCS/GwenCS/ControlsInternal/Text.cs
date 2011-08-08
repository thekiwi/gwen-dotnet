using System;
using System.Drawing;
using Gwen.Controls;

namespace Gwen.ControlsInternal
{
    public class Text : Base
    {
        protected String m_String;

        public Font Font;
        public String String
        {
            get { return m_String; } 
            set { m_String = value; if (AutoSizeToContents) RefreshSize(); Invalidate(); InvalidateParent(); }
        }
        public Color TextColor { get; set; }
        public bool AutoSizeToContents { get; set; } // [omeg] added

        public int Length { get { return String.Length; } }

        public Text(Base parent) : base(parent)
        {
            Font = Skin.DefaultFont;
            String = string.Empty;
            TextColor = Color.Black; // TODO: From skin somehow..
            MouseInputEnabled = false;
        }

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);
            if (Length == 0 || Font.FaceName == null) return;

            skin.Renderer.DrawColor = TextColor;
            skin.Renderer.RenderText(ref Font, Point.Empty, String);
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);
            RefreshSize();
        }

        internal override void onScaleChanged()
        {
            base.onScaleChanged();
            Invalidate();
        }

        public void RefreshSize()
        {
            if (Font.FaceName == null)
            {
                throw new NullReferenceException("Text.RefreshSize() - No Font!!\n");
            }

            Point p = new Point(1, Font.Size);

            if (Length > 0)
            {
                p = Skin.Renderer.MeasureText(ref Font, String);
            }

            if (p.X == Width && p.Y == Height)
                return;

            SetSize(p.X, p.Y);
            InvalidateParent();
            Invalidate();
        }

        public Point GetCharacterPosition(int index)
        {
            if (Length == 0 || index == 0)
            {
                return new Point(1, 0);
            }

            String sub = String.Substring(0, index);
            Point p = Skin.Renderer.MeasureText(ref Font, sub);

            if (p.Y >= Font.Size)
                p = new Point(p.X, p.Y - Font.Size);

            return p;
        }

        public int GetClosestCharacter(Point p)
        {
            int iDistance = Global.MaxCoord;
            int iChar = 0;

            for (int i = 0; i < String.Length + 1; i++)
            {
                Point cp = GetCharacterPosition(i);
                int iDist = Math.Abs(cp.X - p.X) + Math.Abs(cp.Y - p.Y); // this isn't proper // [omeg] todo: sqrt

                if (iDist > iDistance) 
                    continue;

                iDistance = iDist;
                iChar = i;
            }

            return iChar;
        }

    }
}
