using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using HackerCalculator.Model;
using HackerCalculator.Services;

namespace HackerCalculator.ViewModel.Programmer
{
    public class CalculationsViewModel : INotifyPropertyChanged
    {
        public Calculation calculation { get; set; }
        private String result;


        public event PropertyChangedEventHandler? PropertyChanged;

        public String Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CalculationsViewModel(Calculation c,String r)
        {
            calculation = new Calculation(c);
            Result = r;
        }

        public CalculationsViewModel()
        {
            calculation = new Calculation();
            Result = String.Empty;
        }

       

        private bool IsDecimalSeparator(String buttonContent)
        {
            return buttonContent == ButtonsContents.DictDigits[Digits.DecimalSeparator];
        }

        private bool IsEquals(String buttonContent)
        {
            var equals = ButtonsContents.DictOperators[Operators.Equals];
            return equals == buttonContent;
        }
        
        private void ComputeDEL()
        {
            ComupteCalculationsService.ComputeDEL(calculation);
            
        }

        private void ComputeCE()
        {
            ComupteCalculationsService.ComputeCE(calculation,ref result);
        }

        private void ComputeDelOptions(String buttonContent)
        {
            switch (buttonContent)
            {
                case "DEL":
                    ComputeDEL();
                    break;
                case "CE":
                    ComputeCE();
                    break;
            }
            OnPropertyChanged(nameof(Result));
        }

        protected string FormatNumberWithGrouping(string numberStr)
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

        protected void ComputeDigit(String buttonContent)
        {
            ComupteCalculationsService.ComputeDigit(buttonContent, calculation, false);
        }

        protected void ComputeEquals(String buttonContent,int fromBase,int toBase)
        {
            try
            { 
                string decimalPrevOperand = !string.IsNullOrEmpty(calculation.PreviousOperand) ?
                    ToBase10(calculation.PreviousOperand, fromBase) : string.Empty;

                string decimalCurrOperand = !string.IsNullOrEmpty(calculation.CurrentOperand) ?
                    ToBase10(calculation.CurrentOperand, fromBase) : string.Empty;

                string originalPrevOperand = calculation.PreviousOperand;

                string decimalResult = result;
                string decimalCalculation = calculation.CompleteCalculation;

                string tempPrevOperand = decimalPrevOperand;
                string tempOperator = calculation.PreviousOperator;
                string tempCurrOperand = decimalCurrOperand;

                Calculation tempCalculation = new Calculation(tempPrevOperand, tempOperator, tempCurrOperand,decimalCalculation);

                ComupteCalculationsService.ComputeEquals(buttonContent, tempCalculation, ref decimalResult, false);

                calculation.PreviousOperator = tempCalculation.PreviousOperator;

                string displayResult = FromBase10(decimalResult, toBase);

                result = displayResult;
                OnPropertyChanged(nameof(Result));

                if (!string.IsNullOrEmpty(calculation.CurrentOperand))
                {
                    string displayCalculation = FromBase10(tempCalculation.CompleteCalculation, fromBase);
                    calculation.CompleteCalculation = displayCalculation;
                    calculation.PreviousOperand = displayCalculation;
                    calculation.CurrentOperand = String.Empty;
                }
                else if (!string.IsNullOrEmpty(tempCalculation.PreviousOperand))
                {
                    calculation.CompleteCalculation = originalPrevOperand;
                    calculation.PreviousOperand = originalPrevOperand;
                }
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.Message;
            }
        }

        private string ToBase10(string operand, int fromBase)
        {
            if (string.IsNullOrEmpty(operand))
                return "0";

            bool isNegative = operand.StartsWith("-");
            if (isNegative)
                operand = operand.Substring(1);

            long result = 0;
            foreach (char c in operand)
            {
                int digit;
                if (char.IsDigit(c))
                    digit = c - '0';
                else
                    digit = char.ToUpper(c) - 'A' + 10;

                if (digit >= fromBase)
                    throw new ArgumentException($"Invalid digit '{c}' for base {fromBase}");

                result = result * fromBase + digit;
            }

            return (isNegative ? "-" : "") + result.ToString();
        }

        private string FromBase10(string decimalStr, int toBase)
        {
            if (!long.TryParse(decimalStr, out long number))
                return "0";

            if (number == 0)
                return "0";

            bool isNegative = number < 0;
            if (isNegative)
                number = -number;

            string result = "";
            while (number > 0)
            {
                int remainder = (int)(number % toBase);
                char digit = remainder < 10 ?
                    (char)(remainder + '0') :
                    (char)(remainder - 10 + 'A');
                result = digit + result;
                number /= toBase;
            }

            return isNegative ? "-" + result : result;
        }
        protected void ComputeDecimalSeparator(string separator)
        {
            ComupteCalculationsService.ComputeDecimalSeparator(separator, calculation);
        }

        private void ComputeBinaryOperator(String content,int fromBase,int toBase)
        {
            if (calculation.CurrentOperand != String.Empty)
            {
                ComputeEquals("=",fromBase,toBase);
                calculation.CompleteCalculation += content;
                calculation.PreviousOperator = content;
            }
            else
            {
                ComupteCalculationsService.ComputeBinaryOperator(content, calculation, ref result, false);
                OnPropertyChanged(nameof(Result));
            }

        }
        protected void ComputeSingularOperator(String buttonContent)
        {
            ComupteCalculationsService.ComputeSingularOperator(buttonContent, calculation);
        }

        public void ComputeAction(String content,int fromBase,int toBase)
        {
            if (calculation.CompleteCalculation.Length >= 1)
            {
                if (ValidationService.ValidateInput(calculation,content, calculation.CompleteCalculation[calculation.CompleteCalculation.Length - 1].ToString(),
                    content))
                {
                    if (IdentifierService.IsDigit(content))
                        ComputeDigit(content);
                    else if (IsDecimalSeparator(content))
                        ComputeDecimalSeparator(content);
                    else if (IdentifierService.IsBinaryOperator(content))
                        ComputeBinaryOperator(content,fromBase,toBase);
                    else if (IdentifierService.IsSingularOperator(content))
                        ComputeSingularOperator(content);
                    else if (IsEquals(content))
                        ComputeEquals(content,fromBase,toBase);
                    else if (IdentifierService.IsDelOption(content))
                        ComputeDelOptions(content);
                }
            }
            else
            {
                if (IdentifierService.IsDigit(content))
                    ComputeDigit(content);
            }
        }
    }
}
