using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using SFML;
using SFML.Graphics;
using SFML.Window;
using Tao.OpenGl;
using Color = SFML.Graphics.Color;
using Image = SFML.Graphics.Image;

namespace Gwen.Renderer
{
    /// <summary>
    /// SFML renderer.
    /// </summary>
    public class SFML : Renderer.Base, ICacheToTexture
    {
        private RenderTarget m_Target;
        private Color m_Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="SFML"/> class.
        /// </summary>
        /// <param name="target">SFML render target.</param>
        public SFML(RenderTarget target)
        {
            m_Target = target;
        }

        /// <summary>
        /// Cache to texture provider.
        /// </summary>
        public override ICacheToTexture CTT
        {
            get { return this; }
        }

        /// <summary>
        /// Gets or sets the current drawing color.
        /// </summary>
        public override System.Drawing.Color DrawColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_Color.A, m_Color.R, m_Color.G, m_Color.B);
            }
            set
            {
                m_Color = new Color(value.R, value.G, value.B, value.A);
            }
        }

        public override System.Drawing.Color PixelColor(Texture texture, uint x, uint y, System.Drawing.Color defaultColor)
        {
            Sprite tex = texture.RendererData as Sprite;
            if (tex == null)
                return defaultColor;
            var img = tex.Texture.CopyToImage();
            Color pixel = img.GetPixel(x, y);
            return System.Drawing.Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
        }

        public override void DrawLine(int x, int y, int a, int b)
        {
            Translate( ref x, ref y );
            Translate( ref a, ref b );
            // [omeg] todo: sfml.net should have method accepting coords to not create unnecessary objects
            var shape = Shape.Line(new Vector2f(x, y), new Vector2f(a, b), 1.0f, m_Color);
            m_Target.Draw(shape);
            shape.Dispose();
        }

        public override void DrawFilledRect(Rectangle rect)
        {
            rect = Translate(rect);
            // [omeg] todo: sfml.net should have method accepting coords to not create unnecessary objects
            //m_Target.Draw(Shape.Rectangle(new FloatRect(rect.X, rect.Y, rect.Right, rect.Bottom), m_Color)); // [omeg] bug in gwen
            var shape = Shape.Rectangle(new FloatRect(rect.X, rect.Y, rect.Width, rect.Height), m_Color);
            m_Target.Draw(shape);
            shape.Dispose();
        }

        /// <summary>
        /// Loads the specified font.
        /// </summary>
        /// <param name="font">Font to load.</param>
        public override void LoadFont(Font font)
        {
            font.RealSize = font.Size*Scale;
            global::SFML.Graphics.Font sfFont;
            
            Debug.Print("LoadFont: {0} {1}", font.FaceName, font.RendererData);
            try
            {
                sfFont = new global::SFML.Graphics.Font(font.FaceName);
            }
            catch (LoadingFailedException)
            {
                // try to load windows font by this name
                String path = Platform.Windows.GetFontPath(font.FaceName);
                if (path != null)
                {
                    try
                    {
                        sfFont = new global::SFML.Graphics.Font(path);
                    }
                    catch (LoadingFailedException)
                    {
                        // Ideally here we should be setting the font to a system default font here.
                        sfFont = global::SFML.Graphics.Font.DefaultFont;
                        Debug.Print("LoadFont: failed");
                    }
                }
                else
                {
                    // Ideally here we should be setting the font to a system default font here.
                    sfFont = global::SFML.Graphics.Font.DefaultFont;
                    Debug.Print("LoadFont: failed");
                }
            }

            font.RendererData = sfFont;
        }

        /// <summary>
        /// Frees the specified font.
        /// </summary>
        /// <param name="font">Font to free.</param>
        public override void FreeFont(Font font)
        {
            if ( font.RendererData == null ) return;

            Debug.Print("FreeFont: {0} {1}", font.FaceName, font.RendererData);

            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If this is the default font then don't delete it!
            if (sfFont != global::SFML.Graphics.Font.DefaultFont)
            {
                sfFont.Dispose();
            }

            font.RendererData = null;
        }

        public override void RenderText(Font font, Point pos, string text)
        {
            //m_Target.SaveGLStates();
            pos = Translate(pos);
            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If the font doesn't exist, or the font size should be changed
            if (sfFont == null || Math.Abs(font.RealSize - font.Size * Scale) > 2)
            {
                FreeFont(font);
                LoadFont(font);
            }

            if (sfFont == null)
                sfFont = global::SFML.Graphics.Font.DefaultFont;

            Text sfText = new Text(text);
            sfText.Font = sfFont;
            sfText.Position = new Vector2f(pos.X, pos.Y); // [omeg] todo: correct? or origin?
            sfText.CharacterSize = (uint)font.RealSize; // [omeg] round?
            sfText.Color = m_Color;
            m_Target.Draw(sfText);
            sfText.Dispose();
            //m_Target.RestoreGLStates();
        }

        /// <summary>
        /// Returns dimensions of the text using specified font.
        /// </summary>
        /// <param name="font">Font to use.</param>
        /// <param name="text">Text to measure.</param>
        /// <returns>
        /// Width and height of the rendered text.
        /// </returns>
        public override Point MeasureText(Font font, string text)
        {
            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If the font doesn't exist, or the font size should be changed
            if (sfFont == null || Math.Abs(font.RealSize - font.Size * Scale) > 2)
            {
                FreeFont(font);
                LoadFont(font);
            }

            if (sfFont == null)
                sfFont = global::SFML.Graphics.Font.DefaultFont;

            Text sfText = new Text(text);
            sfText.Font = sfFont;
            sfText.CharacterSize = (uint)font.RealSize; // [omeg] round?

            FloatRect fr = sfText.GetRect();
            sfText.Dispose();
            return new Point((int)Math.Round(fr.Width), (int)Math.Round(fr.Height));
        }

        public override void DrawTexturedRect(Texture t, Rectangle targetRect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            Sprite tex = t.RendererData as Sprite;
            if (null == tex)
            {
                DrawMissingImage(targetRect);
                return;
            }

            DrawTexturedRect(tex, targetRect, u1, v1, u2, v2);
        }

        protected void DrawTexturedRect(Sprite tex, Rectangle targetRect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            Rectangle rect = Translate(targetRect);
            //DrawMissingImage(rect); return;
            
            //m_Target.SaveGLStates();
            int x1 = (int)(u1 * tex.Texture.Width);
            int y1 = (int)(v1 * tex.Texture.Height);
            int w = (int)((u2 - u1) * tex.Texture.Width);
            int h = (int)((v2 - v1) * tex.Texture.Height);
            var r = new IntRect(x1, y1, w, h);
            /*
            double delta = 0.0001;
            if (u1 - r.Left > delta)
                throw new InvalidOperationException("u1");
            if (v1 - r.Top > delta)
                throw new InvalidOperationException("v1");
            if (u2-u1 - r.Width > delta)
                throw new InvalidOperationException("width");
            if (v2-v1 - r.Height > delta)
                throw new InvalidOperationException("height");
            */
            tex.Position = new Vector2f(rect.X, rect.Y);
            tex.SubRect = r;
            tex.Width = rect.Width;
            tex.Height = rect.Height;
            m_Target.Draw(tex);
            //m_Target.RestoreGLStates();
            
            /*
            // original rendering code
            tex.Texture.Bind();
            Gl.glColor4f(1, 1, 1, 1);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(u1, v1); Gl.glVertex2f(rect.X, rect.Y);
            Gl.glTexCoord2f(u1, v2); Gl.glVertex2f(rect.X, rect.Bottom);
            Gl.glTexCoord2f(u2, v2); Gl.glVertex2f(rect.Right, rect.Bottom);
            Gl.glTexCoord2f(u2, v1); Gl.glVertex2f(rect.Right, rect.Y);
            Gl.glEnd();
            */
        }

        public override void LoadTexture(Texture texture)
        {
            if (null == texture) return;

            Debug.Print("LoadTexture: {0} {1}", texture.Name, texture.RendererData);

            if (texture.RendererData != null) 
                FreeTexture(texture);

            global::SFML.Graphics.Texture tex;
            Sprite sprite;

            try
            {
                tex = new global::SFML.Graphics.Texture(texture.Name);
                tex.Smooth = true;
                sprite = new Sprite(tex);
            }
            catch (LoadingFailedException)
            {
                Debug.Print("LoadTexture: failed");
                texture.Failed = true;
                return;
            }

            texture.Height = (int)tex.Height;
            texture.Width = (int)tex.Width;
            texture.RendererData = sprite;
        }

        // [omeg] added, pixelData are in RGBA format
        public override void LoadTextureRaw(Texture texture, byte[] pixelData)
        {
            if (null == texture) return;

            Debug.Print("LoadTextureRaw: {0}", texture.RendererData);

            if (texture.RendererData != null) 
                FreeTexture(texture);

            global::SFML.Graphics.Texture tex;
            Sprite sprite;

            try
            {
                var img = new Image((uint)texture.Width, (uint)texture.Height, pixelData); // SFML Image
                tex = new global::SFML.Graphics.Texture(img);
                tex.Smooth = true;
                sprite = new Sprite(tex);
                img.Dispose();
            }
            catch (LoadingFailedException)
            {
                Debug.Print("LoadTextureRaw: failed");
                texture.Failed = true;
                return;
            }

            texture.RendererData = sprite;
        }

        public override void FreeTexture(Texture texture)
        {
            Sprite tex = texture.RendererData as Sprite;
            if (tex != null)
                tex.Dispose();

            Debug.Print("FreeTexture: {0} {1}", texture.Name, texture.RendererData);

            texture.RendererData = null;
        }

        public override void StartClip()
        {
            Rectangle rect = ClipRegion;
            // OpenGL's coords are from the bottom left
            // so we need to translate them here.
            var view = m_Target.GetView();
            var v = m_Target.GetViewport(view);
            view.Dispose();
            rect.Y = v.Height - (rect.Y + rect.Height);

            Gl.glScissor((int)(rect.X * Scale), (int)(rect.Y * Scale),
                         (int)(rect.Width * Scale), (int)(rect.Height * Scale));
            Gl.glEnable(Gl.GL_SCISSOR_TEST);
        }

        public override void EndClip()
        {
            Gl.glDisable(Gl.GL_SCISSOR_TEST);
        }

        #region Implementation of ICacheToTexture

        private Dictionary<Control.Base, RenderTexture> m_RT;
        private Stack<RenderTarget> m_Stack;
        private RenderTarget m_RealRT;

        public void Initialize()
        {
            m_RT = new Dictionary<Control.Base, RenderTexture>();
            m_Stack = new Stack<RenderTarget>();
        }

        public void ShutDown()
        {
            m_RT.Clear();
            if (m_Stack.Count > 0)
                throw new InvalidOperationException("Render stack not empty");
        }

        /// <summary>
        /// Called to set the target up for rendering.
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void SetupCacheTexture(Control.Base control)
        {
            m_RealRT = m_Target;
            m_Stack.Push(m_Target); // save current RT
            m_Target = m_RT[control]; // make cache current RT
        }

        /// <summary>
        /// Called when cached rendering is done.
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void FinishCacheTexture(Control.Base control)
        {
            m_Target = m_Stack.Pop();
        }

        /// <summary>
        /// Called when gwen wants to draw the cached version of the control. 
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void DrawCachedControlTexture(Control.Base control)
        {
            RenderTexture ri = m_RT[control];
            //ri.Display();
            RenderTarget rt = m_Target;
            m_Target = m_RealRT;
            DrawTexturedRect(new Sprite(ri.Texture), control.Bounds);
            //DrawMissingImage(control.Bounds);
            m_Target = rt;
        }

        /// <summary>
        /// Called to actually create a cached texture. 
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void CreateControlCacheTexture(Control.Base control)
        {
            // initialize cache RT
            if (!m_RT.ContainsKey(control))
            {
                m_RT[control] = new RenderTexture((uint)control.Width, (uint)control.Height);
                View view = new View(new FloatRect(0, 0, control.Width, control.Height));
                //view.Viewport = new FloatRect(0, control.Height, control.Width, control.Height);
                m_RT[control].SetView(view);
            }

            RenderTexture ri = m_RT[control];
            ri.Display();
        }

        public void UpdateControlCacheTexture(Control.Base control)
        {
            throw new NotImplementedException();
        }

        public void SetRenderer(Base renderer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
