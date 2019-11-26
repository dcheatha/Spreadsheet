// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeNumericalNode.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

namespace SpreadsheetEngine.Nodes
{
    /// <summary>
    ///     Numerical Expression Tree Node
    /// </summary>
    internal class ExpressionTreeNumericalNode : ExpressionTreeNode
    {
        /// <summary>
        ///     Value of the node
        /// </summary>
        private readonly double value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionTreeNumericalNode" /> class
        /// </summary>
        /// <param name="value">
        ///     Value of the node
        /// </param>
        public ExpressionTreeNumericalNode(double value)
        {
            this.value = value;
        }

        /// <inheritdoc />
        public override bool CanEvaluate()
        {
            return true;
        }

        /// <inheritdoc />
        public override double Evaluate()
        {
            return this.value;
        }

        /// <inheritdoc />
        public override bool IsEndNode()
        {
            return true;
        }
    }
}