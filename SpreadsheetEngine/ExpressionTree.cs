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
    using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1111:ClosingParenthesisMustBeOnLineOfLastParameter",
            Justification = "Reviewed. Suppression is OK here."
        )]
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1009:ClosingParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here."
        )]
        private static readonly Regex TokenRegex = new Regex(
            @"(?<Variable>[a-zA-Z]+[0-9]*[a-zA-Z]*)|(?<Number>\d+\.{0,1}\d*)|(?<ParenthesisOpen>\()|(?<ParenthesisClose>\))|(?<Symbol>[^\(\)a-zA-Z\\s0-9]+)"
        );

        /// <summary>
        ///     Regex matcher for variable names
        /// </summary>
        private static readonly Regex VariableNameMatcher = new Regex("[A-Za-z]+[0-9]*");

        /// <summary>
        ///     Dictionary to store all of the variables
        /// </summary>
        private readonly Dictionary<string, double> variablesDictionary;

        /// <summary>
        ///     The raw expression string
        /// </summary>
        private string rawExpression;

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
            : this(expression, new Dictionary<string, double>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionTree" /> class
        /// </summary>
        /// <param name="expression">
        ///     Expression String
        /// </param>
        /// <param name="variables">
        ///     Dictionary of Variables
        /// </param>
        public ExpressionTree(string expression, Dictionary<string, double> variables)
        {
            this.rawExpression = expression;
            this.variablesDictionary = variables;
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
            if (this.rootNode == null)
            {
                this.rootNode = this.ShuntingYardTree(ReversePolishNotateTokens(Tokenize(this.rawExpression)));
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
        ///     Sets the Expression
        /// </summary>
        /// <param name="expression">
        ///     Expression to use
        /// </param>
        public void SetExpression(string expression)
        {
            this.rawExpression = expression;
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

            this.variablesDictionary[variableName] = variableValue;
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
        /// <param name="position">
        ///     Position to start at
        /// </param>
        /// <returns>
        ///     RPN operators list
        /// </returns>
        internal static List<Tuple<string, TokenType>> ReversePolishNotateTokens(
            List<Tuple<string, TokenType>> tokens,
            int position = 0)
        {
            var operators = new Stack<string>();
            var rpn = new List<Tuple<string, TokenType>>();

            for (var pos = position; pos < tokens.Count; pos++)
            {
                var (token, group) = tokens[pos];
                switch (group)
                {
                    case TokenType.ParenthesisOpen:
                        rpn.Add(new Tuple<string, TokenType>(token, group));
                        var parenthesisTokens = ReversePolishNotateTokens(tokens, pos + 1);
                        pos += parenthesisTokens.Count;
                        rpn.AddRange(parenthesisTokens);
                        break;
                    case TokenType.ParenthesisClose:
                        while (operators.Count != 0)
                        {
                            rpn.Add(new Tuple<string, TokenType>(operators.Pop(), TokenType.Symbol));
                        }

                        rpn.Add(new Tuple<string, TokenType>(token, group));
                        return rpn;
                    case TokenType.Symbol:
                        ReversePolishNotateOperatorHelper(ref operators, ref rpn, token);
                        break;
                    case TokenType.Number:
                    case TokenType.Variable:
                        rpn.Add(new Tuple<string, TokenType>(token, group));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            while (operators.Count != 0)
            {
                rpn.Add(new Tuple<string, TokenType>(operators.Pop(), TokenType.Symbol));
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

            return (from Match match in matches
                    from TokenType groupEnum in Enum.GetValues(typeof(TokenType))
                    let name = groupEnum.ToString()
                    let @group = match.Groups[name]
                    where @group.Success
                    select new Tuple<string, TokenType>(@group.Value.Trim(), groupEnum)).ToList();
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
        /// <param name="leftNode">
        ///     Left Node
        /// </param>
        /// <param name="rightNode">
        ///     Right Node
        /// </param>
        /// <returns>
        ///     Expression Tree Node
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Group not defined
        /// </exception>
        internal ExpressionTreeNode CreateNode(
            string token,
            TokenType group,
            ExpressionTreeNode leftNode = null,
            ExpressionTreeNode rightNode = null)
        {
            switch (group)
            {
                case TokenType.Variable:
                    return new ExpressionTreeVariableNode(this.variablesDictionary, token);
                case TokenType.Number:
                    return new ExpressionTreeNumericalNode(double.Parse(token));
                case TokenType.Symbol:
                    return new ExpressionTreeOperatorNode(OperatorsDictionary[token], leftNode, rightNode);
                default:
                    throw new ArgumentOutOfRangeException(nameof(group), group, null);
            }
        }

        /// <summary>
        ///     Uses the shunting yard algorithm to build a tree
        /// </summary>
        /// <param name="rpn">
        ///     Must be the reverse polish notation of the expression tree
        /// </param>
        /// <returns>
        ///     Expression value
        /// </returns>
        internal ExpressionTreeNode ShuntingYardTree(List<Tuple<string, TokenType>> rpn)
        {
            var pos = 0;
            return this.ShuntingYardTree(rpn, ref pos);
        }

        /// <summary>
        ///     Uses the shunting yard algorithm to build a tree
        /// </summary>
        /// <param name="rpn">
        ///     Must be the reverse polish notation of the expression tree
        /// </param>
        /// <param name="pos">
        ///     Position in the RPN to start at. Will end once amount of parentheses is unbalanced.
        /// </param>
        /// <returns>
        ///     Expression value
        /// </returns>
        internal ExpressionTreeNode ShuntingYardTree(List<Tuple<string, TokenType>> rpn, ref int pos)
        {
            if (pos > rpn.Count)
            {
                throw new ArgumentException();
            }

            var nodeStack = new Stack<ExpressionTreeNode>();

            for (; pos < rpn.Count; pos++)
            {
                var (token, group) = rpn[pos];

                switch (group)
                {
                    case TokenType.Variable:
                    case TokenType.Number:
                        nodeStack.Push(this.CreateNode(token, group));
                        break;
                    case TokenType.ParenthesisOpen:
                        pos++;
                        nodeStack.Push(this.ShuntingYardTree(rpn, ref pos));
                        break;
                    case TokenType.ParenthesisClose:
                        return nodeStack.Pop();
                    case TokenType.Symbol:
                        nodeStack.Push(this.CreateNode(token, group, nodeStack.Pop(), nodeStack.Pop()));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return nodeStack.Pop();
        }

        /// <summary>
        ///     Handles the operators for RPN
        /// </summary>
        /// <param name="operators">
        ///     Operator stack
        /// </param>
        /// <param name="rpn">
        ///     RPN list
        /// </param>
        /// <param name="token">
        ///     Current token
        /// </param>
        private static void ReversePolishNotateOperatorHelper(
            ref Stack<string> operators,
            ref List<Tuple<string, TokenType>> rpn,
            string token)
        {
            if (!OperatorsDictionary.ContainsKey(token))
            {
                throw new ArgumentException();
            }

            if (operators.Count == 0)
            {
                operators.Push(token);
                return;
            }

            if (OperatorsDictionary[token].Precedence > OperatorsDictionary[operators.Peek()].Precedence)
            {
                operators.Push(token);
            }
            else
            {
                while (operators.Count != 0)
                {
                    rpn.Add(new Tuple<string, TokenType>(operators.Pop(), TokenType.Symbol));
                }

                operators.Push(token);
            }
        }
    }
}