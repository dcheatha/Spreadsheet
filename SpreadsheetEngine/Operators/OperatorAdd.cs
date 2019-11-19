// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// OperatorAdd.cs - SpreadsheetEngine
// Created 2019/11/13 at 16:58
// ==================================================

namespace SpreadsheetEngine.Operators
{
    /// <summary>
    ///     Adding operator
    /// </summary>
    internal class OperatorAdd : ExpressionOperator
    {
        /// <summary>
        ///     Gets the precedence of the operator
        /// </summary>
        public new int Precedence { get; } = 0;

        /// <summary>
        ///     Gets Token for operator
        /// </summary>
        public override string Token { get; } = "+";

        /// <summary>
        ///     Evaluates an add expression
        /// </summary>
        /// <param name="leftValue">
        ///     Left hand value
        /// </param>
        /// <param name="rightValue">
        ///     Right hand value
        /// </param>
        /// <returns>
        ///     Expression Result
        /// </returns>
        public override double Evaluate(double leftValue, double rightValue)
        {
            return leftValue + rightValue;
        }
    }
}