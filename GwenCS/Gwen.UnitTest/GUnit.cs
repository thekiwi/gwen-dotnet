using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class GUnit : ControlBase
    {
        public UnitTest UnitTest;

        public GUnit(ControlBase parent) : base(parent)
        {
            
        }

        public void UnitPrint(String str)
        {
            if (UnitTest != null)
                UnitTest.PrintText(str);
        }
    }
}
