﻿#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Program.cs - Spreadsheet_D._Cheatham
// Created 2019/10/29 at 21:11
// ==================================================

#endregion

namespace Spreadsheet_D._Cheatham
{
    #region

    using System;
    using System.Windows.Forms;

    using SpreadsheetEngine;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ExpressionTree.AddDefaultOperators();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpreadsheetForm());
        }
    }
}