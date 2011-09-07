using System;
using System.Drawing;

namespace Gwen.Renderer
{
    /*
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RGBA
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }
    */
    public class Base : IDisposable
    {
        //public Random rnd;
        protected Point m_RenderOffset;
        protected Rectangle m_ClipRegion;
        //protected ICacheToTexture m_RTT;

        public float Scale;

        public Base()
        {
            //rnd = new Random();
            m_RenderOffset = Point.Empty;
            Scale = 1.0f;
            if (CTT != null)
                CTT.Initialize();
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (CTT != null)
                CTT.ShutDown();
        }

        public virtual void Begin() 
        {}

        public virtual void End() 
        {}

        public virtual Color DrawColor { get; set; }

        public virtual void DrawLine(int x, int y, int a, int b)
        {}

        public virtual void DrawFilledRect(Rectangle rect)
        {}

        public virtual void StartClip()
        {}

        public virtual void EndClip()
        {}

        public virtual void LoadTexture(Texture t)
        {}

        // [omeg] Added to draw custom pixel bitmaps. Pixel data should be in RGBA order.
        // texture should have width & height set.
        public virtual void LoadTextureRaw(Texture t, byte[] pixelData)
        {}

        public virtual void FreeTexture(Texture t)
        {}

        public virtual void DrawTexturedRect(Texture t, Rectangle targetRect, float u1=0, float v1=0, float u2=1, float v2=1)
        {}

        public virtual void DrawMissingImage(Rectangle rect)
        {
            //DrawColor = Color.FromArgb(255, rnd.Next(0,255), rnd.Next(0,255), rnd.Next(0, 255));
            DrawColor = Color.Red;
            DrawFilledRect(rect);
        }

        public virtual ICacheToTexture CTT { get { return null; } }

        public virtual void LoadFont(ref Font font)
        {}

        public virtual void FreeFont(ref Font font)
        {}

        public virtual Point MeasureText(ref Font font, String text)
        {
            Point p = new Point(Global.Trunc(font.Size * Scale * text.Length * 0.4f), Global.Trunc(font.Size * Scale));

            return p;
        }

        public virtual void RenderText(ref Font font, Point pos, String text)
        {
            float size = font.Size * Scale;

            for ( int i=0; i<text.Length; i++ )
            {
                char chr = text[i];

                if ( chr == ' ' ) 
                    continue;

                Rectangle r = Global.FloatRect(pos.X + i * size * 0.4f, pos.Y, size * 0.4f - 1, size);

                /*
                    This isn't important, it's just me messing around changing the
                    shape of the rect based on the letter.. just for fun.
                */
                if ( chr == 'l' || chr == 'i' || chr == '!' || chr == 't' )
                {
                    r.Width = 1;
                }
                else if ( chr >= 'a' && chr <= 'z' )
                {
                    r.Y += Global.Trunc(size * 0.5f);
                    r.Height -= Global.Trunc(size * 0.4f);
                }
                else if ( chr == '.' || chr == ',' )
                {
                    r.X += 2;
                    r.Y += r.Height - 2;
                    r.Width = 2;
                    r.Height = 2;
                }
                else if ( chr == '\'' || chr == '`'  || chr == '"' )
                {
                    r.X += 3;
                    r.Width = 2;
                    r.Height = 2;
                }

                if ( chr == 'o' || chr == 'O' || chr == '0' )
                    DrawLinedRect( r );	
                else
                    DrawFilledRect( r );
            }
        }

        //
        // No need to implement these functions in your derived class, but if 
        // you can do them faster than the default implementation it's a good idea to.
        //

        public virtual void DrawLinedRect(Rectangle rect)
        {
            DrawFilledRect(new Rectangle(rect.X, rect.Y, rect.Width, 1));
            DrawFilledRect(new Rectangle(rect.X, rect.Y + rect.Height - 1, rect.Width, 1));

            DrawFilledRect(new Rectangle(rect.X, rect.Y, 1, rect.Height));
            DrawFilledRect(new Rectangle(rect.X + rect.Width - 1, rect.Y, 1, rect.Height));
        }

        public virtual void DrawPixel(int x, int y)
        {
            // [omeg] amazing ;)
            DrawFilledRect(new Rectangle(x, y, 1, 1));
        }

