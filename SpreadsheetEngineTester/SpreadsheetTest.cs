// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// SpreadsheetTest.cs - SpreadsheetEngineTester
// Created 2019/10/30 at 22:43
// ==================================================

namespace SpreadsheetEngineTester
{
    #region

    using System;
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
        [TestMethod]
        public void Base26Conversions()
        {
            Tuple<int, string>[] expected =
            {
                new Tuple<int, string>(0,   "A"), new Tuple<int, string>(25,     "Z"), new Tuple<int, string>(78, "CA"),
                new Tuple<int, string>(26,  "AA"), new Tuple<int, string>(701,   "ZZ"),
                new Tuple<int, string>(702, "AAA"), new Tuple<int, string>(5345, "GWP")
            };

            for (var pos = 0; pos < expected.Length; pos++)
            {
                var (input, output) = expected[pos];
                Assert.AreEqual(input, Spreadsheet.AlphanumericToInteger(ref output));

                Assert.AreEqual(output, Spreadsheet.IntegerToAlphanumeric(ref input));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void EvaluateCell()
        {
            var spreadsheet = new Spreadsheet(100, 100);

            spreadsheet.GetSpreadsheetCell(0, 0).Value = "=(1+2)^3";
            spreadsheet.GetSpreadsheetCell(0, 1).Value = "=A1";

            Assert.AreEqual(spreadsheet.GetSpreadsheetCell(0, 0).Text, spreadsheet.GetSpreadsheetCell(0, 1).Text);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void GetCellLink()
        {
            var spreadsheet = new Spreadsheet(100, 100);

            string[] checks = { "A1", "B5", "Z100" };

            for (var pos = 0; pos < checks.GetLength(0); pos++)
            {
                Assert.AreEqual(checks[pos], spreadsheet.FollowCellLink(checks[pos]).Key);
            }
        }
    }
}