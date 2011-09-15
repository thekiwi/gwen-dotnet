using System;

namespace Gwen.Control
{
    public class StatusBar : Label
    {
        public StatusBar(Base parent) : base(parent)
        {
            Height = 22;
            Dock = Pos.Bottom;
            Padding = new Padding(2, 2, 2, 2);
            Text = "Status Bar"; // [omeg] todo i18n
            Alignment = Pos.Left | Pos.CenterV;
        }

        public void AddControl(Base control, bool right)
        {
            control.Parent = this;
            control.Dock = right ? Pos.Right : Pos.Left;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawStatusBar(this);
        }
    }
}
