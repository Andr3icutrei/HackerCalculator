using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HackerCalculator.Services;

namespace HackerCalculator.Model
{
    
    public static class ButtonsContents
    {
        public static Dictionary<Operators, String> DictOperators { get; set; } = 
            FillDataStructuresService.FillDictOperators();
        public static Dictionary<Digits, String> DictDigits { get; set; } =
            FillDataStructuresService.FillDictDigits();
        public static Dictionary<MemoryOperations, String> DictMemoryOperations { get; set; } =
            FillDataStructuresService.FillDictMemoryOperations();
        public static Dictionary<OtherOperations, String> DictOtherOperations { get; set; } =
            FillDataStructuresService.FillDictOtherOperations();
        public static Dictionary<HexadecimalDigits, String> DictHexaDigits { get; set; } = 
            DictHexaDigits = FillDataStructuresService.FillDictHexaDigits();
        
    }
}
