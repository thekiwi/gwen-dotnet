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
using SFMLTexture = SFML.Graphics.Texture;

namespace Gwen.Renderer
{
    /// <summary>
    /// SFML renderer.
    /// </summary>
    public class SFML : Renderer.Base, ICacheToTexture
    {
        private RenderTarget m_Target;
        private Color m_Color;
        private Vector2f m_ViewScale;
        //private SFMLTexture m_LastSampled;
        //private Image m_SampleCache;
        private RenderStates m_RenderState;
        private uint m_CacheSize;
        private readonly Vertex[] m_VertexCache;

        public const uint CacheSize = 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="SFML"/> class.
        /// </summary>
        /// <param name="target">SFML render target.</param>
        public SFML(RenderTarget target)
        {
            m_Target = target;
            m_VertexCache = new Vertex[CacheSize];
            m_RenderState = new RenderStates(BlendMode.Alpha); // somehow worked without this in previous SFML version (May 9th 2010)
        }

        public override void Begin()
        {
            base.Begin();
            var port = m_Target.GetViewport(m_Target.GetView());
            var scaled = m_Target.ConvertCoords(new Vector2i(port.Width, port.Height));
            m_ViewScale.X = (port.Width/scaled.X)*Scale;
            m_ViewScale.Y = (port.Height/scaled.Y)*Scale;
        }

