// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeTest.cs - SpreadsheetEngineTester
// Created 2019/11/13 at 15:40
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
        public void HasVariables()
        {
            var expression = "Apples * Oranges + (PassionFruit / Fruits) + Apples * Coffee";

            string[] variables = { "Apples", "Oranges", "PassionFruit", "Fruits" };

            var tree = new ExpressionTree(expression);

            foreach (var variable in variables)
            {
                tree.SetVariable(variable, 1.0);
            }

            Assert.IsFalse(tree.HasVariables());

            tree.SetVariable("Coffee", 1.0);

            Assert.IsTrue(tree.HasVariables());
        }

        [TestMethod]
        public void Tokenize()
        {
            var expression = "0+(App1e @@ 43.23) / 0.15";

            string[,] expected =
            {
                { "0", "number" }, { "+", "symbol" }, { "(", "parenthesisOpen" }, { "App1e", "variable" },
                { "@@", "symbol" }, { "43.23", "number" }, { ")", "parenthesisClose" }, { "/", "symbol" },
                { "0.15", "number" }
            };

            var result = ExpressionTree.Tokenize(expression);
            for (var pos = 0; pos < expected.GetLength(0); pos++)
            {
                Assert.AreEqual(expected[pos, 0], result[pos].Item1);
                Assert.AreEqual(expected[pos, 1], result[pos].Item2);
            }
        }
    }
}