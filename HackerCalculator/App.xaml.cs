using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

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
            Settings = JsonHelper.LoadSettings();

            if (Settings.IsStandardMode)
                LoadStandardMode(Settings);
            else
                LoadProgrammerMode(Settings);
        }

        private String GetProjectRootDirectory()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            DirectoryInfo parentDir = directory.Parent;

            if (parentDir != null && (parentDir.Name.Equals("Debug", StringComparison.OrdinalIgnoreCase)
                || parentDir.Name.Equals("Release", StringComparison.OrdinalIgnoreCase)))
            {
                parentDir = parentDir.Parent;
            }

            if (parentDir != null && parentDir.Name.Equals("bin", StringComparison.OrdinalIgnoreCase))
            {
                parentDir = parentDir.Parent;
            }

            return parentDir?.FullName;
        }

        private void LoadStandardMode(AppSettings settings)
        {
            string projectDir = GetProjectRootDirectory();
            string standardXamlPath = Path.Combine(projectDir, "StandardWindow.xaml");

            Application.Current.Dispatcher.Invoke(() =>
            {
                HackerCalculator.StandardWindow window = new HackerCalculator.StandardWindow();
                window.CheckBoxDigitGrouping.IsChecked = settings.IsDigitGroupingActive;
                window.Show();
            });
        }

        private void LoadProgrammerMode(AppSettings settings)
        {
            string projectDir = GetProjectRootDirectory();
            string standardXamlPath = Path.Combine(projectDir, "ProgrammerWindow.xaml");

            Application.Current.Dispatcher.Invoke(() =>
            {
                HackerCalculator.ProgrammerWindow window = new HackerCalculator.ProgrammerWindow();
                if (window.DataContext is ButtonsProgrammerViewModel viewModel)
                {
                    viewModel.SelectedFromBaseItem = settings.FromBase;
                    viewModel.SelectedToBaseItem = settings.ToBase;
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
                if (activeWindow.DataContext is ButtonsProgrammerViewModel viewModel)
                {
                    var fromSelectedBase = viewModel.SelectedFromBaseItem;
                    var toSelectedBase = viewModel.SelectedToBaseItem;
                    Settings = new AppSettings
                    {
                        IsStandardMode = false,
                        IsDigitGroupingActive = false,
                        FromBase = fromSelectedBase.ToString(),
                        ToBase = toSelectedBase.ToString()
                    };
                }
            }
            JsonHelper.SaveSettings(Settings);
            
        }
    }
}
