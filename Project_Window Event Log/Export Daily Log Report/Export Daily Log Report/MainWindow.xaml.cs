using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace EventLogExporter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowLogs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string logType = ((ComboBoxItem)LogTypeComboBox.SelectedItem)?.Content.ToString();
                DateTime? startDate = StartDatePicker.SelectedDate;
                DateTime? endDate = EndDatePicker.SelectedDate;

                if (string.IsNullOrEmpty(logType))
                {
                    StatusTextBlock.Text = "Please select a log type.";
                    return;
                }

                if (startDate == null || endDate == null)
                {
                    StatusTextBlock.Text = "Please select both start and end dates.";
                    return;
                }

                if (startDate > endDate)
                {
                    StatusTextBlock.Text = "Start date cannot be after end date.";
                    return;
                }

                List<EventLogEntryType> selectedLevels = new List<EventLogEntryType>();
                if (CriticalCheckBox.IsChecked == true) selectedLevels.Add(EventLogEntryType.Error); // Critical ใช้ Error
                if (WarningCheckBox.IsChecked == true) selectedLevels.Add(EventLogEntryType.Warning);
                if (ErrorCheckBox.IsChecked == true) selectedLevels.Add(EventLogEntryType.Error);
                if (InformationCheckBox.IsChecked == true) selectedLevels.Add(EventLogEntryType.Information);

                EventLog eventLog = new EventLog(logType);
                var logList = new List<dynamic>();

                foreach (EventLogEntry entry in eventLog.Entries)
                {
                    if (entry.TimeGenerated >= startDate && entry.TimeGenerated <= endDate &&
                        selectedLevels.Contains(entry.EntryType))
                    {
                        logList.Add(new
                        {
                            Time = entry.TimeGenerated,
                            Type = entry.EntryType,
                            Source = entry.Source,
                            Message = entry.Message
                        });
                    }
                }

                LogDataGrid.ItemsSource = logList;
                StatusTextBlock.Text = $"Logs from {logType} displayed successfully.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private void ExportLogs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var logs = LogDataGrid.ItemsSource as List<dynamic>;
                if (logs == null || logs.Count == 0)
                {
                    StatusTextBlock.Text = "No logs to export.";
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|XML Files (*.xml)|*.xml",
                    DefaultExt = "csv"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        if (filePath.EndsWith(".csv"))
                        {
                            writer.WriteLine("Time,Type,Source,Message");
                            foreach (var log in logs)
                            {
                                writer.WriteLine($"{log.Time},{log.Type},{log.Source},{log.Message}");
                            }
                        }
                        else if (filePath.EndsWith(".txt"))
                        {
                            foreach (var log in logs)
                            {
                                writer.WriteLine($"Time: {log.Time}");
                                writer.WriteLine($"Type: {log.Type}");
                                writer.WriteLine($"Source: {log.Source}");
                                writer.WriteLine($"Message: {log.Message}");
                                writer.WriteLine("-------------------------");
                            }
                        }
                        else if (filePath.EndsWith(".xml"))
                        {
                            writer.WriteLine("<EventLogs>");
                            foreach (var log in logs)
                            {
                                writer.WriteLine("  <Event>");
                                writer.WriteLine($"    <Time>{log.Time}</Time>");
                                writer.WriteLine($"    <Type>{log.Type}</Type>");
                                writer.WriteLine($"    <Source>{log.Source}</Source>");
                                writer.WriteLine($"    <Message>{System.Security.SecurityElement.Escape(log.Message)}</Message>");
                                writer.WriteLine("  </Event>");
                            }
                            writer.WriteLine("</EventLogs>");
                        }
                    }

                    StatusTextBlock.Text = $"Logs exported successfully to {filePath}";
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error: {ex.Message}";
            }
        }
    }
}
