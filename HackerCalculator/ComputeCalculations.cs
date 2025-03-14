using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HackerCalculator
{
    public static class ComputeCalculations
    {

        public static void ComputeDEL(ref String previousOperand,ref String previousOperator,ref String currentOperand,
            ref String calculation)
        {
            bool erased = false;
            if (currentOperand != String.Empty)
            {
                currentOperand = currentOperand.Remove(currentOperand.Length - 1);
                erased = true;
            }
            else if (previousOperator != String.Empty)
            {
                previousOperator = previousOperator.Remove(previousOperator.Length - 1);
                erased = true;
            }
            else if (previousOperand != String.Empty)
            {
                previousOperand = previousOperand.Remove(previousOperand.Length - 1);
                erased = true;
            }
            if (erased)
                calculation = calculation.Remove(calculation.Length - 1);
        }

        public static void ComputeCE( String previousOperand, String previousOperator,ref String currentOperand,
            ref String calculation, ref String result)
        {
            if (currentOperand != String.Empty)
            {
                currentOperand = String.Empty;
                calculation = previousOperand + previousOperator;
            }
            result = "0";
        }

        public static void ComputeC(ref String previousOperand, ref String previousOperator,ref String currentOperand,
            ref String calculation, ref String result)
        {
            result = "0";
            calculation = String.Empty;
            previousOperand = currentOperand = previousOperator = String.Empty;
        }

        public static void UpdateDisplayWithGrouping(String previousOperand, String previousOperator, String currentOperand,
            ref String calculation)
        {
            string displayText = String.Empty;

            if (previousOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(previousOperand);
            }

            if (previousOperator != String.Empty)
            {
                displayText += previousOperator;
            }

            if (currentOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(currentOperand);
            }

            calculation = displayText;
        }

        private static string FormatNumberWithGrouping(string numberStr)
        {
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            bool hasDecimalPoint = numberStr.Contains(decimalSeparator);

            string integerPart;
            string decimalPart = "";

            if (hasDecimalPoint)
            {
                string[] parts = numberStr.Split(new[] { decimalSeparator }, StringSplitOptions.None);
                integerPart = parts[0];
                if (parts.Length > 1)
                {
                    decimalPart = decimalSeparator + parts[1];
                }
            }
            else
            {
                integerPart = numberStr;
            }

            if (double.TryParse(integerPart, NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture, out double value))
            {
                string groupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                string formattedInteger = value.ToString("#,0", CultureInfo.CurrentCulture);

                return formattedInteger + decimalPart;
            }

            return numberStr;
        }
        public static void ComputeDigit(String buttonContent,ref String previousOperand,ref String previousOperator,ref String currentOperand,
            ref String calculation, bool isDigitGroupingChecked)
        {
            if (previousOperand == String.Empty)
            {
                previousOperand = buttonContent;
            }
            else
            {
                if (previousOperator == String.Empty)
                {
                    previousOperand += buttonContent;
                }
                else
                {
                    currentOperand += buttonContent;
                }
            }
            calculation += buttonContent;

            if (isDigitGroupingChecked == true)
                UpdateDisplayWithGrouping(previousOperand,previousOperator,currentOperand,ref calculation);
        }

        public static void ComputeBinaryOperator(String buttonContent,ref String previousOperand, ref String previousOperator,
            ref String currentOperand,ref String calculation,ref String result)
        {
            if (previousOperator == String.Empty)
            {
                previousOperator = buttonContent;
                calculation += buttonContent;
            }
            else
            {
                double firstNumber = Convert.ToDouble(previousOperand);
                double secondNumber = Convert.ToDouble(currentOperand);
                double resultCalculation = double.NaN;

                switch (previousOperator)
                {
                    case "+":
                        resultCalculation = firstNumber + secondNumber;
                        break;
                    case "-":
                        resultCalculation = firstNumber - secondNumber;
                        break;
                    case "*":
                        resultCalculation = firstNumber * secondNumber;
                        break;
                    case "/":
                        resultCalculation = firstNumber / secondNumber;
                        break;
                    case "%":
                        resultCalculation = firstNumber % secondNumber;
                        break;
                    default:
                        MessageBox.Show("error computing");
                        break;
                }
                if (Math.Floor(resultCalculation) != resultCalculation)
                    result = Convert.ToString(resultCalculation);
                else
                    result = Convert.ToString(Convert.ToInt32(resultCalculation));
                previousOperand = Convert.ToString(result);
                currentOperand = String.Empty;
                previousOperator = buttonContent;
                calculation = previousOperand + previousOperator;
            }
        }

        public static double ComputeSingularOperatorExpression(String operand, String operation)
        {
            double result = 0.0;
            switch (operation)
            {
                case "sqrt(x)":
                    result = Math.Sqrt(Convert.ToDouble(operand));
                    break;
                case "x^2":
                    result = Convert.ToDouble(operand) * Convert.ToDouble(operand);
                    break;
                case "1/x":
                    result = 1 / Convert.ToDouble(operand);
                    break;
                case "+/-":
                    result = -Convert.ToDouble(operand);
                    break;
            }
            return result;
        }
        public static void ComputeSingularOperator(String buttonContent,ref String currentOperand,ref String previousOperand,
            String previousOperator,ref String calculation)
        {
            double result = 0.0;
            if (currentOperand == String.Empty)
            {
                if (previousOperand != String.Empty)
                {
                    result = ComputeSingularOperatorExpression(previousOperand, buttonContent);
                }
            }
            else
            {
                result = ComputeSingularOperatorExpression(currentOperand, buttonContent);
            }

            string finalResult;

            if (Math.Floor(result) == result)
                finalResult = Convert.ToString(Convert.ToInt32(result));
            else
                finalResult = Convert.ToString(result);

            if (currentOperand != String.Empty)
            {
                currentOperand = finalResult;
                calculation = previousOperand + previousOperator + finalResult;
            }
            else if (previousOperand != String.Empty)
            {
                previousOperand = finalResult;
                calculation = previousOperand;
            }
        }
        public static void ComputeEquals(String buttonContent,ref String previousOperand,ref String previousOperator,ref String currentOperand,
            ref String result,ref String calculation,bool isDigitGroupingChecked)
        {
            if (previousOperator == String.Empty)
            {
                result = previousOperand;
            }
            else
            {
                ComputeBinaryOperator(buttonContent,ref previousOperand,ref previousOperator,ref currentOperand,ref calculation,ref result);
                previousOperator = String.Empty;
                calculation = calculation.Substring(0, calculation.Length - 1);
            }

            if (isDigitGroupingChecked)
                UpdateDisplayWithGrouping(previousOperand, previousOperator, currentOperand,ref calculation);
        }
        public static void ComputeDecimalSeparator(string separator,ref String previousOperand,ref String currentOperand,ref String calculation)
        {
            calculation += separator;
            if (currentOperand == String.Empty)
                previousOperand += separator;
            else
                currentOperand += separator;
        }

        
    }
}
