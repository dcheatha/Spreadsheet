// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Spreadsheet.cs - SpreadsheetEngine
// Created 2019/10/30 at 00:12
// ==================================================

#region

using System.Runtime.CompilerServices;

#endregion

[assembly: InternalsVisibleTo("SpreadsheetEngineTester")]

namespace SpreadsheetEngine
{
    #region

    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

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
            this.ColumnCount = columns;
            this.RowCount    = rows;

            for (var column = 0; column < this.ColumnCount; column++)
            {
                for (var row = 0; row < this.RowCount; row++)
                {
                    var newCell = new SpreadsheetCell(column, row);
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
        public int ColumnCount { get; }

        /// <summary>
        ///     Gets Number of Rows
        /// </summary>
        public int RowCount { get; }

        /// <summary>
        ///     Gets a Cell from the Spreadsheet
        /// </summary>
        /// <param name="column">
        ///     The Cell Column
        /// </param>
        /// <param name="row">
        ///     The Cell Row
        /// </param>
        /// <returns>
        ///     The requested Cell, or null if it does not exist.
        /// </returns>
        public Cell GetCell(int column, int row)
        {
            var key = SpreadsheetCell.GenerateKey(column, row);

            return this.cells.ContainsKey(key) ? this.cells[key] : null;
        }

        /// <summary>
        ///     Runs the demo requested
        /// </summary>
        public void RunDemo()
        {
            var rnd = new Random();

            for (var pos = 0; pos < this.RowCount; pos++)
            {
                var beeCell = this.GetSpreadsheetCell(1, pos);
                beeCell.Text = $"This is cell B{pos + 1}!";

                var ahhCell = this.GetSpreadsheetCell(0, pos);
                ahhCell.Text = $"=B{pos + 1}";
            }

            // I know this can cause an inf loop, but it's just a demo...
            for (var pos = 0; pos < 50; pos++)
            {
                var column = rnd.Next(0, this.ColumnCount);
                var row    = rnd.Next(1, this.RowCount);

                var cell = this.GetSpreadsheetCell(column, row);

                if (cell.Text != string.Empty)
                {
                    pos--;
                }
                else
                {
                    cell.Text = "Random!";
                }
            }
        }

        /// <summary>
        ///     Follows a link in a Cell such as A12
        /// </summary>
        /// <param name="link">
        ///     String of link
        /// </param>
        /// <returns>
        ///     Cell that was linked to
        /// </returns>
        internal SpreadsheetCell FollowCellLink(string link)
        {
            var columnText = Regex.Replace(link, @"[^A-Z]+", string.Empty);
            var rowText    = Regex.Replace(link, @"[A-Z]+",  string.Empty);

            var column = this.AlphanumericToInteger(columnText);
            var row    = int.Parse(rowText);

            return this.GetSpreadsheetCell(column, row - 1);
        }

        /// <summary>
        ///     Converts something like AB to 28.
        /// </summary>
        /// <param name="input">
        ///     Input string
        /// </param>
        /// <returns>
        ///     Integer value
        /// </returns>
        private int AlphanumericToInteger(string input)
        {
            if (input.Length == 0)
            {
                return 0;
            }

            var letter   = char.Parse("A");
            var position = input[0] - letter;

            if (input.Length == 1)
            {
                return position;
            }

            return position + this.AlphanumericToInteger(input.Substring(1));
        }

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
            var c = (SpreadsheetCell)cell;
            this.EvaluateCell(c);
            this.CellPropertyChanged?.Invoke(cell, new PropertyChangedEventArgs(e.ToString()));
        }

        /// <summary>
        ///     Evaluates a Cell
        /// </summary>
        /// <param name="cell">
        ///     Cell to be evaluated
        /// </param>
        private void EvaluateCell(SpreadsheetCell cell)
        {
            if (!cell.Text.StartsWith("="))
            {
                cell.Value = cell.Text;
                return;
            }

            var link       = cell.Text.Substring(1);
            var linkedCell = this.FollowCellLink(link);

            cell.Text = linkedCell.Text;
        }

        /// <summary>
        ///     Gets a SpreadsheetCell from the Spreadsheet
        /// </summary>
        /// <param name="column">
        ///     The Cell Column
        /// </param>
        /// <param name="row">
        ///     The Cell Row
        /// </param>
        /// <returns>
        ///     The requested Cell, or null if it does not exist.
        /// </returns>
        internal SpreadsheetCell GetSpreadsheetCell(int column, int row)
        {
            var key = SpreadsheetCell.GenerateKey(column, row);

            return this.cells.ContainsKey(key) ? (SpreadsheetCell)this.cells[key] : null;
        }
    }
}