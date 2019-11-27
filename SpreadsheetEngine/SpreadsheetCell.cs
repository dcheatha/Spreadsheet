// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetCell.cs - SpreadsheetEngine
// Created 2019/10/30 at 00:23
// ==================================================

#region

using System.Runtime.CompilerServices;

#endregion

[assembly: InternalsVisibleTo("SpreadsheetEngineTester")]

namespace SpreadsheetEngine
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Spreadsheet Cell
    /// </summary>
    internal class SpreadsheetCell : Cell
    {
        /// <summary>
        ///     Expression Tree
        /// </summary>
        private readonly ExpressionTree expressionTree;

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
        ///     Gets or sets Please stop already StyleCop, I'm crying.
        /// </summary>
        public new string Value
        {
            get => base.Value;
            set
            {
                if (value == string.Empty)
                {
                    base.Value = string.Empty;
                }

                if (value.StartsWith("="))
                {
                    this.expressionTree.SetExpression(value.Substring(1));
                    var expressionValue = this.expressionTree.Evaluate();
                    this.Text = expressionValue.ToString("G");
                    base.Value = value;
                    this.expressionTree.SetVariable(this.Key, expressionValue);
                }
                else
                {
                    base.Value = value;
                }
            }
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
    }
}