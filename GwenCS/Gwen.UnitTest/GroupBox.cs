using Gwen.Controls;

namespace Gwen.UnitTest
{
    public class GroupBox : GUnit
    {
        public GroupBox(Control parent) : base(parent)
        {
            {
                Controls.GroupBox gb = new Controls.GroupBox(this);
                gb.Text = "Group Box (centered)";
                gb.SetBounds(10, 10, 200, 100);
                //Align.Center(gb);
            }

            {
                Controls.GroupBox gb = new Controls.GroupBox(this);
                gb.AutoSizeToContents = true;
                gb.Text = "With Label (autosized)";
                gb.SetPosition(250, 10);
                Controls.Label label = new Controls.Label(gb);
                label.AutoSizeToContents = true;
                label.Text = "I'm a label";
            }

            {
                Controls.GroupBox gb = new Controls.GroupBox(this);
                gb.AutoSizeToContents = true;
                gb.Text = "With Label (autosized)";
                gb.SetPosition(250, 50);
                Controls.Label label = new Controls.Label(gb);
                label.AutoSizeToContents = true;
                label.Text = "I'm a label. I'm a really long label!";
            }

            {
                Controls.GroupBox gb = new Controls.GroupBox(this);
                gb.AutoSizeToContents = true;
                gb.Text = "Two docked Labels (autosized)";
                gb.SetPosition(250, 100);
                Controls.Label label1 = new Controls.Label(gb);
                label1.AutoSizeToContents = true;
                label1.Text = "I'm a label";
                label1.Dock = Pos.Top;
                Controls.Label label2 = new Controls.Label(gb);
                label2.AutoSizeToContents = true;
                label2.Text = "I'm a label. I'm a really long label!";
                label2.Dock = Pos.Top;
            }

            {
                Controls.GroupBox gb = new Controls.GroupBox(this);
                gb.AutoSizeToContents = true;
                gb.Text = "Empty (autosized)";
                gb.SetPosition(10, 150);
            }

            {
                Controls.GroupBox gb1 = new Controls.GroupBox(this);
                //Control.Label gb1 = new Control.Label(this);
                gb1.Padding = Padding.Five;
                gb1.Text = "Yo dawg,";
                gb1.SetPosition(10, 200);
                gb1.SetSize(350, 200);
                //gb1.AutoSizeToContents = true;
                
                Controls.GroupBox gb2 = new Controls.GroupBox(gb1);
                gb2.Text = "I herd";
                gb2.Dock = Pos.Left;
                gb2.Margin = Margin.Three;
                gb2.Padding = Padding.Five;
                //gb2.AutoSizeToContents = true;
                
                Controls.GroupBox gb3 = new Controls.GroupBox(gb1);
                gb3.Text = "You like";
                gb3.Dock = Pos.Fill;
                
                Controls.GroupBox gb4 = new Controls.GroupBox(gb3);
                gb4.Text = "Group Boxes,";
                gb4.Dock = Pos.Top;
                gb4.AutoSizeToContents = true;

                Controls.GroupBox gb5 = new Controls.GroupBox(gb3);
                gb5.Text = "So I put Group";
                gb5.Dock = Pos.Fill;
                //gb5.AutoSizeToContents = true;

                Controls.GroupBox gb6 = new Controls.GroupBox(gb5);
                gb6.Text = "Boxes in yo";
                gb6.Dock = Pos.Left;
                gb6.AutoSizeToContents = true;

                Controls.GroupBox gb7 = new Controls.GroupBox(gb5);
                gb7.Text = "Boxes so you can";
                gb7.Dock = Pos.Top;
                gb7.SetSize(100, 100);

                Controls.GroupBox gb8 = new Controls.GroupBox(gb7);
                gb8.Text = "Group Box while";
                gb8.Dock = Pos.Top;
                gb8.Margin = Margin.Five;
                gb8.AutoSizeToContents = true;

                Controls.GroupBox gb9 = new Controls.GroupBox(gb7);
                gb9.Text = "u Group Box";
                gb9.Dock = Pos.Bottom;
                gb9.Padding = Padding.Five;
                gb9.AutoSizeToContents = true;
            }
            
            // at the end to apply to all children
            DrawDebugOutlines = true;
        }
    }
}
