using System;
using System.Drawing;
using Gwen.Controls.Layout;

namespace Gwen.Controls
{
    public class ListBoxRow : TableRow
    {
        protected bool m_bSelected;

        public ListBoxRow(Base parent) : base(parent)
        {
            MouseInputEnabled = true;
            IsSelected = false;
        }

        public bool IsSelected
        {
            get { return m_bSelected; }
            set
            {
                m_bSelected = value;             
                // TODO: Get these values from the skin.
                if (value)
                    SetTextColor(Color.White);
                else
                    SetTextColor(Color.Black);
            }
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawListBoxLine(this, IsSelected, EvenRow);
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            if (pressed)
            {
                //IsSelected = true; // [omeg] ListBox manages that
                onRowSelected();
            }
        }
    }
}
