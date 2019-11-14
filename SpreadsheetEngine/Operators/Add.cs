// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Add.cs - SpreadsheetEngine
// Created 2019/11/13 at 16:58
// ==================================================

namespace SpreadsheetEngine.Operators
{
    /// <summary>
    /// Adding operator
    /// </summary>
    internal class OperatorAdd : ExpressionOperator
    {
        /// <summary>
        /// Evaluates an add expression
        /// </summary>
        /// <param name="leftValue">
        /// Left hand value
        /// </param>
        /// <param name="rightValue">
        /// Right hand value
        /// </param>
        /// <returns>
        /// Expression Result
        /// </returns>
        public override double Evaluate(double leftValue, double rightValue)
        {
            return leftValue + rightValue;
        }

        /// <summary>
        /// Gets Token for operator
        /// </summary>
        public new string Token { get; } = "+";
    }
}