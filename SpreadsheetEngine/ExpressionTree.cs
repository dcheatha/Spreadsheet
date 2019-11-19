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

            this.BuildTree(tokens);

            return this.rootNode.Evaluate();
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
            var matcher = new Regex(
                @"(?<variable>[a-zA-Z]+[0-9]*[a-zA-Z]*)|(?<number>\d+\.{0,1}\d*)|(?<parenthesisOpen>\()|(?<parenthesisClose>\))|(?<symbol>[^\(\)a-zA-Z\\s0-9]+)"
            );

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
            var operatorStack = new Stack<ExpressionTreeNode>();

            return 0.0;
        }

        /// <summary>
        ///     Adds a node to the Expression Tree
        /// </summary>
        /// <param name="node">
        ///     Expression Tree Node
        /// </param>
        /// <param name="group">
        ///     Node type
        /// </param>
        private void AddNode(ExpressionTreeNode node)
        {
            if (this.rootNode == null)
            {
                this.rootNode = node;
                return;
            }

            if (this.rootNode.IsEndNode() && node.IsEndNode())
            {
                throw new ArithmeticException();
            }

            if (this.rootNode.IsEndNode() && !node.IsEndNode())
            {
                var operatorNode = (ExpressionTreeOperatorNode)node;

                operatorNode.LeftTreeNode = this.rootNode;
                this.rootNode = operatorNode;

                return;
            }

            IterateAndPlaceNode(this.rootNode, node);
        }

        /// <summary>
        /// Iterates the tree and place a node
        /// </summary>
        /// <param name="topNode">
        /// Top node to start at
        /// </param>
        /// <param name="newNode">
        /// Node to place
        /// </param>
        private void IterateAndPlaceNode(ExpressionTreeNode topNode, ExpressionTreeNode newNode)
        {
            var currentNode = topNode;

            while (!currentNode.IsEndNode())
            {
                var operatorNode = (ExpressionTreeOperatorNode)currentNode;
                if (operatorNode.LeftTreeNode == null)
                {
                    operatorNode.LeftTreeNode = newNode;
                    return;
                }

                if (operatorNode.RightTreeNode == null)
                {
                    operatorNode.RightTreeNode = newNode;
                    return;
                }

                if (!operatorNode.LeftTreeNode.CanEvaluate())
                {
                    currentNode = operatorNode.LeftTreeNode;
                    continue;
                }

                if (!operatorNode.RightTreeNode.CanEvaluate())
                {
                    currentNode = operatorNode.RightTreeNode;
                    continue;
                }

                if (!newNode.IsEndNode())
                {
                    var temp = operatorNode.RightTreeNode;
                    var newOperatorNode = (ExpressionTreeOperatorNode)newNode;
                    newOperatorNode.LeftTreeNode = temp;
                    operatorNode.RightTreeNode = newOperatorNode;
                    return;
                }

                throw new ArithmeticException();
            }

            throw new ArithmeticException();
        }

        /// <summary>
        ///     Builds a new Expression Tree Node
        /// </summary>
        /// <param name="token">
        ///     Token from expression
        /// </param>
        /// <param name="group">
        ///     Expression Group Name
        /// </param>
        /// <returns>
        ///     Expression Tree Node
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Group not defined
        /// </exception>
        private ExpressionTreeNode BuildNode(string token, string group)
        {
            switch (group)
            {
                case "variable":
                    return new ExpressionTreeVariableNode(this.variablesDictionary, token);
                case "number":
                    return new ExpressionTreeNumericalNode(double.Parse(token));
                case "symbol":
                    return new ExpressionTreeOperatorNode(OperatorsDictionary[token], null, null);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Builds the Expression Tree in memory
        /// </summary>
        /// <param name="tokens">
        ///     List of tokens
        /// </param>
        private void BuildTree(List<Tuple<string, string>> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (tokens.Count <= 0)
            {
                throw new InvalidProgramException();
            }

            foreach (var (token, group) in tokens)
            {
                this.AddNode(this.BuildNode(token, group));
            }
        }
    }
}