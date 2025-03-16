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

        private string _previousOperator;
        private string _previousOperand;
        private string _currentOperand;
        private string _clipboard;
        private TextBox _activeTextBox;

        public StandardWindow()
        {
            InitializeComponent();

            _previousOperand = String.Empty;
            _currentOperand = String.Empty;
            _previousOperator = String.Empty;
            _clipboard = String.Empty;

            _activeTextBox = TextBoxCalculation;

            this.KeyDown += MainWindow_KeyDown;
            this.Closing += ((System.Windows.Application.Current as App)).MainWindow_Closing;

            TextBoxCalculation.GotFocus += TextBox_GotFocus;
            TextBoxResult.GotFocus += TextBox_GotFocus;

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                _activeTextBox = tb;
            }
        }

        public void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.Cut_Executed(sender, e, _activeTextBox, ref _clipboard);
        }

        public void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.Copy_Executed(sender, e, _activeTextBox, ref _clipboard);
        }

        public void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.Paste_Executed(sender, e, _activeTextBox, ref _clipboard);
        }
        public void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CommandManager.Edit_CanExecute(_activeTextBox);
        }

        public void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CommandManager.Paste_CanExecute(_activeTextBox, ref _clipboard);
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

        private void About_Click(object sender,RoutedEventArgs e)
        {
            MessageBox.Show("Arustei Andrei, IA331");
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
        
        private void CheckBoxDigitGrouping_Check(object sender,RoutedEventArgs e)
        {
            String calculation = TextBoxCalculation.Text;
            ComputeCalculations.UpdateDisplayWithGrouping(_previousOperand,_previousOperator,_currentOperand,ref calculation);
            TextBoxCalculation.Text = calculation;
        }
        private void ComputeDelOptions(String buttonContent,ref String calculation,ref String result)
        {
            switch (buttonContent)
            {
                case "DEL":
                    ComputeCalculations.ComputeDEL(ref _previousOperand,ref _previousOperator,ref _currentOperand,ref calculation);
                    break;
                case "CE":
                    ComputeCalculations.ComputeCE(_previousOperand,_previousOperator,ref _currentOperand,ref calculation,ref result);
                    break;
                case "C":
                    ComputeCalculations.ComputeC(ref _previousOperand,ref _previousOperator,ref _currentOperand,ref calculation,ref result);
                    break;
            }
        }
        private void ComputeAction(String content)
        {
            String calculation = TextBoxCalculation.Text;
            String result = TextBoxResult.Text;
            if (TextBoxCalculation.Text.Length >= 1)
            {
                if (ValidateInput(content, TextBoxCalculation.Text[TextBoxCalculation.Text.Length - 1].ToString(),
                    content))
                {
                    if (IsDigit(content))
                        ComputeCalculations.ComputeDigit(content, ref _previousOperand, ref _previousOperator, ref _currentOperand,
                            ref calculation, (bool)CheckBoxDigitGrouping.IsChecked);
                    else if (IsDecimalSeparator(content))
                        ComputeCalculations.ComputeDecimalSeparator(content, ref _previousOperand, ref _currentOperand, ref calculation);
                    else if (IsSingularOperator(content))
                        ComputeCalculations.ComputeSingularOperator(content, ref _currentOperand, ref _previousOperand, _previousOperator,
                            ref calculation);
                    else if (IsBinaryOperator(content))
                        ComputeCalculations.ComputeBinaryOperator(content, ref _previousOperand, ref _previousOperator, ref _currentOperand,
                            ref calculation, ref result,(bool)CheckBoxDigitGrouping.IsChecked);
                    else if (IsEquals(content))
                        ComputeCalculations.ComputeEquals(content, ref _previousOperand, ref _previousOperator, ref _currentOperand,
                            ref result, ref calculation, (bool)CheckBoxDigitGrouping.IsChecked);
                    else if (IsDelOption(content))
                        ComputeDelOptions(content, ref calculation, ref result);
                }
            }
            else
            {
                if (calculation == String.Empty && content == "-" || content == "+")
                {
                    _previousOperator += content;
                    calculation += content; 
                }
                else if (IsDigit(content) || IsDecimalSeparator(content))
                    ComputeCalculations.ComputeDigit(content, ref _previousOperand, ref _previousOperator, ref _currentOperand, ref calculation,
                        (bool)CheckBoxDigitGrouping.IsChecked);
            }
            TextBoxCalculation.Text = calculation;
            TextBoxResult.Text = result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String buttonContent = (sender as Button).Content.ToString();
            ComputeAction(buttonContent);
            //MessageBox.Show(_previousOperand + '\n' + _currentOperand + '\n' + _previousOperator + '\n' + _currentOperator);
        }

        private void ChangeWindow_Click(object sender, RoutedEventArgs e)
        {
            double currentWindowLeft = this.Left;
            double currentWindowTop = this.Top;
            ProgrammerWindow mainWindow = new ProgrammerWindow();
            mainWindow.Left = currentWindowLeft;
            mainWindow.Top = currentWindowTop;
            mainWindow.Show();
            this.Close();
        }
    }
}