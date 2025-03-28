using HackerCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HackerCalculator.Services
{
    public static class IdentifierService
    {

        public static bool IsDelOption(String buttonContent)
        {
            string pattern = @"(DEL|CE|C)";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        public static bool IsDigit(String buttonContent)
        {
            String pattern = @"[0-9A-F]";
            Match digitsMatch = Regex.Match(buttonContent, pattern);
            return digitsMatch.Success && buttonContent.Length == 1;
        }

        public static bool IsDecimalSeparator(String buttonContent)
        {
            return buttonContent == ButtonsContents.DictDigits[Digits.DecimalSeparator];
        }

        public static bool IsEquals(String buttonContent)
        {
            var equals = ButtonsContents.DictOperators[Operators.Equals];
            return equals == buttonContent;
        }

        public static bool IsBinaryOperator(string buttonContent)
        {
            string pattern = @"[\+\-*/%]";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

        public static bool IsSingularOperator(string buttonContent)
        {
            string pattern = @"(1\/x|x\^2|sqrt\(x\)|\+/-)";
            Match match = Regex.Match(buttonContent, pattern);
            return match.Success;
        }

    }
}
