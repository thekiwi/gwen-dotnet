using System;
using System.Collections.Generic;
using System.Drawing;
using Gwen.Skin.Texturing;
using Single = Gwen.Skin.Texturing.Single;

namespace Gwen.Skin
{
    public class TexturedBase : Skin.Base
    {
        protected Color m_colBorderColor;
        protected Color m_colControlOutlineLight;
        protected Color m_colControlOutlineLighter;
        protected Color m_colBG;
        protected Color m_colBGDark;
        protected Color m_colControl;
        protected Color m_colControlBorderHighlight;
        protected Color m_colControlDarker;
        protected Color m_colControlOutlineNormal;
        protected Color m_colControlBright;
        protected Color m_colControlDark;
        protected Color m_colHighlightBG;
        protected Color m_colHighlightBorder;
        protected Color m_colToolTipBackground;
        protected Color m_colToolTipBorder;
        protected Color m_colModal;

        protected Texture m_Texture;

        protected Bordered m_texButton;
        protected Bordered m_texButton_Hovered;
        protected Bordered m_texButton_Pressed;

        protected Bordered m_texMenu_Strip, m_texMenu_Panel, m_texMenu_Panel_Border;
        protected Bordered m_texMenu_Hover;
        protected Bordered m_texShadow;

        protected Bordered m_texTextBox, m_texTextBox_Focus;

        protected Bordered m_texTab_Control, m_texTab, m_texTab_Inactive, m_texTab_Gap, m_texTabBar;

        protected Bordered m_texWindow, m_texWindow_Inactive;
        protected Bordered m_texTreeBG;

        protected Single m_Checkbox, m_Checkbox_Checked;
        protected Single m_RadioButton, m_RadioButton_Checked;

        protected Single m_CheckMark;

        protected Single m_TreeMinus, m_TreePlus;

