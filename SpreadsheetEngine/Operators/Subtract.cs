// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Subtract.cs - SpreadsheetEngine
// Created 2019/11/13 at 17:13
// ==================================================

namespace SpreadsheetEngine.Operators
{
    /// <summary>
    ///     Subtraction operator
    /// </summary>
    internal class OperatorSubtract : ExpressionOperator
    {
        /// <summary>
        ///     Gets the precedence of the operator
        /// </summary>
        public new int Precedence { get; } = 0;

        /// <summary>
        ///     Gets Token for operator
        /// </summary>
        public new string Token { get; } = "-";

        /// <summary>
        ///     Evaluates an expression
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
        public static new double Evaluate(double leftValue, double rightValue)
        {
            return leftValue - rightValue;
        }
    }
}