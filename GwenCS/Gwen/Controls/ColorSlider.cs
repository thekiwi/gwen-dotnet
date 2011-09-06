using System;
using System.Drawing;

namespace Gwen.Controls
{
    public class ColorSlider : Base
    {
        protected int m_iSelectedDist;
        protected bool m_bDepressed;
        protected Texture m_Texture; // [omeg] added

        public event ControlCallback OnSelectionChanged;

        public ColorSlider(Base parent) : base(parent)
        {
            SetSize(32, 128);
            MouseInputEnabled=true;
            m_bDepressed = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            m_Texture.Dispose();
        }

        protected override void Render(Skin.Base skin)
        {
            //Is there any way to move this into skin? Not for now, no idea how we'll "actually" render these

            if (m_Texture == null)
            {
                byte[] pixelData = new byte[Width * Height * 4];

                for (int y = 0; y < Height; y++)
                {
                    Color c = GetColorAtHeight(y);
                    for (int x = 0; x < Width; x++)
                    {
                        pixelData[4 * (x + y * Width)] = c.R;
                        pixelData[4 * (x + y * Width) + 1] = c.G;
                        pixelData[4 * (x + y * Width) + 2] = c.B;
                        pixelData[4 * (x + y * Width) + 3] = c.A;
                    }
                }
                /*
                for (int y = 0; y < Height; y++)
                {
                    float yPercent = y/(float) Height;
                    skin.Renderer.DrawColor = Global.HSVToColor(yPercent*360, 1, 1);
                    skin.Renderer.DrawFilledRect(new Rectangle(5, y, Width - 10, 1));
                }
                */
                m_Texture = new Texture(skin.Renderer);
                m_Texture.Width = Width;
                m_Texture.Height = Height;
                m_Texture.LoadRaw(Width, Height, pixelData);
            }

            skin.Renderer.DrawTexturedRect(m_Texture, new Rectangle(5, 0, Width-10, Height));
            
            int drawHeight = m_iSelectedDist - 3;

            //Draw our selectors
            skin.Renderer.DrawColor = Color.Black;
            skin.Renderer.DrawFilledRect(new Rectangle(0, drawHeight + 2, Width, 1));
            skin.Renderer.DrawFilledRect(new Rectangle(0, drawHeight, 5, 5));
            skin.Renderer.DrawFilledRect(new Rectangle(Width - 5, drawHeight, 5, 5));
            skin.Renderer.DrawColor = Color.White;
            skin.Renderer.DrawFilledRect(new Rectangle(1, drawHeight + 1, 3, 3));
            skin.Renderer.DrawFilledRect(new Rectangle(Width - 4, drawHeight + 1, 3, 3));
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

        internal override void onMouseMoved(int x, int y, int dx, int dy)
        {
            if (m_bDepressed)
            {
                Point cursorPos = CanvasPosToLocal(new Point(x, y));

                if (cursorPos.Y < 0)
                    cursorPos.Y = 0;
                if (cursorPos.Y > Height)
                    cursorPos.Y = Height;

                m_iSelectedDist = cursorPos.Y;
                if (OnSelectionChanged != null)
                    OnSelectionChanged.Invoke(this);
            }
        }

        public Color GetColorAtHeight(int y)
        {
            float yPercent = y / (float)Height;
            return Global.HSVToColor(yPercent * 360, 1, 1);
        }

        public void SetColor(Color color)
        {
            HSV hsv = color.ToHSV();

            m_iSelectedDist = Global.Trunc(hsv.h / 360 * Height);

            if (OnSelectionChanged != null)
                OnSelectionChanged.Invoke(this);
        }

        public Color SelectedColor { get { return GetColorAtHeight(m_iSelectedDist); } }
    }
}
