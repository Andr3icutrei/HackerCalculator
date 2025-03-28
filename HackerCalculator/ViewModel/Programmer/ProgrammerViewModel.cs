using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using HackerCalculator.Model;
using HackerCalculator.Services;

namespace HackerCalculator.ViewModel.Programmer
{
    public class ProgrammerViewModel : INotifyPropertyChanged
    {
        public CalculationsViewModel CalculationsViewModel { get; set; }
        public FileOptionsViewModel FileOptionsViewModel { get; set; }
        public UIViewModel UiViewModel { get; set; }

        public ICommand ButtonClickCommand { get; }
        public ICommand ListBoxFromBaseChangedCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
      
        public ProgrammerViewModel()
        {
            CalculationsViewModel = new CalculationsViewModel();
            FileOptionsViewModel = new FileOptionsViewModel();
            UiViewModel = new UIViewModel();

            ListBoxFromBaseChangedCommand = new RelayCommand(listBoxFromBase_Changed);
            ButtonClickCommand = new RelayCommand(Button_Click);
        }

        private void listBoxFromBase_Changed(object parameter)
        {
            CalculationsViewModel.calculation = new Model.Calculation();
        }

        private void Button_Click(object parameter)
        {
            var dictBases = UiViewModel.DictBases;
            String fromBaseString = UiViewModel.SelectedFromBaseItem;
            String toBaseString = UiViewModel.SelectedToBaseItem;
            int toBase = dictBases[toBaseString];
            int fromBase = dictBases[fromBaseString];
            String content = (string)parameter;

            CalculationsViewModel.ComputeAction(content,fromBase,toBase);
        }

        public ICommand HandleKeyPressCommand => new RelayCommandGeneric<KeyEventArgs>(e =>
        {
            // Inline command logic
            var dictBases = UiViewModel.DictBases;
            String fromBaseString = UiViewModel.SelectedFromBaseItem;
            String toBaseString = UiViewModel.SelectedToBaseItem;
            int toBase = dictBases[toBaseString];
            int fromBase = dictBases[fromBaseString];
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int numberPressed = (e.Key >= Key.D0 && e.Key <= Key.D9)
                    ? e.Key - Key.D0
                    : e.Key - Key.NumPad0;
                if(numberPressed<fromBase)
                    CalculationsViewModel.ComputeAction(Convert.ToString(numberPressed),toBase,fromBase);
                
            }
            else if ((e.Key >= Key.A && e.Key <= Key.F))
            {
                string letter = e.Key.ToString();
                if(UiViewModel.SelectedFromBaseItem=="Hexadecimal")
                {
                    CalculationsViewModel.ComputeAction(letter, toBase, fromBase);
                }
            }

            switch (e.Key)
            {
                case Key.Enter:
                    CalculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Equals], toBase, fromBase);
                    break;
                case Key.Escape:
                    CalculationsViewModel.ComputeAction(ButtonsContents.DictOtherOperations[OtherOperations.CE], toBase, fromBase);
                    break;
                case Key.Multiply:
                    CalculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Multiply], toBase, fromBase);
                    break;
                case Key.Add:
                    CalculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Addition], toBase, fromBase);
                    break;
                case Key.Subtract:
                    CalculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Subtract], toBase, fromBase);
                    break;
                case Key.Divide:
                    CalculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Division], toBase, fromBase);
                    break;
            }
        });
    }
}
