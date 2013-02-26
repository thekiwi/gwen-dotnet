using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class Button : GUnit
    {
        private readonly Controls.Button buttonA, buttonB, buttonC, buttonD, buttonE, buttonF, buttonG, buttonH;

        public Button(Control parent)
            : base(parent)
        {
            buttonA = new Controls.Button(this);
            buttonA.Text = "Event tester";
            buttonA.SetBounds(200, 30, 300, 200);
            buttonA.Pressed += onButtonAp;
            buttonA.Clicked += onButtonAc;
            buttonA.Released += onButtonAr;

            buttonB = new Controls.Button(this);
            buttonB.Text = "\u0417\u0430\u043C\u0435\u0436\u043D\u0430\u044F \u043C\u043E\u0432\u0430";
            buttonB.SetPosition(0, 20);

            buttonC = new Controls.Button(this);
            buttonC.Text = "Image button";
            buttonC.SetImage("test16.png");
            Align.PlaceDownLeft(buttonC, buttonB, 10);

            buttonD = new Controls.Button(this);
            buttonD.SetImage("test16.png");
            buttonD.SetSize(20, 20);
            Align.PlaceDownLeft(buttonD, buttonC, 10);

            buttonE = new Controls.Button(this);
            buttonE.Text = "Toggle me";
            buttonE.IsToggle = true;
            buttonE.Toggled += onToggle;
            buttonE.ToggledOn += onToggleOn;
            buttonE.ToggledOff += onToggleOff;
            Align.PlaceDownLeft(buttonE, buttonD, 10);

            buttonF = new Controls.Button(this);
            buttonF.Text = "Disabled :D";
            buttonF.IsDisabled = true;
            Align.PlaceDownLeft(buttonF, buttonE, 10);

            buttonG = new Controls.Button(this);
            buttonG.Text = "With Tooltip";
            buttonG.SetToolTipText("This is tooltip");
            Align.PlaceDownLeft(buttonG, buttonF, 10);

            buttonH = new Controls.Button(this);
            buttonH.Text = "I'm autosized";
            buttonH.SizeToContents();
            Align.PlaceDownLeft(buttonH, buttonG, 10);
        }

        private void onButtonAc(Control control)
        {
            UnitPrint("Button: Clicked");
            control.Anim_HeightOut(0.5f, true, 0, 2.0f);
        }

        private void onButtonAp(Control control)
        {
            UnitPrint("Button: Pressed");
        }

        private void onButtonAr(Control control)
        {
            UnitPrint("Button: Released");
        }

        private void onToggle(Control control)
        {
            UnitPrint("Button: Toggled");
        }

        private void onToggleOn(Control control)
        {
            UnitPrint("Button: OnToggleOn");
        }

        private void onToggleOff(Control control)
        {
            UnitPrint("Button: ToggledOff");
        }
    }
}
