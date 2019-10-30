// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Spreadsheet.cs - Spreadsheet_D._Cheatham
// Created 2019/10/29 at 21:11
// ==================================================

namespace Spreadsheet_D._Cheatham
{
    #region

    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    ///     The form 1.
    /// </summary>
    public partial class Spreadsheet : Form
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Spreadsheet" /> class.
        /// </summary>
        public Spreadsheet()
        {
            SetProcessDPIAware();
            this.InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// Loads the spreadsheet
        /// </summary>
        /// <param name="sender">
        /// Event Sender
        /// </param>
        /// <param name="e">
        /// Event that is the Event
        /// </param>
        private void SpreadsheetLoad(object sender, EventArgs e)
        {
            for (var pos = 0; pos < 26; pos++)
            {
                var letter = char.Parse("A");
                this.mainDataGridView.Columns.Add(pos.ToString(), ((char)(letter + pos)).ToString());
            }
        }
    }
}