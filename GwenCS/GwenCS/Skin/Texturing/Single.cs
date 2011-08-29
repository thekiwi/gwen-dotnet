using System;
using System.Drawing;

namespace Gwen.Skin.Texturing
{
    public struct Single
    {
        private Texture texture;
        private float[] uv;
        private int iWidth, iHeight;

        public Single(Texture pTexture, float x, float y, float w, float h )
        {
            texture = pTexture;

            float texw = texture.Width;
            float texh = texture.Height;

            uv = new float[4];
            uv[0] = x / texw;
            uv[1] = y / texh;
            uv[2] = (x + w) / texw;
            uv[3] = (y + h) / texh;

            iWidth = (int) w;
            iHeight = (int) h;
        }

        // can't have this as default param
        public void Draw(Renderer.Base render, Rectangle r)
        {
            Draw(render, r, Color.White);
        }

        public void Draw(Renderer.Base render, Rectangle r, Color col)
        {
            render.DrawColor = col;
            render.DrawTexturedRect(texture, r, uv[0], uv[1], uv[2], uv[3]);
        }

        public void DrawCenter(Renderer.Base render, Rectangle r)
        {
            DrawCenter(render, r, Color.White);
        }
        public void DrawCenter(Renderer.Base render, Rectangle r, Color col)
        {
            r.X += (int)((r.Width - iWidth) * 0.5);
            r.Y += (int)((r.Height - iHeight) * 0.5);
            r.Width = iWidth;
            r.Height = iHeight;

            Draw(render, r, col);
        }
    }
}
