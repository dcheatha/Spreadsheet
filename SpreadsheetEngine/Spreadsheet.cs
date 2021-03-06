﻿#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Spreadsheet.cs - SpreadsheetEngine
// Created 2019/10/30 at 00:12
// ==================================================

#endregion

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
    using System.Diagnostics.CodeAnalysis;

    #endregion

    #endregion

    /// <summary>
    ///     Spreadsheet Logic Engine
    /// </summary>
    public class Spreadsheet
    {
        /// <summary>
        ///     Dictionary of cells, in A1, A2, B3, etc format
        /// </summary>
        private readonly Dictionary<string, Cell> cells = new Dictionary<string, Cell>();

        /// <summary>
        ///     Redo stack
        /// </summary>
        private readonly Stack<SpreadsheetCell> redoStack = new Stack<SpreadsheetCell>();

        /// <summary>
        ///     Undo stack
        /// </summary>
        private readonly Stack<SpreadsheetCell> undoStack = new Stack<SpreadsheetCell>();

        /// <summary>
        ///     Dictionary shared among all cells' expression trees
        /// </summary>
        private readonly Dictionary<string, double> variablesDictionary = new Dictionary<string, double>();

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
            this.RowCount = rows;

            for (var column = 0; column < this.ColumnCount; column++)
            {
                for (var row = 0; row < this.RowCount; row++)
                {
                    var newCell = new SpreadsheetCell(column, row, this.variablesDictionary);
                    newCell.PropertyChanged += this.EngineCellChange;
                    this.cells.Add(newCell.Key, newCell);
                }
            }
        }

        /// <summary>
        ///     Fires whenever a cell property changes
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        /// <summary>
        ///     Can I redo
        /// </summary>
        /// <returns>
        ///     What do you think?
        /// </returns>
        public bool CanRedo => this.redoStack.Count > 0;

        /// <summary>
        ///     Can I undo
        /// </summary>
        /// <returns>
        ///     What do you think?
        /// </returns>
        public bool CanUndo => this.undoStack.Count > 0;

        /// <summary>
        ///     Gets Number of Columns
        /// </summary>
        public int ColumnCount { get; }

        /// <summary>
        ///     Gets Number of Rows
        /// </summary>
        public int RowCount { get; }

        /// <summary>
        ///     Converts something like AB to 28.
        /// </summary>
        /// <param name="input">
        ///     Input string
        /// </param>
        /// <returns>
        ///     Integer value
        /// </returns>
        public static int AlphanumericToInteger(ref string input)
        {
            if (input.Length == 0)
            {
                return 0;
            }

            /*
                So, we know that an alphanumeric representation of a cell assume the following established form:
                AAA = 26^2*z + 26*y + x
                Where x, y, and z are the character code respectively, and the characters follow the collation of base 26.
                So, we can just use base 27 and replace the digits with the ones we'd like:
            */

            const char AlphabetStart = 'A';
            var result = 0;

            for (var pos = 0; pos < input.Length; pos++)
            {
                var currentDigit = input[pos] - AlphabetStart + 1;

                result += (int)Math.Pow(26, input.Length - pos - 1) * currentDigit;
            }

            return result - 1;
        }

        /// <summary>
        ///     Converts something like AAA to 703
        /// </summary>
        /// <param name="input">
        ///     Integer value
        /// </param>
        /// <returns>
        ///     Alphanumeric value
        /// </returns>
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1009:ClosingParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        [SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1407:ArithmeticExpressionsMustDeclarePrecedence",
            Justification = "Reviewed. Suppression is OK here."
        )]
        public static string IntegerToAlphanumeric(ref int input)
        {
            var result = string.Empty;
            var value = input + 1;

            do
            {
                value--;
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            while (value > 0);

            return result;
        }

        /// <summary>
        ///     Request to change cell value
        /// </summary>
        /// <param name="column">
        ///     Column number
        /// </param>
        /// <param name="row">
        ///     Row number
        /// </param>
        /// <param name="value">
        ///     Value of cell
        /// </param>
        public void FormCellChange(int column, int row, string value)
        {
            var changedCell = this.GetSpreadsheetCell(column, row);

            if (changedCell == null)
            {
                return;
            }

            if (value == changedCell.Text)
            {
                return;
            }

            foreach (var cellKey in changedCell.GetReferencedCellKeys())
            {
                var referencedCell = this.GetSpreadsheetCell(cellKey);
                changedCell.RemoveReferencedCell(referencedCell);
            }

            changedCell.Value = value;

            foreach (var cellKey in changedCell.GetReferencedCellKeys())
            {
                var referencedCell = this.GetSpreadsheetCell(cellKey);
                changedCell.AddReferencedCell(referencedCell);
            }

            changedCell.Evaluate();
        }

        /// <summary>
        ///     Run this to request a cell's color be updated
        /// </summary>
        /// <param name="column">
        ///     Column id
        /// </param>
        /// <param name="row">
        ///     row id
        /// </param>
        /// <param name="r">
        ///     the red
        /// </param>
        /// <param name="g">
        ///     the green
        /// </param>
        /// <param name="b">
        ///     the blue
        /// </param>
        public void FormCellColorChange(int column, int row, int r, int g, int b)
        {
            var changedCell = this.GetSpreadsheetCell(column, row);

            if (changedCell == null)
            {
                return;
            }

            changedCell.ColorRgb = (255, r, g, b);
        }

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
        ///     Redo function
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     There is nothing to undo
        /// </exception>
        public void Redo()
        {
            if (!this.CanRedo)
            {
                throw new ArgumentOutOfRangeException();
            }

            var cell = this.redoStack.Pop();
            cell.Redo();
        }

        /// <summary>
        ///     Peak Redo
        /// </summary>
        /// <returns>
        ///     Peaked undo
        /// </returns>
        public (string, string, string) RedoPeak()
        {
            if (!this.CanRedo)
            {
                return (null, string.Empty, string.Empty);
            }

            var (property, value) = this.redoStack.Peek().PeakRedo();

            var cell = this.redoStack.Peek();

            return (SpreadsheetCell.GenerateKey(cell.ColumnIndex, cell.RowIndex), property, value);
        }

        /// <summary>
        ///     Undo function
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     There is nothing to undo
        /// </exception>
        public void Undo()
        {
            if (!this.CanUndo)
            {
                throw new ArgumentOutOfRangeException();
            }

            var cell = this.undoStack.Pop();
            cell.Undo();
            this.undoStack.Pop();
            this.redoStack.Push(cell);
        }

        /// <summary>
        ///     Peak Undo
        /// </summary>
        /// <returns>
        ///     Peaked undo
        /// </returns>
        public (string, string, string) UndoPeak()
        {
            if (!this.CanUndo)
            {
                return (null, string.Empty, string.Empty);
            }

            var (property, value) = this.undoStack.Peek().PeakUndo();

            var cell = this.undoStack.Peek();

            return (SpreadsheetCell.GenerateKey(cell.ColumnIndex, cell.RowIndex), property, value);
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

            return this.GetSpreadsheetCell(key);
        }

        /// <summary>
        ///     Gets a SpreadsheetCell from the Spreadsheet
        /// </summary>
        /// <param name="key">
        ///     The cell key
        /// </param>
        /// <returns>
        ///     The requested Cell, or null if it does not exist.
        /// </returns>
        internal SpreadsheetCell GetSpreadsheetCell(string key)
        {
            return this.cells.ContainsKey(key) ? (SpreadsheetCell)this.cells[key] : null;
        }

        /// <summary>
        ///     Helper function to listen to Cell events from the engine.
        /// </summary>
        /// <param name="cell">
        ///     The Cell Object
        /// </param>
        /// <param name="e">
        ///     The event fired with it.
        /// </param>
        private void EngineCellChange(object cell, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "color" || e.PropertyName == "value")
            {
                this.undoStack.Push((SpreadsheetCell)cell);
            }

            this.CellPropertyChanged?.Invoke(cell, new PropertyChangedEventArgs(e.PropertyName));
        }
    }
}