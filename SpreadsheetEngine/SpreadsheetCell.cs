// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetCell.cs - SpreadsheetEngine
// Created 2019/10/30 at 00:23
// ==================================================

namespace SpreadsheetEngine
{
    /// <summary>
    ///     Spreadsheet Cell
    /// </summary>
    internal class SpreadsheetCell : Cell
    {
        /// <inheritdoc />
        public SpreadsheetCell(int columnIndex, int rowIndex)
            : base(columnIndex, rowIndex)
        {
        }

        /// <summary>
        ///     Gets the Key of the Cell used to store it in the map
        /// </summary>
        public string Key => GenerateKey(this.ColumnIndex, this.RowIndex);

        /// <summary>
        /// Gets or sets Please stop already StyleCop, I'm crying.
        /// </summary>
        public new string Value
        {
            get => base.Value;
            set => base.Value = value;
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
            return $"{column}:{row}";
        }
    }
}