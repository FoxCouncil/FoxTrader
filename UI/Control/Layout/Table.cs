using System;
using System.Linq;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control.Layout
{
    /// <summary>Base class for multi-column tables</summary>
    internal class Table : GameControl
    {
        private readonly int[] m_columnWidth;
        private int m_columnCount;
        private int m_maxWidth; // for autosizing, if nonzero - fills last cell up to this size
        // only children of this control should be TableRow.

        private bool m_sizeToContents;

        /// <summary>Initializes a new instance of the <see cref="Table" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public Table(GameControl c_parentControl) : base(c_parentControl)
        {
            m_columnCount = 1;
            DefaultRowHeight = 22;

            m_columnWidth = new int[kMaxTableRowColumns];

            for (var a_i = 0; a_i < kMaxTableRowColumns; a_i++)
            {
                m_columnWidth[a_i] = 20;
            }

            m_sizeToContents = false;
        }

        /// <summary>Column count (default 1)</summary>
        public int ColumnCount
        {
            get
            {
                return m_columnCount;
            }
            set
            {
                SetColumnCount(value);
                Invalidate();
            }
        }

        /// <summary>Row count</summary>
        public int RowCount => Children.Count;

        /// <summary>Gets or sets default height for new table rows</summary>
        public int DefaultRowHeight
        {
            get;
            set;
        }

        /// <summary>Returns specific row of the table</summary>
        /// <param name="c_index">Row index</param>
        /// <returns>Row at the specified index</returns>
        public TableRow this[int c_index] => Children[c_index] as TableRow;

        /// <summary>Sets the number of columns</summary>
        /// <param name="c_count">Number of columns</param>
        public void SetColumnCount(int c_count)
        {
            if (m_columnCount == c_count)
            {
                return;
            }
            foreach (var a_row in Children.OfType<TableRow>())
            {
                a_row.ColumnCount = c_count;
            }

            m_columnCount = c_count;
        }

        /// <summary>Sets the column width (in pixels)</summary>
        /// <param name="c_column">Column index</param>
        /// <param name="c_width">Column width</param>
        public void SetColumnWidth(int c_column, int c_width)
        {
            if (m_columnWidth[c_column] == c_width)
            {
                return;
            }
            m_columnWidth[c_column] = c_width;
            Invalidate();
        }

        /// <summary>Gets the column width (in pixels)</summary>
        /// <param name="c_column">Column index</param>
        /// <returns>Column width</returns>
        public int GetColumnWidth(int c_column)
        {
            return m_columnWidth[c_column];
        }

        /// <summary>Adds a new empty row</summary>
        /// <returns>Newly created row</returns>
        public TableRow AddRow()
        {
            var a_row = new TableRow(this);
            a_row.ColumnCount = m_columnCount;
            a_row.Height = DefaultRowHeight;
            a_row.Dock = Pos.Top;
            return a_row;
        }

        /// <summary>Adds a new row</summary>
        /// <param name="c_row">Row to add</param>
        public void AddRow(TableRow c_row)
        {
            c_row.Parent = this;
            c_row.ColumnCount = m_columnCount;
            c_row.Height = DefaultRowHeight;
            c_row.Dock = Pos.Top;
        }

        /// <summary>Adds a new row with specified text in first column</summary>
        /// <param name="c_text">Text to add</param>
        /// <returns>New row</returns>
        public TableRow AddRow(string c_text)
        {
            var a_row = AddRow();
            a_row.SetCellText(0, c_text);
            return a_row;
        }

        /// <summary>Removes a row by reference</summary>
        /// <param name="c_row">Row to remove</param>
        public void RemoveRow(TableRow c_row)
        {
            RemoveChild(c_row, true);
        }

        /// <summary>Removes a row by index</summary>
        /// <param name="c_idx">Row index</param>
        public void RemoveRow(int c_idx)
        {
            var a_row = Children[c_idx];
            RemoveRow(a_row as TableRow);
        }

        /// <summary>Removes all rows</summary>
        public void RemoveAll()
        {
            while (RowCount > 0)
            {
                RemoveRow(0);
            }
        }

        /// <summary>Gets the index of a specified row</summary>
        /// <param name="c_row">Row to search for</param>
        /// <returns>Row index if found, -1 otherwise</returns>
        public int GetRowIndex(TableRow c_row)
        {
            return Children.IndexOf(c_row);
        }

        /// <summary>Lays out the control's interior according to alignment, padding, dock etc</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);

            var a_even = false;
            foreach (TableRow a_row in Children)
            {
                a_row.EvenRow = a_even;
                a_even = !a_even;
                for (var a_i = 0; a_i < m_columnCount; a_i++)
                {
                    a_row.SetColumnWidth(a_i, m_columnWidth[a_i]);
                }
            }
        }

        protected override void PostLayout(SkinBase c_skin)
        {
            base.PostLayout(c_skin);
            if (m_sizeToContents)
            {
                DoSizeToContents();
                m_sizeToContents = false;
            }
        }

        /// <summary>Sizes to fit contents</summary>
        public void SizeToContents(int c_maxWidth)
        {
            m_maxWidth = c_maxWidth;
            m_sizeToContents = true;
            Invalidate();
        }

        protected void DoSizeToContents()
        {
            var a_height = 0;
            var a_width = 0;

            foreach (TableRow a_row in Children)
            {
                a_row.SizeToContents(); // now all columns fit but only in this particular row

                for (var a_i = 0; a_i < ColumnCount; a_i++)
                {
                    GameControl a_cell = a_row.GetColumn(a_i);
                    if (null != a_cell)
                    {
                        if (a_i < ColumnCount - 1 || m_maxWidth == 0)
                        {
                            m_columnWidth[a_i] = Math.Max(m_columnWidth[a_i], a_cell.Width + a_cell.Margin.m_left + a_cell.Margin.m_right);
                        }
                        else
                        {
                            m_columnWidth[a_i] = m_maxWidth - a_width; // last cell - fill
                        }
                    }
                }
                a_height += a_row.Height;
            }

            // sum all column widths 
            for (var a_i = 0; a_i < ColumnCount; a_i++)
            {
                a_width += m_columnWidth[a_i];
            }

            SetSize(a_width, a_height);
            //InvalidateParent();
        }
    }
}