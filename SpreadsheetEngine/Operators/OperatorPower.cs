#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// OperatorPower.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

#endregion

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
        public override int Precedence { get; } = 3;

        /// <summary>
        ///     Gets TokenType for operator
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
            return Math.Pow(rightValue, leftValue);
        }
    }
}