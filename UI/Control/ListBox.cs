using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FoxTrader.UI.Control.Layout;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    /// <summary>ListBox control</summary>
    internal class ListBox : ScrollControl
    {
        private readonly List<TableRow> m_selectedRows;
        private readonly Table m_table;

        private bool m_multiSelect;
        private Pos m_oldDock; // used while autosizing
        private bool m_sizeToContents;

        /// <summary>Initializes a new instance of the <see cref="ListBox" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public ListBox(GameControl c_parentControl) : base(c_parentControl)
        {
            m_selectedRows = new List<TableRow>();

            EnableScroll(false, true);
            AutoHideBars = true;
            Margin = Margin.m_one;

            m_table = new Table(this);
            m_table.Dock = Pos.Fill;
            m_table.ColumnCount = 1;
            m_table.BoundsChanged += TableResized;

            m_multiSelect = false;
            IsToggle = false;
        }

        /// <summary>Determines whether multiple rows can be selected at once</summary>
        public bool AllowMultiSelect
        {
            get
            {
                return m_multiSelect;
            }
            set
            {
                m_multiSelect = value;
                if (value)
                {
                    IsToggle = true;
                }
            }
        }

        /// <summary>Determines whether rows can be unselected by clicking on them again</summary>
        public bool IsToggle
        {
            get;
            set;
        }

        /// <summary>Number of rows in the list box</summary>
        public int RowCount => m_table.RowCount;

        /// <summary>Returns specific row of the ListBox</summary>
        /// <param name="c_idx">Row index</param>
        /// <returns>Row at the specified index</returns>
        public ListBoxRow this[int c_idx] => m_table[c_idx] as ListBoxRow;

        /// <summary>List of selected rows</summary>
        public IEnumerable<TableRow> SelectedRows => m_selectedRows;

        /// <summary>First selected row (and only if list is not multiselectable)</summary>
        public TableRow SelectedRow
        {
            get
            {
                if (m_selectedRows.Count == 0)
                {
                    return null;
                }

                return m_selectedRows[0];
            }
        }

        /// <summary>Gets the selected row number</summary>
        public int SelectedRowIndex
        {
            get
            {
                var a_selected = SelectedRow;
                if (a_selected == null)
                {
                    return -1;
                }

                return m_table.GetRowIndex(a_selected);
            }
        }

        /// <summary>Column count of table rows</summary>
        public int ColumnCount
        {
            get
            {
                return m_table.ColumnCount;
            }
            set
            {
                m_table.ColumnCount = value;
                Invalidate();
            }
        }

        /// <summary>Invoked when a row has been selected</summary>
        public event SelectionEventHandler RowSelected;

        /// <summary>Invoked whan a row has beed unselected</summary>
        public event SelectionEventHandler RowUnselected;

        /// <summary>Selects the specified row by index</summary>
        /// <param name="c_idx">Row to select</param>
        /// <param name="c_clearOthers">Determines whether to deselect previously selected rows</param>
        public void SelectRow(int c_idx, bool c_clearOthers = false)
        {
            if (c_idx < 0 || c_idx >= m_table.RowCount)
            {
                return;
            }

            SelectRow(m_table.Children[c_idx], c_clearOthers);
        }

        /// <summary>Selects the specified row(s) by text</summary>
        /// <param name="c_rowText">Text to search for (exact match)</param>
        /// <param name="c_clearOthers">Determines whether to deselect previously selected rows</param>
        public void SelectRows(string c_rowText, bool c_clearOthers = false)
        {
            var a_childRows = m_table.Children.OfType<ListBoxRow>().Where(c_x => c_x.Text == c_rowText);

            foreach (var a_row in a_childRows)
            {
                SelectRow(a_row, c_clearOthers);
            }
        }

        /// <summary>Selects the specified row(s) by regex text search</summary>
        /// <param name="c_pattern">Regex pattern to search for</param>
        /// <param name="c_regexOptions">Regex options</param>
        /// <param name="c_clearOthers">Determines whether to deselect previously selected rows</param>
        public void SelectRowsByRegex(string c_pattern, RegexOptions c_regexOptions = RegexOptions.None, bool c_clearOthers = false)
        {
            var a_childRows = m_table.Children.OfType<ListBoxRow>().Where(c_x => Regex.IsMatch(c_x.Text, c_pattern));

            foreach (var a_row in a_childRows)
            {
                SelectRow(a_row, c_clearOthers);
            }
        }

        /// <summary>Slelects the specified row</summary>
        /// <param name="c_control">Row to select</param>
        /// <param name="c_clearOthers">Determines whether to deselect previously selected rows</param>
        public void SelectRow(GameControl c_control, bool c_clearOthers = false)
        {
            if (!AllowMultiSelect || c_clearOthers)
            {
                UnselectAll();
            }

            var a_row = c_control as ListBoxRow;

            if (a_row == null)
            {
                return;
            }

            // TODO: make sure this is one of our rows!
            a_row.IsSelected = true;
            m_selectedRows.Add(a_row);

            if (RowSelected != null)
            {
                RowSelected.Invoke(this);
            }
        }

        /// <summary>Removes the specified row by index</summary>
        /// <param name="c_idx">Row index</param>
        public void RemoveRow(int c_idx)
        {
            m_table.RemoveRow(c_idx); // this calls Dispose()
        }

        /// <summary>Adds a new row</summary>
        /// <param name="c_label">Row text</param>
        /// <returns>Newly created control</returns>
        public TableRow AddRow(string c_label)
        {
            return AddRow(c_label, string.Empty);
        }

        /// <summary>Adds a new row</summary>
        /// <param name="c_label">Row text</param>
        /// <param name="c_name">Internal control name</param>
        /// <returns>Newly created control</returns>
        public TableRow AddRow(string c_label, string c_name)
        {
            var a_row = new ListBoxRow(this);
            m_table.AddRow(a_row);

            a_row.SetCellText(0, c_label);
            a_row.Name = c_name;

            a_row.Selected += OnRowSelected;

            m_table.SizeToContents(Width);

            return a_row;
        }

        /// <summary>Sets the column width (in pixels)</summary>
        /// <param name="c_column">Column index</param>
        /// <param name="c_width">Column width</param>
        public void SetColumnWidth(int c_column, int c_width)
        {
            m_table.SetColumnWidth(c_column, c_width);
            Invalidate();
        }

        /// <summary>Renders the control using specified skin</summary>
        /// <param name="c_skin">Skin to use</param>
        protected override void Render(SkinBase c_skin)
        {
            c_skin.DrawListBox(this);
        }

        /// <summary>Deselects all rows</summary>
        public virtual void UnselectAll()
        {
            foreach (ListBoxRow a_row in m_selectedRows)
            {
                a_row.IsSelected = false;

                if (RowUnselected != null)
                {
                    RowUnselected.Invoke(this);
                }
            }
            m_selectedRows.Clear();
        }

        /// <summary>Unselects the specified row</summary>
        /// <param name="c_row">Row to unselect</param>
        public void UnselectRow(ListBoxRow c_row)
        {
            c_row.IsSelected = false;
            m_selectedRows.Remove(c_row);

            if (RowUnselected != null)
            {
                RowUnselected.Invoke(this);
            }
        }

        /// <summary>Handler for the row selection event</summary>
        /// <param name="c_control">Event source</param>
        protected virtual void OnRowSelected(GameControl c_control)
        {
            // TODO: changed default behavior
            var a_clearFlag = false; // !InputHandler.InputHandler.IsShiftDown;

            var a_row = c_control as ListBoxRow;

            if (a_row == null)
            {
                return;
            }

            if (a_row.IsSelected)
            {
                if (IsToggle)
                {
                    UnselectRow(a_row);
                }
            }
            else
            {
                SelectRow(c_control, a_clearFlag);
            }
        }

        /// <summary>Removes all rows</summary>
        public virtual void Clear()
        {
            UnselectAll();
            m_table.RemoveAll();
        }

        public void SizeToContents()
        {
            m_sizeToContents = true;
            // docking interferes with autosizing so we disable it until sizing is done
            m_oldDock = m_table.Dock;
            m_table.Dock = Pos.None;
            m_table.SizeToContents(0); // autosize without constraints
        }

        private void TableResized(GameControl c_control)
        {
            if (m_sizeToContents)
            {
                SetSize(m_table.Width, m_table.Height);
                m_sizeToContents = false;
                m_table.Dock = m_oldDock;
                Invalidate();
            }
        }
    }
}