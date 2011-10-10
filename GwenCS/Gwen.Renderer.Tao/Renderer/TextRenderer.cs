using System;
using System.Drawing;
using System.Drawing.Text;

namespace Gwen.Renderer
{
    /// <summary>
    /// Uses System.Drawing for 2d text rendering.
    /// </summary>
    public sealed class TextRenderer : IDisposable
    {
        readonly Bitmap bmp;
        readonly Graphics gfx;
        readonly Gwen.Texture texture;
        bool disposed;

        public Texture Texture { get { return texture; } }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="width">The width of the backing store in pixels.</param>
        /// <param name="height">The height of the backing store in pixels.</param>
        /// <param name="renderer">GWEN renderer.</param>
        public TextRenderer(int width, int height, Renderer.Tao renderer)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height");

            bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(bmp);
            //gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
            gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            gfx.Clear(Color.Transparent);

            texture = new Texture(renderer) { Width = width, Height = height };
        }

        /// <summary>
        /// Draws the specified string to the backing store.
        /// </summary>
        /// <param name="text">The <see cref="System.String"/> to draw.</param>
        /// <param name="font">The <see cref="System.Drawing.Font"/> that will be used.</param>
        /// <param name="brush">The <see cref="System.Drawing.Brush"/> that will be used.</param>
        /// <param name="point">The location of the text on the backing store, in 2d pixel coordinates.
        /// The origin (0, 0) lies at the top-left corner of the backing store.</param>
        public void DrawString(string text, System.Drawing.Font font, Brush brush, Point point)
        {
            gfx.DrawString(text, font, brush, point); // render text on the bitmap
            Tao.LoadTextureInternal(texture, bmp); // copy bitmap to gl texture
        }

        void Dispose(bool manual)
        {
            if (!disposed)
            {
                if (manual)
                {
                    bmp.Dispose();
                    gfx.Dispose();
                    texture.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextRenderer()
        {
            Console.WriteLine("[Warning] Resource leaked: {0}", typeof(TextRenderer));
        }
    }
}
