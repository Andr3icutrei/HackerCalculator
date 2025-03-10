using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace HackerCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StandardWindow 
    {
        private ObservableCollection<String> _memory;

        public string _previousOperator { get; set; }
        public string _previousOperand { get; set; }
        public string _currentOperand { get; set; }
        public string _currentOperator { get; set; }

        public StandardWindow()
        {
            InitializeComponent();
            _previousOperand = String.Empty;
            _currentOperand = String.Empty;
            _currentOperator = String.Empty;
            _previousOperator = String.Empty;

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

            if(e.Key == Key.D5 && ModifierKeys.Shift == e.KeyboardDevice.Modifiers)
            {
                ComputeAction(ButtonsContents.DictOperators[Operators.Modulo]);
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

        private bool ValidateSideBySideOperators(string lastCharacter,string penultimateCharacter)
        {
            string pattern = @"[\+\-*/%]";
            Match match1 = Regex.Match(lastCharacter.ToString(), pattern);
            Match match2 = Regex.Match(penultimateCharacter.ToString(), pattern); 
            return match1.Success && match2.Success;
        }

        private bool ValidateSideBySideDecimalSeparator(string lastCharacter,string penultimateCharacter)
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

        private bool ValidateInput(string lastCharacter,string penultimateCharacter ,String content)
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

                if (content == ButtonsContents.DictOperators[Operators.Sqrt])
                    for (int i = TextBoxCalculation.Text.Length - 1; i >= 0; --i)
                    {
                        if (TextBoxCalculation.Text[i] == '+' || TextBoxCalculation.Text[i] == '-')
                            if (ValidateNegativeSqrt(TextBoxCalculation.Text[i].ToString()))
                            {
                                MessageBox.Show("Negative squared root!");
                                return false;
                            }
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

        private bool IsSingularOperator(string buttonContent)
        {
            string pattern = @"(1\/x|x\^2|sqrt\(x\)|\+/-)";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        private bool IsDigit(String buttonContent)
        {
            String pattern = @"[0-9]";
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
            string pattern = @"(DEL|CE|C)";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        private void ComputeDEL()
        {
            bool erased = false;
            if (_currentOperand != String.Empty)
            {
                _currentOperand = _currentOperand.Remove(_currentOperand.Length - 1);
                erased = true;
            }
            else if (_previousOperator != String.Empty)
            {
                _previousOperator = _previousOperator.Remove(_previousOperator.Length - 1);
                erased = true;
            }
            else if (_previousOperand != String.Empty)
            { 
                _previousOperand = _previousOperand.Remove(_previousOperand.Length - 1); 
                erased = true;
            }
            if (erased)
                TextBoxCalculation.Text = TextBoxCalculation.Text.Remove(TextBoxCalculation.Text.Length - 1);
        }

        private void ComputeCE()
        {
            if (_currentOperand != String.Empty)
            { 
                _currentOperand = String.Empty;
                TextBoxCalculation.Text = _previousOperand + _previousOperator;
            }
            TextBoxResult.Text = "0";
        }

        private void ComputeC()
        {
            TextBoxResult.Text = "0";
            TextBoxCalculation.Text = String.Empty;
            _previousOperand = _currentOperand = _previousOperator = _currentOperator=String.Empty;
        }

        private void ComputeDelOptions(String buttonContent)
        {
            switch(buttonContent)
            {
                case "DEL":
                    ComputeDEL();
                    break;
                case "CE":
                    ComputeCE();
                    break;
                case "C":
                    ComputeC();
                    break;
            }
        }

        private void ButtonMC_Click(object sender, EventArgs e)
        {
            var results = (DataContext as ButtonsStandardViewModel).MemoryResults;
            if (results.Count != 0)
                results.Clear();
        }

        private void ButtonMR_Click(object sender, EventArgs e)
        {
            var results = (DataContext as ButtonsStandardViewModel).MemoryResults;
            if (_currentOperand == String.Empty)
            {
                _currentOperand = results[0];
                TextBoxCalculation.Text = _previousOperand + _previousOperator + _currentOperand;
            }
            else if (_previousOperand == String.Empty)
            { 
                _previousOperand = results[0];
                TextBoxCalculation.Text = _previousOperand;
            }
        }

        private void ButtonMAdd_Click(object sender, EventArgs e)
        {
            var results = (DataContext as ButtonsStandardViewModel).MemoryResults;
            if (results.Count != 0 && TextBoxResult.Text!=String.Empty && TextBoxResult.Text!="Result:")
            {
                double calculation =Convert.ToDouble(results[0]) + Convert.ToDouble(TextBoxResult.Text);
                if (calculation == Math.Floor(calculation))
                    results[0] = Convert.ToString(Convert.ToInt32(calculation));
                else
                    results[0] = Convert.ToString(calculation);
            }
        }

        private void ButtonMSubstract_Click(object sender, EventArgs e)
        {
            var results = (DataContext as ButtonsStandardViewModel).MemoryResults;
            if (results.Count != 0 && TextBoxResult.Text != String.Empty && TextBoxResult.Text != "Result:")
            {
                double calculation = Convert.ToDouble(results[0]) - Convert.ToDouble(TextBoxResult.Text);
                if (calculation == Math.Floor(calculation))
                    results[0] = Convert.ToString(Convert.ToInt32(calculation));
                else
                    results[0] = Convert.ToString(calculation);
            }
        }

        private void ButtonMS_Click(object sender, EventArgs e)
        {
            if (TextBoxResult.Text != String.Empty && TextBoxResult.Text != "Result:")
                (DataContext as ButtonsStandardViewModel).MemoryResults.Insert(0, TextBoxResult.Text);
        }
        protected void UpdateDisplayWithGrouping()
        {
            string displayText = "";

            if (_previousOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(_previousOperand);
            }

            if (_previousOperator != String.Empty)
            {
                displayText += _previousOperator;
            }

            if (_currentOperand != String.Empty)
            {
                displayText += FormatNumberWithGrouping(_currentOperand);
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
            if (_previousOperand == String.Empty)
            {
                _previousOperand = buttonContent;
            }
            else
            {
                if (_previousOperator == String.Empty)
                {
                    _previousOperand += buttonContent;
                }
                else
                {
                    _currentOperand += buttonContent;
                }
            }
            TextBoxCalculation.Text += buttonContent;

            UpdateDisplayWithGrouping();
        }

        protected void ComputeBinaryOperator(String buttonContent)
        {
            if (_previousOperator == String.Empty)
            {
                _previousOperator = buttonContent;
                TextBoxCalculation.Text += buttonContent;
            }
            else
            {
                double firstNumber = Convert.ToDouble(_previousOperand);
                double secondNumber = Convert.ToDouble(_currentOperand);
                double result = double.NaN;

                switch (_previousOperator)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "*":
                        result = firstNumber * secondNumber;
                        break;
                    case "/":
                        result = firstNumber / secondNumber;
                        break;
                    case "%":
                        result = firstNumber % secondNumber;
                        break;
                    default:
                        MessageBox.Show("error computing");
                        break;
                }
                if (Math.Floor(result) != result)
                    TextBoxResult.Text = Convert.ToString(result);
                else
                    TextBoxResult.Text = Convert.ToString(Convert.ToInt32(result));
                _previousOperand = Convert.ToString(result);
                _currentOperand = String.Empty;
                _previousOperator = buttonContent;
                _currentOperator = String.Empty;
                TextBoxCalculation.Text = _previousOperand + _previousOperator;
            }
        }

        protected double ComputeSingularOperatorExpression(String operand, String operation)
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
        protected void ComputeSingularOperator(String buttonContent)
        {
            double result = 0.0;
            if (_currentOperand == String.Empty)
            {
                if (_previousOperand != String.Empty)
                {
                    result = ComputeSingularOperatorExpression(_previousOperand, buttonContent);
                }
            }
            else
            {
                result = ComputeSingularOperatorExpression(_currentOperand, buttonContent);
            }

            string finalResult;

            if (Math.Floor(result) == result)
                finalResult = Convert.ToString(Convert.ToInt32(result));
            else
                finalResult = Convert.ToString(result);

            if (_currentOperand != String.Empty)
            {
                _currentOperand = finalResult;
                TextBoxCalculation.Text = _previousOperand + _previousOperator + finalResult;
            }
            else if (_previousOperand != String.Empty)
            {
                _previousOperand = finalResult;
                TextBoxCalculation.Text = _previousOperand;
            }
        }
        protected void ComputeEquals(String buttonContent)
        {
            if (_previousOperator == String.Empty)
            {
                TextBoxResult.Text = _previousOperand;
            }
            else
            {
                ComputeBinaryOperator(buttonContent);
                _previousOperator = String.Empty;
                TextBoxCalculation.Text = TextBoxCalculation.Text.Substring(0, TextBoxCalculation.Text.Length - 1);
            }
        }
        protected void ComputeDecimalSeparator(string separator)
        {
            TextBoxCalculation.Text += separator;
            if (_currentOperand == String.Empty)
                _previousOperand += separator;
            else
                _currentOperand += separator;
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
                    else if (IsBinaryOperator(content))
                        ComputeBinaryOperator(content);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String buttonContent = (sender as Button).Content.ToString();
            ComputeAction(buttonContent);
            //MessageBox.Show(_previousOperand + '\n' + _currentOperand + '\n' + _previousOperator + '\n' + _currentOperator);
        }

        private void ChangeWindow_Click(object sender, RoutedEventArgs e)
        {
            ProgrammerWindow mainWindow = new ProgrammerWindow();
            mainWindow.Show();

            this.Close();
        }
    }
}