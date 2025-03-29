using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using HackerCalculator.ViewModel.Programmer;
using HackerCalculator.Model;
using HackerCalculator.View;
using HackerCalculator.Services;

namespace HackerCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppSettings Settings { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Settings = JsonHelperService.LoadSettings();

            if (Settings.IsStandardMode)
                LoadStandardMode(Settings);
            else
                LoadProgrammerMode(Settings);
        }

        private void LoadStandardMode(AppSettings settings)
        {
            string standardXamlPath = "./View/StandardWindow.xaml";

            Application.Current.Dispatcher.Invoke(() =>
            {
                StandardWindow window = new StandardWindow();
                window.CheckBoxDigitGrouping.IsChecked = settings.IsDigitGroupingActive;
                window.Show();
            });
        }

        private void LoadProgrammerMode(AppSettings settings)
        {   
            string standardXamlPath = "./View/ProgrammerWindow.xaml";

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgrammerWindow window = new ProgrammerWindow();
                if (window.DataContext is ProgrammerViewModel viewModel)
                {
                    viewModel.UiViewModel.SelectedFromBaseItem = settings.FromBase;
                    viewModel.UiViewModel.SelectedToBaseItem = settings.ToBase;
                }
                window.Show();
            });
        }

        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window activeWindow = sender as Window;
            if (activeWindow != null)
            {
                SaveWindowSettings(activeWindow);
            }
        }

        public void SaveWindowSettings(Window activeWindow)
        {
            if (activeWindow.GetType().Name == "StandardWindow")
            {
                bool isDigitGroupingChecked = (activeWindow as StandardWindow)?.CheckBoxDigitGrouping?.IsChecked ?? false;

                Settings = new AppSettings
                {
                    IsStandardMode = true,
                    IsDigitGroupingActive = isDigitGroupingChecked,
                    FromBase = "",
                    ToBase = ""
                };
                
            }
            else
            {
                if (activeWindow.DataContext is ProgrammerViewModel viewModel)
                {
                    var fromSelectedBase = viewModel.UiViewModel.SelectedFromBaseItem;
                    var toSelectedBase = viewModel.UiViewModel.SelectedToBaseItem;
                    Settings = new AppSettings
                    {
                        IsStandardMode = false,
                        IsDigitGroupingActive = false,
                        FromBase = fromSelectedBase.ToString(),
                        ToBase = toSelectedBase.ToString()
                    };
                }
            }
            JsonHelperService.SaveSettings(Settings);  
        }
    }
}
