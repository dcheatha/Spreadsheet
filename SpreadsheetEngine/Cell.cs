#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Cell.cs - SpreadsheetEngine
// Created 2019/10/29 at 22:10
// ==================================================

#endregion

#region

using System.Runtime.CompilerServices;

#endregion

[assembly: InternalsVisibleTo("SpreadsheetEngineTester")]

namespace SpreadsheetEngine
{
    #region

    using System.ComponentModel;

    #endregion

    /// <summary>
    ///     Represents a cell in the spreadsheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell" /> class.
        /// </summary>
        /// <param name="columnIndex">
        ///     ColumnIndex of the cell.
        /// </param>
        /// <param name="rowIndex">
        ///     RowIndex of the cell.
        /// </param>
        protected Cell(int columnIndex, int rowIndex)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.Text = string.Empty;
            this.Value = string.Empty;
        }

        /// <summary>
        ///     Event Handler for Cell
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets ColumnIndex of the cell.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        ///     Gets RowIndex of the cell.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        ///     Gets or sets Text Value of the cell.
        /// </summary>
        public string Text { get; protected set; }

        /// <summary>
        ///     Gets or sets value of the cell
        /// </summary>
        public string Value { get; protected set; }

        /// <summary>
        ///     Emits a property change
        /// </summary>
        /// <param name="property">
        ///     Property that changed
        /// </param>
        protected void EmitPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}