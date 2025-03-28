using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackerCalculator.Model;

namespace HackerCalculator.ViewModel.Standard
{
    public class MemoryViewModel
    {
        private ObservableCollection<String> memory;

        public void ComputeMC()
        {
            if(memory.Count!=0)
                memory.Clear();
        }

        public void ComputeMR(Calculation calculation)
        {
            if (calculation.CurrentOperand == String.Empty)
            {
                calculation.CurrentOperand = memory[0];
                calculation.CompleteCalculation = calculation.PreviousOperand + calculation.PreviousOperator + calculation.CurrentOperand;
            }
            else if (calculation.PreviousOperand == String.Empty)
            {
                calculation.PreviousOperand = memory[0];
                calculation.CompleteCalculation = calculation.PreviousOperand;
            }
        }

        public void ComputeMAdd(String result)
        {
            if (memory.Count != 0 && result != String.Empty && result != "Result:")
            {
                double calculation = Convert.ToDouble(memory[0]) + Convert.ToDouble(result);
                if (calculation == Math.Floor(calculation))
                    memory[0] = Convert.ToString(Convert.ToInt32(calculation));
                else
                    memory[0] = Convert.ToString(calculation);
            }
        }

        public void ComputeMSubstract(String result)
        {
            if (memory.Count != 0 && result != String.Empty && result != "Result:")
            {
                double calculation = Convert.ToDouble(memory[0]) - Convert.ToDouble(result);
                if (calculation == Math.Floor(calculation))
                    memory[0] = Convert.ToString(Convert.ToInt32(calculation));
                else
                    memory[0] = Convert.ToString(calculation);
            }
        }

        public void ComputeMS(String result)
        {
            if (result != String.Empty && result != "0")
                memory.Insert(0, result);
        }
    }
}
