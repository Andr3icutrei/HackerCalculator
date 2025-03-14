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
        private TextBox _activeTextBox;

        public ProgrammerWindow()
        {
            DataContext = this;
            InitializeComponent();

            _previousOperand = string.Empty;
            _currentOperand = string.Empty;
            _previousOperator = string.Empty;

            _activeTextBox = TextBoxCalculation;

            this.KeyDown += MainWindow_KeyDown;
            this.Closing += ((System.Windows.Application.Current as App)).MainWindow_Closing;

            TextBoxCalculation.GotFocus += TextBox_GotFocus;
            TextBoxResult.GotFocus += TextBox_GotFocus;

        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Arustei Andrei, IA331");
        }
        private void TextBox_GotFocus(object sender,RoutedEventArgs e)
        {
            if(sender is TextBox tb)
            {
                _activeTextBox = tb;
            }
        }

        private Button FindButtonByContentRecursive(DependencyObject parent, string content)
        {
            // Loop through all the visual children of the current element.
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is a button, check its content.
                if (child is Button button && button.Content?.ToString() == content)
                {
                    return button;
                }

                // If the child is a container, recurse into its children.
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
            CommandManager.Cut_Executed(sender,e,_activeTextBox,ref _clipboard);
        }

        public void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.Copy_Executed(sender,e,_activeTextBox,ref _clipboard);
        }

        public void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.Paste_Executed(sender,e, _activeTextBox,ref _clipboard);
        }
        public void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CommandManager.Edit_CanExecute(_activeTextBox);
        }

        public void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CommandManager.Paste_CanExecute(_activeTextBox,ref _clipboard);
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
                ref calculation, true);
            TextBoxCalculation.Text = calculation;
        }

        private string FromBase10(String operand,int toBase)
        {

            int number = Convert.ToInt32(operand);
            if (number == 0) return "0";

            string result = "";
            while (number > 0)
            {
                int remainder = number % toBase;
                result = (remainder < 10 ? remainder.ToString() : ((char)(remainder - 10 + 'A')).ToString()) + result;
                number /= toBase;
            }

            return Convert.ToString(result);
        }

        private string ToBase10(String operand,int fromBase)
        {

            int result = 0;
            int power = 0;

            for (int i = operand.Length - 1; i >= 0; i--)
            {
                char digitChar = operand[i];
                int digit = Char.IsDigit(digitChar) ? digitChar - '0' : Char.ToUpper(digitChar) - 'A' + 10;

                if (digit >= fromBase)
                {
                    throw new ArgumentException($"Invalid digit {digitChar} for base {fromBase}");
                }

                result += digit * (int)Math.Pow(fromBase, power);
                power++;
            }

            return Convert.ToString(result);
        }

        protected void ComputeEquals(String buttonContent)
        {
            String calculation = TextBoxCalculation.Text;
            String result = TextBoxResult.Text;

            var dictBases = (DataContext as ButtonsProgrammerViewModel).DictBases;
            String fromBaseString = (DataContext as ButtonsProgrammerViewModel).SelectedFromBaseItem;
            String toBaseString = (DataContext as ButtonsProgrammerViewModel).SelectedToBaseItem;
            int toBase = dictBases[toBaseString];
            int fromBase = dictBases[fromBaseString];
            if (_previousOperand != String.Empty) 
                _previousOperand = ToBase10(_previousOperand,fromBase);
            if(_currentOperand != String.Empty)
                _currentOperand = ToBase10(_currentOperand,fromBase);

            ComputeCalculations.ComputeEquals(buttonContent, ref _previousOperand, ref _previousOperator, ref _currentOperand, ref result, ref calculation, false);
            TextBoxCalculation.Text = FromBase10(calculation, fromBase);
            TextBoxResult.Text = FromBase10(result,toBase);

            _previousOperand = FromBase10(calculation, fromBase);
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
                    ref _currentOperand, ref calculation, ref result);
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

    }
}
