using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen
{
    public class Texture : IDisposable
    {
        public String Name { get; set; }
        public object RendererData { get; set; }
        public bool Failed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Renderer.Base m_Renderer;

        public Texture(Renderer.Base renderer)
        {
            m_Renderer = renderer;
            Width = 4;
            Height = 4;
            Failed = false;
        }

        public void Load(String name)
        {
            Name = name;
            m_Renderer.LoadTexture(this);
        }

        // [omeg] added. pixel data = RGBA order
        public void LoadRaw(int width, int height, byte[] pixelData)
        {
            Width = width;
            Height = height;
            m_Renderer.LoadTextureRaw(this, pixelData);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            m_Renderer.FreeTexture(this);
        }

        #endregion
    }
}
