using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

    public enum HexadecimalDigits :UInt16
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
    public class ButtonControls
    {
        public Dictionary<Operators, String> DictOperators { get; set; }
        public Dictionary<Digits, String> DictDigits { get; set; }
        public Dictionary<MemoryOperations, String> DictMemoryOperations { get; set; }
        public Dictionary<OtherOperations, String> DictOtherOperations { get; set; }
        public Dictionary<HexadecimalDigits, String> DictHexaDigits { get; set; }
        private void FillDictOperators()
        {
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
        }

        private void FillDictHexaDigits()
        {
            DictHexaDigits[HexadecimalDigits.InvalidHexaDigit] = "inv";
            DictHexaDigits[HexadecimalDigits.A] = "A";
            DictHexaDigits[HexadecimalDigits.B] = "B";
            DictHexaDigits[HexadecimalDigits.C] = "C";
            DictHexaDigits[HexadecimalDigits.D] = "D";
            DictHexaDigits[HexadecimalDigits.E] = "E";
            DictHexaDigits[HexadecimalDigits.F] = "F";

        }
        private void FillDictDigits()
        {
            DictDigits[Digits.InvalidDigit] = "inv";
            DictDigits[Digits.Zero] = "0";
            DictDigits[Digits.One] = "1";
            DictDigits[Digits.Two] = "2";
            DictDigits[Digits.Three] = "3"; 
            DictDigits[Digits.Four] = "4";
            DictDigits[Digits.Five] = "5";
            DictDigits[Digits.Six] = "6";
            DictDigits[Digits.Seven] = "7";
            DictDigits[Digits.Nine]= "9";
            DictDigits[Digits.Eight] = "8";
            DictDigits[Digits.DecimalSeparator] = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
        }

        private void FillDictMemoryOperations()
        {
            DictMemoryOperations[MemoryOperations.InvaildMemoryOperation] = "inv";
            DictMemoryOperations[MemoryOperations.MAdd] = "M+";
            DictMemoryOperations[MemoryOperations.MRemove] = "M-";
            DictMemoryOperations[MemoryOperations.MClear] = "MC";
            DictMemoryOperations[MemoryOperations.MStore] = "MS";
            DictMemoryOperations[MemoryOperations.MMemory] = "M>";
            DictMemoryOperations[MemoryOperations.MRecall] = "MR";
        }

        private void FillDictOtherOperations()
        {
            DictOtherOperations[OtherOperations.InvalidOtherOperation] = "inv";
            DictOtherOperations[OtherOperations.CE] = "CE";
            DictOtherOperations[OtherOperations.C] = "C";
            DictOtherOperations[OtherOperations.DEL] = "DEL";
        }

        public ButtonControls()
        {
            DictOperators = new Dictionary<Operators, String>();    
            DictDigits = new Dictionary<Digits, String>();
            DictMemoryOperations = new Dictionary<MemoryOperations, String>();
            DictOtherOperations = new Dictionary<OtherOperations, String>();
            DictHexaDigits = new Dictionary<HexadecimalDigits, string>();

            FillDictDigits();
            FillDictMemoryOperations();
            FillDictOperators();
            FillDictOtherOperations();
            FillDictHexaDigits();
        }
    }
}
