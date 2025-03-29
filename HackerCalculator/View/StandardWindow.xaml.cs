using HackerCalculator.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using HackerCalculator.Model;
using HackerCalculator.Services;
using HackerCalculator.ViewModel;
using HackerCalculator.ViewModel.Programmer;
using HackerCalculator.ViewModel.Standard;

namespace HackerCalculator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StandardWindow 
    {
        private FileOptionsViewModel _fileOptionsViewModel;
        public StandardWindow()
        {
            InitializeComponent();
            _fileOptionsViewModel = new FileOptionsViewModel();
            this.Closing += ((System.Windows.Application.Current as App)).MainWindow_Closing;
        }
        
        private void About_Click(object sender,RoutedEventArgs e)
        {
            MessageBox.Show("Arustei Andrei, IA331");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is StandardViewModel viewModel)
            {
                viewModel.HandleKeyPressCommand.Execute(e);
            }
        }
        private void ChangeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowManagerService.ChangeWindow(this,AvailableWindow.ProgrammerWindow);
        }

        public void MinimizeButton_Click(object sender,RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

       
    }
}