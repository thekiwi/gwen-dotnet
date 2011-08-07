using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Gwen.Controls;

namespace Gwen.DragDrop
{
    public class Package
    {
        public String Name;
        public object UserData;
        public bool IsDraggable;
        public Base DrawControl;
        public Point HoldOffset;
    }
}