        public override void End()
        {
            FlushCache();
            base.End();
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
            SFMLTexture tex = texture.RendererData as SFMLTexture;
            if (tex == null)
                return defaultColor;
            var img = tex.CopyToImage();
            Color pixel = img.GetPixel(x, y);
            return System.Drawing.Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
        }
        /*
        public override void DrawLine(int x1, int y1, int x2, int y2)
        {
            Translate(ref x1, ref y1);
            Translate(ref x2, ref y2);

            Vertex[] line = {new Vertex(new Vector2f(x1, y1), m_Color), new Vertex(new Vector2f(x2, y2), m_Color)};

            m_Target.Draw(line, PrimitiveType.Lines);
        }
        */
        /// <summary>
        /// Loads the specified font.
        /// </summary>
        /// <param name="font">Font to load.</param>
        /// <returns>True if succeeded.</returns>
        public override bool LoadFont(Font font)
        {
            font.RealSize = font.Size*Scale;
            global::SFML.Graphics.Font sfFont;
            bool ret = true;
            
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
                        ret = false;
                    }
                }
                else
                {
                    // Ideally here we should be setting the font to a system default font here.
                    sfFont = global::SFML.Graphics.Font.DefaultFont;
                    Debug.Print("LoadFont: failed");
                    ret = false;
                }
            }

            sfFont.GetTexture((uint) font.Size).Smooth = font.Smooth;
            font.RendererData = sfFont;
            return ret;
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

        /// <summary>
        /// Returns dimensions of the text using specified font.
        /// </summary>
        /// <param name="font">Font to use.</param>
        /// <param name="text">Text to measure.</param>
        /// <returns>
        /// Width and height of the rendered text.
        /// </returns>
        public override Point MeasureText(Font font, String text)
        {
            // todo: cache results, this is slow
            global::SFML.Graphics.Font sfFont = font.RendererData as global::SFML.Graphics.Font;

            // If the font doesn't exist, or the font size should be changed
            if (sfFont == null || Math.Abs(font.RealSize - font.Size * Scale) > 2)
            {
                FreeFont(font);
                LoadFont(font);
            }

            if (sfFont == null)
                sfFont = global::SFML.Graphics.Font.DefaultFont;

            // todo: this is workaround for SFML.Net bug under mono
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                if (text[text.Length - 1] != '\0')
                    text += '\0';
            }

            Point extents = new Point(0, sfFont.GetLineSpacing((uint)font.RealSize));
            char prev = '\0';

            for (int i = 0; i < text.Length; i++)
            {
                char cur = text[i];
                sfFont.GetKerning(prev, cur, (uint)font.RealSize);
                prev = cur;
                if (cur == '\n' || cur == '\v')
                    continue;
                extents.X += sfFont.GetGlyph(cur, (uint) font.RealSize, false).Advance;
            }

            return extents;
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

            // todo: this is workaround for SFML.Net bug under mono
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                if (text[text.Length - 1] != '\0')
                    text += '\0';
            }

            Text sfText = new Text(text);
            sfText.Font = sfFont;
            sfText.Position = new Vector2f(pos.X, pos.Y);
            sfText.CharacterSize = (uint)font.RealSize; // [omeg] round?
            sfText.Color = m_Color;
            m_Target.Draw(sfText);
            sfText.Dispose();
            //m_Target.RestoreGLStates();
        }

        public override void DrawFilledRect(Rectangle rect)
        {
            rect = Translate(rect);
            if (m_RenderState.Texture != null || m_CacheSize + 4 >= CacheSize)
            {
                FlushCache();
                m_RenderState.Texture = null;
            }

            int right = rect.X + rect.Width;
            int bottom = rect.Y + rect.Height;

            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(rect.X, rect.Y), m_Color);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(right, rect.Y), m_Color);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(right, bottom), m_Color);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(rect.X, bottom), m_Color);  
        }

        public override void DrawTexturedRect(Texture t, Rectangle targetRect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            SFMLTexture tex = t.RendererData as SFMLTexture;
            if (null == tex)
            {
                DrawMissingImage(targetRect);
                return;
            }

            DrawTexturedRect(tex, targetRect, u1, v1, u2, v2);
        }

        protected void DrawTexturedRect(SFMLTexture tex, Rectangle targetRect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            Rectangle rect = Translate(targetRect);

            u1 *= tex.Size.X;
            v1 *= tex.Size.Y;
            u2 *= tex.Size.X;
            v2 *= tex.Size.Y;

            if (m_RenderState.Texture != tex || m_CacheSize + 4 >= CacheSize)
            {
                FlushCache();

                // enable the new texture
                m_RenderState.Texture = tex;
            }

            int right = rect.X + rect.Width;
            int bottom = rect.Y + rect.Height;

            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(rect.X, rect.Y), new Vector2f(u1, v1));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(right, rect.Y), new Vector2f(u2, v1));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(right, bottom), new Vector2f(u2, v2));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(rect.X, bottom), new Vector2f(u1, v2));
        }

        public override void LoadTexture(Texture texture)
        {
            if (null == texture) return;

            Debug.Print("LoadTexture: {0} {1}", texture.Name, texture.RendererData);

            if (texture.RendererData != null) 
                FreeTexture(texture);

            SFMLTexture sfTexture;

            try
            {
                sfTexture = new SFMLTexture(texture.Name);
                sfTexture.Smooth = true;
            }
            catch (LoadingFailedException)
            {
                Debug.Print("LoadTexture: failed");
                texture.Failed = true;
                return;
            }

            texture.Width = (int)sfTexture.Size.X;
            texture.Height = (int)sfTexture.Size.Y;
            texture.RendererData = sfTexture;
            texture.Failed = false;
        }

        /// <summary>
        /// Initializes texture from image file data.
        /// </summary>
        /// <param name="texture">Texture to initialize.</param>
        /// <param name="data">Image file as stream.</param>
        public override void LoadTextureStream(Texture texture, System.IO.Stream data)
        {
            if (null == texture) return;

            Debug.Print("LoadTextureStream: {0} {1}", texture.Name, texture.RendererData);

            if (texture.RendererData != null)
                FreeTexture(texture);

            SFMLTexture sfTexture;

            try
            {
                sfTexture = new SFMLTexture(data);
                sfTexture.Smooth = true;
            }
            catch (LoadingFailedException)
            {
                Debug.Print("LoadTextureStream: failed");
                texture.Failed = true;
                return;
            }

            texture.Width = (int)sfTexture.Size.X;
            texture.Height = (int)sfTexture.Size.Y;
            texture.RendererData = sfTexture;
            texture.Failed = false;
        }

        // [omeg] added, pixelData are in RGBA format
        public override void LoadTextureRaw(Texture texture, byte[] pixelData)
        {
            if (null == texture) return;

            Debug.Print("LoadTextureRaw: {0}", texture.RendererData);

            if (texture.RendererData != null) 
                FreeTexture(texture);

            SFMLTexture sfTexture;

            try
            {
                var img = new Image((uint)texture.Width, (uint)texture.Height, pixelData); // SFML Image
                sfTexture = new SFMLTexture(img);
                sfTexture.Smooth = true;
                img.Dispose();
            }
            catch (LoadingFailedException)
            {
                Debug.Print("LoadTextureRaw: failed");
                texture.Failed = true;
                return;
            }

            texture.RendererData = sfTexture;
            texture.Failed = false;
        }

        public override void FreeTexture(Texture texture)
        {
            SFMLTexture tex = texture.RendererData as SFMLTexture;
            if (tex != null)
                tex.Dispose();

            Debug.Print("FreeTexture: {0}", texture.Name);

            texture.RendererData = null;
        }

        public override void StartClip()
        {
            FlushCache();
            Rectangle clip = ClipRegion;
            clip.X = (int) Math.Round(clip.X*m_ViewScale.X);
            clip.Y = (int) Math.Round(clip.Y*m_ViewScale.Y);
            clip.Width = (int) Math.Round(clip.Width*m_ViewScale.X);
            clip.Height = (int) Math.Round(clip.Height*m_ViewScale.Y);

            var view = m_Target.GetView();
            var v = m_Target.GetViewport(view);
            view.Dispose();
            clip.Y = v.Height - (clip.Y + clip.Height);

            Gl.glScissor(clip.X, clip.Y, clip.Width, clip.Height);
            Gl.glEnable(Gl.GL_SCISSOR_TEST);
        }

        public override void EndClip()
        {
            FlushCache();
            Gl.glDisable(Gl.GL_SCISSOR_TEST);
        }

        private void FlushCache()
        {
            Debug.Assert(m_CacheSize % 4 == 0);
            if (m_CacheSize > 0)
            {
                m_Target.Draw(m_VertexCache, 0, m_CacheSize, PrimitiveType.Quads, m_RenderState);
                m_CacheSize = 0;
            }
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
            DrawTexturedRect(ri.Texture, control.Bounds);
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
