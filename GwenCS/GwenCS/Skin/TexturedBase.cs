using System;
using System.Drawing;
using Gwen.Skin.Texturing;
using Single = Gwen.Skin.Texturing.Single;

namespace Gwen.Skin
{
    public struct SkinTextures
    {
        public struct _Window
        {
            public Single Close;
            public Single Close_Hover;
            public Single Close_Down;
            public Single Close_Disabled;
        }

        public struct _Scroller
        {
            public Bordered TrackV;
            public Bordered TrackH;
            public Bordered ButtonV_Normal;
            public Bordered ButtonV_Hover;
            public Bordered ButtonV_Down;
            public Bordered ButtonV_Disabled;
            public Bordered ButtonH_Normal;
            public Bordered ButtonH_Hover;
            public Bordered ButtonH_Down;
            public Bordered ButtonH_Disabled;

            public struct _Button
            {
                public Bordered[] Normal;
                public Bordered[] Hover;
                public Bordered[] Down;
                public Bordered[] Disabled;
            }

            public _Button Button;
        }

        public _Window Window;
        public _Scroller Scroller;
    }

    public class TexturedBase : Skin.Base
    {
        protected SkinTextures Textures;

        protected Color m_colBorderColor;
        protected Color m_colBG;
        protected Color m_colBGDark;
        protected Color m_colControlDarker;
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
        protected Bordered m_texButton_Dead;
        protected Bordered m_texButton_Pressed;

        protected Bordered m_texMenu_Strip, m_texMenuBG, m_texMenuBG_Margin;
        protected Bordered m_texMenuBG_Hover, m_texMenuBG_Spacer;
        protected Bordered m_texShadow;

        protected Bordered m_texTextBox, m_texTextBox_Focus, m_texTextBox_Disabled;

        protected Bordered m_texTab_Control, m_texTabBar;
        protected Bordered m_texTabB_Active, m_texTabB_Inactive;
        protected Bordered m_texTabT_Active, m_texTabT_Inactive;
        protected Bordered m_texTabL_Active, m_texTabL_Inactive;
        protected Bordered m_texTabR_Active, m_texTabR_Inactive;

        protected Bordered m_texWindow, m_texWindow_Inactive;
        protected Bordered m_texTreeBG;

        protected Single m_Checkbox, m_Checkbox_Checked;
        protected Single m_CheckboxD, m_CheckboxD_Checked;
        protected Single m_RadioButton, m_RadioButton_Checked;
        protected Single m_RadioButtonD, m_RadioButtonD_Checked;

        protected Single m_CheckMark;

        protected Single m_TreeMinus, m_TreePlus;

