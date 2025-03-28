using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerCalculator.Model
{
    public class Calculation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public String CurrentOperand {  get; set; }
        public String PreviousOperand { get; set; }
        public String PreviousOperator { get; set; }

        private String completeCalculation;
        public String CompleteCalculation
        {
            get { return completeCalculation; }
            set
            {
                completeCalculation = value;
                OnPropertyChanged(nameof(CompleteCalculation));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Calculation(String previousOperand,String previousOperator,String currentOperand,String calculation)
        {
            completeCalculation = calculation;
            PreviousOperand = previousOperand;
            PreviousOperator = previousOperator;
            CurrentOperand = currentOperand;
        }

        public Calculation(Calculation c)
        {
            PreviousOperand=c.PreviousOperand;
            PreviousOperator=c.PreviousOperator;
            CurrentOperand=c.CurrentOperand;
            completeCalculation = c.CompleteCalculation;
        }

        public Calculation()
        {
            PreviousOperator = PreviousOperand = CompleteCalculation = CurrentOperand = String.Empty;
        }
    }
}
