using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HackerCalculator
{
    public partial class ProgrammerWindow : Window
    {
        private string _fromBaseOperand;
        public ProgrammerWindow()
        {
            DataContext = this;
            InitializeComponent();
            _fromBaseOperand = String.Empty;

            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                int numberPressed = (e.Key >= Key.D0 && e.Key <= Key.D9)
                    ? e.Key - Key.D0
                    : e.Key - Key.NumPad0;

                ComputeAction(Convert.ToString(numberPressed));
                return;
            }


            switch (e.Key)
            {
                case Key.Enter:
                    ComputeAction(ButtonsContents.DictOperators[Operators.Equals]);
                    break;
                case Key.Escape:
                    ComputeAction(ButtonsContents.DictOtherOperations[OtherOperations.C]);
                    break;
                case Key.Multiply:
                    ComputeAction(ButtonsContents.DictOperators[Operators.Multiply]);
                    break;
                case Key.Add:
                    ComputeAction(ButtonsContents.DictOperators[Operators.Addition]);
                    break;
                case Key.Subtract:
                    ComputeAction(ButtonsContents.DictOperators[Operators.Subtract]);
                    break;
                case Key.Divide:
                    ComputeAction(ButtonsContents.DictOperators[Operators.Division]);
                    break;
            }
        }

        private bool ValidateSideBySideOperators(string lastCharacter, string penultimateCharacter)
        {
            string pattern = @"[\+\-*/%]";
            Match match1 = Regex.Match(lastCharacter.ToString(), pattern);
            Match match2 = Regex.Match(penultimateCharacter.ToString(), pattern);
            return match1.Success && match2.Success;
        }

        private bool ValidateSideBySideDecimalSeparator(string lastCharacter, string penultimateCharacter)
        {
            return lastCharacter == "." && penultimateCharacter == ".";
        }

        private bool ValidateDivisionByZero(string lastCharacter, string penultimateCharacter)
        {
            return lastCharacter == "0" && (penultimateCharacter == "/" || penultimateCharacter == "%");
        }

        private bool ValidateNegativeSqrt(string sign)
        {
            return sign == "-";
        }

        private bool ValidateInput(string lastCharacter, string penultimateCharacter, String content)
        {
            if (TextBoxCalculation.Text.Length >= 2)
            {
                if (ValidateSideBySideOperators(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Cannot insert two side by side operators!");
                    return false;
                }

                if (ValidateDivisionByZero(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Division by zero!");
                    return false;
                }

                if (ValidateSideBySideDecimalSeparator(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Division by zero!");
                    return false;
                }

            }
            return true;
        }

        private bool IsBinaryOperator(string buttonContent)
        {
            string pattern = @"[\+\-*/%]";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        private bool IsDigit(String buttonContent)
        {
            String pattern = @"[0-9A-F]";
            Match digitsMatch = Regex.Match(buttonContent, pattern);
            return digitsMatch.Success && buttonContent.Length == 1;
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

        private bool IsDelOption(String buttonContent)
        {
            string pattern = @"(DEL|C)";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        private void ComputeDEL()
        {
            bool erased = false;
            if (_fromBaseOperand != String.Empty)
            {
                _fromBaseOperand = _fromBaseOperand.Remove(_fromBaseOperand.Length - 1);
                erased = true;
            }
            if (erased)
                TextBoxCalculation.Text = TextBoxCalculation.Text.Remove(TextBoxCalculation.Text.Length - 1);
        }

        private void ComputeC()
        {
            TextBoxResult.Text = "0";
            TextBoxCalculation.Text = String.Empty;
            _fromBaseOperand = String.Empty;
        }

        private void ComputeDelOptions(String buttonContent)
        {
            switch (buttonContent)
            {
                case "DEL":
                    ComputeDEL();
                    break;
                case "C(Clear)":
                    ComputeC();
                    break;
            }
        }

        protected void UpdateDisplayWithGrouping()
        {
            string displayText = "";

            if (_fromBaseOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(_fromBaseOperand);
            }

            TextBoxCalculation.Text = displayText;
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
            _fromBaseOperand += buttonContent;
            TextBoxCalculation.Text += buttonContent;

            UpdateDisplayWithGrouping();
        }

        private String ConvertFromOperand()
        {
            var dictBases = (DataContext as ButtonsProgrammerViewModel).DictBases;
            String toBaseString = (DataContext as ButtonsProgrammerViewModel).SelectedToBaseItem;
            String fromBaseString = (DataContext as ButtonsProgrammerViewModel).SelectedFromBaseItem;
            int toBase = dictBases[toBaseString];
            int fromBase = dictBases[fromBaseString];

            int numberInDecimal = Convert.ToInt32(_fromBaseOperand,fromBase);
            string numberInToBase = Convert.ToString(numberInDecimal, toBase).ToUpper();

            return numberInToBase;
        }

        protected void ComputeEquals(String buttonContent)
        {
            if (_fromBaseOperand != String.Empty)
            {
                TextBoxResult.Text = ConvertFromOperand();
            }
        }
        protected void ComputeDecimalSeparator(string separator)
        {
            TextBoxCalculation.Text += separator;
            if (_fromBaseOperand != String.Empty)
                _fromBaseOperand += separator;
        }
        private void ComputeAction(String content)
        {
            if (TextBoxCalculation.Text.Length >= 1)
            {
                if (ValidateInput(content, TextBoxCalculation.Text[TextBoxCalculation.Text.Length - 1].ToString(),
                    content))
                {
                    if (IsDigit(content))
                        ComputeDigit(content);
                    else if (IsDecimalSeparator(content))
                        ComputeDecimalSeparator(content);
                    else if (IsSingularOperator(content))
                        ComputeSingularOperator(content);
                    else if (IsEquals(content))
                        ComputeEquals(content);
                    else if (IsDelOption(content))
                        ComputeDelOptions(content);
                }
            }
            else
            {
                if (IsBinaryOperator(content))
                    MessageBox.Show("An expression cannot start with an operator!");
                else if (IsDigit(content) || IsDecimalSeparator(content))
                    ComputeDigit(content);
            }
        }

        protected void ComputeSingularOperator(String buttonContent)
        {
            double result = 0.0;
            if (_fromBaseOperand != String.Empty)
            {
                result = ComputeSingularOperatorExpression(_fromBaseOperand, buttonContent);
            }

            string finalResult;

            if (Math.Floor(result) == result)
                finalResult = Convert.ToString(Convert.ToInt32(result));
            else
                finalResult = Convert.ToString(result);

            TextBoxCalculation.Text = _fromBaseOperand;
        }
        private bool IsSingularOperator(string buttonContent)
        {
            string pattern = @"\+/-";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }
        protected double ComputeSingularOperatorExpression(String operand, String operation)
        {
            double result = 0.0;
            if(operation=="+/-")
                result = -Convert.ToDouble(operand);
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String buttonContent = (sender as Button).Content.ToString();
            ComputeAction(buttonContent);
        }

        private void ChangeWindow_Click(object sender, RoutedEventArgs e)
        {
            StandardWindow mainWindow = new StandardWindow();
            mainWindow.Show();

            this.Close();
        }
    }
}
