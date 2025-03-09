using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerCalculator
{
    public class ButtonsProgrammerViewModel :INotifyPropertyChanged
    {
        private const int rows = 5;
        private const int columns = 5;

        public readonly ButtonControls Controls;
        public List<List<String>> ButtonsContent { get; set; }

        private ObservableCollection<String> _baseItems;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<String> BaseItems
        {
            get => _baseItems;
            set { _baseItems = value; OnPropertyChanged(nameof(BaseItems)); }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(PropertyChanged, new PropertyChangedEventArgs(name));
        }

        public ButtonsProgrammerViewModel()
        {
            ButtonsContent = new List<List<string>>();
            Controls = new ButtonControls();

            BaseItems = new ObservableCollection<String>
            {
                 "Binary", "Octal" ,"Decimal","Hexadecimal"
            };

            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add("");
                }
                ButtonsContent.Add(row);
            }

            ButtonsContent[0][0] = Controls.DictHexaDigits[HexadecimalDigits.A];
            ButtonsContent[0][1] = Controls.DictHexaDigits[HexadecimalDigits.B];
            ButtonsContent[0][2] = Controls.DictOtherOperations[OtherOperations.CE];
            ButtonsContent[0][3] = Controls.DictOtherOperations[OtherOperations.DEL];
            ButtonsContent[0][4] = Controls.DictOperators[Operators.Division];
            ButtonsContent[0][4] = Controls.DictOperators[Operators.Division];

            ButtonsContent[1][0] = Controls.DictHexaDigits[HexadecimalDigits.C];
            ButtonsContent[1][1] = Controls.DictDigits[Digits.Seven];
            ButtonsContent[1][2] = Controls.DictDigits[Digits.Eight];
            ButtonsContent[1][3] = Controls.DictDigits[Digits.Nine];
            ButtonsContent[1][4] = Controls.DictOperators[Operators.Multiply];

            ButtonsContent[2][0] = Controls.DictHexaDigits[HexadecimalDigits.D];
            ButtonsContent[2][1] = Controls.DictDigits[Digits.Four];
            ButtonsContent[2][2] = Controls.DictDigits[Digits.Five];
            ButtonsContent[2][3] = Controls.DictDigits[Digits.Six];
            ButtonsContent[2][4] = Controls.DictOperators[Operators.Subtract];

            ButtonsContent[3][0] = Controls.DictHexaDigits[HexadecimalDigits.E];
            ButtonsContent[3][1] = Controls.DictDigits[Digits.One];
            ButtonsContent[3][2] = Controls.DictDigits[Digits.Two];
            ButtonsContent[3][3] = Controls.DictDigits[Digits.Three];
            ButtonsContent[3][4] = Controls.DictOperators[Operators.Addition];

            ButtonsContent[4][0] = Controls.DictHexaDigits[HexadecimalDigits.F];
            ButtonsContent[4][1] = Controls.DictOperators[Operators.AdditiveInverse];
            ButtonsContent[4][2] = Controls.DictDigits[Digits.Zero];
            ButtonsContent[4][3] = Controls.DictDigits[Digits.DecimalSeparator];
            ButtonsContent[4][4] = Controls.DictOperators[Operators.Equals];
        }
    }
}
