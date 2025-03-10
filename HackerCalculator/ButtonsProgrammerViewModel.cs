using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HackerCalculator
{
    public class ButtonsProgrammerViewModel : INotifyPropertyChanged
    {
        private const int rows = 5;
        private const int columns = 4;

        public Dictionary<String,int> DictBases { get; set; }

        public ObservableCollection<ObservableCollection<ButtonData>> ButtonsData { get; set; }
        public List<List<String>> ButtonText { get; set; }
        public bool[,] ButtonEnabledMatrix { get; set; }

        private String _selectedFromBaseItem;
        private String _selectedToBaseItem;

        public event PropertyChangedEventHandler PropertyChanged;
      
        public ObservableCollection<String> BaseItems { get; set; }
        public String SelectedFromBaseItem
        {
            get => _selectedFromBaseItem;
            set
            {
                _selectedFromBaseItem = value;
                OnPropertyChanged(nameof(SelectedFromBaseItem));
                UpdateEnabledMatrix();
            }
        }

        public String SelectedToBaseItem
        {
            get => _selectedToBaseItem;
            set
            {
                _selectedToBaseItem = value;
                OnPropertyChanged(nameof(SelectedToBaseItem));
            }
        }

        private void UpdateEnabledMatrix()
        {
            ObservableCollection<ObservableCollection<ButtonData>> buttonDatas = null;
            switch (SelectedFromBaseItem)
            {
                case "Binary":
                    buttonDatas = FillDataStructures.FillButtonsDataProgrammer(FillDataStructures.FillEnabledBinaryMatrix(),
                        ButtonText);
                    break;
                case "Octal":
                    buttonDatas = FillDataStructures.FillButtonsDataProgrammer(FillDataStructures.FillEnabledOctalMatrix(),
                       ButtonText);
                    break;
                case "Decimal":
                    buttonDatas = FillDataStructures.FillButtonsDataProgrammer(FillDataStructures.FillEnabledDecimalMatrix(),
                       ButtonText);
                    break;
                case "Hexadecimal":
                    buttonDatas = FillDataStructures.FillButtonsDataProgrammer(FillDataStructures.FillEnabledHexadecimalMatrix(),
                       ButtonText);
                    break;
            }
            ButtonsData = buttonDatas;
            OnPropertyChanged(nameof(ButtonsData));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ButtonsProgrammerViewModel()
        {
            ButtonText = FillDataStructures.FillButtonsContentProgrammer(rows,columns);
            ButtonEnabledMatrix = FillDataStructures.FillEnabledDecimalMatrix();
            SelectedFromBaseItem = "Decimal";
            SelectedToBaseItem = "Decimal";

            ButtonsData = FillDataStructures.FillButtonsDataProgrammer(ButtonEnabledMatrix, ButtonText);

            BaseItems = new ObservableCollection<String>
            {
                "Binary", "Octal" ,"Decimal","Hexadecimal"
            };

            DictBases = FillDataStructures.FillDictBases();
        }
    }
}
