using System;
using System.Drawing;

namespace Gwen.Skin
{
    // bit ugly...
    public struct SkinColors
    {
        public struct _Window
        {
            public Color TitleActive;
            public Color TitleInactive;
        }

        public struct _Button
        {
            public Color Normal;
            public Color Hover;
            public Color Down;
            public Color Disabled;
        }

        public struct _Tab
        {
            public struct _Inactive
            {
                public Color Normal;
                public Color Hover;
                public Color Down;
                public Color Disabled;
            }

            public struct _Active
            {
                public Color Normal;
                public Color Hover;
                public Color Down;
                public Color Disabled;
            }

            public _Inactive Inactive;
            public _Active Active;
        }

        public struct _Label
        {
            public Color Default;
            public Color Bright;
            public Color Dark;
            public Color Highlight;
        }

        public struct _Tree
        {
            public Color Lines;
            public Color Normal;
            public Color Hover;
            public Color Selected;
        }

        public struct _Properties
        {
            public Color Line_Normal;
            public Color Line_Selected;
            public Color Line_Hover;
            public Color Column_Normal;
            public Color Column_Selected;
            public Color Column_Hover;
            public Color Label_Normal;
            public Color Label_Selected;
            public Color Label_Hover;
            public Color Border;
            public Color Title;
        }

        public struct _Category
        {
            public Color Header;
            public Color Header_Closed;

            public struct _Line
            {
                public Color Text;
                public Color Text_Hover;
                public Color Text_Selected;
                public Color Button;
                public Color Button_Hover;
                public Color Button_Selected;
            }

            public struct _LineAlt
            {
                public Color Text;
                public Color Text_Hover;
                public Color Text_Selected;
                public Color Button;
                public Color Button_Hover;
                public Color Button_Selected;
            }

            public _Line Line;
            public _LineAlt LineAlt;
        }

        public Color ModalBackground;
        public Color TooltipText;

        public _Window Window;
        public _Button Button;
        public _Tab Tab;
        public _Label Label;
        public _Tree Tree;
        public _Properties Properties;
        public _Category Category;
    }

    public class Base : IDisposable
    {
        protected Font m_DefaultFont = new Font();
        protected Renderer.Base m_Render;

        public SkinColors Colors;

        public Font DefaultFont { get { return m_DefaultFont; } }

        public Renderer.Base Renderer { get { return m_Render; } }

        protected Base(Renderer.Base renderer)
        {
            m_Render = renderer;
        }
        
        public virtual void Dispose()
        {
            ReleaseFont(m_DefaultFont);
        }
        
        protected virtual void ReleaseFont(Font font)
        {
            if (font == null)
                return;
            if (m_Render == null)
                return;
            m_Render.FreeFont(font);
        }
        
        public virtual void SetDefaultFont(String faceName, int size = 10)
        {
            m_DefaultFont.FaceName = faceName;
            m_DefaultFont.Size = size;
        }

        public virtual void DrawButton(Controls.Base control, bool depressed, bool hovered, bool disabled) { }
        public virtual void DrawTabButton(Controls.Base control, bool active, Pos dir) { }
        public virtual void DrawTabControl(Controls.Base control) { }
        public virtual void DrawTabTitleBar(Controls.Base control) { }

        public virtual void DrawMenuItem(Controls.Base control, bool submenuOpen, bool isChecked) { }
        public virtual void DrawMenuRightArrow(Controls.Base control) { }
        public virtual void DrawMenuStrip(Controls.Base control) { }
        public virtual void DrawMenu(Controls.Base control, bool paddingDisabled) { }
        public virtual void DrawRadioButton(Controls.Base control, bool selected, bool depressed) { }
        public virtual void DrawCheckBox(Controls.Base control, bool selected, bool depressed) { }
        public virtual void DrawGroupBox(Controls.Base control, int textStart, int textHeight, int textWidth) { }
        public virtual void DrawTextBox(Controls.Base control) { }
        public virtual void DrawWindow(Controls.Base control, int topHeight, bool inFocus) { }
        public virtual void DrawWindowCloseButton(Controls.Base control, bool depressed, bool hovered, bool disabled) { }
        public virtual void DrawHighlight(Controls.Base control) { }
        public virtual void DrawStatusBar(Controls.Base control) { }

        public virtual void DrawShadow(Controls.Base control) { }
        public virtual void DrawScrollBarBar(Controls.Base control, bool depressed, bool hovered, bool horizontal) { }
        public virtual void DrawScrollBar(Controls.Base control, bool horizontal, bool depressed) { }
        public virtual void DrawScrollButton(Controls.Base control, Pos direction, bool depressed, bool hovered, bool disabled) { }
        public virtual void DrawProgressBar(Controls.Base control, bool horizontal, float progress) { }

