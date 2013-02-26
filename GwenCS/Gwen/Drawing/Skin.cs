using System;
using System.Drawing;
using Gwen.Controls;

namespace Gwen.Drawing
{
    /// <summary>
    /// Base skin.
    /// </summary>
    public abstract class Skin : IDisposable
    {
        protected Font m_DefaultFont;
        protected readonly Renderer m_Renderer;

        /// <summary>
        /// Colors of various UI elements.
        /// </summary>
        public SkinColors Colors;

        /// <summary>
        /// Default font to use when rendering text if none specified.
        /// </summary>
        public Font DefaultFont
        {
            get { return m_DefaultFont; }
            set
            {
                m_DefaultFont.Dispose();
                m_DefaultFont = value;
            }
        }

        /// <summary>
        /// Renderer used.
        /// </summary>
        public Renderer Renderer { get { return m_Renderer; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base"/> class.
        /// </summary>
        /// <param name="renderer">Renderer to use.</param>
        protected Skin(Renderer renderer)
        {
            m_DefaultFont = new Font(renderer);
            m_Renderer = renderer;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            m_DefaultFont.Dispose();
            GC.SuppressFinalize(this);
        }

#if DEBUG
        ~Skin()
        {
            throw new InvalidOperationException(String.Format("IDisposable object finalized: {0}", GetType()));
            //Debug.Print(String.Format("IDisposable object finalized: {0}", GetType()));
        }
#endif

        /// <summary>
        /// Releases the specified font.
        /// </summary>
        /// <param name="font">Font to release.</param>
        protected virtual void ReleaseFont(Font font)
        {
            if (font == null)
                return;
            if (m_Renderer == null)
                return;
            m_Renderer.FreeFont(font);
        }
        
        /// <summary>
        /// Sets the default text font.
        /// </summary>
        /// <param name="faceName">Font name. Meaning can vary depending on the renderer.</param>
        /// <param name="size">Font size.</param>
        public virtual void SetDefaultFont(String faceName, int size = 10)
        {
            m_DefaultFont.FaceName = faceName;
            m_DefaultFont.Size = size;
        }

        #region UI elements
        public virtual void DrawButton(Control control, bool depressed, bool hovered, bool disabled) { }
        public virtual void DrawTabButton(Control control, bool active, Pos dir) { }
        public virtual void DrawTabControl(Control control) { }
        public virtual void DrawTabTitleBar(Control control) { }
        public virtual void DrawMenuItem(Control control, bool submenuOpen, bool isChecked) { }
        public virtual void DrawMenuRightArrow(Control control) { }
        public virtual void DrawMenuStrip(Control control) { }
        public virtual void DrawMenu(Control control, bool paddingDisabled) { }
        public virtual void DrawRadioButton(Control control, bool selected, bool depressed) { }
        public virtual void DrawCheckBox(Control control, bool selected, bool depressed) { }
        public virtual void DrawGroupBox(Control control, int textStart, int textHeight, int textWidth) { }
        public virtual void DrawTextBox(Control control) { }
        public virtual void DrawWindow(Control control, int topHeight, bool inFocus) { }
        public virtual void DrawWindowCloseButton(Control control, bool depressed, bool hovered, bool disabled) { }
        public virtual void DrawHighlight(Control control) { }
        public virtual void DrawStatusBar(Control control) { }
        public virtual void DrawShadow(Control control) { }
        public virtual void DrawScrollBarBar(Control control, bool depressed, bool hovered, bool horizontal) { }
        public virtual void DrawScrollBar(Control control, bool horizontal, bool depressed) { }
        public virtual void DrawScrollButton(Control control, Pos direction, bool depressed, bool hovered, bool disabled) { }
        public virtual void DrawProgressBar(Control control, bool horizontal, float progress) { }
        public virtual void DrawListBox(Control control) { }
        public virtual void DrawListBoxLine(Control control, bool selected, bool even) { }
        public virtual void DrawSlider(Control control, bool horizontal, int numNotches, int barSize) { }
        public virtual void DrawSliderButton(Control control, bool depressed, bool horizontal) { }
        public virtual void DrawComboBox(Control control, bool down, bool isMenuOpen) { }
        public virtual void DrawComboBoxArrow(Control control, bool hovered, bool depressed, bool open, bool disabled) { }
        public virtual void DrawKeyboardHighlight(Control control, Rectangle rect, int offset) { }
        public virtual void DrawToolTip(Control control) { }
        public virtual void DrawNumericUpDownButton(Control control, bool depressed, bool up) { }
        public virtual void DrawTreeButton(Control control, bool open) { }
        public virtual void DrawTreeControl(Control control) { }

        public virtual void DrawDebugOutlines(Control control)
        {
            m_Renderer.DrawColor = control.PaddingOutlineColor;
            Rectangle inner = new Rectangle(control.Bounds.Left + control.Padding.Left,
                                            control.Bounds.Top + control.Padding.Top,
                                            control.Bounds.Width - control.Padding.Right - control.Padding.Left,
                                            control.Bounds.Height - control.Padding.Bottom - control.Padding.Top);
            m_Renderer.DrawLinedRect(inner);

            m_Renderer.DrawColor = control.MarginOutlineColor;
            Rectangle outer = new Rectangle(control.Bounds.Left - control.Margin.Left,
                                            control.Bounds.Top - control.Margin.Top,
                                            control.Bounds.Width + control.Margin.Right + control.Margin.Left,
                                            control.Bounds.Height + control.Margin.Bottom + control.Margin.Top);
            m_Renderer.DrawLinedRect(outer);

            m_Renderer.DrawColor = control.BoundsOutlineColor;
            m_Renderer.DrawLinedRect(control.Bounds);
        }

        public virtual void DrawTreeNode(Control ctrl, bool open, bool selected, int labelHeight, int labelWidth, int halfWay, int lastBranch, bool isRoot)
        {
            Renderer.DrawColor = Colors.Tree.Lines;

            if (!isRoot)
                Renderer.DrawFilledRect(new Rectangle(8, halfWay, 16 - 9, 1));

            if (!open) return;

            Renderer.DrawFilledRect(new Rectangle(14 + 7, labelHeight + 1, 1, lastBranch + halfWay - labelHeight));
        }

        public virtual void DrawPropertyRow(Control control, int iWidth, bool bBeingEdited, bool hovered)
        {
            Rectangle rect = control.RenderBounds;

            if (bBeingEdited)
                m_Renderer.DrawColor = Colors.Properties.Column_Selected;
            else if (hovered)
                m_Renderer.DrawColor = Colors.Properties.Column_Hover;
            else
                m_Renderer.DrawColor = Colors.Properties.Column_Normal;

            m_Renderer.DrawFilledRect(new Rectangle(0, rect.Y, iWidth, rect.Height));

            if (bBeingEdited)
                m_Renderer.DrawColor = Colors.Properties.Line_Selected;
            else if (hovered)
                m_Renderer.DrawColor = Colors.Properties.Line_Hover;
            else
                m_Renderer.DrawColor = Colors.Properties.Line_Normal;

            m_Renderer.DrawFilledRect(new Rectangle(iWidth, rect.Y, 1, rect.Height));

            rect.Y += rect.Height - 1;
            rect.Height = 1;

            m_Renderer.DrawFilledRect(rect);
        }

        public virtual void DrawColorDisplay(Control control, Color color) { }
        public virtual void DrawModalControl(Control control) { }
        public virtual void DrawMenuDivider(Control control) { }
        public virtual void DrawCategoryHolder(Control control) { }
        public virtual void DrawCategoryInner(Control control, bool collapsed) { }

        public virtual void DrawPropertyTreeNode(Control control, int BorderLeft, int BorderTop)
        {
            Rectangle rect = control.RenderBounds;

            m_Renderer.DrawColor = Colors.Properties.Border;

            m_Renderer.DrawFilledRect(new Rectangle(rect.X, rect.Y, BorderLeft, rect.Height));
            m_Renderer.DrawFilledRect(new Rectangle(rect.X + BorderLeft, rect.Y, rect.Width - BorderLeft, BorderTop));
        }
#endregion

        #region Symbols for Simple skin
        /*
        Here we're drawing a few symbols such as the directional arrows and the checkbox check

        Texture'd skins don't generally use these - but the Simple skin does. We did originally
        use the marlett font to draw these.. but since that's a Windows font it wasn't a very
        good cross platform solution.
        */
        
        public virtual void DrawArrowDown(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 0.0f, rect.Y + y * 1.0f, x, y * 1.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 1.0f, x, y * 2.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x, y * 3.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 1.0f, x, y * 2.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 4.0f, rect.Y + y * 1.0f, x, y * 1.0f));
        }

        public virtual void DrawArrowUp(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 0.0f, rect.Y + y * 3.0f, x, y * 1.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x, y * 2.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x, y * 3.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 2.0f, x, y * 2.0f));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 4.0f, rect.Y + y * 3.0f, x, y * 1.0f));
        }

        public virtual void DrawArrowLeft(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 0.0f, x * 1.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x * 2.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x * 3.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 3.0f, x * 2.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 4.0f, x * 1.0f, y));
        }

        public virtual void DrawArrowRight(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 0.0f, x * 1.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 1.0f, x * 2.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x * 3.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 3.0f, x * 2.0f, y));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 4.0f, x * 1.0f, y));
        }

        public virtual void DrawCheck(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 0.0f, rect.Y + y * 3.0f, x * 2, y * 2));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 4.0f, x * 2, y * 2));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 3.0f, x * 2, y * 2));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 1.0f, x * 2, y * 2));
            m_Renderer.DrawFilledRect(Util.FloatRect(rect.X + x * 4.0f, rect.Y + y * 0.0f, x * 2, y * 2));
        }
        #endregion
    }
}
