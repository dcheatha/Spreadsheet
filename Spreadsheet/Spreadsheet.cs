// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Spreadsheet.cs - Spreadsheet_D._Cheatham
// Created 2019/10/29 at 21:11
// ==================================================

namespace Spreadsheet_D._Cheatham
{
    #region

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
    }
}