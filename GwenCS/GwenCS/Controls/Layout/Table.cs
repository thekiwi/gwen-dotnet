using System;
using System.Collections.Generic;
using System.Linq;

namespace Gwen.Controls.Layout
{
    public class Table : Base
    {
        protected bool m_bSizeToContents;
        protected int m_iColumnCount;
        protected int m_iDefaultRowHeight;

        protected int[] m_ColumnWidth = new int[TableRow.MaxColumns];

        public int ColumnCount { get { return m_iColumnCount; } set {SetColumnCount(value);}}
        public int RowCount { get { return Children.Count; } }

        public Table(Base parent) : base(parent)
        {
            m_iColumnCount = 1;
            m_iDefaultRowHeight = 22;

            for (int i = 0; i < TableRow.MaxColumns; i++)
            {
                m_ColumnWidth[i] = 20;
            }

            m_bSizeToContents = false;
        }

        protected void SetColumnCount(int i)
        {
            if (m_iColumnCount == i) return;
            foreach (TableRow row in Children.OfType<TableRow>())
            {
                row.ColumnCount = i;
            }

            m_iColumnCount = i;
        }

        public void SetColumnWidth(int i, int iWidth)
        {
            if (m_ColumnWidth[i] == iWidth) return;
            m_ColumnWidth[i] = iWidth;
            Invalidate();
        }

        public TableRow AddRow()
        {
            TableRow row = new TableRow(this);
            row.ColumnCount = m_iColumnCount;
            row.Height = m_iDefaultRowHeight;
            row.Dock = Pos.Top;
            return row;
        }

        public void AddRow(TableRow row)
        {
            row.Parent = this;
            row.ColumnCount = m_iColumnCount;
            row.Height = m_iDefaultRowHeight;
            row.Dock = Pos.Top;
        }

        public void RemoveRow(TableRow row)
        {
            Children.Remove(row);
            row.Dispose();
        }

        public void RemoveRow(int idx)
        {
            var row = Children[idx];
            RemoveRow(row as TableRow);
        }

        public void Clear()
        {
            foreach (TableRow child in Children.OfType<TableRow>())
            {
                RemoveRow(child);
            }
        }

        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);
         
            if (m_bSizeToContents)
            {
                DoSizeToContents();
            }

            foreach (TableRow row in Children.OfType<TableRow>())
            {
                for (int i = 0; i < TableRow.MaxColumns && i < m_iColumnCount; i++)
                {
                    row.SetColumnWidth(i, m_ColumnWidth[i]);
                }
            }
        }

        protected override void PostLayout(Skin.Base skin)
        {
            if (m_bSizeToContents)
            {
                SizeToChildren();
                m_bSizeToContents = false;
            }
        }

        public void SizeToContents()
        {
            m_bSizeToContents = true;
            Invalidate();
        }

        protected void DoSizeToContents()
        {
            for (int i = 0; i < TableRow.MaxColumns; i++)
            {
                m_ColumnWidth[i] = 10;
            }

            foreach (TableRow pRow in Children.OfType<TableRow>())
            {
                pRow.SizeToContents();

                for (int i = 0; i < TableRow.MaxColumns; i++)
                {
                    if (null != pRow.m_Columns[i])
                    {
                        m_ColumnWidth[i] = Math.Max(m_ColumnWidth[i], pRow.m_Columns[i].Width);
                    }
                }
                //iBottom += pRow->Height();
            }

            InvalidateParent();
        }
    }
}
