// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetForm.cs - Spreadsheet_D._Cheatham
// Created 2019/10/29 at 21:11
// ==================================================

#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetForm.cs - Spreadsheet_D._Cheatham
// Created 2019/10/29 at 21:11
// ==================================================

#endregion

#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetForm.cs - Spreadsheet_D._Cheatham
// Created 2019/10/29 at 21:11
// ==================================================

#endregion

namespace Spreadsheet_D._Cheatham
{
    #region

    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using SpreadsheetEngine;

    #endregion

    /// <summary>
    ///     The form 1.
    /// </summary>
    public partial class SpreadsheetForm : Form
    {
        /// <summary>
        ///     Spreadsheet that represents the DataGridView
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpreadsheetForm" /> class.
        /// </summary>
        public SpreadsheetForm()
        {
            SetProcessDPIAware();
            this.InitializeComponent();
        }

        /// <summary>
        ///     Makes the process DPI Aware
        /// </summary>
        /// <returns>
        ///     Returns Awesomeness
        /// </returns>
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        ///     Runs when a cell is changed in the engine
        /// </summary>
        /// <param name="sender">
        ///     Spreadsheet Engine
        /// </param>
        /// <param name="e">
        ///     Part that changed
        /// </param>
        private void OnEngineCellChange(object sender, EventArgs e)
        {
            var cell = (Cell)sender;
            Console.WriteLine($@"EngineCell changed with value {cell.Value}");
            this.mainDataGridView.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
        }

        /// <summary>
        ///     Fired when a cell is changed in the UI
        /// </summary>
        /// <param name="sender">
        ///     The form
        /// </param>
        /// <param name="e">
        ///     Event parameters
        /// </param>
        private void OnFormCellChange(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($@"FormCell changed {e.ColumnIndex}:{e.RowIndex}");
        }

        /// <summary>
        ///     Loads the spreadsheet
        /// </summary>
        /// <param name="sender">
        ///     Event Sender
        /// </param>
        /// <param name="e">
        ///     Event that is the Event
        /// </param>
        private void SpreadsheetLoad(object sender, EventArgs e)
        {
            var columns = 50;
            var rows = 150;

            for (var pos = 1; pos < columns + 1; pos++)
            {
                this.mainDataGridView.Columns.Add(pos.ToString(), Spreadsheet.IntegerToAlphanumeric(ref pos));
            }

            this.mainDataGridView.Rows.Add(rows);
            for (var pos = 0; pos < rows; pos++)
            {
                // Set headers for all of the rows.
                this.mainDataGridView.Rows[pos].HeaderCell.Value = (pos + 1).ToString();
            }

            this.spreadsheet = new Spreadsheet(columns, rows);
            this.spreadsheet.CellPropertyChanged += this.OnEngineCellChange;

            this.mainDataGridView.CellValueChanged += this.OnFormCellChange;
        }
    }
}