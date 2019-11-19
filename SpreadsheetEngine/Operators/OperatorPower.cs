// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// OperatorPower.cs - SpreadsheetEngine
// Created 2019/11/13 at 17:13
// ==================================================

namespace SpreadsheetEngine.Operators
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     Power operator
    /// </summary>
    internal class OperatorPower : ExpressionOperator
    {
        /// <summary>
        ///     Gets the precedence of the operator
        /// </summary>
        public new int Precedence { get; } = 0;

        /// <summary>
        ///     Gets Token for operator
        /// </summary>
        public override string Token { get; } = "^";

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
        public override double Evaluate(double leftValue, double rightValue)
        {
            return Math.Pow(leftValue, rightValue);
        }
    }
}