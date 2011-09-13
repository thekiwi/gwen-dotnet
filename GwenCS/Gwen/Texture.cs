using System;

namespace Gwen
{
    public class Texture : IDisposable
    {
        public String Name;
        public object RendererData;
        public bool Failed;
        public int Width;
        public int Height;

        private Renderer.Base m_Renderer;

        public Texture(Renderer.Base renderer)
        {
            m_Renderer = renderer;
            Width = 4;
            Height = 4;
            Failed = false;
            Name = null;
            RendererData = null;
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            m_Renderer.FreeTexture(this);
        }
    }
}
