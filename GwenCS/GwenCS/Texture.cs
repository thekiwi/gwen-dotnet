using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen
{
    public class Texture
    {
        public String Name { get; set; }
        public object Data { get; set; }
        public bool Failed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Texture()
        {
            Width = 4;
            Height = 4;
            Failed = false;
        }

        public void Load(String name, Renderer.Base renderer)
        {
            Name = name;
            renderer.LoadTexture(this);
        }

        // [omeg] added. pixel data = RGBA order
        public void LoadRaw(int width, int height, byte[] pixelData, Renderer.Base renderer)
        {
            Width = width;
            Height = height;
            renderer.LoadTextureRaw(this, pixelData);
        }

        // todo: IDisposable
        public void Release(Renderer.Base renderer)
        {
            renderer.FreeTexture(this);
        }
    }
}
