using System.Drawing;
using Gwen.Controls;

namespace Gwen
{
    public static class ToolTip
    {
        private static Base g_ToolTip;

        public static void Enable(Base pControl)
        {
            if (null == pControl.ToolTip)
                return;

            g_ToolTip = pControl;
        }

        public static void Disable(Base pControl)
        {
            if (g_ToolTip == pControl)
            {
                g_ToolTip = null;
            }
        }

        public static void ControlDeleted(Base pControl)
        {
            Disable(pControl);
        }

        public static void RenderToolTip(Skin.Base skin)
        {
            if (null == g_ToolTip) return;

            Renderer.Base render = skin.Renderer;

            Point pOldRenderOffset = render.RenderOffset;
            Point MousePos = Input.Input.MousePosition;
            Rectangle Bounds = g_ToolTip.ToolTip.Bounds;

            Rectangle rOffset = Global.FloatRect(MousePos.X - Bounds.Width*0.5, MousePos.Y - Bounds.Height - 10,
                                                 Bounds.Width, Bounds.Height);
            rOffset = Global.ClampRectToRect(rOffset, g_ToolTip.GetCanvas().Bounds);

            //Calculate offset on screen bounds
            render.AddRenderOffset(rOffset);
            render.EndClip();

            skin.DrawToolTip(g_ToolTip.ToolTip);
            g_ToolTip.ToolTip.DoRender(skin);

            render.RenderOffset = pOldRenderOffset;
        }
    }
}
