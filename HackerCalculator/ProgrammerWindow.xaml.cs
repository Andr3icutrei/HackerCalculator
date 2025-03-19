using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    public static class Extensions
    {
        public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            foreach (object child in LogicalTreeHelper.GetChildren(parent))
            {
                if (child is T found)
                {
                    yield return found;
                }

                if (child is DependencyObject depChild)
                {
                    foreach (T nestedChild in depChild.FindLogicalChildren<T>())
                    {
                        yield return nestedChild;
                    }
                }
            }
        }
    }
    public partial class ProgrammerWindow : Window
    {
        private string _previousOperand;
        private string _previousOperator;
        private string _currentOperand;
        private string _clipboard;

        public ProgrammerWindow()
        {
            DataContext = this;
            InitializeComponent();

            _previousOperand = string.Empty;
            _currentOperand = string.Empty;
            _previousOperator = string.Empty;


            this.KeyDown += MainWindow_KeyDown;
            this.Closing += ((System.Windows.Application.Current as App)).MainWindow_Closing;

        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Arustei Andrei, IA331");
        }

        private Button FindButtonByContentRecursive(DependencyObject parent, string content)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button button && button.Content?.ToString() == content)
                {
                    return button;
                }

                if (child is DependencyObject depChild)
                {
                    var foundButton = FindButtonByContentRecursive(depChild, content);
                    if (foundButton != null)
                    {
                        return foundButton;
                    }
                }
            }

            return null;
        }

        public void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String calculation = TextBoxCalculation.Text;
            CommandManager.Cut_Executed(sender,e,ref calculation,ref _previousOperand,_previousOperator,
                ref _currentOperand,ref _clipboard);
            TextBoxCalculation.Text = calculation;
        }

        public void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String calculation = TextBoxCalculation.Text;
            CommandManager.Copy_Executed(sender, e, ref calculation, ref _previousOperand, _previousOperator,
                ref _currentOperand, ref _clipboard);
            TextBoxCalculation.Text = calculation;
        }
        public void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String calculation = TextBoxCalculation.Text;
            CommandManager.Paste_Executed(sender, e, ref calculation, ref _previousOperand, _previousOperator,
                ref _currentOperand, ref _clipboard);
            TextBoxCalculation.Text = calculation;
        }
        public void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CommandManager.Edit_CanExecute(TextBoxCalculation);
        }

        public void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CommandManager.Paste_CanExecute(TextBoxCalculation, ref _clipboard);
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Button toFindButton;

            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int numberPressed = (e.Key >= Key.D0 && e.Key <= Key.D9)
                    ? e.Key - Key.D0
                    : e.Key - Key.NumPad0;
                toFindButton = FindButtonByContentRecursive(this,numberPressed.ToString());
                if (toFindButton != null && toFindButton.IsEnabled)
                { 
                    ComputeAction(Convert.ToString(numberPressed));
                }
            }
            else if ((e.Key >= Key.A && e.Key <= Key.F))
            {
                string letter = e.Key.ToString();
                toFindButton = FindButtonByContentRecursive(this,letter);
                if (toFindButton != null && toFindButton.IsEnabled)
                { 
                    ComputeAction(letter); 
                }
            }

            switch (e.Key)
            {
                case Key.Enter:
                    ComputeAction(ButtonsContents.DictOperators[Operators.Equals]);
                    break;
                case Key.Escape:
                    ComputeAction(ButtonsContents.DictOtherOperations[OtherOperations.CE]);
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

        private bool ValidateInput(string lastCharacter, string penultimateCharacter, String content)
        {
            if (TextBoxCalculation.Text.Length >= 2)
            {
                if (ValidateSideBySideOperators(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Cannot insert two side by side operators!");
                    return false;
                }
            }
            return true;
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
            string pattern = @"(DEL|CE)";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        private void ComputeDEL()
        {
            String calculation = TextBoxCalculation.Text;
            ComputeCalculations.ComputeDEL(ref _previousOperand,ref _previousOperator,ref _currentOperand, ref calculation);
            TextBoxCalculation.Text = calculation;
        }

        private void ComputeCE()
        {
            String calculation = TextBoxCalculation.Text;
            String result = TextBoxResult.Text;
            ComputeCalculations.ComputeCE(_previousOperand,_previousOperator, ref _currentOperand, ref calculation, ref result);
            TextBoxCalculation.Text= calculation;
            TextBoxResult.Text= result;
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
            String calculation = TextBoxCalculation.Text;
            ComputeCalculations.ComputeDigit(buttonContent, ref _previousOperand, ref _previousOperator, ref _currentOperand,
                ref calculation, false);
            TextBoxCalculation.Text = calculation;
        }

        protected void ComputeEquals(String buttonContent)
        {
            try
            {
                String calculation = TextBoxCalculation.Text;
                String result = TextBoxResult.Text;
                var dictBases = (DataContext as ButtonsProgrammerViewModel).DictBases;
                String fromBaseString = (DataContext as ButtonsProgrammerViewModel).SelectedFromBaseItem;
                String toBaseString = (DataContext as ButtonsProgrammerViewModel).SelectedToBaseItem;
                int toBase = dictBases[toBaseString];
                int fromBase = dictBases[fromBaseString];

                // Convert operands to base 10 without changing the original values
                string decimalPrevOperand = !string.IsNullOrEmpty(_previousOperand) ?
                    ToBase10(_previousOperand, fromBase) : string.Empty;

                string decimalCurrOperand = !string.IsNullOrEmpty(_currentOperand) ?
                    ToBase10(_currentOperand, fromBase) : string.Empty;

                // Keep original values for display purposes
                string originalPrevOperand = _previousOperand;

                // Perform calculation in base 10
                string decimalResult = result;
                string decimalCalculation = calculation;

                // Make copies of the variables to pass to ComputeEquals
                string tempPrevOperand = decimalPrevOperand;
                string tempOperator = _previousOperator;
                string tempCurrOperand = decimalCurrOperand;

                // Call compute with decimal values
                ComputeCalculations.ComputeEquals(
                    buttonContent,
                    ref tempPrevOperand,
                    ref tempOperator,
                    ref tempCurrOperand,
                    ref decimalResult,
                    ref decimalCalculation,
                    false
                );

                // Update operator for next calculation
                _previousOperator = tempOperator;

                // Convert result back to target base
                string displayResult = FromBase10(decimalResult, toBase);

                // Update the display
                TextBoxResult.Text = displayResult;

                if (!string.IsNullOrEmpty(_currentOperand))
                {
                    string displayCalculation = FromBase10(decimalCalculation, fromBase);
                    TextBoxCalculation.Text = displayCalculation;
                    _previousOperand = displayCalculation;
                    _currentOperand = String.Empty;
                }
                else if (!string.IsNullOrEmpty(tempPrevOperand))
                {
                    TextBoxCalculation.Text = originalPrevOperand;
                    _previousOperand = originalPrevOperand;
                }
            }
            catch (Exception ex)
            {
                TextBoxResult.Text = "Error: " + ex.Message;
            }
        }

        // Improved conversion methods
        private string ToBase10(string operand, int fromBase)
        {
            if (string.IsNullOrEmpty(operand))
                return "0";

            // Handle negative numbers
            bool isNegative = operand.StartsWith("-");
            if (isNegative)
                operand = operand.Substring(1);

            // Convert digit by digit
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
            // Parse the decimal string to a number
            if (!long.TryParse(decimalStr, out long number))
                return "0"; // Fallback or handle error as needed

            // Handle zero separately
            if (number == 0)
                return "0";

            // Handle negative numbers
            bool isNegative = number < 0;
            if (isNegative)
                number = -number;

            // Convert to target base
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
            String calculation = TextBoxCalculation.Text;
            ComputeCalculations.ComputeDecimalSeparator(separator, ref _previousOperand, ref _currentOperand, ref calculation);
            TextBoxCalculation.Text = calculation;
        }



        private bool IsBinaryOperator(String content)
        {
            return "+-*".Contains(content);
        }

        private void ComputeBinaryOperator(String content)
        {
            String calculation = TextBoxCalculation.Text;
            String result = TextBoxResult.Text;

            if (_currentOperand!= String.Empty)
            { 
                ComputeEquals("=");
                TextBoxCalculation.Text += content;
                _previousOperator = content;
            }
            else
            {
                ComputeCalculations.ComputeBinaryOperator(content, ref _previousOperand, ref _previousOperator,
                    ref _currentOperand, ref calculation, ref result,false);
                TextBoxCalculation.Text = calculation;
                TextBoxResult.Text = result;
            }
            
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
                    else if (IsBinaryOperator(content))
                        ComputeBinaryOperator(content);
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
                if(IsDigit(content))
                    ComputeDigit(content);
            }
        }

        protected void ComputeSingularOperator(String buttonContent)
        {
            String calculation = TextBoxCalculation.Text;
            ComputeCalculations.ComputeSingularOperator(buttonContent,ref _currentOperand,ref _previousOperand, _previousOperator,ref calculation);
            TextBoxCalculation.Text = calculation;
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
            double currentWindowLeft = this.Left;
            double currentWindowTop = this.Top;
            StandardWindow mainWindow = new StandardWindow();
            mainWindow.Left = currentWindowLeft;
            mainWindow.Top = currentWindowTop;
            mainWindow.Show();

            this.Close();
        }

        private void listBoxFromBase_Changed(object sender, RoutedEventArgs e)
        {
            if(TextBoxCalculation!=null)
                _previousOperand = _previousOperator = _currentOperand = TextBoxCalculation.Text = String.Empty;
        }

        public void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
