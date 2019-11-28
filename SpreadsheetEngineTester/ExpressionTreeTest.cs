#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeTest.cs - SpreadsheetEngineTester
// Created 2019/11/13 at 15:40
// ==================================================

#endregion

namespace SpreadsheetEngineTester
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SpreadsheetEngine;

    using static SpreadsheetEngine.ExpressionTree;

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
    public class ExpressionTreeTest
    {
        /// <summary>
        /// </summary>
        /// <param name="context">
        /// </param>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1614:ElementParameterDocumentationMustHaveText",
            Justification = "Reviewed. Suppression is OK here."
        )]
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            AddDefaultOperators();
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Evaluate()
        {
            string[] expressions =
            {
                "1+1", "(9)", "1-(9)", "0^0", "1-(App1e*43.23^(0.15))", "(5*3^2)/(1+(54)/23)^0.21",
                "MagicBeans * Giant"
            };

            double[] expected = { 2, 9, -8, 1, -7.797, 34.915, 0 };

            var trees = new List<ExpressionTree>();

            while (trees.Count < expressions.Length)
            {
                trees.Add(new ExpressionTree(expressions[trees.Count]));
            }

            trees[4].SetVariable("App1e", 5);
            trees[6].SetVariable("MagicBeans", 1);

            for (var pos = 0; pos < expressions.Length; pos++)
            {
                Assert.AreEqual(expected[pos], trees[pos].Evaluate(), 0.001);
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void FindVariables()
        {
            var expression = "A1 + Duck * (FlyingCow061 * MapleTree) + 1";
            var found = ExpressionTree.FindVariables(expression);

            Assert.IsNotNull(found);

            string[] variables = { "A1", "Duck", "FlyingCow061", "MapleTree" };
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

        /// <summary>
        /// </summary>
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

        /// <summary>
        /// </summary>
        [TestMethod]
        public void ReversePolishNotation()
        {
            var expression = "1+5*3^7";

            Tuple<string, TokenType>[] expectedStrings =
            {
                new Tuple<string, TokenType>("1", TokenType.Number),
                new Tuple<string, TokenType>("5", TokenType.Number),
                new Tuple<string, TokenType>("3", TokenType.Number),
                new Tuple<string, TokenType>("7", TokenType.Number),
                new Tuple<string, TokenType>("^", TokenType.Symbol),
                new Tuple<string, TokenType>("*", TokenType.Symbol),
                new Tuple<string, TokenType>("+", TokenType.Symbol)
            };

            var rpn = ReversePolishNotateTokens(ExpressionTree.Tokenize(expression));

            for (var pos = 0; pos < expectedStrings.Length; pos++)
            {
                Assert.AreEqual(expectedStrings[pos].Item1, rpn[pos].Item1);
                Assert.AreEqual(expectedStrings[pos].Item2, rpn[pos].Item2);
            }
        }

        /// <summary>
        /// </summary>
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