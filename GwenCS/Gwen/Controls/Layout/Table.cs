using System;
using System.Linq;

namespace Gwen.Controls.Layout
{
    public class Table : Base
    {
        protected bool m_SizeToContents;
        protected int m_ColumnCount;
        protected int m_DefaultRowHeight;

        protected int[] m_ColumnWidth = new int[TableRow.MaxColumns];

        public int ColumnCount { get { return m_ColumnCount; } set {SetColumnCount(value);}}
        public int RowCount { get { return Children.Count; } }

        public Table(Base parent) : base(parent)
        {
            m_ColumnCount = 1;
            m_DefaultRowHeight = 22;

            for (int i = 0; i < TableRow.MaxColumns; i++)
            {
                m_ColumnWidth[i] = 20;
            }

            m_SizeToContents = false;
        }

        protected void SetColumnCount(int i)
        {
            if (m_ColumnCount == i) return;
            foreach (TableRow row in Children.OfType<TableRow>())
            {
                row.ColumnCount = i;
            }

            m_ColumnCount = i;
        }

        public void SetColumnWidth(int i, int width)
        {
            if (m_ColumnWidth[i] == width) return;
            m_ColumnWidth[i] = width;
            Invalidate();
        }

        public TableRow AddRow()
        {
            TableRow row = new TableRow(this);
            row.ColumnCount = m_ColumnCount;
            row.Height = m_DefaultRowHeight;
            row.Dock = Pos.Top;
            return row;
        }

        public void AddRow(TableRow row)
        {
            row.Parent = this;
            row.ColumnCount = m_ColumnCount;
            row.Height = m_DefaultRowHeight;
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
         
            if (m_SizeToContents)
            {
                DoSizeToContents();
            }

            bool even = false;
            foreach (TableRow row in Children.OfType<TableRow>())
            {
                row.EvenRow = even;
                even = !even;
                for (int i = 0; i < TableRow.MaxColumns && i < m_ColumnCount; i++)
                {
                    row.SetColumnWidth(i, m_ColumnWidth[i]);
                }
            }
        }

        protected override void PostLayout(Skin.Base skin)
        {
            if (m_SizeToContents)
            {
                SizeToChildren();
                m_SizeToContents = false;
            }
        }

        public void SizeToContents()
        {
            m_SizeToContents = true;
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

        public override void Dispose()
        {
            foreach (Base child in Children)
            {
                child.Dispose();
            }
            base.Dispose();
        }
    }
}
