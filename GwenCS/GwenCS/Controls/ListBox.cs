using System;
using System.Collections.Generic;
using System.Linq;
using Gwen.Controls.Layout;

namespace Gwen.Controls
{
    public class ListBox : ScrollControl
    {
        protected Table m_Table;
        protected List<TableRow> m_SelectedRows;

        protected bool m_bMultiSelect;

        public bool AllowMultiSelect { get { return m_bMultiSelect; } set { m_bMultiSelect = value; } }
        public IList<TableRow> SelectedRows { get { return m_SelectedRows; } }
        public TableRow SelectedRow
        {
            get
            {
                if (m_SelectedRows.Count == 0) return null;
                return m_SelectedRows[0];
            }
        }
        public Table Table { get { return m_Table; } }
        public int ColumnCount { get { return m_Table.ColumnCount; } set { m_Table.ColumnCount = value; } }

        public event ControlCallback OnRowSelected;

        public ListBox(Base parent)
            : base(parent)
        {
            m_SelectedRows = new List<TableRow>();

            SetScroll(false, true);
            AutoHideBars = true;
            Margin = new Margin(1, 1, 1, 1);

            m_Table = new Table(this);
            m_Table.Dock = Pos.Top;
            m_Table.ColumnCount = 1;

            m_bMultiSelect = false;
        }

        public override void Dispose()
        {
            m_Table.Dispose();
            base.Dispose();
        }

        public TableRow AddItem(String label)
        {
            return AddItem(label, String.Empty);
        }

        // [omeg] added
        public void SelectRow(int index, bool clearOthers = false)
        {
            if (index < 0 || index >= m_Table.RowCount)
                return;

            SelectRow(m_Table.Children[index], clearOthers);
        }

        public void SelectRow(String rowText, bool clearOthers = false)
        {
            throw new NotImplementedException();
        }

        public void SelectRow(Base control, bool clearOthers = false)
        {
            if (!AllowMultiSelect || clearOthers)
                UnselectAll();

            ListBoxRow row = control as ListBoxRow;
            if (row == null)
                return;

            // TODO: make sure this is one of our rows!
            row.IsSelected = true;
            m_SelectedRows.Add(row);
            if (OnRowSelected != null)
                OnRowSelected.Invoke(this);
        }

        // [omeg] added
        public void RemoveRow(int idx)
        {
            m_Table.RemoveRow(idx); // this calls Dispose()
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
            foreach (ListBoxRow row in m_SelectedRows)
            {
                row.IsSelected = false;
            }
            m_SelectedRows.Clear();
        }

        public void UnselectRow(ListBoxRow row)
        {
            row.IsSelected = false;
            m_SelectedRows.Remove(row);
        }

        protected virtual void onRowSelected(Base pControl)
        {
            // [omeg] changed default behavior
            bool clear = false;// !Input.Input.IsShiftDown;
            ListBoxRow row = pControl as ListBoxRow;
            if (row == null)
                return;
            if (row.IsSelected)
                UnselectRow(row);
            else
                SelectRow(pControl, clear);
        }

        public virtual void Clear()
        {
            UnselectAll();
            m_Table.Clear();
        }
    }
}
