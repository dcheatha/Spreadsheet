// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetTest.cs - SpreadsheetEngineTester
// Created 2019/10/30 at 22:43
// ==================================================

namespace SpreadsheetEngineTester
{
    #region

    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SpreadsheetEngine;

    #endregion

    /// <summary>
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1606:ElementDocumentationMustHaveSummaryText",
        Justification = "Reviewed. Suppression is OK here."
    )]
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
    public class SpreadsheetTest
    {
        /// <summary>
        /// </summary>
        [TestMethod]
        public void EvaluateCell()
        {
            var spreadsheet = new Spreadsheet(100, 100);

            spreadsheet.GetSpreadsheetCell(0, 0).Value = "Test";
            spreadsheet.GetSpreadsheetCell(0, 1).Value = "=A1";

            Assert.AreEqual(spreadsheet.GetSpreadsheetCell(0, 0).Text, spreadsheet.GetSpreadsheetCell(0, 1).Text);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void GetCellLink()
        {
            var spreadsheet = new Spreadsheet(100, 100);

            string[,] match = { { "A1", "0:0" }, { "B2", "1:1" }, { "Z100", "25:99" } };

            for (var pos = 0; pos < match.GetLength(0); pos++)
            {
                Assert.AreEqual(spreadsheet.FollowCellLink(match[pos, 0]).Key, match[pos, 1]);
            }
        }
    }
}