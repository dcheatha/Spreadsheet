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

    using SpreadsheetEngine.Nodes;
    using SpreadsheetEngine.Operators;

    #endregion

    /// <summary>
    ///     Class for evaluating arithmetic expressions
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        ///     Dictionary to store all of the operators
        /// </summary>
        private static readonly Dictionary<string, ExpressionOperator> OperatorsDictionary =
            new Dictionary<string, ExpressionOperator>();

        /// <summary>
        ///     Regex matcher for tokens
        /// </summary>
        private static readonly Regex TokenRegex = new Regex(
            @"(?<variable>[a-zA-Z]+[0-9]*[a-zA-Z]*)|(?<number>\d+\.{0,1}\d*)|(?<parenthesisOpen>\()|(?<parenthesisClose>\))|(?<symbol>[^\(\)a-zA-Z\\s0-9]+)"
            // ReSharper disable StyleCop.SA1009
        );
        // ReSharper restore StyleCop.SA1009

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
        ///     Root of the Expression Tree
        /// </summary>
        private ExpressionTreeNode rootNode;

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
        ///     TokenType enumerator
        /// </summary>
        internal enum TokenType
        {
            /// <summary>
            ///     Variable TokenType
            /// </summary>
            Variable,

            /// <summary>
            ///     Number TokenType
            /// </summary>
            Number,

            /// <summary>
            ///     Parenthesis Open TokenType
            /// </summary>
            ParenthesisOpen,

            /// <summary>
            ///     Parenthesis Close TokenType
            /// </summary>
            ParenthesisClose,

            /// <summary>
            ///     Symbol TokenType
            /// </summary>
            Symbol
        }

        /// <summary>
        ///     Adds default expression operators (Add, Subtract, Divide, Multiply, and Power)
        /// </summary>
        public static void AddDefaultOperators()
        {
            ExpressionOperator[] defaultOperators =
            {
                new OperatorAdd(), new OperatorDivide(), new OperatorMultiply(), new OperatorPower(),
                new OperatorSubtract()
            };

            foreach (var op in defaultOperators)
            {
                AddOperator(op);
            }
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
            // For Homework #5, we can ignore parentheses, and we will always have the same symbol.
            var tokens = Tokenize(this.rawExpression);

            if (this.rootNode == null)
            {
                this.BuildTree(tokens);
            }

            return this.rootNode.Evaluate();
        }

        /// <summary>
        ///     Checks if all of the variables in the expression are defined
        /// </summary>
        /// <returns>
        ///     Truth value
        /// </returns>
        public bool HasVariables()
        {
            var foundVariables = FindVariables(this.rawExpression);

            return foundVariables.All(variable => this.variablesDictionary.ContainsKey(variable));
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

        /// <inheritdoc />
        public override string ToString()
        {
            return this.rawExpression;
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
        ///     Takes a list of tokens and puts it into Reverse Polish Notation
        /// </summary>
        /// <param name="tokens">
        ///     List of tokens generated by ExpressionTree.Tokenize
        /// </param>
        /// <returns>
        ///     RPN operators list
        /// </returns>
        internal static List<Tuple<string, TokenType>> ReversePolishNotateTokens(List<Tuple<string, TokenType>> tokens)
        {
            var operators = new Stack<string>();
            var rpn       = new List<Tuple<string, TokenType>>();

            foreach (var (token, group) in tokens)
            {
            }

            return rpn;
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
        internal static List<Tuple<string, TokenType>> Tokenize(string expression)
        {
            var matches = TokenRegex.Matches(expression);

            var tokens = new List<Tuple<string, TokenType>>();

            foreach (Match match in matches)
            {
                foreach (TokenType groupEnum in Enum.GetValues(typeof(TokenType)))
                {
                    var name  = groupEnum.ToString();
                    var group = match.Groups[name];
                    if (group.Success)
                    {
                        tokens.Add(new Tuple<string, TokenType>(group.Value.Trim(), groupEnum));
                    }
                }
            }

            return tokens;
        }

        /// <summary>
        ///     Uses the shunting yard algorithm to evaluate an expression
        /// </summary>
        /// <returns>
        ///     Expression value
        /// </returns>
        internal double ShuntingYardEvaluator()
        {
            var operatorStack = new Stack<ExpressionTreeOperatorNode>();
            var treeStack     = new Stack<ExpressionTreeNode>();

            return 0.0;
        }

        /// <summary>
        ///     Builds the Expression Tree in memory
        /// </summary>
        /// <param name="tokens">
        ///     List of tokens
        /// </param>
        private void BuildTree(List<Tuple<string, TokenType>> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (tokens.Count <= 0)
            {
                throw new InvalidProgramException();
            }

            /*
            foreach (var (token, group) in tokens)
            {
                this.AddNode(this.BuildNode(token, group));
            }
            */
        }
    }
}