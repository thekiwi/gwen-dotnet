using System;
using System.Drawing;
using Gwen.Control;

namespace Gwen.ControlInternal
{
    /// <summary>
    /// Displays text.
    /// </summary>
    public class Text : Base
    {
        private String m_String;
        private Font m_Font;

        /// <summary>
        /// Font used to display the text.
        /// </summary>
        public Font Font
        {
            get { return m_Font; }
            set
            {
                if (m_Font != null)
                    m_Font.Dispose();
                m_Font = value;
                SizeToContents(); 
                Invalidate();
            }
        }

        /// <summary>
        /// Text to display.
        /// </summary>
        public String String
        {
            get { return m_String; } 
            set { m_String = value; if (AutoSizeToContents) SizeToContents(); Invalidate(); /*InvalidateParent();*/ }
        }

        /// <summary>
        /// Text color.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Determines whether the control should be automatically resized to fit the text.
        /// </summary>
        public bool AutoSizeToContents { get; set; } // [omeg] added

        /// <summary>
        /// Text length in characters.
        /// </summary>
        public int Length { get { return String.Length; } }

        /// <summary>
        /// Text color override - used by tooltips.
        /// </summary>
        public Color TextColorOverride { get; set; }

        /// <summary>
        /// Text override - used to display different string.
        /// </summary>
        public String TextOverride { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public Text(Base parent)
            : base(parent)
        {
            Font = Skin.DefaultFont;
            String = string.Empty;
            TextColor = Skin.Colors.Label.Default;
            MouseInputEnabled = false;
            TextColorOverride = Color.FromArgb(0, 255, 255, 255); // A==0, override disabled
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_Font.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            if (Length == 0 || Font == null) return;

            if (TextColorOverride.A == 0)
                skin.Renderer.DrawColor = TextColor;
            else
                skin.Renderer.DrawColor = TextColorOverride;

            skin.Renderer.RenderText(Font, Point.Empty, TextOverride ?? String);
        }

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            SizeToContents();
        }

        /// <summary>
        /// Handler invoked when control's scale changes.
        /// </summary>
        protected override void OnScaleChanged()
        {
            Invalidate();
        }

        /// <summary>
        /// Sizes the control to its contents.
        /// </summary>
        public void SizeToContents()
        {
            if (String == null)
                return;

            if (Font == null)
            {
                throw new InvalidOperationException("Text.SizeToContents() - No Font!!\n");
            }

            Point p = new Point(1, Font.Size);

            if (Length > 0)
            {
                p = Skin.Renderer.MeasureText(Font, TextOverride ?? String);
            }

            if (p.X == Width && p.Y == Height)
                return;

            SetSize(p.X, p.Y);
            InvalidateParent();
            Invalidate();
        }

        /// <summary>
        /// Gets the coordinates of specified character in the text.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <returns>Character position in local coordinates.</returns>
        public Point GetCharacterPosition(int index)
        {
            if (Length == 0 || index == 0)
            {
                return new Point(1, 0);
            }

            String sub = (TextOverride ?? String).Substring(0, index);
            Point p = Skin.Renderer.MeasureText(Font, sub);

            if (p.Y >= Font.Size)
                p = new Point(p.X, p.Y - Font.Size);

            return p;
        }

        /// <summary>
        /// Searches for a character closest to given point.
        /// </summary>
        /// <param name="p">Point.</param>
        /// <returns>Character index.</returns>
        public int GetClosestCharacter(Point p)
        {
            int distance = MaxCoord;
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
