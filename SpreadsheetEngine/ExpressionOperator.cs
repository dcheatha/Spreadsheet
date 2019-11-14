// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionOperator.cs - SpreadsheetEngine
// Created 2019/11/13 at 16:43
// ==================================================

namespace SpreadsheetEngine
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     Class for evaluating operators
    /// </summary>
    internal abstract class ExpressionOperator
    {
        /// <summary>
        ///     Gets the precedence of the operator
        /// </summary>
        public int Precedence { get; } = 0;

        /// <summary>
        ///     Gets Token for operator
        /// </summary>
        public string Token { get; } = null;

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
        public static double Evaluate(double leftValue, double rightValue)
        {
            throw new InvalidOperationException();
        }
    }
}