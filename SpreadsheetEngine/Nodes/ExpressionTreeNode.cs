// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeNode.cs - SpreadsheetEngine
// Created 2019/11/17 at 23:46
// ==================================================

namespace SpreadsheetEngine.Nodes
{
    /// <summary>
    /// Expression Tree Node
    /// </summary>
    internal abstract class ExpressionTreeNode
    {
        /// <summary>
        /// Evaluates the given node
        /// </summary>
        /// <returns>
        /// Floating point value of the node
        /// </returns>
        public abstract double Evaluate();
    }
}