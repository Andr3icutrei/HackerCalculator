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
        public static void Cut_Executed(object sender, ExecutedRoutedEventArgs e,TextBox activeTextBox,ref String clipboard)
        {
            if (activeTextBox != null && activeTextBox.SelectionLength > 0)
            {
                clipboard = activeTextBox.SelectedText;

                int selectionStart = activeTextBox.SelectionStart;
                activeTextBox.Text = activeTextBox.Text.Remove(selectionStart, activeTextBox.SelectionLength);
                activeTextBox.SelectionStart = selectionStart;
            }
        }

        public static void Copy_Executed(object sender, ExecutedRoutedEventArgs e, TextBox activeTextBox, ref String clipboard)
        {
            if (activeTextBox != null && activeTextBox.SelectionLength > 0)
            {
                clipboard = activeTextBox.SelectedText;
            }
        }

        public static void Paste_Executed(object sender, ExecutedRoutedEventArgs e,TextBox activeTextBox, ref String clipboard)
        {
            if (!string.IsNullOrEmpty(clipboard) && activeTextBox != null)
            {
                int selectionStart = activeTextBox.SelectionStart;
                int selectionLength = activeTextBox.SelectionLength;

                activeTextBox.Text = activeTextBox.Text.Remove(selectionStart, selectionLength)
                .Insert(selectionStart, clipboard);

                activeTextBox.SelectionStart = selectionStart + clipboard.Length;
            }
        }
        public static bool Edit_CanExecute( TextBox activeTextBox)
        {
            return activeTextBox != null && activeTextBox.SelectionLength > 0;
        }

        public static bool Paste_CanExecute(TextBox activeTextBox, ref String clipboard)
        {
            return !string.IsNullOrEmpty(clipboard) && activeTextBox != null;
        }
    }
}
