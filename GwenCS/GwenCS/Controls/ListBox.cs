using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Gwen.Controls.Layout;

namespace Gwen.Controls
{
    public class ListBox : Base
    {
        protected Table m_Table;
        protected List<TableRow> m_SelectedRows;
        protected ScrollControl m_ScrollControl;

        protected bool m_bMultiSelect;

        public bool AllowMultiSelect { get { return m_bMultiSelect; } set { m_bMultiSelect = value; } }
        public IList<TableRow> SelectedRows { get { return m_SelectedRows; } }
        public TableRow SelectedRow { get { if (m_SelectedRows.Count == 0) return null;
            return m_SelectedRows[0];
        } }
        public ScrollControl Scroller { get { return m_ScrollControl; } }
        public Table Table { get { return m_Table; } }
        public int ColumnCount { get { return m_Table.ColumnCount; } set { m_Table.ColumnCount = value; } }

        public event ControlCallback OnRowSelected;

        public ListBox(Base parent)
            : base(parent)
        {
            m_SelectedRows = new List<TableRow>();

            m_ScrollControl = new ScrollControl(this);
            m_ScrollControl.Dock = Pos.Fill;
            m_ScrollControl.SetScroll(false, true);
            m_ScrollControl.AutoHideBars = true;
            m_ScrollControl.Margin = new Margin(1, 1, 1, 1);

            m_InnerPanel = m_ScrollControl;

            m_Table = new Table(this);
            m_Table.Dock = Pos.Top;
            m_Table.ColumnCount = 1;

            m_bMultiSelect = false;
        }

        internal override void onChildBoundsChanged(Rectangle oldChildBounds, Base child)
        {
            m_ScrollControl.UpdateScrollBars();
        }

        public TableRow AddItem(String label)
        {
            return AddItem(label, String.Empty);
        }

        public TableRow AddItem(String label, String name)
        {
            ListBoxRow pRow = new ListBoxRow(this);
            m_Table.AddRow(pRow);

            pRow.SetCellText(0, label);
            pRow.Name = name;

            pRow.OnRowSelected += onRowSelected;

            m_Table.SizeToContents();

            return pRow;
        }

        protected override void Render(Skin.Base skin)
        {
            skin.DrawListBox(this);
        }

        public virtual void UnselectAll()
        {
            foreach (ListBoxRow row in m_SelectedRows.Select(selectedRow => selectedRow as ListBoxRow))
            {
                row.IsSelected = false;
            }
            m_SelectedRows.Clear();
        }

        protected virtual void UpdateSelected()
        {
            m_SelectedRows.RemoveAll(row => !(row as ListBoxRow).IsSelected);
        }

        internal virtual void SelectRow(TableRow row)
        {
            m_SelectedRows.Add(row);
        }

        protected virtual void onRowSelected(Base pControl)
        {
            ListBoxRow pRow = pControl as ListBoxRow;
            if (null == pRow) return;

            if (!m_bMultiSelect)
                UnselectAll();
            else
                UpdateSelected();
            SelectRow(pRow);
            
            if (OnRowSelected!=null)
                OnRowSelected.Invoke(this);
        }

        public virtual void Clear()
        {
            UnselectAll();
            m_Table.Clear();
        }
    }
}
