// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// CellTest.cs - SpreadsheetEngineTester
// Created 2019/10/30 at 22:28
// ==================================================

namespace SpreadsheetEngineTester
{
    #region

    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SpreadsheetEngine;

    #endregion

    [SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1009:ClosingParenthesisMustBeSpacedCorrectly",
        Justification = "Reviewed. Suppression is OK here."
    )]
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here."
    )]
    [TestClass]
    public class CellTest
    {
        [TestMethod]
        public void PropertyChangedEmitter()
        {
            var cell = new SpreadsheetCell(0, 0);

            var passing = false;

            cell.PropertyChanged += (sender, args) => { passing = true; };

            cell.Text = "Test";

            Assert.IsTrue(passing);
        }
    }
}