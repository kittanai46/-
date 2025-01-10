using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfStaticIPApp.Services;

namespace WpfStaticIPApp
{
    public partial class LogLevelWindow : Window
    {
        private readonly LevelManager levelManager;
        private readonly string logType;
        private readonly DateTimeService dateTimeService;

        public LogLevelWindow(string logType)
        {
            InitializeComponent();
            this.logType = logType;
            levelManager = new LevelManager();

            // Display the selected log type
            LogTypeText.Text = $"Selected Log: {logType}";

            // Set the default path
            PathTextBlock.Text = levelManager.GetDefaultPath();

            // Initialize DateTimeService
            dateTimeService = new DateTimeService();

            // Set the current date and time
            SetCurrentDate();

            // Set the initial status to PENDING
            levelManager.UpdateStatus(StatusTextBlock, "PENDING");
        }

        private void SetCurrentDate()
        {
            try
            {
                dateTimeService.StartDateTimeUpdate(DateText, TimeText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting date and time: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToggleEventLevel(string level, Border border)
        {
            levelManager.ToggleEventSelection(level, border);
            levelManager.UpdateStatus(StatusTextBlock, "PENDING");
        }

        private void CriticalCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLevel("Critical", CriticalBorder);
        }

        private void ErrorCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLevel("Error", ErrorBorder);
        }

        private void WarningCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLevel("Warning", WarningBorder);
        }

        private void InformationCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLevel("Information", InformationBorder);
        }

        private void VerboseCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLevel("Verbose", VerboseBorder);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update status to PENDING
            levelManager.UpdateStatus(StatusTextBlock, "PENDING");

            try
            {
                // Call SaveLevels in LevelManager
                levelManager.SaveLevels();

                // Update status to SUCCESS
                levelManager.UpdateStatus(StatusTextBlock, "SUCCESS", "Levels Saved Successfully");
            }
            catch (Exception ex)
            {
                // Update status to ERROR
                levelManager.UpdateStatus(StatusTextBlock, "ERROR", ex.Message);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = levelManager.ValidatePath(PathTextBlock.Text);
            if (outputPath != null)
            {
                levelManager.ExportLog(logType, outputPath, StatusTextBlock);
            }
            else
            {
                levelManager.UpdateStatus(StatusTextBlock, "ERROR", "Path not set");
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Open FolderBrowserDialog and update the path
            PathTextBlock.Text = levelManager.BrowsePath(PathTextBlock.Text);

            // Update status to PENDING
            levelManager.UpdateStatus(StatusTextBlock, "PENDING");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open MainWindow and close the current window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
