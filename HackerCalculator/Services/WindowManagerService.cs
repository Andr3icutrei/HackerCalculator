using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HackerCalculator.View;

namespace HackerCalculator.Services
{
    public enum AvailableWindow : UInt16
    {
        InvalidWindow = 0,
        StandardWindow,
        ProgrammerWindow
    }
    public static class WindowManagerService
    {
        public static void ChangeWindow(Window currentWindow,AvailableWindow windowToBeChanged)
        {
            double currentWindowLeft = currentWindow.Left;
            double currentWindowTop = currentWindow.Top;

            Window toOpen = null;
            try
            {
                switch (windowToBeChanged)
                {
                    case AvailableWindow.InvalidWindow:
                        throw new Exception("Invalid Window");
                    case AvailableWindow.StandardWindow:
                        toOpen = new StandardWindow();
                        break;
                    case AvailableWindow.ProgrammerWindow:
                        toOpen = new ProgrammerWindow();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }

            toOpen.Left = currentWindowLeft;
            toOpen.Top = currentWindowTop;
            toOpen.Show();

            currentWindow.Close();
        }
    }
}
