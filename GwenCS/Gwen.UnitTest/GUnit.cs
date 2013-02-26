using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class GUnit : Control
    {
        public UnitTest UnitTest;

        public GUnit(Control parent) : base(parent)
        {
            
        }

        public void UnitPrint(String str)
        {
            if (UnitTest != null)
                UnitTest.PrintText(str);
        }
    }
}
