using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation.Web.Api
{
    public static class MathEvaluator
    {
        public static double Parse(string expression)
        {
            if (double.TryParse(expression, out double d))
            {
                //The expression is a decimal number, so we are just returning it
                return d;
            }
            else
            {
                return CalculateValue(expression);
            }
        }

        /// <summary>
        /// Determines if an expression contains only valid characters
        /// </summary>
        /// <param name="s">The expression to check</param>
        /// <returns>Indicates whether only valid characters were used in the expression</returns>
        public static bool IsExpression(string s)
        {
            //Determines whether the string contains illegal characters
            return Regex.IsMatch(s, @"^(\d+[-+/*])+\d+|\d+(\.\d+[-+/*])\d+$");
        }

        /// <summary>
        /// Splits an expression into elements
        /// </summary>
        /// <param name="expression">Mathematical expression</param>
        /// <param name="operators">Operators used as delimiters</param>
        /// <returns>The list of elements</returns>
        private static List<string> TokenizeExpression(string expression, Dictionary<char, int> operators)
        {
            List<string> elements = new List<string>();
            string currentElement = string.Empty;

            for (int i = 0; i < expression.Length; i++)
            {
                if (operators.Keys.Contains(expression[i]))
                {
                    //The current character is an operator
                    elements.Add(currentElement);
                    elements.Add(expression[i].ToString());
                    currentElement = string.Empty;
                }
                else if (expression[i] != ' ')
                {
                    //The current character is neither an operator nor a space
                    currentElement += expression[i];
                }
                     
            }

            //Add the last element (which follows the last operation) to the list
            if (currentElement.Length > 0)
            {
                elements.Add(currentElement);
            }

            return elements;
        }

        /// <summary>
        /// Calculates the value of an expression
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <returns>The value of the expression</returns>
        private static double CalculateValue(string expression)
        {
            //Dictionary to store the supported operations
            //Key: Operation; Value: Precedence (higher number indicates higher precedence)
            Dictionary<char, int> operators = new Dictionary<char, int>
            {
                {'+', 1}, {'-', 1}, {'*', 2}, {'/', 2}
            };

            //Tokenize the expression
            List<string> elements = TokenizeExpression(expression, operators);

            //define a value which will be used as the return value of the function
            double value = 0;

            //loop from the highest precedence to the lowest
            for (int i = operators.Values.Max(); i >= operators.Values.Min(); i--)
            {

                //loop while there are any operators left in the list from the current precedence level
                while (elements.Count >= 3
                    && elements.Any(element => element.Length == 1 &&
                        operators.Where(op => op.Value == i)
                        .Select(op => op.Key).Contains(element[0])))
                {
                    //get the position of this element
                    int pos = elements
                        .FindIndex(element => element.Length == 1 &&
                        operators.Where(op => op.Value == i)
                        .Select(op => op.Key).Contains(element[0]));

                    //evaluate it's value
                    value = EvaluateOperation(elements[pos], elements[pos - 1], elements[pos + 1]);

                    //change the first operand of the operation to the calculated value of the operation
                    elements[pos - 1] = value.ToString();

                    //remove the operator and the second operand from the list
                    elements.RemoveRange(pos, 2);
                }
            }

            return value;
        }

        /// <summary>
        /// Performs an operation on the operands
        /// </summary>
        /// <param name="oper">Operator</param>
        /// <param name="operand1">Left operand</param>
        /// <param name="operand2">Right operand</param>
        /// <returns>Value of the operation</returns>
        private static double EvaluateOperation(string oper, string operand1, string operand2)
        {
            if (oper.Length == 1)
            {
                double op1 = Parse(operand1);
                double op2 = Parse(operand2);

                double value;
                switch (oper[0])
                {
                    case '+':
                        value = op1 + op2;
                        break;
                    case '-':
                        value = op1 - op2;
                        break;
                    case '*':
                        value = op1 * op2;
                        break;
                    case '/':
                        value = op1 / op2;
                        break;
                    default:
                        throw new ArgumentException("Unsupported operator");
                }
                return value;
            }
            else
            {
                throw new ArgumentException("Unsupported operator");
            }
        }
    }
}