        public virtual void Init(String TextureName)
        {
            m_colBorderColor = Color.FromArgb(255, 80, 80, 80);
            m_colBG = Color.FromArgb(255, 248, 248, 248);
            m_colBGDark = Color.FromArgb(255, 235, 235, 235);

            m_colControl = Color.FromArgb(255, 240, 240, 240);
            m_colControlBright = Color.FromArgb(255, 255, 255, 255);
            m_colControlDark = Color.FromArgb(255, 214, 214, 214);
            m_colControlDarker = Color.FromArgb(255, 180, 180, 180);

            m_colControlOutlineNormal = Color.FromArgb(255, 112, 112, 112);
            m_colControlOutlineLight = Color.FromArgb(255, 144, 144, 144);
            m_colControlOutlineLighter = Color.FromArgb(255, 210, 210, 210);

            m_colHighlightBG = Color.FromArgb(255, 192, 221, 252);
            m_colHighlightBorder = Color.FromArgb(255, 51, 153, 255);

            m_colToolTipBackground = Color.FromArgb(255, 255, 225, 255);
            m_colToolTipBorder = Color.FromArgb(255, 0, 0, 0);

            m_colModal = Color.FromArgb(150, 25, 25, 25);

            m_DefaultFont.FaceName = "Microsoft Sans Serif";
            m_DefaultFont.Size = 11;

            m_Texture = new Texture();
            m_Texture.Load(TextureName, Renderer);

            m_texButton = new Bordered(m_Texture, 194, 0, 24, 24, new Margin(8, 8, 8, 8));
            m_texButton_Hovered = new Bordered(m_Texture, 194, 25, 24, 24, new Margin(8, 8, 8, 8));
            m_texButton_Pressed = new Bordered(m_Texture, 194, 50, 24, 24, new Margin(8, 8, 8, 8));

            m_texMenu_Strip = new Bordered(m_Texture, 194, 75, 62, 21, new Margin(8, 8, 8, 8));
            m_texMenu_Panel = new Bordered(m_Texture, 194, 130, 62, 32, new Margin(8, 8, 8, 8));
            m_texMenu_Panel_Border = new Bordered(m_Texture, 194, 97, 62, 32, new Margin(24, 8, 8, 8));
            m_texMenu_Hover = new Bordered(m_Texture, 219, 50, 24, 24, new Margin(8, 8, 8, 8));

            m_texShadow = new Bordered(m_Texture, 223, 0, 32, 32, new Margin(8, 8, 8, 8));

            m_texTextBox = new Bordered(m_Texture, 0, 122, 24, 24, new Margin(8, 8, 8, 8));
            m_texTextBox_Focus = new Bordered(m_Texture, 25, 122, 24, 24, new Margin(8, 8, 8, 8));

            m_texTab = new Bordered(m_Texture, 0, 97, 24, 24, new Margin(8, 8, 8, 8));
            m_texTab_Inactive = new Bordered(m_Texture, 25, 97, 24, 24, new Margin(8, 8, 8, 8));
            m_texTab_Control = new Bordered(m_Texture, 50, 97, 24, 24, new Margin(8, 8, 8, 8));
            m_texTab_Gap = new Bordered(m_Texture, 50 + 8, 97 + 8, 8, 8, new Margin(8, 8, 8, 8));
            m_texTabBar = new Bordered(m_Texture, 0, 147, 74, 16, new Margin(4, 4, 4, 4));


            m_texWindow = new Bordered(m_Texture, 0, 0, 96, 96, new Margin(16, 32, 16, 16));
            m_texWindow_Inactive = new Bordered(m_Texture, 97, 0, 96, 96, new Margin(16, 32, 16, 16));

            m_Checkbox = new Single(m_Texture, 75, 97, 16, 16);
            m_Checkbox_Checked = new Single(m_Texture, 93, 97, 16, 16);

            m_RadioButton = new Single(m_Texture, 110, 97, 16, 16);
            m_RadioButton_Checked = new Single(m_Texture, 127, 97, 16, 16);


            m_CheckMark = new Single(m_Texture, 145, 97, 16, 16);
            m_TreeMinus = new Single(m_Texture, 75, 115, 11, 11);
            m_TreePlus = new Single(m_Texture, 93, 115, 11, 11);

            m_texTreeBG = new Bordered(m_Texture, 0, 164, 49, 49, new Margin(16, 16, 16, 16));
        }

