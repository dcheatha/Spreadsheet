// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeTest.cs - SpreadsheetEngineTester
// Created 2019/11/13 at 15:40
// ==================================================

namespace SpreadsheetEngineTester
{
    #region

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SpreadsheetEngine;
    using SpreadsheetEngine.Operators;
    using static SpreadsheetEngine.ExpressionTree;

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
        public void Evaluate()
        {
            var expression = "1-(App1e*43.23^(0.15))";
            var tree       = new ExpressionTree(expression);

            ExpressionTree.AddDefaultOperators();

            tree.SetVariable("App1e", 5);

            Assert.AreEqual(-7.797, tree.Evaluate(), 0.001);
        }

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

            Tuple<string, TokenType>[] expected =
            {
                new Tuple<string, TokenType>("0",     TokenType.Number),
                new Tuple<string, TokenType>("+",     TokenType.Symbol),
                new Tuple<string, TokenType>("(",     TokenType.ParenthesisOpen),
                new Tuple<string, TokenType>("App1e", TokenType.Variable),
                new Tuple<string, TokenType>("@@",    TokenType.Symbol),
                new Tuple<string, TokenType>("43.23", TokenType.Number),
                new Tuple<string, TokenType>(")",     TokenType.ParenthesisClose),
                new Tuple<string, TokenType>("/",     TokenType.Symbol),
                new Tuple<string, TokenType>("0.15",  TokenType.Number)
            };

            var actual = ExpressionTree.Tokenize(expression);
            for (var pos = 0; pos < expected.GetLength(0); pos++)
            {
                Assert.AreEqual(expected[pos].Item1, actual[pos].Item1);
                Assert.AreEqual(expected[pos].Item2, actual[pos].Item2);
            }
        }
    }
}