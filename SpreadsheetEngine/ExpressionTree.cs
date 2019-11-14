// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTree.cs - SpreadsheetEngine
// Created 2019/11/05 at 19:32
// ==================================================

namespace SpreadsheetEngine
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     Class for evaluating arithmetic expressions
    /// </summary>
    internal class ExpressionTree
    {
        /// <summary>
        ///     The raw expression string
        /// </summary>
        private readonly string rawExpression;

        /// <summary>
        ///     Dictionary to store all of the variables
        /// </summary>
        private readonly Dictionary<string, double> variablesDictionary = new Dictionary<string, double>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionTree" /> class
        /// </summary>
        /// <param name="expression">
        ///     Expression String
        /// </param>
        public ExpressionTree(string expression)
        {
            this.rawExpression = expression;
        }

        /// <summary>
        ///     Evaluates an expression
        /// </summary>
        /// <returns>
        ///     Expression result
        /// </returns>
        public double Evaluate()
        {
            return 0.0;
        }

        /// <summary>
        ///     Defines a variable in the expression tree
        /// </summary>
        /// <param name="variableName">
        ///     Name of the variable
        /// </param>
        /// <param name="variableValue">
        ///     Value of the variable
        /// </param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (string.IsNullOrEmpty(variableName))
            {
                throw new Exception("variableName must not be empty or null");
            }

            this.variablesDictionary.Add(variableName, variableValue);
        }

        /// <summary>
        ///     Finds all variables referenced in the expression
        /// </summary>
        /// <param name="expression">
        ///     Expression to work on
        /// </param>
        /// <returns>
        ///     All of the found variables
        /// </returns>
        internal static HashSet<string> FindVariables(string expression)
        {
            var foundVariables = new HashSet<string>();

            var matcher        = new Regex("[A-Za-z]+[0-9]*");
            var invalidMatcher = new Regex("[0-9]+[A-Za-z]+");

            if (invalidMatcher.IsMatch(expression))
            {
                throw new SyntaxErrorException($"Invalid variable name defined in expression {expression}");
            }

            var matches = matcher.Matches(expression);

            foreach (Match match in matches)
            {
                foundVariables.Add(match.Value);
            }

            return foundVariables;
        }

        /// <summary>
        ///     Checks if all of the variables in the expression are defined
        /// </summary>
        /// <returns>
        ///     Truth value
        /// </returns>
        internal bool HasVariables()
        {
            var foundVariables = FindVariables(this.rawExpression);

            foreach (var variable in foundVariables)
            {
                if (!this.variablesDictionary.ContainsKey(variable))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Uses the shunting yard algorithm to evaluate an expression
        /// </summary>
        /// <returns>
        ///     Expression value
        /// </returns>
        internal double ShuntingYardEvaluator()
        {
            var tokens = new Stack<string>();

            return 0.0;
        }
    }
}