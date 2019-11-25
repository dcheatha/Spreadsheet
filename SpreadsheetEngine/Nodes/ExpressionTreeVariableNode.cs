// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionTreeVariableNode.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

namespace SpreadsheetEngine.Nodes
{
    #region

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Variable Expression Tree Node
    /// </summary>
    internal class ExpressionTreeVariableNode : ExpressionTreeNode
    {
        /// <summary>
        ///     Name of the variable
        /// </summary>
        private readonly string variableName;

        /// <summary>
        ///     Reference to the variables dictionary
        /// </summary>
        private readonly Dictionary<string, double> variablesDictionary;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionTreeVariableNode" /> class
        /// </summary>
        /// <param name="variablesDictionary">
        ///     Variables Dictionary
        /// </param>
        /// <param name="variableName">
        ///     Name of variable
        /// </param>
        public ExpressionTreeVariableNode(Dictionary<string, double> variablesDictionary, string variableName)
        {
            this.variablesDictionary = variablesDictionary;
            this.variableName        = variableName;
        }

        /// <inheritdoc />
        public override bool CanEvaluate()
        {
            return true;
        }

        /// <inheritdoc />
        public override double Evaluate()
        {
            if (this.variablesDictionary.ContainsKey(this.variableName))
            {
                return this.variablesDictionary[this.variableName];
            }

            throw new KeyNotFoundException();
        }

        /// <inheritdoc />
        public override bool IsEndNode()
        {
            return true;
        }
    }
}