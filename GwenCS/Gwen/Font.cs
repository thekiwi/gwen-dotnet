using System;

namespace Gwen
{
    public struct Font
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
        /*
        public Font()
        {
            FaceName = "Arial";
            Size = 10;
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
            f.RendererData = RendererData;

            return f;
        }
         */
    }
}
