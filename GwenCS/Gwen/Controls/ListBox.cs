using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gwen.Controls.Layout;

namespace Gwen.Controls
{
    /// <summary>
    /// ListBox control.
    /// </summary>
    public class ListBox : ScrollControl
    {
        protected readonly Table m_Table;
        protected readonly List<TableRow> m_SelectedRows;

        protected bool m_MultiSelect;

        /// <summary>
        /// Determines whether multiple rows can be selected at once.
        /// </summary>
        public bool AllowMultiSelect { get { return m_MultiSelect; } set { m_MultiSelect = value; } }

        /// <summary>
        /// List of selected rows.
        /// </summary>
        public IList<TableRow> SelectedRows { get { return m_SelectedRows; } }

        /// <summary>
        /// First selected row (and only if list is not multiselectable).
        /// </summary>
        public TableRow SelectedRow
        {
            get
            {
                if (m_SelectedRows.Count == 0) return null;
                return m_SelectedRows[0];
            }
        }

        //public Table Table { get { return m_Table; } }
        /// <summary>
        /// Column count of table rows.
        /// </summary>
        public int ColumnCount { get { return m_Table.ColumnCount; } set { m_Table.ColumnCount = value; } }

        /// <summary>
        /// Invoked when a row has been selected.
        /// </summary>
        public event ControlCallback OnRowSelected;

        /// <summary>
        /// Invoked whan a row has beed unselected.
        /// </summary>
        public event ControlCallback OnRowUnselected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public ListBox(Base parent)
            : base(parent)
        {
            m_SelectedRows = new List<TableRow>();

            EnableScroll(false, true);
            AutoHideBars = true;
            Margin = new Margin(1, 1, 1, 1);

            m_Table = new Table(this);
            m_Table.Dock = Pos.Top;
            m_Table.ColumnCount = 1;

            m_MultiSelect = false;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            m_Table.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <param name="label">Row text.</param>
        /// <returns>Newly created control.</returns>
        public TableRow AddItem(String label)
        {
            return AddItem(label, String.Empty);
        }

        /// <summary>
        /// Selects the specified row by index.
        /// </summary>
        /// <param name="index">Row to select.</param>
        /// <param name="clearOthers">Determines whether to deselect previously selected rows.</param>
        public void SelectRow(int index, bool clearOthers = false)
        {
            if (index < 0 || index >= m_Table.RowCount)
                return;

            SelectRow(m_Table.Children[index], clearOthers);
        }

        /// <summary>
        /// Selects the specified row(s) by text.
        /// </summary>
        /// <param name="rowText">Text to search for (exact match).</param>
        /// <param name="clearOthers">Determines whether to deselect previously selected rows.</param>
        public void SelectRows(String rowText, bool clearOthers = false)
        {
            var rows = m_Table.Children.OfType<ListBoxRow>().Where(x => x.Text == rowText);
            foreach (ListBoxRow row in rows)
            {
                SelectRow(row, clearOthers);
            }
        }

        /// <summary>
        /// Selects the specified row(s) by regex text search.
        /// </summary>
        /// <param name="pattern">Regex pattern to search for.</param>
        /// <param name="regexOptions">Regex options.</param>
        /// <param name="clearOthers">Determines whether to deselect previously selected rows.</param>
        public void SelectRowsByRegex(String pattern, RegexOptions regexOptions = RegexOptions.None, bool clearOthers = false)
        {
            var rows = m_Table.Children.OfType<ListBoxRow>().Where(x => Regex.IsMatch(x.Text, pattern) );
            foreach (ListBoxRow row in rows)
            {
                SelectRow(row, clearOthers);
            }
        }

        /// <summary>
        /// Slelects the specified row.
        /// </summary>
        /// <param name="control">Row to select.</param>
        /// <param name="clearOthers">Determines whether to deselect previously selected rows.</param>
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

        /// <summary>
        /// Removes the specified row by index.
        /// </summary>
        /// <param name="idx">Row index.</param>
        public void RemoveRow(int idx)
        {
            m_Table.RemoveRow(idx); // this calls Dispose()
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <param name="label">Row text.</param>
        /// <param name="name">Internal control name.</param>
        /// <returns>Newly created control.</returns>
        public TableRow AddItem(String label, String name)
        {
            ListBoxRow row = new ListBoxRow(this);
            m_Table.AddRow(row);

            row.SetCellText(0, label);
            row.Name = name;

            row.OnRowSelected += onRowSelected;

            m_Table.SizeToContents();

            return row;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawListBox(this);
        }

        /// <summary>
        /// Deselects all rows.
        /// </summary>
        public virtual void UnselectAll()
        {
            foreach (ListBoxRow row in m_SelectedRows)
            {
                row.IsSelected = false;
                if (OnRowUnselected != null)
                    OnRowUnselected.Invoke(this);
            }
            m_SelectedRows.Clear();
        }

        /// <summary>
        /// Unselects the specified row.
        /// </summary>
        /// <param name="row">Row to unselect.</param>
        public void UnselectRow(ListBoxRow row)
        {
            row.IsSelected = false;
            m_SelectedRows.Remove(row);

            if (OnRowUnselected != null)
                OnRowUnselected.Invoke(this);
        }

        /// <summary>
        /// Handler for the row selection event.
        /// </summary>
        /// <param name="control">Event source.</param>
        protected virtual void onRowSelected(Base control)
        {
            // [omeg] changed default behavior
            bool clear = false;// !Input.Input.IsShiftDown;
            ListBoxRow row = control as ListBoxRow;
            if (row == null)
                return;
            if (row.IsSelected)
                UnselectRow(row);
            else
                SelectRow(control, clear);
        }

        /// <summary>
        /// Removes all rows.
        /// </summary>
        public virtual void Clear()
        {
            UnselectAll();
            m_Table.Clear();
        }
    }
}
