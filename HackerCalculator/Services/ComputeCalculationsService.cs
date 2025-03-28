using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HackerCalculator.Model;

namespace HackerCalculator.Services
{
    public static class ComupteCalculationsService
    {

        public static void ComputeDEL(Calculation calculation)
        {
            bool erased = false;
            if (calculation.CurrentOperand != String.Empty)
            {
                calculation.CurrentOperand = calculation.CurrentOperand.Remove(calculation.CurrentOperand.Length - 1);
                erased = true;
            }
            else if (calculation.PreviousOperator != String.Empty)
            {
                calculation.PreviousOperator = calculation.PreviousOperator.Remove(calculation.PreviousOperator.Length - 1);
                erased = true;
            }
            else if (calculation.PreviousOperand != String.Empty)
            {
                calculation.PreviousOperand = calculation.PreviousOperand.Remove(calculation.PreviousOperand.Length - 1);
                erased = true;
            }
            if (erased)
                calculation.CompleteCalculation = calculation.CompleteCalculation.Remove(calculation.CompleteCalculation.Length - 1);
        }

        public static void ComputeCE(Calculation calculation,ref String result)
        {
            if (calculation.CurrentOperand != String.Empty)
            {
                calculation.CurrentOperand = String.Empty;
                calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator;
            }
            result = "0";
        }

        public static void ComputeC(Calculation calculation,ref string result)
        {
            result = "0";
            calculation.CompleteCalculation = String.Empty;
            calculation.PreviousOperand = calculation.CurrentOperand = calculation.PreviousOperator = String.Empty;
        }

        public static void UpdateDisplayWithGrouping(Calculation calculation)
        {
            string displayText = String.Empty;

            if (calculation.PreviousOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(calculation.PreviousOperand);
            }

            if (calculation.PreviousOperator != String.Empty)
            {
                displayText += calculation.PreviousOperator;
            }

            if (calculation.CurrentOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(calculation.CurrentOperand);
            }

            calculation.CompleteCalculation = displayText;
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
        public static void ComputeDigit(String buttonContent,Calculation calculation, bool isDigitGroupingChecked)
        {
            if (calculation.PreviousOperand == String.Empty)
            {
                calculation.PreviousOperand = buttonContent;
            }
            else
            {
                if (calculation.PreviousOperator == String.Empty)
                {
                    calculation.PreviousOperand += buttonContent;
                }
                else
                {
                    calculation.CurrentOperand += buttonContent;
                }
            }
            calculation.CompleteCalculation += buttonContent;

            if (isDigitGroupingChecked)
            { 
                UpdateDisplayWithGrouping(calculation);
            }
        }

        public static void ComputeBinaryOperator(String buttonContent,Calculation calculation,ref String result,bool isDigitGroupingChecked)
        {
            if (calculation.PreviousOperator == String.Empty)
            {
                calculation.PreviousOperator = buttonContent;
                calculation.CompleteCalculation += buttonContent;
            }
            else
            {
                double firstNumber = Convert.ToDouble(calculation.PreviousOperand);
                double secondNumber = Convert.ToDouble(calculation.CurrentOperand);
                double resultCalculation = double.NaN;

                switch (calculation.PreviousOperator)
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

                try
                {
                    if (Math.Floor(resultCalculation) != resultCalculation)
                        result = Convert.ToString(resultCalculation);
                    else
                        result = Convert.ToString(Convert.ToInt64(resultCalculation));
                }
                catch (OverflowException e)
                {
                    calculation.PreviousOperand = calculation.PreviousOperator = calculation.CurrentOperand = calculation.CompleteCalculation =
                         result = String.Empty;
                    MessageBox.Show("Given calculation resulted into too big of a number!");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error:");
                    return;
                }

                calculation.PreviousOperand = Convert.ToString(result);
                if (isDigitGroupingChecked)
                { 
                    calculation.PreviousOperand = FormatNumberWithGrouping(calculation.PreviousOperand);
                    result = FormatNumberWithGrouping(result);
                }

                calculation.CurrentOperand = String.Empty;
                calculation.PreviousOperator = buttonContent;
                
                calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator;
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

        public static void ComputeSingularOperator(String buttonContent,Calculation calculation)
        {
            double result = 0.0;
            if (calculation.CurrentOperand == String.Empty)
            {
                if (calculation.PreviousOperand != String.Empty)
                {
                    result = ComputeSingularOperatorExpression(calculation.PreviousOperand, buttonContent);
                }
            }
            else
            {
                result = ComputeSingularOperatorExpression(calculation.CurrentOperand, buttonContent);
            }

            string finalResult;

            if (Math.Floor(result) == result)
                finalResult = Convert.ToString(Convert.ToInt32(result));
            else
                finalResult = Convert.ToString(result);

            if (calculation.CurrentOperand != String.Empty)
            {
                calculation.CurrentOperand = finalResult;
                calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator + finalResult;
            }
            else if (calculation.PreviousOperand != String.Empty)
            {
                calculation.PreviousOperand = finalResult;
                calculation.CompleteCalculation = calculation.PreviousOperand;
            }
        }
        public static void ComputeEquals(String buttonContent,Calculation calculation,ref string result,bool isDigitGroupingChecked)
        {
            if (calculation.PreviousOperator == String.Empty)
            {
                result = calculation.PreviousOperand;
            }
            else
            {
                ComputeBinaryOperator(buttonContent, calculation,ref result,isDigitGroupingChecked);
                calculation.PreviousOperator = String.Empty;
                if(calculation.CompleteCalculation.Length >=1)
                    calculation.CompleteCalculation = calculation.CompleteCalculation.Substring(0, calculation.CompleteCalculation.Length - 1);
            }

            if (isDigitGroupingChecked)
            { 
                UpdateDisplayWithGrouping(calculation);
                result = FormatNumberWithGrouping(result);
            }
        }
        public static void ComputeDecimalSeparator(string separator,Calculation calculation)
        {
            calculation.CompleteCalculation += separator;
            if (calculation.CurrentOperand == String.Empty)
                calculation.PreviousOperand += separator;
            else
                calculation.CurrentOperand += separator;
        }
    }
}
