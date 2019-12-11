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

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    #endregion

    /// <summary>
    ///     Represents a cell in the spreadsheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        ///     Redo history of this cell
        /// </summary>
        protected readonly Stack<(string, string)> RedoHistory = new Stack<(string, string)>();

        /// <summary>
        ///     Undo history of this cell
        /// </summary>
        protected readonly Stack<(string, string)> UndoHistory = new Stack<(string, string)>();

        /// <summary>
        ///     Color newValue
        /// </summary>
        private uint color = uint.MaxValue;

        /// <summary>
        ///     Text newValue
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        ///     Value newValue
        /// </summary>
        private string value = string.Empty;

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
        ///     Gets or sets color, from snow white to coal black and all the hue in between
        /// </summary>
        public uint Color
        {
            get => this.color;
            protected set
            {
                if (this.color == value)
                {
                    return;
                }

                var oldValue = this.color;
                this.color = value;
                this.EmitPropertyChanged("color", oldValue.ToString());
            }
        }

        /// <summary>
        ///     Gets or sets the ARGB newValue of Color
        /// </summary>
        public (int alpha, int red, int green, int blue) ColorRgb
        {
            get
            {
                var values = new byte[4];
                for (var pos = 0; pos < values.Length; pos++)
                {
                    values[pos] = (byte)(this.Color >> (pos * 8));
                }

                return (values[3], values[2], values[1], values[0]);
            }

            set => this.Color = (uint)((value.alpha << 24) | (value.red << 16) | (value.green << 8) | value.blue);
        }

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
        public string Text
        {
            get => this.text;
            protected set
            {
                if (this.text == value)
                {
                    return;
                }

                var oldValue = this.text;
                this.text = value;
                this.EmitPropertyChanged("text", oldValue);
            }
        }

        /// <summary>
        ///     Gets or sets newValue of the cell
        /// </summary>
        public string Value
        {
            get => this.value;
            protected set
            {
                if (this.value == value)
                {
                    return;
                }

                var oldValue = this.value;
                this.value = value;
                this.EmitPropertyChanged("value", oldValue);
            }
        }

        /// <summary>
        ///     Emits a property change
        /// </summary>
        /// <param name="property">
        ///     Property that changed
        /// </param>
        /// <param name="oldValue">
        ///     the old value of the change
        /// </param>
        private void EmitPropertyChanged(string property, string oldValue)
        {
            if (property == "color" || property == "value")
            {
                this.UndoHistory.Push((property, oldValue));
                Console.WriteLine($"Got undo item {property} with value {oldValue}");
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}