        public virtual Color PixelColour(Texture texture, uint x, uint y)
        {
            return PixelColour(texture, x, y, Color.White);
        }

        public virtual Color PixelColour(Texture texture, uint x, uint y, Color defaultColor)
        {
            return defaultColor;
        }

        public virtual void DrawShavedCornerRect(Rectangle rect, bool slight = false)
        {
            // Draw INSIDE the w/h.
            rect.Width -= 1;
            rect.Height -= 1;

            if (slight)
            {
                DrawFilledRect(new Rectangle(rect.X + 1, rect.Y, rect.Width - 1, 1));
                DrawFilledRect(new Rectangle(rect.X + 1, rect.Y + rect.Height, rect.Width - 1, 1));

                DrawFilledRect(new Rectangle(rect.X, rect.Y + 1, 1, rect.Height - 1));
                DrawFilledRect(new Rectangle(rect.X + rect.Width, rect.Y + 1, 1, rect.Height - 1));
                return;
            }

            DrawPixel(rect.X + 1, rect.Y + 1);
            DrawPixel(rect.X + rect.Width - 1, rect.Y + 1);

            DrawPixel(rect.X + 1, rect.Y + rect.Height - 1);
            DrawPixel(rect.X + rect.Width - 1, rect.Y + rect.Height - 1);

            DrawFilledRect(new Rectangle(rect.X + 2, rect.Y, rect.Width - 3, 1));
            DrawFilledRect(new Rectangle(rect.X + 2, rect.Y + rect.Height, rect.Width - 3, 1));

            DrawFilledRect(new Rectangle(rect.X, rect.Y + 2, 1, rect.Height - 3));
            DrawFilledRect(new Rectangle(rect.X + rect.Width, rect.Y + 2, 1, rect.Height - 3));
        }

        public int TranslateX(int x)
        {
            int x1 = x + m_RenderOffset.X;
            return Global.Ceil(x1 * Scale);
        }

        public int TranslateY(int y)
        {
            int y1 = y + m_RenderOffset.Y;
            return Global.Ceil(y1 * Scale);
        }

        //
        // Translate a panel's local drawing coordinate
        //  into view space, taking Offset's into account.
        //
        public void Translate(ref int x, ref int y)
        {
            x += m_RenderOffset.X;
            y += m_RenderOffset.Y;

            x = Global.Ceil(x * Scale);
            y = Global.Ceil(y * Scale);
        }

        public Point Translate(Point p)
        {
            int x = p.X;
            int y = p.Y;
            Translate(ref x, ref y);
            return new Point(x, y);
        }

        public Rectangle Translate(Rectangle rect)
        {
            return new Rectangle(TranslateX(rect.X), TranslateY(rect.Y), Global.Ceil(rect.Width * Scale), Global.Ceil(rect.Height * Scale));
        }

        //
        // Set the rendering offset. You shouldn't have to 
        // touch these, ever.
        //
        public Point RenderOffset { get { return m_RenderOffset; } set { m_RenderOffset = value; } }

        public void AddRenderOffset(Rectangle offset)
        {
            m_RenderOffset = new Point(m_RenderOffset.X + offset.X, m_RenderOffset.Y + offset.Y);
        }

        public void AddClipRegion(Rectangle rect)
        {

            rect.X = m_RenderOffset.X;
            rect.Y = m_RenderOffset.Y;

            Rectangle r = rect;
            if (rect.X < m_ClipRegion.X)
            {
                r.Width -= (m_ClipRegion.X - r.X);
                r.X = m_ClipRegion.X;
            }

            if (rect.Y < m_ClipRegion.Y)
            {
                r.Height -= (m_ClipRegion.Y - r.Y);
                r.Y = m_ClipRegion.Y;
            }

            if (rect.Right > m_ClipRegion.Right)
            {
                r.Width = m_ClipRegion.Right - r.X;
            }

            if (rect.Bottom > m_ClipRegion.Bottom)
            {
                r.Height = m_ClipRegion.Bottom - r.Y;
            }

            m_ClipRegion = r;
        }

        public bool ClipRegionVisible
        {
            get
            {
                if (m_ClipRegion.Width <= 0 || m_ClipRegion.Height <= 0)
                    return false;

                return true;
            }
        }

        public Rectangle ClipRegion { get { return m_ClipRegion; } set { m_ClipRegion = value; } }
    }
}