        public override void DrawButton(Controls.Base control, bool bDepressed, bool bHovered)
        {
            if (bDepressed)
                m_texButton_Pressed.Draw(Renderer, control.RenderBounds);
            else
                m_texButton.Draw(Renderer, control.RenderBounds);

            if (bHovered)
                m_texButton_Hovered.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawMenuItem(Controls.Base control, bool bSubmenuOpen, bool bChecked)
        {
            if (bSubmenuOpen || control.IsHovered)
                m_texMenu_Hover.Draw(Renderer, control.RenderBounds);

            if (bChecked)
                m_CheckMark.Draw(Renderer, new Rectangle(control.RenderBounds.X + 2, control.RenderBounds.Y + 2, 16, 16));
        }

        public override void DrawMenuStrip(Controls.Base control)
        {
            m_texMenu_Strip.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawMenu(Controls.Base control, bool bPaddingDisabled)
        {
            if (!bPaddingDisabled)
            {
                m_texMenu_Panel_Border.Draw(Renderer, control.RenderBounds);
                return;
            }

            m_texMenu_Panel.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawShadow(Controls.Base control)
        {
            Rectangle r = control.RenderBounds;
            r.X -= 8;
            r.Y -= 8;
            r.Width += 16;
            r.Height += 16;
            //	m_texShadow.Draw( r );
        }

        public override void DrawRadioButton(Controls.Base control, bool bSelected, bool bDepressed)
        {
            if (bSelected)
                m_RadioButton_Checked.Draw(Renderer, control.RenderBounds);
            else
                m_RadioButton.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawCheckBox(Controls.Base control, bool bSelected, bool bDepressed)
        {
            if (bSelected)
                m_Checkbox_Checked.Draw(Renderer, control.RenderBounds);
            else
                m_Checkbox.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawGroupBox(Controls.Base control, int textStart, int textHeight, int textWidth)
        {
            Rectangle rect = control.RenderBounds;

            rect.Y += Global.Trunc(textHeight * 0.5f);
            rect.Height -= Global.Trunc(textHeight * 0.5f);

            Color m_colDarker = Color.FromArgb(50, 0, 50, 60);
            Color m_colLighter = Color.FromArgb(150, 255, 255, 255);

            Renderer.DrawColor = m_colLighter;

            Renderer.DrawFilledRect(new Rectangle(rect.X + 1, rect.Y + 1, textStart - 3, 1));
            Renderer.DrawFilledRect(new Rectangle(rect.X + 1 + textStart + textWidth, rect.Y + 1, rect.Width - textStart + textWidth - 2, 1));
            Renderer.DrawFilledRect(new Rectangle(rect.X + 1, (rect.Y + rect.Height) - 1, rect.X + rect.Width - 2, 1));

            Renderer.DrawFilledRect(new Rectangle(rect.X + 1, rect.Y + 1, 1, rect.Height));
            Renderer.DrawFilledRect(new Rectangle((rect.X + rect.Width) - 2, rect.Y + 1, 1, rect.Height - 1));

            Renderer.DrawColor = m_colDarker;

            Renderer.DrawFilledRect(new Rectangle(rect.X + 1, rect.Y, textStart - 3, 1));
            Renderer.DrawFilledRect(new Rectangle(rect.X + 1 + textStart + textWidth, rect.Y, rect.Width - textStart - textWidth - 2, 1));
            Renderer.DrawFilledRect(new Rectangle(rect.X + 1, (rect.Y + rect.Height) - 1, rect.X + rect.Width - 2, 1));

            Renderer.DrawFilledRect(new Rectangle(rect.X, rect.Y + 1, 1, rect.Height - 1));
            Renderer.DrawFilledRect(new Rectangle((rect.X + rect.Width) - 1, rect.Y + 1, 1, rect.Height - 1));
        }

        public override void DrawTextBox(Controls.Base control)
        {
            Rectangle rect = control.RenderBounds;
            bool bHasFocus = control.HasFocus;

            if (bHasFocus)
                m_texTextBox_Focus.Draw(Renderer, control.RenderBounds);
            else
                m_texTextBox.Draw(Renderer, control.RenderBounds);


            //I dunno what this is for yet
            /*
            if ( CursorRect.Width == 1 )
            {
                if ( bHasFocus )
                {
                    Renderer.DrawColor =  Gwen::Color( 0, 0, 0, 200 ) );
                    Renderer.DrawFilledRect( CursorRect );	
                }
            }
            else
            {
                if ( bHasFocus )
                {
                    Renderer.DrawColor =  Gwen::Color( 50, 150, 255, 250 ) );
                    Renderer.DrawFilledRect( CursorRect );	
                }
            }
            */
        }

        public override void DrawTabButton(Controls.Base control, bool bActive)
        {
            if (bActive)
                m_texTab.Draw(Renderer, control.RenderBounds);
            else
                m_texTab_Inactive.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawTabControl(Controls.Base control, Rectangle CurrentButtonRect)
        {
            m_texTab_Control.Draw(Renderer, control.RenderBounds);

            if (CurrentButtonRect.Width > 0 && CurrentButtonRect.Height > 0)
                m_texTab_Gap.Draw(Renderer, CurrentButtonRect);
        }

        public override void DrawTabTitleBar(Controls.Base control)
        {
            m_texTabBar.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawWindow(Controls.Base control, int topHeight, bool inFocus)
        {
            Rectangle rect = control.RenderBounds;

            if (inFocus) m_texWindow.Draw(Renderer, control.RenderBounds);
            else m_texWindow_Inactive.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawHighlight(Controls.Base control)
        {
            Rectangle rect = control.RenderBounds;
            Renderer.DrawColor = Color.FromArgb(255, 255, 100, 255);
            Renderer.DrawFilledRect(rect);
        }

        public override void DrawScrollBar(Controls.Base control, bool isHorizontal, bool bDepressed)
        {
            Rectangle rect = control.RenderBounds;
            if (bDepressed)
                Renderer.DrawColor = m_colControlDarker;
            else
                Renderer.DrawColor = m_colControlBright;
            Renderer.DrawFilledRect(rect);
        }

        public override void DrawScrollBarBar(Controls.Base control, bool bDepressed, bool isHovered, bool isHorizontal)
        {
            //TODO: something specialized
            DrawButton(control, bDepressed, isHovered);
        }

        public override void DrawProgressBar(Controls.Base control, bool isHorizontal, float progress)
        {
            Rectangle rect = control.RenderBounds;
            Color FillColour = Color.FromArgb(255, 0, 211, 40);

            if (isHorizontal)
            {
                //Background
                Renderer.DrawColor = m_colControlDark;
                Renderer.DrawFilledRect(new Rectangle(1, 1, rect.Width - 2, rect.Height - 2));

                //Right half
                Renderer.DrawColor = FillColour;
                Renderer.DrawFilledRect(new Rectangle(1, 1, Global.Trunc(rect.Width * progress - 2), rect.Height - 2));

                Renderer.DrawColor = Color.FromArgb(150, 255, 255, 255);
                Renderer.DrawFilledRect(new Rectangle(1, 1, rect.Width - 2, Global.Trunc(rect.Height * 0.45f)));
            }
            else
            {
                //Background 
                Renderer.DrawColor = m_colControlDark;
                Renderer.DrawFilledRect(new Rectangle(1, 1, rect.Width - 2, rect.Height - 2));

                //Top half
                Renderer.DrawColor = FillColour;
                Renderer.DrawFilledRect(Global.FloatRect(1, 1 + (rect.Height * (1 - progress)), rect.Width - 2, rect.Height * progress - 2));

                Renderer.DrawColor = Color.FromArgb(150, 255, 255, 255);
                Renderer.DrawFilledRect(new Rectangle(1, 1, Global.Trunc(rect.Width * 0.45f), rect.Height - 2));
            }

            Renderer.DrawColor = Color.FromArgb(150, 255, 255, 255);
            Renderer.DrawShavedCornerRect(new Rectangle(1, 1, rect.Width - 2, rect.Height - 2));

            Renderer.DrawColor = Color.FromArgb(70, 255, 255, 255);
            Renderer.DrawShavedCornerRect(new Rectangle(2, 2, rect.Width - 4, rect.Height - 4));

            Renderer.DrawColor = m_colBorderColor;
            Renderer.DrawShavedCornerRect(rect);
        }

        public override void DrawListBox(Controls.Base control)
        {
            Rectangle rect = control.RenderBounds;

            Renderer.DrawColor = m_colControlBright;
            Renderer.DrawFilledRect(rect);

            Renderer.DrawColor = m_colBorderColor;
            Renderer.DrawLinedRect(rect);
        }

        public override void DrawListBoxLine(Controls.Base control, bool bSelected)
        {
            Rectangle rect = control.RenderBounds;

            if (bSelected)
            {
                Renderer.DrawColor = m_colHighlightBorder;
                Renderer.DrawFilledRect(rect);
            }
            else if (control.IsHovered)
            {
                Renderer.DrawColor = m_colHighlightBG;
                Renderer.DrawFilledRect(rect);
            }
        }

        public override void DrawSlider(Controls.Base control, bool bIsHorizontal, int numNotches, int barSize)
        {
            Rectangle rect = control.RenderBounds;
            Rectangle notchRect = rect;

            if (bIsHorizontal)
            {
                rect.Y += Global.Trunc(rect.Height * 0.4);
                rect.Height -= Global.Trunc(rect.Height * 0.8);
            }
            else
            {
                rect.X += Global.Trunc(rect.Width * 0.4);
                rect.Width -= Global.Trunc(rect.Width * 0.8);
            }

            Renderer.DrawColor = m_colBGDark;
            Renderer.DrawFilledRect(rect);

            Renderer.DrawColor = m_colControlDarker;
            Renderer.DrawLinedRect(rect);
        }

        public override void DrawComboBox(Controls.Base control)
        {
            DrawTextBox(control);
        }

        public override void DrawKeyboardHighlight(Controls.Base control, Rectangle r, int iOffset)
        {
            Rectangle rect = r;

            rect.X += iOffset;
            rect.Y += iOffset;
            rect.Width -= iOffset * 2;
            rect.Height -= iOffset * 2;

            //draw the top and bottom
            bool skip = true;
            for (int i = 0; i < rect.Width * 0.5; i++)
            {
                m_Render.DrawColor = Color.Black;
                if (!skip)
                {
                    Renderer.DrawPixel(rect.X + (i * 2), rect.Y);
                    Renderer.DrawPixel(rect.X + (i * 2), rect.Y + rect.Height - 1);
                }
                else
                    skip = !skip;
            }
            skip = false;
            for (int i = 0; i < rect.Height * 0.5; i++)
            {
                Renderer.DrawColor = Color.Black;
                if (!skip)
                {
                    Renderer.DrawPixel(rect.X, rect.Y + i * 2);
                    Renderer.DrawPixel(rect.X + rect.Width - 1, rect.Y + i * 2);
                }
                else
                    skip = !skip;
            }
        }

        public override void DrawToolTip(Controls.Base control)
        {
            Rectangle rct = control.RenderBounds;
            rct.X -= 3;
            rct.Y -= 3;
            rct.Width += 6;
            rct.Height += 6;

            Renderer.DrawColor = m_colToolTipBackground;
            Renderer.DrawFilledRect(rct);

            Renderer.DrawColor = m_colToolTipBorder;
            Renderer.DrawLinedRect(rct);
        }

        public override void DrawScrollButton(Controls.Base control, Pos iDirection, bool bDepressed)
        {
            DrawButton(control, bDepressed, false);

            m_Render.DrawColor = Color.FromArgb(240, 0, 0, 0);

            Rectangle r = new Rectangle(control.Width / 2 - 2, control.Height / 2 - 2, 5, 5);

            if (iDirection == Pos.Top) DrawArrowUp(r);
            else if (iDirection == Pos.Bottom) DrawArrowDown(r);
            else if (iDirection == Pos.Left) DrawArrowLeft(r);
            else DrawArrowRight(r);
        }

        public override void DrawComboBoxButton(Controls.Base control, bool bDepressed)
        {
            m_Render.DrawColor = Color.FromArgb(240, 0, 0, 0);

            Rectangle r = new Rectangle(control.Width / 2 - 2, control.Height / 2 - 2, 5, 5);
            DrawArrowDown(r);
        }

        public override void DrawNumericUpDownButton(Controls.Base control, bool bDepressed, bool bUp)
        {
            //	DrawButton( control.Widthidth(), control.Heighteight(), bDepressed, false, true );

            m_Render.DrawColor = Color.FromArgb(240, 0, 0, 0);

            Rectangle r = new Rectangle(control.Width / 2 - 2, control.Height / 2 - 2, 5, 5);

            if (bUp) DrawArrowUp(r);
            else DrawArrowDown(r);
        }

        public override void DrawStatusBar(Controls.Base control)
        {
            DrawBackground(control);
        }


        public override void DrawBackground(Controls.Base control)
        {
            Rectangle rect = control.RenderBounds;
            m_Render.DrawColor = m_colBGDark;
            m_Render.DrawFilledRect(rect);
            m_Render.DrawColor = m_colControlDarker;
            m_Render.DrawLinedRect(rect);
        }

        public override void DrawTreeButton(Controls.Base control, bool bOpen)
        {
            Rectangle rect = control.RenderBounds;

            rect.X += 2;
            rect.Y += 2;
            rect.Width -= 2;
            rect.Height -= 2;
            if (bOpen)
            {
                m_TreeMinus.Draw(Renderer, rect);
            }
            else
                m_TreePlus.Draw(Renderer, rect);
        }

        public override void DrawTreeControl(Controls.Base control)
        {
            m_texTreeBG.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawPropertyRow(Controls.Base control, int iWidth, bool bBeingEdited)
        {
            Rectangle rect = control.RenderBounds;

            if (bBeingEdited)
            {
                Renderer.DrawColor = m_colHighlightBG;
                Renderer.DrawFilledRect(new Rectangle(0, rect.Y, iWidth, rect.Height));
            }

            Renderer.DrawColor = m_colControlOutlineLighter;

            Renderer.DrawFilledRect(new Rectangle(iWidth, rect.Y, 1, rect.Height));

            rect.Y += rect.Height - 1;
            rect.Height = 1;

            Renderer.DrawFilledRect(rect);
        }

        public override void DrawPropertyTreeNode(Controls.Base control, int BorderLeft, int BorderTop)
        {
            Rectangle rect = control.RenderBounds;

            Renderer.DrawColor = m_colControlOutlineLighter;

            Renderer.DrawFilledRect(new Rectangle(rect.X, rect.Y, BorderLeft, rect.Height));
            Renderer.DrawFilledRect(new Rectangle(rect.X + BorderLeft, rect.Y, rect.Width - BorderLeft, BorderTop));
        }

        public override void DrawTreeNode(Controls.Base ctrl, bool bOpen, bool bSelected, int iLabelHeight, int iLabelWidth, int iHalfWay, int iLastBranch, bool bIsRoot)
        {
            if (bSelected)
            {
                Renderer.DrawColor = Color.FromArgb(100, 0, 150, 255);
                Renderer.DrawFilledRect(new Rectangle(17, 0, iLabelWidth + 2, iLabelHeight - 1));
                Renderer.DrawColor = Color.FromArgb(200, 0, 150, 255);
                Renderer.DrawLinedRect(new Rectangle(17, 0, iLabelWidth + 2, iLabelHeight - 1));
            }

            Renderer.DrawColor = Color.FromArgb(50, 0, 0, 0);

            if (!bIsRoot)
                Renderer.DrawFilledRect(new Rectangle(9, iHalfWay, 16 - 9, 1));

            if (!bOpen) return;

            Renderer.DrawFilledRect(new Rectangle(14 + 8, iLabelHeight, 1, iLastBranch + iHalfWay - iLabelHeight));
        }

        public override void DrawColorDisplay(Controls.Base control, Color color)
        {
            Rectangle rect = control.RenderBounds;

            if (color.A != 255)
            {
                Renderer.DrawColor = Color.FromArgb(255, 255, 255, 255);
                Renderer.DrawFilledRect(rect);

                Renderer.DrawColor = Color.FromArgb(128, 128, 128, 128);

                Renderer.DrawFilledRect(Global.FloatRect(0, 0, rect.Width * 0.5, rect.Height * 0.5));
                Renderer.DrawFilledRect(Global.FloatRect(rect.Width * 0.5, rect.Height * 0.5, rect.Width * 0.5, rect.Height * 0.5));
            }

            Renderer.DrawColor = color;
            Renderer.DrawFilledRect(rect);

            Renderer.DrawColor = Color.Black;
            Renderer.DrawLinedRect(rect);
        }

        public override void DrawModalControl(Controls.Base control)
        {
            if (control.ShouldDrawBackground)
            {
                Rectangle rect = control.RenderBounds;
                Renderer.DrawColor = m_colModal;
                Renderer.DrawFilledRect(rect);
            }
        }

        public override void DrawMenuDivider(Controls.Base control)
        {
            Rectangle rect = control.RenderBounds;
            Renderer.DrawColor = m_colBGDark;
            Renderer.DrawFilledRect(rect);
            Renderer.DrawColor = m_colControlDarker;
            Renderer.DrawLinedRect(rect);
        }
    }
}
