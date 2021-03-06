﻿#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeNode.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

#endregion

namespace SpreadsheetEngine.Nodes
{
    /// <summary>
    ///     Expression Tree Node
    /// </summary>
    internal abstract class ExpressionTreeNode
    {
        /// <summary>
        ///     Can the node evaluate?
        /// </summary>
        /// <returns>
        ///     True or False
        /// </returns>
        public abstract bool CanEvaluate();

        /// <summary>
        ///     Evaluates the given node
        /// </summary>
        /// <returns>
        ///     Floating point value of the node
        /// </returns>
        public abstract double Evaluate();

        /// <summary>
        ///     Determines if the node is an end node
        /// </summary>
        /// <returns>
        ///     Is end node or not
        /// </returns>
        public abstract bool IsEndNode();
    }
}