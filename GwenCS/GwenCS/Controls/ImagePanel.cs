using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gwen.Controls
{
    public class ImagePanel : Base, IDisposable
    {
        protected Texture m_Texture;
        protected float[] m_uv;
        protected Color m_DrawColor;

        public ImagePanel(Base parent) : base(parent)
        {
            m_uv = new float[4];
            m_Texture = new Texture(Skin.Renderer);
            SetUV(0, 0, 1, 1);
            MouseInputEnabled = false;
            m_DrawColor = Color.White;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
        {
            base.Dispose();
            m_Texture.Dispose();
        }

        public virtual void SetUV(float u1, float v1, float u2, float v2)
        {
            m_uv[0] = u1;
            m_uv[1] = v1;
            m_uv[2] = u2;
            m_uv[3] = v2;
        }

        public String ImageName { get { return m_Texture.Name; } set { m_Texture.Load(value); } }
        public virtual void SetDrawColor(Color c)
        {
            m_DrawColor = c;
        }

        protected override void Render(Skin.Base skin)
        {
            base.Render(skin);
            skin.Renderer.DrawColor = m_DrawColor;
            skin.Renderer.DrawTexturedRect(m_Texture, RenderBounds, m_uv[0], m_uv[1], m_uv[2], m_uv[3]);
        }

        public virtual void SizeToContents()
        {
            SetSize(m_Texture.Width, m_Texture.Height);
        }
    }
}
