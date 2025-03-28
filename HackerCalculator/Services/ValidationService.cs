using HackerCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace HackerCalculator.Services
{
    public static class ValidationService
    {

        public static bool ValidateInput(Calculation calculation,string lastCharacter, string penultimateCharacter, String content)
        {
            if (calculation.CompleteCalculation.Length >= 2)
            {
                if (ValidateSideBySideOperators(lastCharacter, penultimateCharacter))
                {
                    MessageBox.Show("Cannot insert two side by side operators!");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateSideBySideOperators(string lastCharacter, string penultimateCharacter)
        {
            string pattern = @"[\+\-*/%]";
            Match match1 = Regex.Match(lastCharacter.ToString(), pattern);
            Match match2 = Regex.Match(penultimateCharacter.ToString(), pattern);
            return match1.Success && match2.Success;
        }

        public static bool ValidateSideBySideDecimalSeparator(string lastCharacter, string penultimateCharacter)
        {
            return lastCharacter == "." && penultimateCharacter == ".";
        }

        public static bool ValidateDivisionByZero(string lastCharacter, string penultimateCharacter)
        {
            return lastCharacter == "0" && (penultimateCharacter == "/" || penultimateCharacter == "%");
        }

        public static bool ValidateNegativeSqrt(string sign)
        {
            return sign == "-";
        }
    }
}
