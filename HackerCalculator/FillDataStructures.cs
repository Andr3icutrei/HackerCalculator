using HackerCalculator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HackerCalculator
{
    public enum Operators : UInt16
    {
        InvalidOperator = 0,
        Multiply,
        Addition,
        Subtract,
        Division,
        Modulo,
        Sqrt,
        MultiplicativeInverse,
        Square,
        Equals,
        AdditiveInverse,
    }

    public enum Digits : UInt16
    {
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        DecimalSeparator,
        InvalidDigit
    }

    public enum MemoryOperations : UInt16
    {
        InvaildMemoryOperation = 0,
        MAdd,
        MRemove,
        MClear,
        MStore,
        MMemory,
        MRecall
    }

    public enum HexadecimalDigits : UInt16
    {
        InvalidHexaDigit = 0,
        A,
        B,
        C,
        D,
        E,
        F
    }
    public enum OtherOperations : UInt16
    {
        InvalidOtherOperation = 0,
        CE,
        C,
        DEL
    }
    public static class FillDataStructures
    {
        public static Dictionary<Operators, String> FillDictOperators()
        {
            Dictionary<Operators, String> DictOperators = new Dictionary<Operators, String>();
            DictOperators[Operators.InvalidOperator] = "inv";
            DictOperators[Operators.Multiply] = "*";
            DictOperators[Operators.Addition] = "+";
            DictOperators[Operators.Subtract] = "-";
            DictOperators[Operators.Division] = "/";
            DictOperators[Operators.Modulo] = "%";
            DictOperators[Operators.Sqrt] = "sqrt(x)";
            DictOperators[Operators.MultiplicativeInverse] = "1/x";
            DictOperators[Operators.Square] = "x^2";
            DictOperators[Operators.Equals] = "=";
            DictOperators[Operators.AdditiveInverse] = "+/-";
            return DictOperators;
        }

        public static Dictionary<HexadecimalDigits, String> FillDictHexaDigits()
        {
            Dictionary<HexadecimalDigits, String> DictHexaDigits = new Dictionary<HexadecimalDigits, String>();
            DictHexaDigits[HexadecimalDigits.InvalidHexaDigit] = "inv";
            DictHexaDigits[HexadecimalDigits.A] = "A";
            DictHexaDigits[HexadecimalDigits.B] = "B";
            DictHexaDigits[HexadecimalDigits.C] = "C";
            DictHexaDigits[HexadecimalDigits.D] = "D";
            DictHexaDigits[HexadecimalDigits.E] = "E";
            DictHexaDigits[HexadecimalDigits.F] = "F";
            return DictHexaDigits;
        }

        public static Dictionary<Digits, String> FillDictDigits()
        {
            Dictionary<Digits, String> DictDigits = new Dictionary<Digits, string>();
            DictDigits[Digits.InvalidDigit] = "inv";
            DictDigits[Digits.Zero] = "0";
            DictDigits[Digits.One] = "1";
            DictDigits[Digits.Two] = "2";
            DictDigits[Digits.Three] = "3";
            DictDigits[Digits.Four] = "4";
            DictDigits[Digits.Five] = "5";
            DictDigits[Digits.Six] = "6";
            DictDigits[Digits.Seven] = "7";
            DictDigits[Digits.Nine] = "9";
            DictDigits[Digits.Eight] = "8";
            DictDigits[Digits.DecimalSeparator] = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            return DictDigits;
        }

        public static Dictionary<MemoryOperations, String> FillDictMemoryOperations()
        {
            Dictionary<MemoryOperations, String> DictMemoryOperations = new Dictionary<MemoryOperations, string>();
            DictMemoryOperations[MemoryOperations.InvaildMemoryOperation] = "inv";
            DictMemoryOperations[MemoryOperations.MAdd] = "M+";
            DictMemoryOperations[MemoryOperations.MRemove] = "M-";
            DictMemoryOperations[MemoryOperations.MClear] = "MC";
            DictMemoryOperations[MemoryOperations.MStore] = "MS";
            DictMemoryOperations[MemoryOperations.MMemory] = "M>";
            DictMemoryOperations[MemoryOperations.MRecall] = "MR";
            return DictMemoryOperations;
        }

        public static Dictionary<OtherOperations, String> FillDictOtherOperations()
        {
            Dictionary<OtherOperations, String> DictOtherOperations = new Dictionary<OtherOperations, string>();
            DictOtherOperations[OtherOperations.InvalidOtherOperation] = "inv";
            DictOtherOperations[OtherOperations.CE] = "CE";
            DictOtherOperations[OtherOperations.C] = "C";
            DictOtherOperations[OtherOperations.DEL] = "DEL";
            return DictOtherOperations;
        }

        public static List<List<String>> FillButtonsContentProgrammer(int rows, int columns)
        {
            List<List<String>> contents = new List<List<string>>();
            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add("");
                }
                contents.Add(row);
            }

            contents[0][0] = ButtonsContents.DictHexaDigits[HexadecimalDigits.A];
            contents[0][1] = ButtonsContents.DictHexaDigits[HexadecimalDigits.B];
            contents[0][2] = ButtonsContents.DictOtherOperations[OtherOperations.CE];
            contents[0][3] = ButtonsContents.DictOtherOperations[OtherOperations.DEL];

            contents[1][0] = ButtonsContents.DictHexaDigits[HexadecimalDigits.C];
            contents[1][1] = ButtonsContents.DictDigits[Digits.Seven];
            contents[1][2] = ButtonsContents.DictDigits[Digits.Eight];
            contents[1][3] = ButtonsContents.DictDigits[Digits.Nine];

            contents[2][0] = ButtonsContents.DictHexaDigits[HexadecimalDigits.D];
            contents[2][1] = ButtonsContents.DictDigits[Digits.Four];
            contents[2][2] = ButtonsContents.DictDigits[Digits.Five];
            contents[2][3] = ButtonsContents.DictDigits[Digits.Six];

            contents[3][0] = ButtonsContents.DictHexaDigits[HexadecimalDigits.E];
            contents[3][1] = ButtonsContents.DictDigits[Digits.One];
            contents[3][2] = ButtonsContents.DictDigits[Digits.Two];
            contents[3][3] = ButtonsContents.DictDigits[Digits.Three];

            contents[4][0] = ButtonsContents.DictHexaDigits[HexadecimalDigits.F];
            contents[4][1] = ButtonsContents.DictOperators[Operators.AdditiveInverse];
            contents[4][2] = ButtonsContents.DictDigits[Digits.Zero];
            contents[4][3] = ButtonsContents.DictOperators[Operators.Equals];

            return contents;
        }

        public static ObservableCollection<ObservableCollection<ButtonData>> FillButtonsDataProgrammer
            (bool[,] IsEnabledMatrix, List<List<String>> ContentsMatrix)
        {
            ObservableCollection<ObservableCollection<ButtonData>> buttonDatas =
                new ObservableCollection<ObservableCollection<ButtonData>>();

            int rows = IsEnabledMatrix.GetLength(0);
            int cols = IsEnabledMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                ObservableCollection<ButtonData> rowButtons = new ObservableCollection<ButtonData>();

                for (int j = 0; j < cols; j++)
                {
                    ButtonData buttonData = new ButtonData
                    {
                        Content = ContentsMatrix[i][j],
                        IsEnabled = IsEnabledMatrix[i, j]
                    };

                    rowButtons.Add(buttonData);
                }

                buttonDatas.Add(rowButtons);
            }

            return buttonDatas;
        }

        public static ObservableCollection<ObservableCollection<ButtonData>> FillOperatorsDataProgrammer()
        {
            ObservableCollection<ObservableCollection<ButtonData>> operatorsDatas =
                new ObservableCollection<ObservableCollection<ButtonData>>();

            ObservableCollection<ButtonData> rowButtons1 = new ObservableCollection<ButtonData>();
            rowButtons1.Add(new ButtonData { Content = "+", IsEnabled = true });

            ObservableCollection<ButtonData> rowButtons2 = new ObservableCollection<ButtonData>();
            rowButtons2.Add(new ButtonData { Content= "-", IsEnabled = true });

            ObservableCollection<ButtonData> rowButton3 = new ObservableCollection<ButtonData>();
            rowButton3.Add(new ButtonData { Content="*",IsEnabled = true});
            operatorsDatas.Add(rowButtons1);
            operatorsDatas.Add(rowButtons2);
            operatorsDatas.Add(rowButton3);

            return operatorsDatas;
        }
        public static List<List<String>> FillButtonsContentStandard(int rows, int columns)
        {
            List<List<String>> contents = new List<List<string>>();
            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add("");
                }
                contents.Add(row);
            }

            contents[0][0] = ButtonsContents.DictOperators[Operators.Modulo];
            contents[0][1] = ButtonsContents.DictOtherOperations[OtherOperations.CE];
            contents[0][2] = ButtonsContents.DictOtherOperations[OtherOperations.C];
            contents[0][3] = ButtonsContents.DictOtherOperations[OtherOperations.DEL];
            contents[1][0] = ButtonsContents.DictOperators[Operators.MultiplicativeInverse];
            contents[1][1] = ButtonsContents.DictOperators[Operators.Square];
            contents[1][2] = ButtonsContents.DictOperators[Operators.Sqrt];
            contents[1][3] = ButtonsContents.DictOperators[Operators.Division];

            for (int i = 2; i <= 4; ++i)
            {
                for (int j = 0; j <= 2; ++j)
                {
                    contents[i][j] = ButtonsContents.DictDigits[(Digits)(3 * (5 - i) - 2 + j)];
                }
            }

            contents[2][3] = ButtonsContents.DictOperators[Operators.Multiply];
            contents[3][3] = ButtonsContents.DictOperators[Operators.Subtract];
            contents[4][3] = ButtonsContents.DictOperators[Operators.Addition];

            contents[5][0] = ButtonsContents.DictOperators[Operators.AdditiveInverse];
            contents[5][1] = ButtonsContents.DictDigits[Digits.Zero];
            contents[5][2] = ButtonsContents.DictDigits[Digits.DecimalSeparator];
            contents[5][3] = ButtonsContents.DictOperators[Operators.Equals];
            return contents;
        }

        public static bool[,] FillEnabledBinaryMatrix()
        {
            return new bool[,]
            {
                { false, false, true, true },
                { false, false, false, false },
                { false, false, false, false },
                { false, true, false, false },
                { false, true, true, true }
            };
        }
        public static bool[,] FillEnabledOctalMatrix()
        {
            return new bool[,]
            {
                { false, false, true, true },
                { false, true, false, false },
                { false, true, true, true },
                { false, true, true, true },
                { false, true, true, true }
            };
        }
        public static bool[,] FillEnabledDecimalMatrix()
        {
            return new bool[,]
            {
                { false, false, true, true },
                { false, true, true, true },
                { false, true, true, true },
                { false, true, true, true },
                { false, true, true, true }
            };
        }
        public static bool[,] FillEnabledHexadecimalMatrix()
        {
            return new bool[,]
            {
                { true, true, true, true },
                { true, true, true, true },
                { true, true, true, true },
                { true, true, true, true },
                { true, true, true, true }
            };
        }
        public static Dictionary<String,int> FillDictBases()
        {
            Dictionary<string, int> DictBases = new Dictionary<string, int>();

            DictBases["Binary"] = 2;
            DictBases["Octal"] = 8;
            DictBases["Decimal"] = 10;
            DictBases["Hexadecimal"] = 16;
            return DictBases;
        }
    }

}
