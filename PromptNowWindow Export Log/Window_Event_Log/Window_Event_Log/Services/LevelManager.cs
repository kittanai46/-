using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfStaticIPApp.Services
{
    public class LevelManager
    {
        private readonly List<string> selectedLevels; // List of selected levels
        private readonly string defaultPath = "PLEASE SELECT YOUR PATH"; // Default path

        public LevelManager()
        {
            selectedLevels = new List<string>();
        }

        /// <summary>
        /// Add or remove a level from the selected list.
        /// </summary>
        /// <param name="level">Level name</param>
        /// <param name="isSelected">True to add, False to remove</param>
        public void UpdateLevelSelection(string level, bool isSelected)
        {
            if (isSelected && !selectedLevels.Contains(level))
            {
                selectedLevels.Add(level);
            }
            else if (!isSelected && selectedLevels.Contains(level))
            {
                selectedLevels.Remove(level);
            }
        }

        /// <summary>
        /// Save the selected levels.
        /// </summary>
        public void SaveLevels()
        {
            if (selectedLevels.Count == 0)
            {
                throw new Exception("No levels selected to save.");
            }
        }

        /// <summary>
        /// Retrieve the list of selected levels.
        /// </summary>
        /// <returns>List of selected levels</returns>
        public List<string> GetSelectedLevels()
        {
            return new List<string>(selectedLevels);
        }

        /// <summary>
        /// Get the default export path.
        /// </summary>
        /// <returns>Default path as a string</returns>
        public string GetDefaultPath()
        {
            return $"EXPORT PATH: {defaultPath}";
        }

        /// <summary>
        /// Open a folder browser dialog to select an export path.
        /// </summary>
        /// <param name="currentPath">Current export path</param>
        /// <returns>Selected path</returns>
        public string BrowsePath(string currentPath)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select the folder to save the logs:";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return $"Export Path: {dialog.SelectedPath}";
                }
            }
            return currentPath;
        }

        /// <summary>
        /// Validate the provided export path.
        /// </summary>
        /// <param name="pathText">Path text</param>
        /// <returns>Valid path or null</returns>
        public string ValidatePath(string pathText)
        {
            if (string.IsNullOrWhiteSpace(pathText))
            {
                return null;
            }

            string outputPath = pathText.Replace("Export Path: ", "").Trim();
            if (string.IsNullOrWhiteSpace(outputPath) || !Directory.Exists(outputPath))
            {
                return null;
            }

            return outputPath;
        }

        /// <summary>
        /// Export the log file based on the selected log type and levels in CSV format.
        /// </summary>
        /// <param name="logType">Log type</param>
        /// <param name="outputPath">Export path</param>
        /// <param name="statusTextBlock">TextBlock for status updates</param>
        public void ExportLog(string logType, string outputPath, TextBlock statusTextBlock)
        {
            try
            {
                if (selectedLevels.Count == 0)
                {
                    UpdateStatus(statusTextBlock, "ERROR", "No levels selected");
                    return;
                }

                // ตรวจสอบว่ามี Log Type นี้ในระบบหรือไม่
                if (!EventLog.Exists(logType))
                {
                    // สร้างไฟล์เปล่าถ้า Log Type ไม่มีอยู่
                    string emptyFilePath = Path.Combine(outputPath, $"{logType}_NoLogs_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                    using (var writer = new StreamWriter(emptyFilePath))
                    {
                        writer.WriteLine("Timestamp,Level,Source,Message"); // Header เปล่า
                    }

                    UpdateStatus(statusTextBlock, "SUCCESS", $"Created empty file for '{logType}': {emptyFilePath}");
                    return;
                }

                // ถ้ามี Log Type ให้ Export Log
                string fileName = $"{logType}_ExportedLog_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string fullFilePath = Path.Combine(outputPath, fileName);

                using (var writer = new StreamWriter(fullFilePath))
                {
                    writer.WriteLine("Timestamp,Level,Source,Message"); // Header
                    EventLog eventLog = new EventLog(logType);

                    foreach (EventLogEntry entry in eventLog.Entries)
                    {
                        if (selectedLevels.Contains(entry.EntryType.ToString()))
                        {
                            string logMessage = entry.Message.Replace(",", ";"); // Handle commas in message
                            writer.WriteLine($"{entry.TimeGenerated.ToString("o", CultureInfo.InvariantCulture)},{entry.EntryType},{entry.Source},{logMessage}");
                        }
                    }
                }

                UpdateStatus(statusTextBlock, "SUCCESS", fullFilePath);
            }
            catch (Exception ex)
            {
                UpdateStatus(statusTextBlock, "ERROR", ex.Message);
            }
        }

        /// <summary>
        /// Update the status displayed in the UI.
        /// </summary>
        /// <param name="statusTextBlock">TextBlock to display the status</param>
        /// <param name="status">Status message</param>
        /// <param name="additionalInfo">Additional information (e.g., file path)</param>
        public void UpdateStatus(TextBlock statusTextBlock, string status, string additionalInfo = "")
        {
            switch (status.ToUpper())
            {
                case "SUCCESS":
                    statusTextBlock.Text = $"SUCCESS: {additionalInfo}";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                    break;

                case "ERROR":
                    statusTextBlock.Text = $"FAILED: {additionalInfo}";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                    break;

                case "PENDING":
                    statusTextBlock.Text = "IN PROGRESS...";
                    statusTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 136, 0));
                    break;

                default:
                    statusTextBlock.Text = status.ToUpper();
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.White);
                    break;
            }
        }

        /// <summary>
        /// Toggle the selection for event levels (for UI interaction with images).
        /// </summary>
        /// <param name="level">The event level</param>
        /// <param name="border">The Border UI element to toggle its color</param>
        public void ToggleEventSelection(string level, Border border)
        {
            bool isSelected = selectedLevels.Contains(level);

            if (isSelected)
            {
                selectedLevels.Remove(level);
                border.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                selectedLevels.Add(level);
                border.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }
    }
}
