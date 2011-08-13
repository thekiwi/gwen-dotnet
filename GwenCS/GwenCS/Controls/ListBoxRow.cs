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
            SetSelected(false);
        }

        public bool IsSelected
        {
            get { return m_bSelected; }
            set { SetSelected(value); }
        }

        protected override void SetSelected(bool b)
        {
            m_bSelected = b;
            // TODO: Get these values from the skin.
            if (b)
            {
                //add to the listbox's selected list
                ((ListBox)Parent.Parent).SelectRow(this);
                SetTextColor(Color.White);
            }
            else
                SetTextColor(Color.Black);
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawListBoxLine(this, IsSelected);
        }

        internal override void onMouseClickLeft(int x, int y, bool pressed)
        {
            if (pressed)
            {
                if (!m_bSelected)
                {
                    onRowSelected();

                    if (!((ListBox)Parent.Parent).AllowMultiSelect)
                    {
                        m_bSelected = true;
                        SetTextColor(Color.White);
                    }
                }

                if (((ListBox)Parent.Parent).AllowMultiSelect)
                {
                    m_bSelected = !m_bSelected;
                    if (m_bSelected)
                        SetTextColor(Color.White);
                    else
                        SetTextColor(Color.Black);
                }
            }
        }
    }
}
