// ==================================================
// D. Cheatham (SID: xxxxxxxx)
// Program.cs - ExpressionTreeConsoleApp
// Created 2019/11/13 at 15:00
// ==================================================

namespace ExpressionTreeConsoleApp
{
    #region

    using System;

    using SpreadsheetEngine;

    #endregion

    /// <summary>
    ///     Console app for demoing the ExpressionTree
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Menu strings
        /// </summary>
        private static readonly string[] MenuStrings =
        {
            "Enter a new expression", "Set a variable value", "Evaluate tree", "Quit"
        };

        /// <summary>
        ///     Current expression tree
        /// </summary>
        private static ExpressionTree tree;

        /// <summary>
        ///     Draws the menu
        /// </summary>
        private static void DrawMenu()
        {
            Console.Write("Menu (");
            Console.ForegroundColor = ConsoleColor.Blue;
            if (tree == null)
            {
                Console.Write("No expression defined");
            }
            else
            {
                Console.Write("{0}", tree);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(")\n");

            for (var pos = 0; pos < MenuStrings.Length; pos++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" {0}", pos + 1);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" = {0}", MenuStrings[pos]);
            }
        }

        /// <summary>
        ///     Entry point for console app
        /// </summary>
        private static void Main()
        {
            ExpressionTree.AddDefaultOperators();
            var selection = int.MinValue;

            while (selection != 4)
            {
                DrawMenu();
                selection = int.MinValue;

                while (selection < 1 || selection > 4)
                {
                    Console.Write("Please make a selection: ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    var input = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;

                    int.TryParse(input, out selection);
                }

                switch (selection)
                {
                    case 1:
                    {
                        Console.Write("Please enter an expression: ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        var expression = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        tree                    = new ExpressionTree(expression);
                        break;
                    }
                    case 2 when tree != null:
                    {
                        Console.Write("Please enter a variable name: ");

                        Console.ForegroundColor = ConsoleColor.Blue;
                        var name = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;

                        var value = double.MinValue;

                        while (Math.Abs(value - double.MinValue) < double.Epsilon)
                        {
                            Console.Write("Please enter the variable value: ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            var stringValue = Console.ReadLine();
                            double.TryParse(stringValue, out value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        tree.SetVariable(name, value);
                        break;
                    }
                    case 3 when tree != null && tree.HasVariables():
                    {
                        Console.Write("Tree value: ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(tree.Evaluate());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    case 3 when tree != null && !tree.HasVariables():
                    {
                        Console.WriteLine("Not all variables are defined");
                        break;
                    }
                }

                Console.WriteLine();
            }
        }
    }
}