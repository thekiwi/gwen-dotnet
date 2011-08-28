using System;
using System.Collections.Generic;
using System.Drawing;
using SFML;
using SFML.Graphics;
using SFML.Window;
using Tao.OpenGl;
using Color = SFML.Graphics.Color;
using Image = SFML.Graphics.Image;


namespace Gwen.Renderer
{
    public class SFML : Renderer.Base, ICacheToTexture
    {
        protected RenderTarget m_Target;
        protected Color m_Color;

        public SFML(RenderTarget target)
        {
            m_Target = target;
        }

        public override ICacheToTexture CTT
        {
            get { return this; }
        }
        
        public override System.Drawing.Color DrawColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_Color.A, m_Color.R, m_Color.G, m_Color.B); ;
            }
            set
            {
                m_Color = new Color(value.R, value.G, value.B, value.A);
            }
        }

        public override System.Drawing.Color PixelColour(Texture texture, uint x, uint y, System.Drawing.Color defaultColor)
        {
            global::SFML.Graphics.Texture tex = texture.RendererData as global::SFML.Graphics.Texture;
            if (tex == null)
                return defaultColor;
            var img = tex.CopyToImage();
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

        public override void LoadFont(ref Font font)
        {
            font.RealSize = font.Size*Scale;
            global::SFML.Graphics.Font sfFont;
            
            try
            {
                sfFont = new global::SFML.Graphics.Font(font.FaceName);
            }
            catch (LoadingFailedException)
            {
                // Ideally here we should be setting the font to a system default font here.
                sfFont = global::SFML.Graphics.Font.DefaultFont;
            }

            font.RendererData = sfFont;
        }

        public override void FreeFont(ref Font font)
        {
            if ( font.RendererData == null ) return;

            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If this is the default font then don't delete it!
            if (sfFont != global::SFML.Graphics.Font.DefaultFont)
            {
                sfFont.Dispose();
            }

            font.RendererData = null;
        }

        public override void RenderText(ref Font font, Point pos, string text)
        {
            //m_Target.SaveGLStates();
            pos = Translate(pos);
            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If the font doesn't exist, or the font size should be changed
            if (sfFont == null || Math.Abs(font.RealSize - font.Size * Scale) > 2)
            {
                FreeFont(ref font);
                LoadFont(ref font);
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

        public override Point MeasureText(ref Font font, string text)
        {
            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If the font doesn't exist, or the font size should be changed
            if (sfFont == null || Math.Abs(font.RealSize - font.Size * Scale) > 2)
            {
                FreeFont(ref font);
                LoadFont(ref font);
            }

            if (sfFont == null)
                sfFont = global::SFML.Graphics.Font.DefaultFont;

            Text sfText = new Text(text);
            sfText.Font = sfFont;
            sfText.CharacterSize = (uint)font.RealSize; // [omeg] round?
            sfText.Color = m_Color; // [omeg] not needed?

            FloatRect fr = sfText.GetRect();
            sfText.Dispose();
            return new Point(Global.Trunc(fr.Width), Global.Trunc(fr.Height));
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
            int x1 = Global.Trunc(u1 * tex.Texture.Width);
            int y1 = Global.Trunc(v1*tex.Texture.Height);
            int w = Global.Trunc((u2 - u1)*tex.Texture.Width);
            int h = Global.Trunc((v2 - v1)*tex.Texture.Height);
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

        public override void LoadTexture(Texture pTexture)
        {
            if (null == pTexture) return;
            if (pTexture.RendererData != null) FreeTexture(pTexture);

            global::SFML.Graphics.Texture tex;
            Sprite sprite;

            try
            {
                tex = new global::SFML.Graphics.Texture(pTexture.Name);
                tex.Smooth = true;
                sprite = new Sprite(tex);
            }
            catch (LoadingFailedException)
            {
                pTexture.Failed = true;
                return;
            }

            pTexture.Height = (int)tex.Height;
            pTexture.Width = (int)tex.Width;
            pTexture.RendererData = sprite;
        }

        // [omeg] added
        public override void LoadTextureRaw(Texture pTexture, byte[] pixelData)
        {
            if (null == pTexture) return;
            if (pTexture.RendererData != null) FreeTexture(pTexture);

            global::SFML.Graphics.Texture tex;
            Sprite sprite;

            try
            {
                var img = new Image((uint)pTexture.Width, (uint)pTexture.Height, pixelData);
                tex = new global::SFML.Graphics.Texture(img);
                tex.Smooth = true;
                sprite = new Sprite(tex);
                img.Dispose();
            }
            catch (LoadingFailedException)
            {
                pTexture.Failed = true;
                return;
            }

            pTexture.RendererData = sprite;
        }

        public override void FreeTexture(Texture t)
        {
            Sprite tex = t.RendererData as Sprite;
            if (tex != null)
                tex.Dispose();

            t.RendererData = null;
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
            
            Gl.glScissor(Global.Trunc(rect.X*Scale), Global.Trunc(rect.Y*Scale),
                         Global.Trunc(rect.Width*Scale), Global.Trunc(rect.Height*Scale));
            Gl.glEnable(Gl.GL_SCISSOR_TEST);
        }

        public override void EndClip()
        {
            Gl.glDisable(Gl.GL_SCISSOR_TEST);
        }

        #region Implementation of ICacheToTexture

        private Dictionary<Controls.Base, RenderTexture> m_RT;
        private Stack<RenderTarget> m_Stack;
        private RenderTarget m_RealRT;

        public void Initialize()
        {
            m_RT = new Dictionary<Controls.Base, RenderTexture>();
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
        public void SetupCacheTexture(Controls.Base control)
        {
            m_RealRT = m_Target;
            m_Stack.Push(m_Target); // save current RT
            m_Target = m_RT[control]; // make cache current RT
        }

        /// <summary>
        /// Called when cached rendering is done.
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void FinishCacheTexture(Controls.Base control)
        {
            m_Target = m_Stack.Pop();
        }

        /// <summary>
        /// Called when gwen wants to draw the cached version of the control. 
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void DrawCachedControlTexture(Controls.Base control)
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
        public void CreateControlCacheTexture(Controls.Base control)
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

        public void UpdateControlCacheTexture(Controls.Base control)
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
