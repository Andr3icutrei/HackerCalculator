using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using HackerCalculator.Model;

namespace HackerCalculator.Services
{
    public class ClipboardService
    {
        public String Clipboard = String.Empty;
        public void Cut(Calculation calculation)
        {
            if(calculation.CurrentOperand != String.Empty)
            {
                Clipboard = calculation.CurrentOperand;
                calculation.CurrentOperand = String.Empty;
                calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator;
            }
            else if(calculation.PreviousOperator == String.Empty && calculation.PreviousOperand != String.Empty)
            {
                Clipboard = calculation.PreviousOperand;
                calculation.PreviousOperand = String.Empty;
                calculation.CompleteCalculation = String.Empty;
            }
        }

        public void Copy(Calculation calculation)
        {
            if (calculation.CurrentOperand != String.Empty)
            {
                Clipboard = calculation.CurrentOperand;
            }
            else if (calculation.PreviousOperator == String.Empty && calculation.PreviousOperand != String.Empty)
            {
                Clipboard = calculation.PreviousOperand;
            }
        }

        public void Paste(Calculation calculation)
        {
            if (!string.IsNullOrEmpty(Clipboard))
            {
                if (calculation.CurrentOperand == String.Empty && calculation.PreviousOperator != String.Empty)
                {
                    calculation.CurrentOperand = Clipboard;
                    calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator + calculation.CurrentOperand;
                }
                else if(calculation.PreviousOperand == String.Empty)
                {
                    calculation.PreviousOperand = Clipboard;
                    calculation.CompleteCalculation = calculation.PreviousOperand;
                }
            }
        }
        
    }
}
