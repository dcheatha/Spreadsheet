// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionOperator.cs - SpreadsheetEngine
// Created 2019/11/13 at 16:43
// ==================================================

namespace SpreadsheetEngine
{
    internal abstract class ExpressionOperator
    {
        /// <summary>
        ///     Token for operator
        /// </summary>
        public string Token { get; } = string.Empty;

        /// <summary>
        ///     Evaluates an operator expression
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
        public abstract double Evaluate(double leftValue, double rightValue);
    }
}