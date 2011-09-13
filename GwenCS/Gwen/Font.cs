using System;

namespace Gwen
{
    public class Font
    {
        public String FaceName;
        public int Size;
        public bool Bold;
        public bool DropShadow;
        
        // This should be set by the renderer
        // if it tries to use a font where it's
        // NULL.
        public object RendererData;

        // This is the real font size, after it's
        // been scaled by Render->Scale()
        public float RealSize;
        
        public Font() : this("Arial", 10)
        {

        }

        public Font(String faceName, int size = 10)
        {
            FaceName = faceName;
            Size = size;
            DropShadow = false;
            Bold = false;
        }
        
        public Font Copy()
        {
            Font f = new Font();
            f.FaceName = FaceName;
            f.Size = Size;
            f.RealSize = RealSize;
            f.Bold = Bold;
            f.DropShadow = DropShadow;
            f.RendererData = null; // must be reinitialized

            return f;
        }
    }
}
