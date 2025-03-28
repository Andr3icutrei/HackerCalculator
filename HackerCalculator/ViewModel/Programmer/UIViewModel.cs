using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackerCalculator.Model;
using HackerCalculator.Services;

namespace HackerCalculator.ViewModel.Programmer
{
    public class UIViewModel :INotifyPropertyChanged
    {
        private String selectedFromBaseItem;
        public String SelectedFromBaseItem
        {
            get => selectedFromBaseItem;
            set
            {
                selectedFromBaseItem = value;
                OnPropertyChanged(nameof(SelectedFromBaseItem));
                UpdateEnabledMatrix();
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private String selectedToBaseItem;
        public String SelectedToBaseItem
        {
            get => selectedToBaseItem;
            set
            {
                selectedToBaseItem = value;
                OnPropertyChanged(nameof(SelectedToBaseItem));
            }
        }


        private const int rows = 5;
        private const int columns = 4;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Dictionary<String, int> DictBases { get; set; }
        public ObservableCollection<ObservableCollection<ButtonData>> ButtonsData { get; set; }
        public ObservableCollection<ObservableCollection<ButtonData>> OperatorsData { get; set; }
        public List<List<String>> ButtonText { get; set; }
        public String Clipboard { get; set; }
        public bool[,] ButtonEnabledMatrix { get; set; }
        public ObservableCollection<String> BaseItems { get; set; }
        

        private void UpdateEnabledMatrix()
        {
            ObservableCollection<ObservableCollection<ButtonData>> buttonDatas = null;
            switch (SelectedFromBaseItem)
            {
                case "Binary":
                    buttonDatas = FillDataStructuresService.FillButtonsDataProgrammer(FillDataStructuresService.FillEnabledBinaryMatrix(),
                        ButtonText);
                    break;
                case "Octal":
                    buttonDatas = FillDataStructuresService.FillButtonsDataProgrammer(FillDataStructuresService.FillEnabledOctalMatrix(),
                       ButtonText);
                    break;
                case "Decimal":
                    buttonDatas = FillDataStructuresService.FillButtonsDataProgrammer(FillDataStructuresService.FillEnabledDecimalMatrix(),
                       ButtonText);
                    break;
                case "Hexadecimal":
                    buttonDatas = FillDataStructuresService.FillButtonsDataProgrammer(FillDataStructuresService.FillEnabledHexadecimalMatrix(),
                       ButtonText);
                    break;
            }
            ButtonsData = buttonDatas;
            OnPropertyChanged(nameof(ButtonsData));
        }

        public UIViewModel()
        {
            ButtonText = FillDataStructuresService.FillButtonsContentProgrammer(rows, columns);
            ButtonEnabledMatrix = FillDataStructuresService.FillEnabledDecimalMatrix();
            SelectedFromBaseItem = "Decimal";
            SelectedToBaseItem = "Decimal";
            OperatorsData = FillDataStructuresService.FillOperatorsDataProgrammer();
            ButtonsData = FillDataStructuresService.FillButtonsDataProgrammer(ButtonEnabledMatrix, ButtonText);

            BaseItems = new ObservableCollection<String>
            {
                "Binary", "Octal" ,"Decimal","Hexadecimal"
            };

            DictBases = FillDataStructuresService.FillDictBases();
        }
    }
}
