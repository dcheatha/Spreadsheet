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

    #endregion

    /// <summary>
    ///     Spreadsheet Cell
    /// </summary>
    internal class SpreadsheetCell : Cell
    {
        /// <summary>
        ///     Expression Tree
        /// </summary>
        private readonly ExpressionTree expressionTree = new ExpressionTree(string.Empty);

        /// <inheritdoc />
        public SpreadsheetCell(int columnIndex, int rowIndex)
            : base(columnIndex, rowIndex)
        {
        }

        /// <summary>
        ///     Gets the Key of the Cell used to store it in the map
        /// </summary>
        public Tuple<int, int> Key => GenerateKey(this.ColumnIndex, this.RowIndex);

        /// <summary>
        ///     Gets or sets Please stop already StyleCop, I'm crying.
        /// </summary>
        public new string Value
        {
            get => base.Value;
            set
            {
                if (value.StartsWith("="))
                {
                    this.expressionTree.SetExpression(value.Substring(1));
                    base.Text = this.expressionTree.Evaluate().ToString("G");
                    base.Value = value;
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
        public static Tuple<int, int> GenerateKey(int column, int row)
        {
            return new Tuple<int, int>(column, row);
        }
    }
}