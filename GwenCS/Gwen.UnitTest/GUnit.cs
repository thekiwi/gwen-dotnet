using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class GUnit : Base
    {
        public UnitTest UnitTest;

        public GUnit(Base parent) : base(parent)
        {
            
        }

        public void UnitPrint(String str)
        {
            UnitTest.PrintText(str);
        }
    }
}
