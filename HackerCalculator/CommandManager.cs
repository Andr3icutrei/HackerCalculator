using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace HackerCalculator
{
    public static class CommandManager
    {
        public static void Cut_Executed(object sender, ExecutedRoutedEventArgs e,ref String activeTextBox,
            ref String previousOperand,String previousOperator, ref String currentOperand,ref String clipboard)
        {
            if (activeTextBox != null)
            {
                if(currentOperand != String.Empty)
                {
                    clipboard = currentOperand;
                    currentOperand = String.Empty;
                    activeTextBox = previousOperand + previousOperator;
                }
                else if(previousOperator ==String.Empty && previousOperand !=String.Empty)
                {
                    clipboard = previousOperand;
                    previousOperand = String.Empty;
                    activeTextBox = String.Empty;
                }
            }
        }

        public static void Copy_Executed(object sender, ExecutedRoutedEventArgs e,ref String activeTextBox,
            ref String previousOperand, String previousOperator, ref String currentOperand, ref String clipboard)
        {
            if (activeTextBox != null)
            {
                if (currentOperand != String.Empty)
                {
                    clipboard = currentOperand;
                    currentOperand = String.Empty;
                }
                else if (previousOperator == String.Empty && previousOperand != String.Empty)
                {
                    clipboard = previousOperand;
                    previousOperand = String.Empty;
                }
            }
        }

        public static void Paste_Executed(object sender, ExecutedRoutedEventArgs e,ref String activeTextBox,
            ref String previousOperand, String previousOperator, ref String currentOperand, ref String clipboard)
        {
            if (!string.IsNullOrEmpty(clipboard) && activeTextBox != null)
            {
                if (currentOperand == String.Empty && previousOperator != String.Empty)
                {
                    currentOperand = clipboard;
                    activeTextBox = previousOperand + previousOperator + currentOperand;
                }
                else if(previousOperand == String.Empty)
                {
                    previousOperand = clipboard;
                    activeTextBox = previousOperand;
                }
            }
        }
        public static bool Edit_CanExecute( TextBox activeTextBox)
        {
            return activeTextBox != null;
        }

        public static bool Paste_CanExecute(TextBox activeTextBox, ref String clipboard)
        {
            return !string.IsNullOrEmpty(clipboard) && activeTextBox != null;
        }
    }
}
