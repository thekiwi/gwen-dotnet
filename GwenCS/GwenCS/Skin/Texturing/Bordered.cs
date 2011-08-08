using System.Drawing;

namespace Gwen.Skin.Texturing
{
    public struct SubRect
    {
        public float[] uv;
    }

    public class Bordered
    {
        private Texture texture;

        private SubRect[] rects;

        private Margin margin;

        private float width;
        private float height;

        public Bordered(Texture pTexture, float x, float y, float w, float h, Margin in_margin, float DrawMarginScale = 1.0f)
        {
            rects = new SubRect[9];
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].uv = new float[4];
            }

            Init(pTexture, x, y, w, h, in_margin, DrawMarginScale);
        }

        void DrawRect(Renderer.Base render, int i, int x, int y, int w, int h)
        {
            render.DrawTexturedRect(texture,
                                    new Rectangle(x, y, w, h),
                                    rects[i].uv[0], rects[i].uv[1], rects[i].uv[2], rects[i].uv[3]);
        }

        void SetRect(int iNum, float x, float y, float w, float h)
        {
            float texw = texture.Width;
            float texh = texture.Height;

            //x -= 1.0f;
            //y -= 1.0f;

            rects[iNum].uv[0] = x / texw;
            rects[iNum].uv[1] = y / texh;

            rects[iNum].uv[2] = (x + w) / texw;
            rects[iNum].uv[3] = (y + h) / texh;

            //	rects[iNum].uv[0] += 1.0f / texture->width;
            //	rects[iNum].uv[1] += 1.0f / texture->width;
        }

        protected void Init(Texture pTexture, float x, float y, float w, float h, Margin in_margin, float DrawMarginScale = 1.0f)
        {
            texture = pTexture;

            margin = in_margin;

            SetRect(0, x, y, margin.left, margin.top);
            SetRect(1, x + margin.left, y, w - margin.left - margin.right - 1, margin.top);
            SetRect(2, (x + w) - margin.right, y, margin.right, margin.top);

            SetRect(3, x, y + margin.top, margin.left, h - margin.top - margin.bottom - 1);
            SetRect(4, x + margin.left, y + margin.top, w - margin.left - margin.right - 1,
                    h - margin.top - margin.bottom - 1);
            SetRect(5, (x + w) - margin.right, y + margin.top, margin.right, h - margin.top - margin.bottom - 1);

            SetRect(6, x, (y + h) - margin.bottom, margin.left, margin.bottom);
            SetRect(7, x + margin.left, (y + h) - margin.bottom, w - margin.left - margin.right - 1, margin.bottom);
            SetRect(8, (x + w) - margin.right, (y + h) - margin.bottom, margin.right, margin.bottom);

            margin.left = Global.Trunc(margin.left*DrawMarginScale);
            margin.right = Global.Trunc(margin.right*DrawMarginScale);
            margin.top = Global.Trunc(margin.top*DrawMarginScale);
            margin.bottom = Global.Trunc(margin.bottom*DrawMarginScale);

            width = w - x;
            height = h - y;
        }

        // can't have this as default param
        public void Draw(Renderer.Base render, Rectangle r)
        {
            Draw(render, r, Color.White);
        }

        public void Draw(Renderer.Base render, Rectangle r, Color col)
        {
            render.DrawColor = col;

            if (r.Width < width && r.Height < height)
            {
                render.DrawTexturedRect(texture, r, rects[0].uv[0], rects[0].uv[1], rects[8].uv[2], rects[8].uv[3]);
                return;
            }

            DrawRect(render, 0, r.X, r.Y, margin.left, margin.top);
            DrawRect(render, 1, r.X + margin.left, r.Y, r.Width - margin.left - margin.right, margin.top);
            DrawRect(render, 2, (r.X + r.Width) - margin.right, r.Y, margin.right, margin.top);

            DrawRect(render, 3, r.X, r.Y + margin.top, margin.left, r.Height - margin.top - margin.bottom);
            DrawRect(render, 4, r.X + margin.left, r.Y + margin.top, r.Width - margin.left - margin.right,
                     r.Height - margin.top - margin.bottom);
            DrawRect(render, 5, (r.X + r.Width) - margin.right, r.Y + margin.top, margin.right,
                     r.Height - margin.top - margin.bottom);

            DrawRect(render, 6, r.X, (r.Y + r.Height) - margin.bottom, margin.left, margin.bottom);
            DrawRect(render, 7, r.X + margin.left, (r.Y + r.Height) - margin.bottom,
                     r.Width - margin.left - margin.right, margin.bottom);
            DrawRect(render, 8, (r.X + r.Width) - margin.right, (r.Y + r.Height) - margin.bottom, margin.right,
                     margin.bottom);
        }
    }
}
