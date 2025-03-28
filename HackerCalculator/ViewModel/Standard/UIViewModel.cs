using HackerCalculator.Model;
using HackerCalculator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HackerCalculator.ViewModel.Standard
{
    public class UIViewModel
    {
        private const int rows = 6;
        private const int columns = 4;
        private String _selectedResult;

        public String SelectedResult
        {
            get { return _selectedResult; }
            set
            {
                _selectedResult = value;
                OnPropertyChanged(nameof(SelectedResult));
            }
        }

        public ObservableCollection<String> MemoryResults { get; set; }

        public List<List<String>> ButtonsContent { get; set; }
        public List<List<String>> TopRowContent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UIViewModel()
        {
            ButtonsContent = FillDataStructuresService.FillButtonsContentStandard(rows, columns);
            MemoryResults = new ObservableCollection<string>();

            TopRowContent = new List<List<string>>();
            List<string> topRow = new List<string>
            {
                ButtonsContents.DictMemoryOperations[MemoryOperations.MClear],
                ButtonsContents.DictMemoryOperations[MemoryOperations.MRecall],
                ButtonsContents.DictMemoryOperations[MemoryOperations.MAdd],
                ButtonsContents.DictMemoryOperations[MemoryOperations.MRemove],
                ButtonsContents.DictMemoryOperations[MemoryOperations.MStore],
                ButtonsContents.DictMemoryOperations[MemoryOperations.MMemory]
            };
            TopRowContent.Add(topRow);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }

        private void TextBoxCaculation_Changed(object sender, RoutedEventArgs e)
        {

        }
    }
}
