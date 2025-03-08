using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
    public partial class MainWindow : Window
    {
        private String _previousOperand;
        private String _currentOperand;
        private String _currentOperator;
        private String _previousOperator;
        private int _decimalPower;
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;
            _previousOperand = String.Empty;
            _currentOperand = String.Empty;
            _currentOperator = String.Empty;
            _previousOperator = String.Empty;
            _decimalPower = 0;
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

        private bool ValidateInput(string lastCharacter,string penultimateCharacter ,object sender)
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

                if ((sender as Button).Content == (DataContext as ButtonsViewModel).Controls.DictOperators[Operators.Sqrt])
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

        private double ExecuteOperation()
        {
            return 0.0;
        }

        private bool IsDigit(String buttonContent)
        {
            String pattern = @"[0-9]";
            Match digitsMatch = Regex.Match(buttonContent, pattern);
            return digitsMatch.Success && buttonContent.Length==1;
        }

        private void ComputeDigit(String buttonContent)
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
        }

        private bool IsDecimalSeparator(String buttonContent)
        {
            return buttonContent == (DataContext as ButtonsViewModel).Controls.DictDigits[Digits.DecimalSeparator];
        }

        private void ComputeBinaryOperator(String buttonContent)
        {
            if(_previousOperator == String.Empty)
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
                _currentOperand = String.Empty ;
                _previousOperator = buttonContent;
                _currentOperator = String.Empty;
                TextBoxCalculation.Text = _previousOperand + _previousOperator;
            }
        }

        private double ComputeSingularOperatorExpression(String operand,String operation)
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
        private void ComputeSingularOperator(String buttonContent)
        {
            double result=0.0;
            if(_currentOperand == String.Empty)
            {
                if(_previousOperand !=String.Empty)
                {
                    result = ComputeSingularOperatorExpression(_previousOperand, buttonContent);
                }
            }
            else
            {
                result = ComputeSingularOperatorExpression(_currentOperand, buttonContent);
            }

            if (Math.Floor(result) == result)
                TextBoxResult.Text = Convert.ToString(Convert.ToInt32(result));
            else
                TextBoxResult.Text = Convert.ToString(result);

            _currentOperand = result.ToString();
        }   

        private bool IsEquals(String buttonContent)
        {
            var equals = (DataContext as ButtonsViewModel).Controls.DictOperators[Operators.Equals]; 
            return equals == buttonContent;
        }

        private void ComputeEquals(String buttonContent)
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
        private void ComputeDecimalSeparator(string separator)
        {
            _decimalPower = 1;
            TextBoxCalculation.Text += separator;
            if (_currentOperand == String.Empty)
                _previousOperand += separator;
            else
                _currentOperand += separator;
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

        private void ComputeDelOptions(String buttonContent)
        {
            switch(buttonContent)
            {
                case "DEL":
                    ComputeDEL();
                    break;
                case "CE":
                    break;
                case "C":
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String buttonContent = (sender as Button).Content.ToString();
            if (TextBoxCalculation.Text.Length >= 1)
            {
                if (ValidateInput(buttonContent, TextBoxCalculation.Text[TextBoxCalculation.Text.Length - 1].ToString(),
                    sender))
                {
                    if (IsDigit(buttonContent))
                        ComputeDigit(buttonContent);
                    else if (IsDecimalSeparator(buttonContent))
                        ComputeDecimalSeparator(buttonContent);
                    else if (IsSingularOperator(buttonContent))
                        ComputeSingularOperator(buttonContent);
                    else if (IsBinaryOperator(buttonContent))
                        ComputeBinaryOperator(buttonContent);
                    else if (IsEquals(buttonContent))
                        ComputeEquals(buttonContent);
                    else if(IsDelOption(buttonContent))
                        ComputeDelOptions(buttonContent);
                }
            }
            else
            {
                if (IsBinaryOperator(buttonContent))
                    MessageBox.Show("An expression cannot start with an operator!");
                else if (IsDigit(buttonContent) || IsDecimalSeparator(buttonContent))
                    ComputeDigit(buttonContent);
            }
            //MessageBox.Show(_previousOperand + '\n' + _currentOperand + '\n' + _previousOperator + '\n' + _currentOperator);
        }

    }
}