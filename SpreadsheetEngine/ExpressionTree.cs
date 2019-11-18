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
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     Class for evaluating arithmetic expressions
    /// </summary>
    internal class ExpressionTree
    {
        /// <summary>
        ///     Dictionary to store all of the operators
        /// </summary>
        private static readonly Dictionary<string, ExpressionOperator> OperatorsDictionary =
            new Dictionary<string, ExpressionOperator>();

        /// <summary>
        ///     Regex matcher for variable names
        /// </summary>
        private static readonly Regex VariableNameMatcher = new Regex("[A-Za-z]+[0-9]*");

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
        ///     Adds an ExpressionOperator
        /// </summary>
        /// <param name="expressionOperator">
        ///     Expression Operator
        /// </param>
        public static void AddOperator(ExpressionOperator expressionOperator)
        {
            OperatorsDictionary.Add(expressionOperator.Token, expressionOperator);
        }

        /// <summary>
        ///     Evaluates an expression
        /// </summary>
        /// <returns>
        ///     Expression result
        /// </returns>
        public double Evaluate()
        {
            // For Homework #5, we can ignore parentheses
            var tokens = Tokenize(this.rawExpression);

            foreach (var token in tokens)
            {
            }

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

            var invalidMatcher = new Regex("[0-9]+[A-Za-z]+");

            if (invalidMatcher.IsMatch(expression))
            {
                throw new SyntaxErrorException($"Invalid variable name defined in expression {expression}");
            }

            var matches = VariableNameMatcher.Matches(expression);

            foreach (Match match in matches)
            {
                foundVariables.Add(match.Value);
            }

            return foundVariables;
        }

        /// <summary>
        ///     Splits an expression into tokens
        /// </summary>
        /// <param name="expression">
        ///     Expression string
        /// </param>
        /// <returns>
        ///     Tokenized List
        /// </returns>
        internal static List<Tuple<string, string>> Tokenize(string expression)
        {
            var matcher = new Regex(@"(?<variable>[a-zA-Z]+[0-9]*[a-zA-Z]*)|(?<number>\d+\.{0,1}\d*)|(?<parenthesisOpen>\()|(?<parenthesisClose>\))|(?<symbol>[^\(\)a-zA-Z\\s0-9]+)");

            var matches = matcher.Matches(expression);

            var tokens = new List<Tuple<string, string>>();

            // Have to create a list of the group names, because C# seems to order regex groups randomly...
            string[] groups = { "variable", "number", "parenthesisOpen", "parenthesisClose", "symbol" };

            foreach (Match match in matches)
            {
                foreach (var groupName in groups)
                {
                    var group = match.Groups[groupName];
                    if (group.Success)
                    {
                        tokens.Add(new Tuple<string, string>(group.Value.Trim(), groupName));
                    }
                }
            }

            return tokens;
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

            return foundVariables.All(variable => this.variablesDictionary.ContainsKey(variable));
        }

        /// <summary>
        ///     Uses the shunting yard algorithm to evaluate an expression
        /// </summary>
        /// <returns>
        ///     Expression value
        /// </returns>
        internal double ShuntingYardEvaluator()
        {
            // var tokens = new Stack<string>();
            return 0.0;
        }
    }
}