using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace StaticIPApp
{
    public partial class MainWindow : Window
    {
        // ตัวแปรเพื่อเปิด/ปิด Simulation Mode
        private bool IsSimulationMode = true; // เปลี่ยนเป็น false หากต้องการทดสอบจริง

        public MainWindow()
        {
            InitializeComponent();
            LoadNetworkAdapters();
        }

        // โหลด Network Adapters ลงใน ComboBox
        private void LoadNetworkAdapters()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces()
                                           .Where(a => a.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                                       a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                           .Select(a => a.Name);
            AdapterComboBox.ItemsSource = adapters;
        }

        // ตั้งค่า Static IP
        private void SetStaticIp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string adapterName = AdapterComboBox.SelectedItem?.ToString();
                string ipAddress = IpAddressTextBox.Text;
                string subnetMask = SubnetMaskTextBox.Text;
                string gateway = GatewayTextBox.Text;
                string dns = DnsTextBox.Text;

                if (string.IsNullOrWhiteSpace(adapterName) ||
                    string.IsNullOrWhiteSpace(ipAddress) ||
                    string.IsNullOrWhiteSpace(subnetMask) ||
                    string.IsNullOrWhiteSpace(gateway) ||
                    string.IsNullOrWhiteSpace(dns))
                {
                    MessageBox.Show("Please fill in all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ตรวจสอบว่าค่า IP ถูกต้องหรือไม่
                if (!IsValidIpAddress(ipAddress) || !IsValidIpAddress(subnetMask) ||
                    !IsValidIpAddress(gateway) || !IsValidIpAddress(dns))
                {
                    MessageBox.Show("One or more IP fields are invalid. Please check and try again.",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // แสดงคำสั่งจำลองใน Simulation Mode
                ExecuteCommand($"netsh interface ip set address name=\"{adapterName}\" static {ipAddress} {subnetMask} {gateway}");
                ExecuteCommand($"netsh interface ip set dns name=\"{adapterName}\" static {dns}");

                MessageBox.Show($"Static IP configuration simulated for {adapterName}.", "Simulation Mode",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ตั้งค่า DHCP
        private void SetDhcp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string adapterName = AdapterComboBox.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(adapterName))
                {
                    MessageBox.Show("Please select a network adapter!", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // แสดงคำสั่งจำลองใน Simulation Mode
                ExecuteCommand($"netsh interface ip set address name=\"{adapterName}\" source=dhcp");
                ExecuteCommand($"netsh interface ip set dns name=\"{adapterName}\" source=dhcp");

                MessageBox.Show($"DHCP simulation enabled for {adapterName}.", "Simulation Mode",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // รันคำสั่ง netsh ผ่าน Command Prompt หรือแสดงคำสั่งใน Simulation Mode
        private void ExecuteCommand(string command)
        {
            if (IsSimulationMode)
            {
                // โหมดจำลอง: แสดงคำสั่งที่ควรรัน
                MessageBox.Show($"Simulation Mode: {command}", "Command Simulation",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // โหมดจริง: รันคำสั่งจริง
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C {command}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Verb = "runas"
                    }
                };
                process.Start();
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show($"Command failed:\n{error}", "Command Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(output, "Command Output", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurred:\n{ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ตรวจสอบว่า IP Address ถูกต้องหรือไม่
        private bool IsValidIpAddress(string ipAddress)
        {
            return System.Net.IPAddress.TryParse(ipAddress, out _);
        }
    }
}
