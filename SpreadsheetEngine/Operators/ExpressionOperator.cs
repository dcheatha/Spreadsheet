// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// ExpressionOperator.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

namespace SpreadsheetEngine.Operators
{
    #region

    #endregion

    /// <summary>
    ///     Class for evaluating operators
    /// </summary>
    public abstract class ExpressionOperator
    {
        /// <summary>
        ///     Gets the precedence of the operator
        /// </summary>
        public abstract int Precedence { get; }

        /// <summary>
        ///     Gets TokenType for operator
        /// </summary>
        public abstract string Token { get; }

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