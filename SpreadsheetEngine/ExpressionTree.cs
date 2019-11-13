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

    #endregion

    /// <summary>
    /// Class for evaluating arithmetic expressions
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
        private readonly Dictionary<string, double> variablesDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class
        /// </summary>
        /// <param name="expression">
        /// Expression String
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
        /// Uses the shunting yard algorithm to evaluate an expression
        /// </summary>
        /// <returns>
        /// Expression value
        /// </returns>
        private double ShuntingYardEvaluator()
        {
            return 0.0;
        }

        /// <summary>
        /// Replaces variables with their numerical values
        /// </summary>
        /// <param name="expression">
        /// Expression to work on
        /// </param>
        /// <returns>
        /// Expression without any variables
        /// </returns>
        private string ReplaceVariables(string expression)
        {
            return string.Empty;
        }

        /// <summary>
        /// Finds all variables referenced in the expression
        /// </summary>
        /// <param name="expression">
        /// Expression to work on
        /// </param>
        /// <returns>
        /// All of the found variables
        /// </returns>
        private List<string> FindVariables(string expression)
        {
            return null;
        }
    }
}