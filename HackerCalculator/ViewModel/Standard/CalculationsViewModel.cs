using HackerCalculator.Model;
using HackerCalculator.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace HackerCalculator.ViewModel.Standard
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

        public CalculationsViewModel()
        {
            calculation = new Calculation();
            Result = String.Empty;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        private bool ValidateInput(Calculation Calculation,string lastCharacter, string penultimateCharacter, String content)
        {
            if (Calculation.CompleteCalculation.Length >= 2)
            {
                if (ValidationService.ValidateSideBySideOperators(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Cannot insert two side by side operators!");
                    return false;
                }

                if (ValidationService.ValidateDivisionByZero(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Division by zero!");
                    return false;
                }

                if (ValidationService.ValidateSideBySideDecimalSeparator(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Division by zero!");
                    return false;
                }

                if (content == ButtonsContents.DictOperators[Operators.Sqrt])
                    for (int i = Calculation.CompleteCalculation.Length - 1; i >= 0; --i)
                    {
                        if (Calculation.CompleteCalculation[i] == '+' || Calculation.CompleteCalculation[i] == '-')
                            if (ValidationService.ValidateNegativeSqrt(Calculation.CompleteCalculation[i].ToString()))
                            {
                                MessageBox.Show("Negative squared root!");
                                return false;
                            }
                    }
            }
            return true;
        }

        private void ComputeDelOptions(String buttonContent)
        {
            switch (buttonContent)
            {
                case "DEL":
                    ComupteCalculationsService.ComputeDEL(calculation);
                    break;
                case "CE":
                    ComupteCalculationsService.ComputeCE(calculation,ref result);
                    OnPropertyChanged(nameof(Result));
                    break;
                case "C":
                    ComupteCalculationsService.ComputeC(calculation,ref result);
                    OnPropertyChanged(nameof(Result));
                    break;
            }
        }
        public void ComputeAction(String content,bool isDigitGroupingChecked)
        {
            if (calculation.CompleteCalculation.Length >= 1)
            {
                if (ValidateInput(calculation,content, calculation.CompleteCalculation[calculation.CompleteCalculation.Length - 1].ToString(),
                    content))
                {
                    if (IdentifierService.IsDigit(content))
                        ComupteCalculationsService.ComputeDigit(content, calculation, isDigitGroupingChecked);
                    else if (IdentifierService.IsDecimalSeparator(content))
                        ComupteCalculationsService.ComputeDecimalSeparator(content, calculation);
                    else if (IdentifierService.IsSingularOperator(content))
                        ComupteCalculationsService.ComputeSingularOperator(content, calculation);
                    else if (IdentifierService.IsBinaryOperator(content))
                    {
                        ComupteCalculationsService.ComputeBinaryOperator(content, calculation, ref result, isDigitGroupingChecked);
                        OnPropertyChanged(nameof(Result));
                    }
                    else if (IdentifierService.IsEquals(content))
                    {
                        ComupteCalculationsService.ComputeEquals(content, calculation, ref result, isDigitGroupingChecked);
                        OnPropertyChanged(nameof(Result));
                    }
                    else if (IdentifierService.IsDelOption(content))
                        ComputeDelOptions(content);
                }
                
            }
            else
            {
                if (calculation.CompleteCalculation == String.Empty && content == "-" || content == "+")
                {
                    calculation.PreviousOperator += content;
                    calculation.CompleteCalculation += content;
                }
                else if (IdentifierService.IsDigit(content) || IdentifierService.IsDecimalSeparator(content))
                    ComupteCalculationsService.ComputeDigit(content,calculation,isDigitGroupingChecked);
            }
            Debug.Print(calculation.CompleteCalculation);
        }
        
    }
}
