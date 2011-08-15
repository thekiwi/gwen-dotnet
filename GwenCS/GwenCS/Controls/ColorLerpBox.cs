using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class ColorLerpBox : Base
    {
        protected Point cursorPos;
        protected bool m_bDepressed;
        protected byte m_Hue;
        protected Texture m_Texture; // [omeg] added

        public event ControlCallback OnSelectionChanged;

        public ColorLerpBox(Base parent) : base(parent)
        {
            SetColor(Color.FromArgb(255, 255, 128, 0));
            SetSize(128, 128);
            MouseInputEnabled = true;
            m_bDepressed = false;

            // texture initialized in Render() if null
        }

        public static Color Lerp(Color toColor, Color fromColor, float amount)
        {
            Color delta = toColor.Subtract(fromColor);
            delta = delta.Multiply(amount);
            return fromColor.Add(delta);
        }

        public Color SelectedColor
        {
            get { return GetColorAt(cursorPos.X, cursorPos.Y); }
        }

        public void SetColor(Color value, bool onlyHue=true)
        {
            HSV hsv = value.ToHSV();
            m_Hue = (byte)(hsv.h);
            if (!onlyHue)
            {
                cursorPos.X = Global.Trunc(hsv.s * Width);
                cursorPos.Y = Global.Trunc((1 - hsv.v) * Height);
            }
            Invalidate();

            if (OnSelectionChanged != null)
                OnSelectionChanged.Invoke(this);
        }

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            if (m_bDepressed)
            {
                cursorPos = CanvasPosToLocal(new Point(x, y));
                //Do we have clamp?
                if (cursorPos.X < 0)
                    cursorPos.X = 0;
                if (cursorPos.X > Width)
                    cursorPos.X = Width;

                if (cursorPos.Y < 0)
                    cursorPos.Y = 0;
                if (cursorPos.Y > Height)
                    cursorPos.Y = Height;

                if (OnSelectionChanged != null)
                    OnSelectionChanged.Invoke(this);
            }
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            m_bDepressed = pressed;
            if (pressed)
                Global.MouseFocus = this;
            else
                Global.MouseFocus = null;

            onMouseMoved(x, y, 0, 0);
        }

        public Color GetColorAt(int x, int y)
        {
            float xPercent = (x / (float)Width);
            float yPercent = 1 - (y / (float)Height);

            Color result = Global.HSVToColor(m_Hue, xPercent, yPercent);

            return result;
        }

        // [omeg] added
        public override void Invalidate()
        {
            if (m_Texture != null)
            {
                m_Texture.Release(Skin.Renderer);
                m_Texture = null;
            }
            base.Invalidate();
        }

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);
            if (m_Texture == null)
            {
                byte[] pixelData = new byte[Width*Height*4];

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Color c = GetColorAt(x, y);
                        pixelData[4*(x + y*Width)] = c.R;
                        pixelData[4*(x + y*Width) + 1] = c.G;
                        pixelData[4*(x + y*Width) + 2] = c.B;
                        pixelData[4*(x + y*Width) + 3] = c.A;
                    }
                }

                m_Texture = new Texture();
                m_Texture.Width = Width;
                m_Texture.Height = Height;
                m_Texture.LoadRaw(Width, Height, pixelData, skin.Renderer);
            }

            skin.Renderer.DrawTexturedRect(m_Texture, RenderBounds);


            skin.Renderer.DrawColor = Color.Black;
            skin.Renderer.DrawLinedRect(RenderBounds);

            Color selected = SelectedColor;
            if ((selected.R + selected.G + selected.B)/3 < 170)
                skin.Renderer.DrawColor = Color.White;
            else
                skin.Renderer.DrawColor = Color.Black;

            Rectangle testRect = new Rectangle(cursorPos.X - 3, cursorPos.Y - 3, 6, 6);

            skin.Renderer.DrawShavedCornerRect(testRect);
        }
    }
}
