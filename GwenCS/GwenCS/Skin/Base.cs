using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Skin
{
    public class Base
    {
        protected Font m_DefaultFont = new Font();
        protected Renderer.Base m_Render;

        public virtual Font DefaultFont { get { return m_DefaultFont; } }

        public virtual Renderer.Base Renderer { get { return m_Render; } set { m_Render = value; } }

        public Base()
        {
            m_DefaultFont.FaceName = "Arial";
            m_DefaultFont.Size = 10;
        }

        ~Base()
        {
            ReleaseFont(m_DefaultFont);
        }

        public virtual void ReleaseFont(Font font)
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

        public virtual void DrawButton(Controls.Base control, bool bDepressed, bool bHovered) { }
        public virtual void DrawTabButton(Controls.Base control, bool bActive) { }
        public virtual void DrawTabControl(Controls.Base control, Rectangle CurrentButtonRect) { }
        public virtual void DrawTabTitleBar(Controls.Base control) { }

        public virtual void DrawMenuItem(Controls.Base control, bool bSubmenuOpen, bool bChecked) { }
        public virtual void DrawMenuStrip(Controls.Base control) { }
        public virtual void DrawMenu(Controls.Base control, bool bPaddingDisabled) { }
        public virtual void DrawRadioButton(Controls.Base control, bool bSelected, bool bDepressed) { }
        public virtual void DrawCheckBox(Controls.Base control, bool bSelected, bool bDepressed) { }
        public virtual void DrawGroupBox(Controls.Base control, int textStart, int textHeight, int textWidth) { }
        public virtual void DrawTextBox(Controls.Base control) { }
        public virtual void DrawWindow(Controls.Base control, int topHeight, bool inFocus) { }
        public virtual void DrawHighlight(Controls.Base control) { }
        public virtual void DrawBackground(Controls.Base control) { }
        public virtual void DrawStatusBar(Controls.Base control) { }

        public virtual void DrawShadow(Controls.Base control) { }
        public virtual void DrawScrollBarBar(Controls.Base control, bool bDepressed, bool isHovered, bool isHorizontal) { }
        public virtual void DrawScrollBar(Controls.Base control, bool isHorizontal, bool bDepressed) { }
        public virtual void DrawScrollButton(Controls.Base control, Pos iDirection, bool bDepressed) { }
        public virtual void DrawProgressBar(Controls.Base control, bool isHorizontal, float progress) { }

        public virtual void DrawListBox(Controls.Base control) { }
        public virtual void DrawListBoxLine(Controls.Base control, bool bSelected) { }

        public virtual void DrawSlider(Controls.Base control, bool bIsHorizontal, int numNotches, int barSize) { }
        public virtual void DrawComboBox(Controls.Base control) { }
        public virtual void DrawComboBoxButton(Controls.Base control, bool bDepressed) { }
        public virtual void DrawKeyboardHighlight(Controls.Base control, Rectangle rect, int offset) { }
        //public virtual void DrawComboBoxKeyboardHighlight( Controls.Base control );
        public virtual void DrawToolTip(Controls.Base control) { }

        public virtual void DrawNumericUpDownButton(Controls.Base control, bool bDepressed, bool bUp) { }

        public virtual void DrawTreeButton(Controls.Base control, bool bOpen) { }
        public virtual void DrawTreeControl(Controls.Base control) { }
        public virtual void DrawTreeNode(Controls.Base ctrl, bool bOpen, bool bSelected, int iLabelHeight, int iLabelWidth, int iHalfWay, int iLastBranch, bool bIsRoot) { }

        public virtual void DrawPropertyRow(Controls.Base control, int iWidth, bool bBeingEdited) { }
        public virtual void DrawPropertyTreeNode(Controls.Base control, int BorderLeft, int BorderTop) { }
        public virtual void DrawColorDisplay(Controls.Base control, Color color) { }
        public virtual void DrawModalControl(Controls.Base control) { }
        public virtual void DrawMenuDivider(Controls.Base control) { }

        /*
        Here we're drawing a few symbols such as the directional arrows and the checkbox check

        Texture'd skins don't generally use these - but the Simple skin does. We did originally
        use the marlett font to draw these.. but since that's a Windows font it wasn't a very
        good cross platform solution.
        */

        public virtual void DrawArrowDown(Rectangle rect)
        {
            double x = (rect.Width / 5.0);
            double y = (rect.Height / 5.0);

            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 0.0, rect.Y + y * 1.0, x, y * 1.0));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0, rect.Y + y * 1.0, x, y * 2.0));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 2.0, rect.Y + y * 1.0, x, y * 3.0));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 3.0, rect.Y + y * 1.0, x, y * 2.0));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 4.0, rect.Y + y * 1.0, x, y * 1.0));
        }

        public virtual void DrawArrowUp(Rectangle rect)
        {
            double x = (rect.Width / 5.0);
            double y = (rect.Height / 5.0);

            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 0.0f, rect.Y + y * 3.0f, x, y * 1.0f));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x, y * 2.0f));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x, y * 3.0f));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 3.0f, rect.Y + y * 2.0f, x, y * 2.0f));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 4.0f, rect.Y + y * 3.0f, x, y * 1.0f));
        }

        public virtual void DrawArrowLeft(Rectangle rect)
        {
            double x = (rect.Width / 5.0);
            double y = (rect.Height / 5.0);

            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 3.0f, rect.Y + y * 0.0f, x * 1.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 2.0f, rect.Y + y * 1.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x * 3.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 2.0f, rect.Y + y * 3.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 3.0f, rect.Y + y * 4.0f, x * 1.0f, y));
        }

        public virtual void DrawArrowRight(Rectangle rect)
        {
            double x = (rect.Width / 5.0);
            double y = (rect.Height / 5.0);

            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 0.0f, x * 1.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 1.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 2.0f, x * 3.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 3.0f, x * 2.0f, y));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 4.0f, x * 1.0f, y));
        }

        public virtual void DrawCheck(Rectangle rect)
        {
            double x = (rect.Width / 5.0);
            double y = (rect.Height / 5.0);

            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 0.0f, rect.Y + y * 3.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 1.0f, rect.Y + y * 4.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 2.0f, rect.Y + y * 3.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 3.0f, rect.Y + y * 1.0f, x * 2, y * 2));
            m_Render.DrawFilledRect(Global.FloatRect(rect.X + x * 4.0f, rect.Y + y * 0.0f, x * 2, y * 2));
        }
    }
}
