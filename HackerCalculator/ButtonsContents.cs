using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HackerCalculator
{
    
    public static class ButtonsContents
    {
        public static Dictionary<Operators, String> DictOperators { get; set; } = 
            FillDataStructures.FillDictOperators();
        public static Dictionary<Digits, String> DictDigits { get; set; } =
            FillDataStructures.FillDictDigits();
        public static Dictionary<MemoryOperations, String> DictMemoryOperations { get; set; } =
            FillDataStructures.FillDictMemoryOperations();
        public static Dictionary<OtherOperations, String> DictOtherOperations { get; set; } =
            FillDataStructures.FillDictOtherOperations();
        public static Dictionary<HexadecimalDigits, String> DictHexaDigits { get; set; } = 
            DictHexaDigits = FillDataStructures.FillDictHexaDigits();
        
    }
}