        public TexturedBase(Renderer.Base renderer, String TextureName) : base(renderer)
        {
            m_colBorderColor = Color.FromArgb(255, 80, 80, 80);
            m_colBG = Color.FromArgb(255, 248, 248, 248);
            m_colBGDark = Color.FromArgb(255, 235, 235, 235);

            m_colControlBright = Color.FromArgb(255, 255, 255, 255);
            m_colControlDark = Color.FromArgb(255, 214, 214, 214);
            m_colControlDarker = Color.FromArgb(255, 180, 180, 180);

            m_colHighlightBG = Color.FromArgb(255, 192, 221, 252);
            m_colHighlightBorder = Color.FromArgb(255, 51, 153, 255);

            m_colToolTipBackground = Color.FromArgb(255, 255, 225, 255);
            m_colToolTipBorder = Color.FromArgb(255, 0, 0, 0);

            m_colModal = Color.FromArgb(150, 25, 25, 25);

            m_DefaultFont.FaceName = "Microsoft Sans Serif";
            m_DefaultFont.Size = 11;

            m_Texture = new Texture(Renderer);
            m_Texture.Load(TextureName);

            Colors.Window.TitleActive = Renderer.PixelColour(m_Texture, 4, 508, Color.Red);
            Colors.Window.TitleInactive = Renderer.PixelColour(m_Texture, 12, 508, Color.Magenta);

            m_texWindow = new Bordered(m_Texture, 0, 0, 127, 127, new Margin(8, 32, 8, 8));
            m_texWindow_Inactive= new Bordered(m_Texture, 128, 0, 127, 127, new Margin(8, 32, 8, 8));
            m_texButton= new Bordered(m_Texture, 480, 0, 31, 31, new Margin(8, 8, 8, 8));
            m_texButton_Hovered= new Bordered(m_Texture, 480, 32, 31, 31, new Margin(8, 8, 8, 8));
            m_texButton_Dead= new Bordered(m_Texture, 480, 64, 31, 31, new Margin(8, 8, 8, 8));
            m_texButton_Pressed= new Bordered(m_Texture, 480, 96, 31, 31, new Margin(8, 8, 8, 8));
            m_texShadow= new Bordered(m_Texture, 448, 0, 31, 31, new Margin(8, 8, 8, 8));
            m_texTreeBG= new Bordered(m_Texture, 256, 128, 127, 127, new Margin(16, 16, 16, 16));
            m_Checkbox_Checked= new Single(m_Texture, 448, 32, 15, 15);
            m_Checkbox = new Single(m_Texture, 464, 32, 15, 15);
            m_CheckboxD_Checked = new Single(m_Texture, 448, 48, 15, 15);
            m_CheckboxD = new Single(m_Texture, 464, 48, 15, 15);
            m_RadioButton_Checked = new Single(m_Texture, 448, 64, 15, 15);
            m_RadioButton = new Single(m_Texture, 464, 64, 15, 15);
            m_RadioButtonD_Checked = new Single(m_Texture, 448, 80, 15, 15);
            m_RadioButtonD = new Single(m_Texture, 464, 80, 15, 15);
            m_TreePlus = new Single(m_Texture, 448, 96, 15, 15);
            m_TreeMinus = new Single(m_Texture, 464, 96, 15, 15);
            m_texMenu_Strip= new Bordered(m_Texture, 0, 128, 127, 21, new Margin(1, 1, 1, 1));
            m_texTextBox= new Bordered(m_Texture, 0, 150, 127, 21, new Margin(4, 4, 4, 4));
            m_texTextBox_Focus= new Bordered(m_Texture, 0, 172, 127, 21, new Margin(4, 4, 4, 4));
            m_texTextBox_Disabled= new Bordered(m_Texture, 0, 193, 127, 21, new Margin(4, 4, 4, 4));
            m_texMenuBG_Margin= new Bordered(m_Texture, 128, 128, 127, 63, new Margin(24, 8, 8, 8));
            m_texMenuBG= new Bordered(m_Texture, 128, 192, 127, 63, new Margin(8, 8, 8, 8));
            m_texMenuBG_Hover= new Bordered(m_Texture, 128, 256, 127, 31, new Margin(8, 8, 8, 8));
            m_texMenuBG_Spacer= new Bordered(m_Texture, 128, 288, 127, 3, new Margin(8, 8, 8, 8)); // TODO!

            m_texTab_Control= new Bordered(m_Texture, 0, 256, 127, 127, new Margin(8, 8, 8, 8));
            m_texTabB_Active= new Bordered(m_Texture, 0, 416, 63, 31, new Margin(8, 8, 8, 8));
            m_texTabB_Inactive= new Bordered(m_Texture, 0 + 128, 416, 63, 31, new Margin(8, 8, 8, 8));
            m_texTabT_Active= new Bordered(m_Texture, 0, 384, 63, 31, new Margin(8, 8, 8, 8));
            m_texTabT_Inactive= new Bordered(m_Texture, 0 + 128, 384, 63, 31, new Margin(8, 8, 8, 8));
            m_texTabL_Active= new Bordered(m_Texture, 64, 384, 31, 63, new Margin(8, 8, 8, 8));
            m_texTabL_Inactive= new Bordered(m_Texture, 64 + 128, 384, 31, 63, new Margin(8, 8, 8, 8));
            m_texTabR_Active= new Bordered(m_Texture, 96, 384, 31, 63, new Margin(8, 8, 8, 8));
            m_texTabR_Inactive= new Bordered(m_Texture, 96 + 128, 384, 31, 63, new Margin(8, 8, 8, 8));
            m_texTabBar= new Bordered(m_Texture, 128, 352, 127, 31, new Margin(4, 4, 4, 4));

            m_CheckMark = new Single(m_Texture, 145, 97, 16, 16);

            Textures.Window.Close = new Single(m_Texture, 0, 224, 24, 24);
            Textures.Window.Close_Hover = new Single(m_Texture, 32, 224, 24, 24);
            Textures.Window.Close_Hover = new Single(m_Texture, 64, 224, 24, 24);
            Textures.Window.Close_Hover = new Single(m_Texture, 96, 224, 24, 24);

            Textures.Scroller.TrackV = new Bordered(m_Texture, 384, 208, 15, 127, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonV_Normal= new Bordered(m_Texture, 384 + 16, 208, 15, 127, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonV_Hover= new Bordered(m_Texture, 384 + 32, 208, 15, 127, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonV_Down= new Bordered(m_Texture, 384 + 48, 208, 15, 127, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonV_Disabled= new Bordered(m_Texture, 384 + 64, 208, 15, 127, new Margin(4, 4, 4, 4));

            Textures.Scroller.TrackH = new Bordered(m_Texture, 384, 128, 127, 15, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonH_Normal= new Bordered(m_Texture, 384, 128 + 16, 127, 15, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonH_Hover= new Bordered(m_Texture, 384, 128 + 32, 127, 15, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonH_Down= new Bordered(m_Texture, 384, 128 + 48, 127, 15, new Margin(4, 4, 4, 4));
            Textures.Scroller.ButtonH_Disabled= new Bordered(m_Texture, 384, 128 + 64, 127, 15, new Margin(4, 4, 4, 4));

            Textures.Scroller.Button.Normal = new Bordered[4];
            Textures.Scroller.Button.Disabled = new Bordered[4];
            Textures.Scroller.Button.Hover = new Bordered[4];
            Textures.Scroller.Button.Down = new Bordered[4];

            for (int i = 0; i < 4; i++)
            {
                Textures.Scroller.Button.Normal[i] = new Bordered(m_Texture, 464 + 0, 208 + i * 16, 15, 15, new Margin(2, 2, 2, 2));
                Textures.Scroller.Button.Hover[i] = new Bordered(m_Texture, 480, 208 + i * 16, 15, 15, new Margin(2, 2, 2, 2));
                Textures.Scroller.Button.Down[i] = new Bordered(m_Texture, 464, 272 + i * 16, 15, 15, new Margin(2, 2, 2, 2));
                Textures.Scroller.Button.Disabled[i] = new Bordered(m_Texture, 480 + 48, 272 + i * 16, 15, 15, new Margin(2, 2, 2, 2));
            }
        }

        public override void DrawButton(Controls.Base control, bool bDepressed, bool bHovered, bool disabled)
        {
            if (bDepressed)
                m_texButton_Pressed.Draw(Renderer, control.RenderBounds);
            else
            {
                if (bHovered)
                    m_texButton_Hovered.Draw(Renderer, control.RenderBounds);
                else
                    m_texButton.Draw(Renderer, control.RenderBounds);
            }
        }

        public override void DrawMenuItem(Controls.Base control, bool bSubmenuOpen, bool bChecked)
        {
            if (bSubmenuOpen || control.IsHovered)
                m_texMenuBG_Hover.Draw(Renderer, control.RenderBounds);

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
                m_texMenuBG_Margin.Draw(Renderer, control.RenderBounds);
                return;
            }

            m_texMenuBG.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawShadow(Controls.Base control)
        {
            Rectangle r = control.RenderBounds;
            r.X -= 4;
            r.Y -= 4;
            r.Width += 10;
            r.Height += 10;
            m_texShadow.Draw(Renderer, r);
        }

        public override void DrawRadioButton(Controls.Base control, bool bSelected, bool bDepressed)
        {
            if (bSelected)
            {
                if (control.IsDisabled)
                    m_RadioButtonD_Checked.Draw(Renderer, control.RenderBounds);
                else
                    m_RadioButton_Checked.Draw(Renderer, control.RenderBounds);
            }
            else
            {
                if (control.IsDisabled)
                    m_RadioButtonD.Draw(Renderer, control.RenderBounds);
                else
                    m_RadioButton.Draw(Renderer, control.RenderBounds);
            }
        }

        public override void DrawCheckBox(Controls.Base control, bool bSelected, bool bDepressed)
        {
            if (bSelected)
            {
                if (control.IsDisabled)
                    m_CheckboxD_Checked.Draw(Renderer, control.RenderBounds);
                else
                    m_Checkbox_Checked.Draw(Renderer, control.RenderBounds);
            }
            else
            {
                if (control.IsDisabled)
                    m_CheckboxD.Draw(Renderer, control.RenderBounds);
                else
                    m_Checkbox.Draw(Renderer, control.RenderBounds);
            }
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
            if (control.IsDisabled)
            {
                m_texTextBox_Disabled.Draw(Renderer, control.RenderBounds);
                return;
            }

            if (control.HasFocus)
                m_texTextBox_Focus.Draw(Renderer, control.RenderBounds);
            else
                m_texTextBox.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawTabButton(Controls.Base control, bool bActive, Pos dir)
        {
            if (bActive)
            {
                DrawActiveTabButton(control, dir);
                return;
            }

            if (dir == Pos.Top)
            {
                m_texTabT_Inactive.Draw(Renderer, control.RenderBounds);
                return;
            }
            if (dir == Pos.Left)
            {
                m_texTabL_Inactive.Draw(Renderer, control.RenderBounds);
                return;
            }
            if (dir == Pos.Bottom)
            {
                m_texTabB_Inactive.Draw(Renderer, control.RenderBounds);
                return;
            }
            if (dir == Pos.Right)
            {
                m_texTabR_Inactive.Draw(Renderer, control.RenderBounds);
                return;
            }
        }

        private void DrawActiveTabButton(Controls.Base control, Pos dir)
        {
            Renderer.EndClip();
            if (dir == Pos.Top)
            {
                m_texTabT_Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(0, 0, 0, 8)));
                return;
            }
            if (dir == Pos.Left)
            {
                m_texTabL_Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(0, 0, 8, 0)));
                return;
            }
            if (dir == Pos.Bottom)
            {
                m_texTabB_Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(0, -8, 0, 8)));
                return;
            }
            if (dir == Pos.Right)
            {
                m_texTabR_Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(-8, 0, 8, 0)));
                return;
            }
        }

