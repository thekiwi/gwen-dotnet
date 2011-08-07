using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Skin.Texturing
{
    public class Single
    {
        Texture texture;
        float[] uv;

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
    }
}
