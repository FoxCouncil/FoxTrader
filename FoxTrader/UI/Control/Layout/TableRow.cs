using System;
using System.Drawing;
using FoxTrader.UI.Platform;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control.Layout
{
    /// <summary>Single table row</summary>
    internal class TableRow : GameControl
    {
        private readonly Label[] m_columns;
        private int m_columnCount;

        /// <summary>Initializes a new instance of the <see cref="TableRow" /> class</summary>
        /// <param name="c_parentControl">Parent control</param>
        public TableRow(GameControl c_parentControl) : base(c_parentControl)
        {
            m_columns = new Label[kMaxTableRowColumns];
            m_columnCount = 0;
            KeyboardInputEnabled = true;
        }

        /// <summary>Column count</summary>
        public int ColumnCount
        {
            get
            {
                return m_columnCount;
            }
            set
            {
                SetColumnCount(value);
            }
        }

        /// <summary>Indicates whether the row is even or odd (used for alternate coloring)</summary>
        public bool EvenRow
        {
            get;
            set;
        }

        /// <summary>Text of the first column</summary>
        public string Text
        {
            get
            {
                return GetText(0);
            }
            set
            {
                SetCellText(0, value);
            }
        }

        internal Label GetColumn(int c_idx)
        {
            return m_columns[c_idx];
        }

        /// <summary>Invoked when the row has been selected</summary>
        public event SelectionEventHandler Selected;

        /// <summary>Sets the number of columns</summary>
        /// <param name="c_columnCount">Number of columns</param>
        protected void SetColumnCount(int c_columnCount)
        {
            if (c_columnCount == m_columnCount)
            {
                return;
            }

            if (c_columnCount >= kMaxTableRowColumns)
            {
                throw new ArgumentException("Invalid column count", "columnCount");
            }

            for (var a_i = 0; a_i < kMaxTableRowColumns; a_i++)
            {
                if (a_i < c_columnCount)
                {
                    if (null == m_columns[a_i])
                    {
                        m_columns[a_i] = new Label(this);
                        m_columns[a_i].Padding = Padding.m_three;
                        m_columns[a_i].Margin = new Margin(0, 0, 2, 0); // to separate them slightly

                        if (a_i == c_columnCount - 1)
                        {
                            // last column fills remaining space
                            m_columns[a_i].Dock = Pos.Fill;
                        }
                        else
                        {
                            m_columns[a_i].Dock = Pos.Left;
                        }
                    }
                }
                else if (m_columns[a_i] != null)
                {
                    RemoveChild(m_columns[a_i], true);
                    m_columns[a_i] = null;
                }

                m_columnCount = c_columnCount;
            }
        }

        /// <summary>Sets the column width (in pixels)</summary>
        /// <param name="c_column">Column index</param>
        /// <param name="c_width">Column width</param>
        public void SetColumnWidth(int c_column, int c_width)
        {
            if (m_columns[c_column] == null)
            {
                return;
            }

            if (m_columns[c_column].Width == c_width)
            {
                return;
            }

            m_columns[c_column].Width = c_width;
        }

        /// <summary>Sets the text of a specified cell</summary>
        /// <param name="c_column">Column number</param>
        /// <param name="c_text">Text to set</param>
        public void SetCellText(int c_column, string c_text)
        {
            if (m_columns[c_column] == null)
            {
                return;
            }

            m_columns[c_column].Text = c_text;
        }

        /// <summary>Sets the contents of a specified cell</summary>
        /// <param name="c_column">Column number</param>
        /// <param name="c_control">Cell contents</param>
        /// <param name="c_enableMouseInput">Determines whether mouse input should be enabled for the cell</param>
        public void SetCellContents(int c_column, GameControl c_control, bool c_enableMouseInput = false)
        {
            if (m_columns[c_column] == null)
            {
                return;
            }

            c_control.Parent = m_columns[c_column];
            m_columns[c_column].MouseInputEnabled = c_enableMouseInput;
        }

        /// <summary>Gets the contents of a specified cell</summary>
        /// <param name="c_column">Column number</param>
        /// <returns>Control embedded in the cell</returns>
        internal GameControl GetCellContents(int c_column)
        {
            return m_columns[c_column];
        }

        protected virtual void OnRowSelected()
        {
            if (Selected != null)
            {
                Selected.Invoke(this);
            }
        }

        /// <summary>Sizes all cells to fit contents</summary>
        public void SizeToContents()
        {
            var a_width = 0;
            var a_height = 0;

            for (var a_i = 0; a_i < m_columnCount; a_i++)
            {
                if (m_columns[a_i] == null)
                {
                    continue;
                }

                // Note, more than 1 child here, because the 
                // label has a child built in ( The Text )
                if (m_columns[a_i].Children.Count > 1)
                {
                    m_columns[a_i].SizeToChildren();
                }
                else
                {
                    m_columns[a_i].SizeToContents();
                }

                //if (i == m_columnCount - 1) // last column
                //    m_columns[i].Width = Parent.Width - width; // fill if not autosized

                a_width += m_columns[a_i].Width + m_columns[a_i].Margin.m_left + m_columns[a_i].Margin.m_right;
                a_height = Math.Max(a_height, m_columns[a_i].Height + m_columns[a_i].Margin.m_top + m_columns[a_i].Margin.m_bottom);
            }

            SetSize(a_width, a_height);
        }

        /// <summary>Sets the text color for all cells</summary>
        /// <param name="c_color">Text color</param>
        public void SetTextColor(Color c_color)
        {
            for (var a_i = 0; a_i < m_columnCount; a_i++)
            {
                if (null == m_columns[a_i])
                {
                    continue;
                }

                m_columns[a_i].TextColor = c_color;
            }
        }

        /// <summary>Returns text of a specified row cell (default first)</summary>
        /// <param name="c_column">Column index</param>
        /// <returns>Column cell text</returns>
        public string GetText(int c_column = 0)
        {
            return m_columns[c_column].Text;
        }

        /// <summary>Handler for Copy event</summary>
        /// <param name="c_fromControl">Source control</param>
        protected override void OnCopy(GameControl c_fromControl)
        {
            Neutral.SetClipboardText(Text);
        }
    }
}