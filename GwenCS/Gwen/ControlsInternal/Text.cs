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
            set { m_String = value; if (AutoSizeToContents) RefreshSize(); Invalidate(); /*InvalidateParent();*/ }
        }
        public Color TextColor { get; set; }
        public bool AutoSizeToContents { get; set; } // [omeg] added
        public int Length { get { return String.Length; } }

        public Color TextColorOverride { get; set; }

        public Text(Base parent) : base(parent)
        {
            Font = Skin.DefaultFont;
            String = string.Empty;
            TextColor = Skin.Colors.Label.Default;
            MouseInputEnabled = false;
            TextColorOverride = Color.FromArgb(0, 255, 255, 255); // A==0, override disabled
        }

        protected override void Render(Skin.Base skin)
        {
            if (Length == 0 || Font.FaceName == null) return;

            if (TextColorOverride.A == 0)
                skin.Renderer.DrawColor = TextColor;
            else
                skin.Renderer.DrawColor = TextColorOverride;
            skin.Renderer.RenderText(ref Font, Point.Empty, String);
        }

        protected override void Layout(Skin.Base skin)
        {
            RefreshSize();
        }

        internal override void onScaleChanged()
        {
            Invalidate();
        }

        public void RefreshSize()
        {
            if (Font.FaceName == null)
            {
                throw new InvalidOperationException("Text.RefreshSize() - No Font!!\n");
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
            int distance = Global.MaxCoord;
            int c = 0;

            for (int i = 0; i < String.Length + 1; i++)
            {
                Point cp = GetCharacterPosition(i);
                int dist = Math.Abs(cp.X - p.X) + Math.Abs(cp.Y - p.Y); // this isn't proper // [omeg] todo: sqrt

                if (dist > distance) 
                    continue;

                distance = dist;
                c = i;
            }

            return c;
        }
    }
}
