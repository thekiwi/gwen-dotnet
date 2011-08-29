using System;
using System.Drawing;

namespace Gwen.Controls.Layout
{
    public class TableRow : Base
    {
        // [omeg] todo: get rid of this
        public static int MaxColumns = 5;

        protected int m_ColumnCount;
        protected bool m_bEvenRow;
        internal Label[] m_Columns = new Label[MaxColumns];

        public event ControlCallback OnRowSelected;

        public int ColumnCount { get { return m_ColumnCount; } set { SetColumnCount(value); } }
        public bool EvenRow { get { return m_bEvenRow; } set { m_bEvenRow = value; } }

        public TableRow(Base parent)
            : base(parent)
        {
            for (int i = 0; i < MaxColumns; i++)
                m_Columns[i] = null;

            m_ColumnCount = 0;
        }

        protected void SetColumnCount(int iCount)
        {
            if (iCount == m_ColumnCount) return;

            if (iCount >= MaxColumns)
                m_ColumnCount = MaxColumns;

            for (int i = 0; i < MaxColumns; i++)
            {
                if (i < iCount)
                {
                    if (null == m_Columns[i])
                    {
                        m_Columns[i] = new Label(this);
                        m_Columns[i].Dock = Pos.Left;
                        m_Columns[i].Padding = new Padding(3, 3, 3, 3);
                    }
                }
                else if (null != m_Columns[i])
                {
                    //m_Columns[i].DelayedDelete();
                    m_Columns[i] = null;
                }

                m_ColumnCount = iCount;
            }
        }

        public void SetColumnWidth(int i, int iWidth)
        {
            if (null==m_Columns[i]) return;
            if (m_Columns[i].Width == iWidth) return;

            m_Columns[i].Width = iWidth;
        }

        public void SetCellText(int i, String text)
        {
            if (null == m_Columns[i]) return;
            m_Columns[i].Text = text;
        }

        public void SetCellContents(int i, Base pControl, bool bEnableMouseInput = false)
        {
            if (null == m_Columns[i]) return;
            pControl.Parent = m_Columns[i];

            m_Columns[i].MouseInputEnabled = bEnableMouseInput;
        }

        public Base GetCellContents(int i)
        {
            return m_Columns[i];
        }

        protected void onRowSelected()
        {
            if (OnRowSelected!=null)
                OnRowSelected.Invoke(this);
        }
        
        public void SizeToContents()
        {
            int iHeight = 0;

            for (int i = 0; i < m_ColumnCount; i++)
            {
                if (null==m_Columns[i]) continue;

                // Note, more than 1 child here, because the 
                // label has a child built in ( The Text )
                if (m_Columns[i].ChildrenCount > 1)
                {
                    m_Columns[i].SizeToChildren();
                }
                else
                {
                    m_Columns[i].SizeToContents();
                }

                iHeight = Math.Max(iHeight, m_Columns[i].Height);
            }

            Height = iHeight;
        }

        public void SetTextColor(Color color)
        {
            for (int i = 0; i < m_ColumnCount; i++)
            {
                if (null==m_Columns[i]) continue;
                m_Columns[i].TextColor=color;
            }
        }

        public String GetText(int i)
        {
            return m_Columns[i].Text;
        }

        protected virtual void SetSelected( bool b ) 
        {}
    }
}
