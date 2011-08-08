using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SFML;
using SFML.Graphics;
using Tao.OpenGl;
using Color = SFML.Graphics.Color;
using Image = SFML.Graphics.Image;


namespace Gwen.Renderer
{
    public class SFML : Renderer.Base
    {
        protected RenderTarget m_Target;
        protected Color m_Color;

        public SFML(RenderTarget target)
        {
            m_Target = target;
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
        
        public override void DrawLine(int x, int y, int a, int b)
        {
            Translate( ref x, ref y );
            Translate( ref a, ref b );
            // [omeg] todo: sfml.net should have method accepting coords to not create unnecessary objects
            m_Target.Draw(Shape.Line(new Vector2(x, y), new Vector2(a, b), 1.0f, m_Color));
        }

        public override void DrawFilledRect(Rectangle rect)
        {
            rect = Translate(rect);
            // [omeg] todo: sfml.net should have method accepting coords to not create unnecessary objects
            //m_Target.Draw(Shape.Rectangle(new FloatRect(rect.X, rect.Y, rect.Right, rect.Bottom), m_Color)); // [omeg] bug in gwen
            m_Target.Draw(Shape.Rectangle(new FloatRect(rect.X, rect.Y, rect.Width, rect.Height), m_Color));
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
            sfText.Position = new Vector2(pos.X, pos.Y); // [omeg] todo: correct? or origin?
            sfText.CharacterSize = (uint)font.RealSize; // [omeg] round?
            sfText.Color = m_Color;
            m_Target.Draw(sfText);
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
            return new Point(Global.Trunc(fr.Width), Global.Trunc(fr.Height));
        }

        public override void DrawTexturedRect(Texture t, Rectangle targetRect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            Image tex = t.Data as Image;
            if (null == tex)
            {
                DrawMissingImage(targetRect);
                return;
            }

            Rectangle rect = Translate(targetRect);
            tex.Bind();

            Gl.glColor4f(1, 1, 1, 1);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(u1, v1); Gl.glVertex2f(rect.X, rect.Y);
            Gl.glTexCoord2f(u1, v2); Gl.glVertex2f(rect.X, rect.Bottom);
            Gl.glTexCoord2f(u2, v2); Gl.glVertex2f(rect.Right, rect.Bottom);
            Gl.glTexCoord2f(u2, v1); Gl.glVertex2f(rect.Right, rect.Y);
            Gl.glEnd();
        }

        public override void LoadTexture(Texture pTexture)
        {
            if (null == pTexture) return;
            if (pTexture.Data != null) FreeTexture(pTexture);

            Image tex;

            try
            {
                tex = new Image(pTexture.Name);
            }
            catch (LoadingFailedException)
            {
                pTexture.Failed = true;
                return;
            }

            tex.Smooth = true;

            pTexture.Height = (int)tex.Height;
            pTexture.Width = (int)tex.Width;
            pTexture.Data = tex;
        }

        public override void FreeTexture(Texture t)
        {
            Image tex = t.Data as Image;
            if (tex != null)
                tex.Dispose();

            t.Data = null;
        }

        public override void StartClip()
        {
            Rectangle rect = ClipRegion;
            // OpenGL's coords are from the bottom left
            // so we need to translate them here.
            {
                int[] view = new int[4];
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, view);
                rect.Y = view[3] - (rect.Y + rect.Height);
            }
            Gl.glScissor(Global.Trunc(rect.X*Scale), Global.Trunc(rect.Y*Scale),
                         Global.Trunc(rect.Width*Scale), Global.Trunc(rect.Height*Scale));
            Gl.glEnable(Gl.GL_SCISSOR_TEST);
        }

        public override void EndClip()
        {
            Gl.glDisable(Gl.GL_SCISSOR_TEST);
        }
    }
}
