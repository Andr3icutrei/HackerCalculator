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
    public class ButtonsViewModel : INotifyPropertyChanged
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
       
        public readonly ButtonControls Controls;
        public List<List<String>> ButtonsContent { get; set; }
        public List<List<String>> TopRowContent { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ButtonsViewModel()
        {
            ButtonsContent = new List<List<string>>();
            Controls = new ButtonControls();
            MemoryResults = new ObservableCollection<string>();

            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add("");
                }
                ButtonsContent.Add(row);
            }

            ButtonsContent[0][0] = Controls.DictOperators[Operators.Modulo];
            ButtonsContent[0][1] = Controls.DictOtherOperations[OtherOperations.CE];
            ButtonsContent[0][2] = Controls.DictOtherOperations[OtherOperations.C];
            ButtonsContent[0][3] = Controls.DictOtherOperations[OtherOperations.DEL];
            ButtonsContent[1][0] = Controls.DictOperators[Operators.MultiplicativeInverse];
            ButtonsContent[1][1] = Controls.DictOperators[Operators.Square];
            ButtonsContent[1][2] = Controls.DictOperators[Operators.Sqrt];
            ButtonsContent[1][3] = Controls.DictOperators[Operators.Division];

            for (int i = 2; i <= 4; ++i)
            {
                for (int j = 0; j <= 2; ++j)
                {
                    ButtonsContent[i][j] = Controls.DictDigits[(Digits)(3 * (5 - i) - 2 + j)];
                }
            }

            ButtonsContent[2][3] = Controls.DictOperators[Operators.Multiply];
            ButtonsContent[3][3] = Controls.DictOperators[Operators.Subtract];
            ButtonsContent[4][3] = Controls.DictOperators[Operators.Addition];

            ButtonsContent[5][0] = Controls.DictOperators[Operators.AdditiveInverse];
            ButtonsContent[5][1] = Controls.DictDigits[Digits.Zero];
            ButtonsContent[5][2] = Controls.DictDigits[Digits.DecimalSeparator];
            ButtonsContent[5][3] = Controls.DictOperators[Operators.Equals];

            TopRowContent = new List<List<string>>();
            List<string> topRow = new List<string>
            {
                Controls.DictMemoryOperations[MemoryOperations.MClear],
                Controls.DictMemoryOperations[MemoryOperations.MRecall],
                Controls.DictMemoryOperations[MemoryOperations.MAdd],
                Controls.DictMemoryOperations[MemoryOperations.MRemove],
                Controls.DictMemoryOperations[MemoryOperations.MStore],
                Controls.DictMemoryOperations[MemoryOperations.MMemory]
            };
            TopRowContent.Add(topRow);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }
}
