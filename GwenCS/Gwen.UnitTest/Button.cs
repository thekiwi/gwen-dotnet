using System;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class Button : GUnit
    {
        public Button(Base parent)
            : base(parent)
        {
            Controls.Button buttonA = new Controls.Button(this);
            buttonA.Text = "Event tester";
            buttonA.SetBounds(200, 30, 300, 200);
            buttonA.OnPress += onButtonA;

            var buttonB = new Controls.Button(this);
            buttonB.Text = "\u0417\u0430\u043C\u0435\u0436\u043D\u0430\u044F \u043C\u043E\u0432\u0430";
            buttonB.SetPos(0, 20);

            var buttonC = new Controls.Button(this);
            buttonC.Text = "Image button";
            buttonC.SetImage("test16.png");
            Align.PlaceBelow(buttonC, buttonB, 10);

            var buttonD = new Controls.Button(this);
            buttonD.SetImage("test16.png");
            buttonD.SetSize(20, 20);
            Align.PlaceBelow(buttonD, buttonC, 10);

            var buttonE = new Controls.Button(this);
            buttonE.Text = "Toggle me";
            buttonE.IsToggle = true;
            buttonE.OnToggle += onToggle;
            buttonE.OnToggleOn += onToggleOn;
            buttonE.OnToggleOff += onToggleOff;
            Align.PlaceBelow(buttonE, buttonD, 10);

            var buttonF = new Controls.Button(this);
            buttonF.Text = "Disabled :D";
            buttonF.IsDisabled = true;
            Align.PlaceBelow(buttonF, buttonE, 10);

            var buttonG = new Controls.Button(this);
            buttonG.Text = "With Tooltip";
            buttonG.SetToolTipText("This is tooltip");
            Align.PlaceBelow(buttonG, buttonF, 10);
        }

        private void onButtonA(Base control)
        {
            UnitPrint("Button: OnPress");
        }

        private void onToggle(Base control)
        {
            UnitPrint("Button: OnToggle");
        }

        private void onToggleOn(Base control)
        {
            UnitPrint("Button: OnToggleOn");
        }

        private void onToggleOff(Base control)
        {
            UnitPrint("Button: OnToggleOff");
        }
    }
}
