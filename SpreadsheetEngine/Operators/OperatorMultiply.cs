﻿#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// OperatorMultiply.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

#endregion

#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// OperatorMultiply.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

#endregion

#region a

// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// OperatorMultiply.cs - SpreadsheetEngine
// Created 2019/11/18 at 23:14
// ==================================================

#endregion

namespace SpreadsheetEngine.Operators
{
    /// <summary>
    ///     Adding operator
    /// </summary>
    internal class OperatorMultiply : ExpressionOperator
    {
        /// <summary>
        ///     Gets the precedence of the operator
        /// </summary>
        public override int Precedence { get; } = 2;

        /// <summary>
        ///     Gets TokenType for operator
        /// </summary>
        public override string Token { get; } = "*";

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
            return rightValue * leftValue;
        }
    }
}