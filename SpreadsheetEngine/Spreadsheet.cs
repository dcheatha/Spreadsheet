// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Spreadsheet.cs - SpreadsheetEngine
// Created 2019/10/30 at 00:12
// ==================================================

namespace SpreadsheetEngine
{
    #region

    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    #endregion

    #endregion

    /// <summary>
    ///     Spreadsheet Logic Engine
    /// </summary>
    public class Spreadsheet
    {
        /// <summary>
        ///     Dictionary of cells in column:row format
        /// </summary>
        private readonly Dictionary<string, Cell> cells = new Dictionary<string, Cell>();

        /// <summary>
        ///     Amount of Columns the Spreadsheet contains
        /// </summary>
        private readonly int columns;

        /// <summary>
        ///     Amount of Rows the Spreadsheet contains
        /// </summary>
        private readonly int rows;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Spreadsheet" /> class.
        /// </summary>
        /// <param name="columns">
        ///     Amount of Columns the Spreadsheet will contain
        /// </param>
        /// <param name="rows">
        ///     Amount of Rows the Spreadsheet will contain
        /// </param>
        public Spreadsheet(int columns, int rows)
        {
            this.columns = columns;
            this.rows    = rows;

            for (var column = 0; column < this.columns; column++)
            {
                for (var row = 0; row < this.rows; row++)
                {
                    var newCell = new SpreadsheetCell(column + 1, row + 1);
                    newCell.PropertyChanged += this.CellChange;
                    this.cells.Add(newCell.Key, newCell);
                }
            }
        }

        /// <summary>
        ///     Fires whenever a cell property changes
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        /// <summary>
        ///     Gets Number of Columns
        /// </summary>
        public int ColumnCount => this.columns;

        /// <summary>
        ///     Gets Number of Rows
        /// </summary>
        public int RowCount => this.rows;

        /// <summary>
        ///     Helper function to listen to Cell events.
        /// </summary>
        /// <param name="cell">
        ///     The Cell Object
        /// </param>
        /// <param name="e">
        ///     The event fired with it.
        /// </param>
        private void CellChange(object cell, EventArgs e)
        {
            this.CellPropertyChanged?.Invoke(cell, new PropertyChangedEventArgs(e.ToString()));
        }
    }
}