        public virtual void DrawListBox(Controls.Base control) { }
        public virtual void DrawListBoxLine(Controls.Base control, bool selected, bool even) { }

        public virtual void DrawSlider(Controls.Base control, bool horizontal, int numNotches, int barSize) { }
        public virtual void DrawSliderButton(Controls.Base control, bool depressed, bool horizontal) { }

        public virtual void DrawComboBox(Controls.Base control, bool down, bool isMenuOpen) { }
        public virtual void DrawComboBoxArrow(Controls.Base control, bool hovered, bool depressed, bool open, bool disabled) { }
        public virtual void DrawKeyboardHighlight(Controls.Base control, Rectangle rect, int offset) { }
        public virtual void DrawToolTip(Controls.Base control) { }

        public virtual void DrawNumericUpDownButton(Controls.Base control, bool depressed, bool up) { }

        public virtual void DrawTreeButton(Controls.Base control, bool open) { }
        public virtual void DrawTreeControl(Controls.Base control) { }
        public virtual void DrawTreeNode(Controls.Base ctrl, bool open, bool selected, int labelHeight, int labelWidth, int halfWay, int lastBranch, bool isRoot)
        {
            Renderer.DrawColor = Colors.Tree.Lines;

            if (!isRoot)
                Renderer.DrawFilledRect(new Rectangle(8, halfWay, 16 - 9, 1));

            if (!open) return;

            Renderer.DrawFilledRect(new Rectangle(14 + 7, labelHeight + 1, 1, lastBranch + halfWay - labelHeight));
        }

        public virtual void DrawPropertyRow(Controls.Base control, int iWidth, bool bBeingEdited, bool hovered)
        {
            Rectangle rect = control.RenderBounds;

            if (bBeingEdited)
                m_Render.DrawColor = Colors.Properties.Column_Selected;
            else if (hovered)
                m_Render.DrawColor = Colors.Properties.Column_Hover;
            else
                m_Render.DrawColor = Colors.Properties.Column_Normal;

            m_Render.DrawFilledRect(new Rectangle(0, rect.Y, iWidth, rect.Height));

            if (bBeingEdited)
                m_Render.DrawColor = Colors.Properties.Line_Selected;
            else if (hovered)
                m_Render.DrawColor = Colors.Properties.Line_Hover;
            else
                m_Render.DrawColor = Colors.Properties.Line_Normal;

            m_Render.DrawFilledRect(new Rectangle(iWidth, rect.Y, 1, rect.Height));

            rect.Y += rect.Height - 1;
            rect.Height = 1;

            m_Render.DrawFilledRect(rect);
        }

        public virtual void DrawColorDisplay(Controls.Base control, Color color) { }
        public virtual void DrawModalControl(Controls.Base control) { }
        public virtual void DrawMenuDivider(Controls.Base control) { }

        public virtual void DrawCategoryHolder(Controls.Base control) { }
        public virtual void DrawCategoryInner(Controls.Base control, bool collapsed) { }

        public virtual void DrawPropertyTreeNode(Controls.Base control, int BorderLeft, int BorderTop)
        {
            Rectangle rect = control.RenderBounds;

            m_Render.DrawColor = Colors.Properties.Border;

            m_Render.DrawFilledRect(new Rectangle(rect.X, rect.Y, BorderLeft, rect.Height));
            m_Render.DrawFilledRect(new Rectangle(rect.X + BorderLeft, rect.Y, rect.Width - BorderLeft, BorderTop));
        }

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

            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 0.0f, rect.Y + y * 1.0f, x, y * 1.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 1.0f, x, y * 2.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x, y * 3.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 1.0f, x, y * 2.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 4.0f, rect.Y + y * 1.0f, x, y * 1.0f));
        }

        public virtual void DrawArrowUp(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 0.0f, rect.Y + y * 3.0f, x, y * 1.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x, y * 2.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x, y * 3.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 2.0f, x, y * 2.0f));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 4.0f, rect.Y + y * 3.0f, x, y * 1.0f));
        }

        public virtual void DrawArrowLeft(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 0.0f, x * 1.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x * 3.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 3.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 4.0f, x * 1.0f, y));
        }

        public virtual void DrawArrowRight(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 0.0f, x * 1.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 1.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x * 3.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 3.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 4.0f, x * 1.0f, y));
        }

        public virtual void DrawCheck(Rectangle rect)
        {
            float x = (rect.Width / 5.0f);
            float y = (rect.Height / 5.0f);

            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 0.0f, rect.Y + y * 3.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 1.0f, rect.Y + y * 4.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 2.0f, rect.Y + y * 3.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 3.0f, rect.Y + y * 1.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Util.FloatRect(rect.X + x * 4.0f, rect.Y + y * 0.0f, x * 2, y * 2));
        }
    }
}