        public override void DrawTabControl(Controls.Base control)
        {
            m_texTab_Control.Draw(Renderer, control.RenderBounds);
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
            if (isHorizontal)
                Textures.Scroller.TrackH.Draw(Renderer, control.RenderBounds);
            else
                Textures.Scroller.TrackV.Draw(Renderer, control.RenderBounds);
        }

        public override void DrawScrollBarBar(Controls.Base control, bool bDepressed, bool isHovered, bool isHorizontal)
        {
            if (!isHorizontal)
            {
                if (control.IsDisabled)
                {
                    Textures.Scroller.ButtonV_Disabled.Draw(Renderer, control.RenderBounds);
                    return;
                }

                if (bDepressed)
                {
                    Textures.Scroller.ButtonV_Down.Draw(Renderer, control.RenderBounds);
                    return;
                }

                if (isHovered)
                {
                    Textures.Scroller.ButtonV_Hover.Draw(Renderer, control.RenderBounds);
                    return;
                }

                Textures.Scroller.ButtonV_Normal.Draw(Renderer, control.RenderBounds);
                return;
            }

            if (control.IsDisabled)
            {
                Textures.Scroller.ButtonH_Disabled.Draw(Renderer, control.RenderBounds);
                return;
            }

            if (bDepressed)
            {
                Textures.Scroller.ButtonH_Down.Draw(Renderer, control.RenderBounds);
                return;
            }

            if (isHovered)
            {
                Textures.Scroller.ButtonH_Hover.Draw(Renderer, control.RenderBounds);
                return;
            }

            Textures.Scroller.ButtonH_Normal.Draw(Renderer, control.RenderBounds);
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
                rect.Y += Global.Trunc(rect.Height * 0.4f);
                rect.Height -= Global.Trunc(rect.Height * 0.8f);
            }
            else
            {
                rect.X += Global.Trunc(rect.Width * 0.4f);
                rect.Width -= Global.Trunc(rect.Width * 0.8f);
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

        public override void DrawScrollButton(Controls.Base control, Pos iDirection, bool bDepressed, bool hovered, bool disabled)
        {
            int i = 0;
            if (iDirection == Pos.Top) i = 1;
            if (iDirection == Pos.Right) i = 2;
            if (iDirection == Pos.Bottom) i = 3;

            if (disabled)
            {
                Textures.Scroller.Button.Disabled[i].Draw(Renderer, control.RenderBounds);
                return;
            }

            if (bDepressed)
            {
                Textures.Scroller.Button.Down[i].Draw(Renderer, control.RenderBounds);
                return;
            }

            if (hovered)
            {
                Textures.Scroller.Button.Hover[i].Draw(Renderer, control.RenderBounds);
                return;
            }

            Textures.Scroller.Button.Normal[i].Draw(Renderer, control.RenderBounds);
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

            if (bOpen)
                m_TreeMinus.Draw(Renderer, rect);
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

            //Renderer.DrawColor = m_colControlOutlineLighter;

            Renderer.DrawFilledRect(new Rectangle(iWidth, rect.Y, 1, rect.Height));

            rect.Y += rect.Height - 1;
            rect.Height = 1;

            Renderer.DrawFilledRect(rect);
        }

        public override void DrawPropertyTreeNode(Controls.Base control, int BorderLeft, int BorderTop)
        {
            Rectangle rect = control.RenderBounds;

            //Renderer.DrawColor = m_colControlOutlineLighter;

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

                Renderer.DrawFilledRect(Global.FloatRect(0, 0, rect.Width * 0.5f, rect.Height * 0.5f));
                Renderer.DrawFilledRect(Global.FloatRect(rect.Width * 0.5f, rect.Height * 0.5f, rect.Width * 0.5f, rect.Height * 0.5f));
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

        public override void DrawWindowCloseButton(Controls.Base control, bool depressed, bool hovered, bool disabled)
        {

            if (disabled)
            {
                Textures.Window.Close_Disabled.Draw(Renderer, control.RenderBounds);
                return;
            }

            if (depressed)
            {
                Textures.Window.Close_Down.Draw(Renderer, control.RenderBounds);
                return;
            }

            if (hovered)
            {
                Textures.Window.Close_Hover.Draw(Renderer, control.RenderBounds);
                return;
            }

            Textures.Window.Close.Draw(Renderer, control.RenderBounds);
        }
    }
}
