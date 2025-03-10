using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HackerCalculator
{
    public class ButtonsStandardViewModel
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

        public ButtonsStandardViewModel()
        {
            ButtonsContent = FillDataStructures.FillButtonsContentStandard(rows,columns);
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
    }
}
