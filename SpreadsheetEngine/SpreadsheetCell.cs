#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetCell.cs - SpreadsheetEngine
// Created 2019/10/30 at 00:23
// ==================================================

#endregion

#region

using System.Runtime.CompilerServices;

#endregion

[assembly: InternalsVisibleTo("SpreadsheetEngineTester")]

namespace SpreadsheetEngine
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     Spreadsheet Cell
    /// </summary>
    internal class SpreadsheetCell : Cell
    {
        /// <summary>
        ///     Cell Name Matcher
        /// </summary>
        private static readonly Regex CellNameMatcher = new Regex("[A-Z]+[0-9]+");

        /// <summary>
        ///     Expression Tree
        /// </summary>
        private readonly ExpressionTree expressionTree;

        /// <summary>
        ///     List of referenced cells
        /// </summary>
        private readonly List<SpreadsheetCell> referencedCells = new List<SpreadsheetCell>();

        /// <inheritdoc />
        public SpreadsheetCell(int columnIndex, int rowIndex, Dictionary<string, double> variablesDictionary)
            : base(columnIndex, rowIndex)
        {
            this.expressionTree = new ExpressionTree(string.Empty, variablesDictionary);
        }

        /// <summary>
        ///     Gets the Key of the Cell used to store it in the map
        /// </summary>
        public string Key => GenerateKey(this.ColumnIndex, this.RowIndex);

        /// <summary>
        ///     Gets or sets the Cell Value
        /// </summary>
        public new string Value
        {
            get => base.Value;
            set => this.Evaluate(value);
        }

        /// <summary>
        ///     Generates a Key for the Spreadsheet Dictionary
        /// </summary>
        /// <param name="column">
        ///     ColumnIndex of the cell.
        /// </param>
        /// <param name="row">
        ///     RowIndex of the cell.
        /// </param>
        /// <returns>
        ///     Dictionary Key
        /// </returns>
        public static string GenerateKey(int column, int row)
        {
            return $"{Spreadsheet.IntegerToAlphanumeric(ref column)}{row + 1}";
        }

        /// <summary>
        ///     Add a reference to a cell
        /// </summary>
        /// <param name="cell">
        ///     Spreadsheet Cell
        /// </param>
        public void AddReferencedCell(SpreadsheetCell cell)
        {
            cell.PropertyChanged += this.OnVariableChanged;
            this.referencedCells.Add(cell);
        }

        /// <summary>
        ///     Can I redo
        /// </summary>
        /// <returns>
        ///     What do you think?
        /// </returns>
        public bool CanRedo()
        {
            return this.RedoHistory.Count != 0;
        }

        /// <summary>
        ///     Can I undo
        /// </summary>
        /// <returns>
        ///     What do you think?
        /// </returns>
        public bool CanUndo()
        {
            return this.UndoHistory.Count != 0;
        }

        /// <summary>
        ///     Manual request to evaluate cell
        /// </summary>
        public void Evaluate()
        {
            this.Evaluate(this.Value);
        }

        /// <summary>
        ///     Returns a list of all referenced cell keys
        /// </summary>
        /// <returns>
        ///     List of cell keys
        /// </returns>
        public List<string> GetReferencedCellKeys()
        {
            var matches = new List<string>();

            var allVariables = this.expressionTree.GetVariables;

            foreach (var variable in allVariables)
            {
                if (CellNameMatcher.IsMatch(variable))
                {
                    matches.Add(variable);
                }
            }

            return matches;
        }

        /// <summary>
        ///     Fire this when a variable is changed
        /// </summary>
        /// <param name="sender">
        ///     Sender object
        /// </param>
        /// <param name="e">
        ///     Event args
        /// </param>
        public void OnVariableChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Evaluate(this.Value);
        }

        /// <summary>
        ///     Peak the redoHistory
        /// </summary>
        /// <returns>
        ///     Peaked redoHistory
        /// </returns>
        public (string, string) PeakRedo()
        {
            return this.RedoHistory.Peek();
        }

        /// <summary>
        ///     Peak the undoHistory
        /// </summary>
        /// <returns>
        ///     Peaked undoHistory
        /// </returns>
        public (string, string) PeakUndo()
        {
            return this.UndoHistory.Peek();
        }

        /// <summary>
        ///     Redo Function
        /// </summary>
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1009:ClosingParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        [SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1126:PrefixCallsCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        public void Redo()
        {
            var (property, value) = this.RedoHistory.Pop();

            if (property == "value")
            {
                this.Evaluate(value);
            }

            if (property == "color")
            {
                this.Color = uint.Parse(value);
            }
        }

        /// <summary>
        ///     Remove a reference to a cell
        /// </summary>
        /// <param name="cell">
        ///     Spreadsheet Cell
        /// </param>
        public void RemoveReferencedCell(SpreadsheetCell cell)
        {
            cell.PropertyChanged -= this.OnVariableChanged;
            this.referencedCells.Remove(cell);
        }

        /// <summary>
        ///     Undo Function
        /// </summary>
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1009:ClosingParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        [SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1126:PrefixCallsCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        public void Undo()
        {
            var (property, oldValue) = this.UndoHistory.Pop();

            if (property == "value")
            {
                this.RedoHistory.Push((property, this.Value));
                this.Evaluate(oldValue);
                this.UndoHistory.Pop();
            }

            if (property == "color")
            {
                this.RedoHistory.Push((property, this.Color.ToString()));
                this.Color = uint.Parse(oldValue);
                this.UndoHistory.Pop();
            }
        }

        /// <summary>
        /// Determines if the cell has a circular reference
        /// </summary>
        /// <param name="visitedCells">
        /// List of visited cells
        /// </param>
        /// <param name="key">
        /// Key of cell to check
        /// </param>
        /// <returns>
        /// True or false
        /// </returns>
        private bool HasCircularReference(List<SpreadsheetCell> visitedCells, string key)
        {
            foreach (var cell in this.referencedCells)
            {
                if (visitedCells.Contains(cell))
                {
                    continue;
                }

                visitedCells.Add(cell);

                if (cell.Key == key)
                {
                    return true;
                }

                if (cell.HasCircularReference(visitedCells, key))
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Determines if the cell has a circular reference
        /// </summary>
        /// <returns>
        /// True or false
        /// </returns>
        private bool HasCircularReference()
        {
            return this.HasCircularReference(new List<SpreadsheetCell>(), this.Key);
        }


        /// <summary>
        ///     Evaluate the cell
        /// </summary>
        /// <param name="value">
        ///     Value to evaluate with
        /// </param>
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1009:ClosingParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        [SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1126:PrefixCallsCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        private void Evaluate(string value)
        {
            if (this.HasCircularReference())
            {
                this.Text = "[circularReference]";
                return;
            }

            if (value == string.Empty)
            {
                this.Text = string.Empty;
                base.Value = string.Empty;
                return;
            }

            if (double.TryParse(value, out _))
            {
                value = "=" + value;
            }

            var matcher = new Regex("^=[A-Z]+[0-9]+$");

            if (matcher.IsMatch(value) && this.referencedCells.Count == 1)
            {
                if (this.Text == this.referencedCells.First().Text)
                {
                    return;
                }
            }

            if (matcher.IsMatch(value) && this.referencedCells.Count == 1)
            {
                this.Text = this.referencedCells.First().Text;
                this.Value = value;
                if (double.TryParse(this.Value, out var result))
                {
                    this.expressionTree.SetVariable(this.Key, result);
                }
                else
                {
                    this.expressionTree.SetVariable(this.Key, 0);
                }
            }
            else if (value.StartsWith("="))
            {
                this.expressionTree.SetExpression(value.Substring(1));
                var expressionValue = this.expressionTree.Evaluate();
                base.Value = value;

                this.expressionTree.SetVariable(this.Key, expressionValue);
                this.Text = expressionValue.ToString("G");
            }
            else
            {
                base.Value = value;
                this.Text = value;

                this.expressionTree.SetVariable(this.Key, 0);
            }
        }
    }
}