﻿// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeTest.cs - SpreadsheetEngineTester
// Created 2019/11/13 at 15:40
// ==================================================

namespace SpreadsheetEngineTester
{
    #region

    using System.Collections.Generic;
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
    public class ExpressionTreeTest
    {
        [TestMethod]
        public void FindVariables()
        {
            var expression = "A1 + Duck * (FlyingCow061 * MapleTree) + 1";
            var found      = ExpressionTree.FindVariables(expression);

            Assert.IsNotNull(found);

            string[] variables    = { "A1", "Duck", "FlyingCow061", "MapleTree" };
            string[] notVariables = { "(", ")", "*", "+", "1", " " };

            foreach (var variable in variables)
            {
                Assert.IsTrue(found.Contains(variable));
            }

            foreach (var variable in notVariables)
            {
                Assert.IsFalse(found.Contains(variable));
            }
        }

        [TestMethod]
        public void ReplaceVariables()
        {
            var expression = "A1 + Duck * (FlyingCow061 * MapleTree * A1) + 1";

            var varDictionary = new Dictionary<string, double>
                                {
                                    ["A1"] = 1.0, ["Duck"] = 2.0, ["FlyingCow061"] = 9.8, ["MapleTree"] = 412312.34
                                };

            var tree = new ExpressionTree(expression);

            foreach (var entry in varDictionary)
            {
                tree.SetVariable(entry.Key, entry.Value);
            }

            var result   = tree.ReplaceVariables();
            var expected = "1.0 + 2.0 * (9.8 * 412312.34 * 1.0) + 1";

            Assert.AreEqual(expected, result);
        }
    }
}