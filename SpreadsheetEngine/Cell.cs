﻿#region a

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
    using System.ComponentModel;

    #endregion

    /// <summary>
    ///     Represents a cell in the spreadsheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        ///     Color value
        /// </summary>
        private uint color;

        /// <summary>
        ///     Text value
        /// </summary>
        private string text;

        /// <summary>
        ///     Value value
        /// </summary>
        private string value;

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

                this.color = value;
                this.EmitPropertyChanged("color");
            }
        }

        /// <summary>
        ///     Gets or sets the ARGB value of Color
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

                this.text = value;
                this.EmitPropertyChanged("text");
            }
        }

        /// <summary>
        ///     Gets or sets value of the cell
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

                this.value = value;
                this.EmitPropertyChanged("value");
            }
        }

        /// <summary>
        ///     Emits a property change
        /// </summary>
        /// <param name="property">
        ///     Property that changed
        /// </param>
        private void EmitPropertyChanged(string property)
        {
            Console.WriteLine($"Sending event {property}");
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}