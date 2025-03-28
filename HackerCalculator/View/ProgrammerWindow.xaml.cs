using HackerCalculator.Services;
using System.Windows;
using HackerCalculator.View;
using HackerCalculator.ViewModel;
using System.Windows.Input;
using HackerCalculator.ViewModel.Programmer;

namespace HackerCalculator.View
{
    public partial class ProgrammerWindow : Window
    {
        public ProgrammerWindow()
        {
            InitializeComponent();

            this.Closing += ((System.Windows.Application.Current as App)).MainWindow_Closing;
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Arustei Andrei, IA331");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is ProgrammerViewModel viewModel)
            {
                viewModel.HandleKeyPressCommand.Execute(e);
            }
        }

        private void ChangeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowManagerService.ChangeWindow(this,AvailableWindow.StandardWindow);
        }

        public void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
