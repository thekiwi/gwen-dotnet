using Gwen.Controls;

namespace Gwen
{
    public static class Global
    {
        public static Base HoveredControl;
        public static Base KeyboardFocus;
        public static Base MouseFocus;

        public const int MaxCoord = 4096; // added here from various places in code
    }
}
