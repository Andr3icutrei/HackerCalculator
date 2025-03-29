using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackerCalculator.Model;

namespace HackerCalculator.ViewModel.Standard
{
    public class MemoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<String> Memory {  get; set; }
        private String selectedResult;

        public event PropertyChangedEventHandler? PropertyChanged;

        public String SelectedResult
        {
            get { return selectedResult; }
            set
            {
                selectedResult = value;
                OnPropertyChnaged(nameof(SelectedResult));
            }
        }
            
        private void OnPropertyChnaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ComputeMC()
        {
            if(Memory.Count!=0)
                Memory.Clear();
        }

        public MemoryViewModel()
        {
            Memory = new ObservableCollection<String>();
        }

        public void ComputeMR(Calculation calculation)
        {
            if (calculation.CurrentOperand == String.Empty)
            {
                calculation.CurrentOperand = Memory[0];
                calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator + calculation.CurrentOperand;
            }
            else if (calculation.PreviousOperand == String.Empty)
            {
                calculation.PreviousOperand = Memory[0];
                calculation.CompleteCalculation = calculation.PreviousOperand;
            }
        }

        public void ComputeMAdd(String result)
        {
            if (Memory.Count != 0 && result != String.Empty && result != "Result:")
            {
                double calculation = Convert.ToDouble(Memory[0]) + Convert.ToDouble(result);
                if (calculation == Math.Floor(calculation))
                    Memory[0] = Convert.ToString(Convert.ToInt32(calculation));
                else
                    Memory[0] = Convert.ToString(calculation);
            }
        }

        public void ComputeMSubstract(String result)
        {
            if (Memory.Count != 0 && result != String.Empty && result != "Result:")
            {
                double calculation = Convert.ToDouble(Memory[0]) - Convert.ToDouble(result);
                if (calculation == Math.Floor(calculation))
                    Memory[0] = Convert.ToString(Convert.ToInt32(calculation));
                else
                    Memory[0] = Convert.ToString(calculation);
            }
        }

        public void ComputeMS(String result)
        {
            if (result != String.Empty && result != "0")
                Memory.Insert(0, result);
        }
    }
}
