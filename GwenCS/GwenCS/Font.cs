using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public double RealSize;

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
            f.RendererData = null; // can't copy

            return f;
        }
    }
}
