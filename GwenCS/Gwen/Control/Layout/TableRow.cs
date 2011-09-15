using System;
using System.Drawing;

namespace Gwen.Control.Layout
{
    /// <summary>
    /// Single table row.
    /// </summary>
    public class TableRow : Base
    {
        // [omeg] todo: get rid of this
        public static int MaxColumns = 5;

        protected int m_ColumnCount;
        protected bool m_bEvenRow;
        internal readonly Label[] m_Columns;

        /// <summary>
        /// Invoked when the row is selected.
        /// </summary>
        public event ControlCallback OnRowSelected;

        /// <summary>
        /// Column count.
        /// </summary>
        public int ColumnCount { get { return m_ColumnCount; } set { SetColumnCount(value); } }

        /// <summary>
        /// Indicates whether the row is even or odd (used for alternate coloring).
        /// </summary>
        public bool EvenRow { get { return m_bEvenRow; } set { m_bEvenRow = value; } }

        /// <summary>
        /// Text of the first column.
        /// </summary>
        public String Text { get { return GetText(0); } } // text of 1st column

        /// <summary>
        /// Initializes a new instance of the <see cref="TableRow"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public TableRow(Base parent)
            : base(parent)
        {
            m_Columns = new Label[MaxColumns];
            m_ColumnCount = 0;
            KeyboardInputEnabled = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            foreach (Label column in m_Columns)
                if (column != null)
                    column.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// Sets the number of columns.
        /// </summary>
        /// <param name="columnCount">Number of columns.</param>
        protected void SetColumnCount(int columnCount)
        {
            if (columnCount == m_ColumnCount) return;

            if (columnCount >= MaxColumns)
                throw new ArgumentException("Invalid column count", "columnCount");

            for (int i = 0; i < MaxColumns; i++)
            {
                if (i < columnCount)
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
                    m_Columns[i].Dispose();
                    m_Columns[i] = null;
                }

                m_ColumnCount = columnCount;
            }
        }

        /// <summary>
        /// Sets the column width (in pixels).
        /// </summary>
        /// <param name="column">Column index.</param>
        /// <param name="width">Column width.</param>
        public void SetColumnWidth(int column, int width)
        {
            if (null == m_Columns[column]) return;
            if (m_Columns[column].Width == width) return;

            m_Columns[column].Width = width;
        }

        /// <summary>
        /// Sets the text of a specified cell.
        /// </summary>
        /// <param name="column">Column number.</param>
        /// <param name="text">Text to set.</param>
        public void SetCellText(int column, String text)
        {
            if (null == m_Columns[column]) return;
            m_Columns[column].Text = text;
        }

        /// <summary>
        /// Sets the contents of a specified cell.
        /// </summary>
        /// <param name="column">Column number.</param>
        /// <param name="control">Cell contents.</param>
        /// <param name="enableMouseInput">Determines whether mouse input should be enabled for the cell.</param>
        public void SetCellContents(int column, Base control, bool enableMouseInput = false)
        {
            if (null == m_Columns[column]) return;
            control.Parent = m_Columns[column];

            m_Columns[column].MouseInputEnabled = enableMouseInput;
        }

        /// <summary>
        /// Gets the contents of a specified cell.
        /// </summary>
        /// <param name="column">Column number.</param>
        /// <returns>Control embedded in the cell.</returns>
        public Base GetCellContents(int column)
        {
            return m_Columns[column];
        }

        protected void onRowSelected()
        {
            if (OnRowSelected != null)
                OnRowSelected.Invoke(this);
        }

        /// <summary>
        /// Sizes all cells to fit contents.
        /// </summary>
        public void SizeToContents()
        {
            int height = 0;

            for (int i = 0; i < m_ColumnCount; i++)
            {
                if (null == m_Columns[i]) continue;

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

                height = Math.Max(height, m_Columns[i].Height);
            }

            Height = height;
        }

        /// <summary>
        /// Sets the text color for all cells.
        /// </summary>
        /// <param name="color">Text color.</param>
        public void SetTextColor(Color color)
        {
            for (int i = 0; i < m_ColumnCount; i++)
            {
                if (null == m_Columns[i]) continue;
                m_Columns[i].TextColor = color;
            }
        }

        /// <summary>
        /// Returns text of a specified row cell (default first).
        /// </summary>
        /// <param name="column">Column index.</param>
        /// <returns>Column cell text.</returns>
        public String GetText(int column = 0)
        {
            return m_Columns[column].Text;
        }

        /// <summary>
        /// Handler for Copy event.
        /// </summary>
        /// <param name="from">Source control.</param>
        protected override void onCopy(Base from)
        {
            Platform.Neutral.SetClipboardText(Text);
        }
    }
}
