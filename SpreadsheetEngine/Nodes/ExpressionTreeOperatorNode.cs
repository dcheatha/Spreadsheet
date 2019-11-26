﻿// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeOperatorNode.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

namespace SpreadsheetEngine.Nodes
{
    #region

    using System;

    using SpreadsheetEngine.Operators;

    #endregion

    #region

    #endregion

    /// <summary>
    ///     Operator Expression Tree Node
    /// </summary>
    internal class ExpressionTreeOperatorNode : ExpressionTreeNode
    {
        /// <summary>
        ///     Holds a reference to the evaluator
        /// </summary>
        private readonly ExpressionOperator evaluator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionTreeOperatorNode" /> class
        /// </summary>
        /// <param name="evaluator">
        ///     Object that evaluates the expression
        /// </param>
        /// <param name="leftTreeNode">
        ///     Left node
        /// </param>
        /// <param name="rightTreeNode">
        ///     Right node
        /// </param>
        public ExpressionTreeOperatorNode(
            ExpressionOperator evaluator,
            ExpressionTreeNode leftTreeNode,
            ExpressionTreeNode rightTreeNode)
        {
            this.evaluator     = evaluator;
            this.LeftTreeNode  = leftTreeNode;
            this.RightTreeNode = rightTreeNode;
        }

        /// <summary>
        ///     Gets or sets Node on the left
        /// </summary>
        public ExpressionTreeNode LeftTreeNode { get; set; }

        /// <summary>
        ///     Gets or sets Node on the right
        /// </summary>
        public ExpressionTreeNode RightTreeNode { get; set; }

        /// <inheritdoc />
        public override bool CanEvaluate()
        {
            if (this.LeftTreeNode == null || this.RightTreeNode == null)
            {
                return false;
            }

            return this.LeftTreeNode.CanEvaluate() && this.RightTreeNode.CanEvaluate();
        }

        /// <inheritdoc />
        public override double Evaluate()
        {
            double leftValue = this.LeftTreeNode.Evaluate();
            double rightValue = this.RightTreeNode.Evaluate();
            double evalValue = this.evaluator.Evaluate(leftValue, rightValue);
            Console.WriteLine($"{evalValue} = {rightValue} {this.evaluator.Token} {leftValue}");
            return evalValue;
            // return this.evaluator.Evaluate(this.LeftTreeNode.Evaluate(), this.RightTreeNode.Evaluate());
        }

        /// <inheritdoc />
        public override bool IsEndNode()
        {
            return false;
        }
    }
}