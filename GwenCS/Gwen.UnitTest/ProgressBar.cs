using System;
using System.Drawing;
using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class ProgressBar : GUnit
    {
        public ProgressBar(Base parent) : base(parent)
        {
            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 20, 200, 20));
                pb.Value = 0.27f;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 50, 200, 20));
                pb.Value = 0.66f;
                pb.Alignment = Pos.Right | Pos.CenterV;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 80, 200, 20));
                pb.Value = 0.88f;
                pb.Alignment = Pos.Left | Pos.CenterV;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 110, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 0.20f;
                pb.Alignment = Pos.Right | Pos.CenterV;
                pb.SetText("40,245 MB");
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 140, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 1.00f;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 170, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 0.00f;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(110, 200, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 0.50f;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(20, 20, 25, 200));
                pb.IsHorizontal = false;
                pb.Value = 0.25f;
                pb.Alignment = Pos.Top | Pos.CenterH;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(50, 20, 25, 200));
                pb.IsHorizontal = false;
                pb.Value = 0.40f;
            }

            {
                Controls.ProgressBar pb = new Controls.ProgressBar(this);
                pb.SetBounds(new Rectangle(80, 20, 25, 200));
                pb.IsHorizontal = false;
                pb.Alignment = Pos.Bottom | Pos.CenterH;
                pb.Value = 0.65f;
            }
        }
    }
}
