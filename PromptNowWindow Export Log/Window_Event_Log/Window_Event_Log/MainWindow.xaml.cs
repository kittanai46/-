using System;
using System.Windows;
using WpfStaticIPApp.Services;

namespace WpfStaticIPApp
{
    public partial class MainWindow : Window
    {
        private readonly DateTimeService dateTimeService;

        public MainWindow()
        {
            InitializeComponent();

            // สร้าง Instance ของ DateTimeService
            dateTimeService = new DateTimeService();

            // ตั้งค่าเวลาและวันที่
            SetCurrentDate();
        }

        private void SetCurrentDate()
        {
            try
            {
                dateTimeService.StartDateTimeUpdate(DateText, TimeText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting date and time: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavigateToLogLevelWindow(string logType)
        {
            var logLevelWindow = new LogLevelWindow(logType);
            logLevelWindow.Show();
            this.Close();
        }

        private void ApplicationLog_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogLevelWindow("Application");
        }

        private void SystemLog_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogLevelWindow("System");
        }

        private void SecurityLog_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogLevelWindow("Security");
        }

        private void SetupLog_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogLevelWindow("Setup");
        }

        private void ForwardedEventsLog_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogLevelWindow("Forwarded Events");
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // ตัวอย่างฟังก์ชันปุ่มย้อนกลับ
            MessageBox.Show("Back button clicked!");
        }
    }
}
