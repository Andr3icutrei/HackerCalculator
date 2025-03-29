using HackerCalculator.Model;
using HackerCalculator.Services;
using HackerCalculator.ViewModel.Programmer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace HackerCalculator.ViewModel.Standard
{
    public class StandardViewModel : INotifyPropertyChanged
    {
        public FileOptionsViewModel FileOptionsViewModel { get; set; }
        public CalculationsViewModel calculationsViewModel { get; set; }
        public MemoryViewModel memoryViewModel { get; set; }
        public UIViewModel uiViewModel {  get; set; }
        private bool isDigitGroupingChecked;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsDigitGroupingChecked
        {
            get => isDigitGroupingChecked;
            set
            {
                isDigitGroupingChecked = value;
                OnPropertyChanged(nameof(IsDigitGroupingChecked));
                CheckBoxDigitGrouping_Check();
            }
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public ICommand ButtonMCCommand { get;}
        public ICommand ButtonMRCommand { get; }
        public ICommand ButtonMAddCommand { get;}
        public ICommand ButtonMSubstractCommand { get; }
        public ICommand ButtonMSCommand { get;}
        public ICommand ButtonClickCommand { get;}

        public StandardViewModel()
        {
            calculationsViewModel = new CalculationsViewModel();
            memoryViewModel = new MemoryViewModel();
            uiViewModel = new UIViewModel();
            FileOptionsViewModel = new FileOptionsViewModel();

            ButtonMAddCommand = new RelayCommand(ButtonMAdd_Click);
            ButtonMRCommand = new RelayCommand(ButtonMR_Click);
            ButtonMSubstractCommand = new RelayCommand(ButtonMSubstract_Click);
            ButtonMSCommand = new RelayCommand(ButtonMS_Click);
            ButtonMCCommand = new RelayCommand(ButtonMC_Click);
            ButtonClickCommand = new RelayCommand(Button_Click);
        }

        public void CheckBoxDigitGrouping_Check()
        {
            ComupteCalculationsService.UpdateDisplayWithGrouping(calculationsViewModel.calculation);
        }

        private void ButtonMC_Click(object parameter)
        {
            memoryViewModel.ComputeMC();
        }

        private void ButtonMR_Click(object parameter)
        {
            memoryViewModel.ComputeMR(calculationsViewModel.calculation);
        }

        private void ButtonMAdd_Click(object parameter)
        {
            memoryViewModel.ComputeMAdd(calculationsViewModel.Result);
        }

        private void ButtonMSubstract_Click(object parameter)
        {
            memoryViewModel.ComputeMSubstract(calculationsViewModel.Result);
        }

        private void ButtonMS_Click(object parameter)
        {
            memoryViewModel.ComputeMS(calculationsViewModel.Result);
        }

        private void Button_Click(object parameter)
        {
            String content = (string)parameter;
            calculationsViewModel.ComputeAction(content, IsDigitGroupingChecked);
        }

        public ICommand HandleKeyPressCommand => new RelayCommandGeneric<KeyEventArgs>(e =>
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int numberPressed = (e.Key >= Key.D0 && e.Key <= Key.D9)
                    ? e.Key - Key.D0
                    : e.Key - Key.NumPad0;
                calculationsViewModel.ComputeAction(Convert.ToString(numberPressed),isDigitGroupingChecked);
            }

            switch (e.Key)
            {
                case Key.Enter:
                    calculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Equals], isDigitGroupingChecked);
                    break;
                case Key.Escape:
                    calculationsViewModel.ComputeAction(ButtonsContents.DictOtherOperations[OtherOperations.CE], isDigitGroupingChecked);
                    break;
                case Key.Multiply:
                    calculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Multiply], isDigitGroupingChecked);
                    break;
                case Key.Add:
                    calculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Addition], isDigitGroupingChecked);
                    break;
                case Key.Subtract:
                    calculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Subtract], isDigitGroupingChecked);
                    break;
                case Key.Divide:
                    calculationsViewModel.ComputeAction(ButtonsContents.DictOperators[Operators.Division], isDigitGroupingChecked);
                    break;
            }
        });
    }
}
