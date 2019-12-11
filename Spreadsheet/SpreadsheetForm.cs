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
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
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
        ///     Place to hold the current input box
        /// </summary>
        private TextBox editBox;

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
        ///     Launch the Change Background Color thing
        /// </summary>
        /// <param name="sender">
        ///     Event Sender
        /// </param>
        /// <param name="e">
        ///     Event E thing
        /// </param>
        private void ChangeBackgroundColorToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();

            var color = this.colorDialog1.Color;

            foreach (DataGridViewTextBoxCell cell in this.mainDataGridView.SelectedCells)
            {
                this.spreadsheet.FormCellColorChange(cell.ColumnIndex, cell.RowIndex, color.R, color.G, color.B);
            }
        }

        /// <summary>
        ///     On cell start edit
        /// </summary>
        /// <param name="sender">
        ///     Sender object
        /// </param>
        /// <param name="e">
        ///     Event e
        /// </param>
        private void OnCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var value = this.spreadsheet.GetCell(e.ColumnIndex, e.RowIndex).Value;
            this.mainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;

            if (this.editBox != null)
            {
                this.editBox.Text = value;
            }
        }

        /// <summary>
        ///     On Cell End Edit
        /// </summary>
        /// <param name="sender">
        ///     Sender object
        /// </param>
        /// <param name="e">
        ///     Event e
        /// </param>
        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.mainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                this.spreadsheet.GetCell(e.ColumnIndex, e.RowIndex).Text;
        }

        /// <summary>
        ///     Fired when a cell is selected
        /// </summary>
        /// <param name="sender">
        ///     The form
        /// </param>
        /// <param name="e">
        ///     Event parameters
        /// </param>
        private void OnCellSelect(object sender, EventArgs e)
        {
            var columnIndex = this.mainDataGridView.CurrentCell.ColumnIndex;
            var rowIndex = this.mainDataGridView.CurrentCell.RowIndex;
            var cell = this.spreadsheet.GetCell(columnIndex, rowIndex);
            this.formulaBox.Text = $@"({columnIndex + 1}, {rowIndex + 1}) Text: {cell.Text} Value: {cell.Value}";
        }

        /// <summary>
        ///     On Edit Control Showing
        /// </summary>
        /// <param name="sender">
        ///     Something sending
        /// </param>
        /// <param name="e">
        ///     the event
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
        private void OnEditControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!(e.Control is TextBox box))
            {
                return;
            }

            this.editBox = box;
            var rowIndex = this.mainDataGridView.CurrentCell.RowIndex;
            var columnIndex = this.mainDataGridView.CurrentCell.ColumnIndex;

            this.editBox.Text = this.spreadsheet.GetCell(columnIndex, rowIndex).Value;
        }

        /// <summary>
        ///     Runs when a cell is changed in the engine
        /// </summary>
        /// <param name="sender">
        ///     Spreadsheet Engine
        /// </param>
        /// <param name="e">
        ///     Part that changed
        /// </param>
        [SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1126:PrefixCallsCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        private void OnEngineCellChange(object sender, PropertyChangedEventArgs e)
        {
            var engineCell = (Cell)sender;
            var formCell = this.mainDataGridView.Rows[engineCell.RowIndex].Cells[engineCell.ColumnIndex];
            switch (e.PropertyName)
            {
                case "value":
                {
                    break;
                }

                case "text":
                {
                    formCell.Value = engineCell.Text;
                    break;
                }

                case "color":
                {
                    var (a, r, g, b) = engineCell.ColorRgb;
                    formCell.Style.BackColor = Color.FromArgb(a, r, g, b);
                    break;
                }
            }

            this.ProcessUndoRedo();
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
            var columnIndex = e.ColumnIndex;
            var rowIndex = e.RowIndex;

            var value = (string)this.mainDataGridView.Rows[rowIndex].Cells[columnIndex].Value;

            this.spreadsheet.FormCellChange(columnIndex, rowIndex, value);
        }

        /// <summary>
        ///     Process undo and redo states
        /// </summary>
        private void ProcessUndoRedo()
        {
            if (this.spreadsheet.CanUndo)
            {
                var (cell, property, value) = this.spreadsheet.UndoPeak();
                this.undoToolStripMenuItem.Text = $@"Undo (Set {cell} {property} to '{value}')";
            }
            else
            {
                this.undoToolStripMenuItem.Text = @"Undo";
            }

            if (this.spreadsheet.CanRedo)
            {
                var (cell, property, value) = this.spreadsheet.RedoPeak();
                this.redoToolStripMenuItem.Text = $@"Redo (Set {cell} {property} to '{value}')";
            }
            else
            {
                this.redoToolStripMenuItem.Text = @"Redo";
            }

            this.undoToolStripMenuItem.Enabled = this.spreadsheet.CanUndo;
            this.redoToolStripMenuItem.Enabled = this.spreadsheet.CanRedo;
        }

        /// <summary>
        ///     Redoes stuff
        /// </summary>
        /// <param name="sender">
        ///     Sender sender
        /// </param>
        /// <param name="e">
        ///     Event event
        /// </param>
        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.spreadsheet.Redo();
            this.ProcessUndoRedo();
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

            for (var pos = 0; pos < columns; pos++)
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

            this.mainDataGridView.SelectionChanged += this.OnCellSelect;

            this.mainDataGridView.CellBeginEdit += this.OnCellBeginEdit;
            this.mainDataGridView.CellEndEdit += this.OnCellEndEdit;
            this.mainDataGridView.EditingControlShowing += this.OnEditControlShowing;
            this.ProcessUndoRedo();
        }

        /// <summary>
        ///     Undo Stuff
        /// </summary>
        /// <param name="sender">
        ///     Sender sender
        /// </param>
        /// <param name="e">
        ///     Event e
        /// </param>
        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.spreadsheet.Undo();
            this.ProcessUndoRedo();
        }
    }
}