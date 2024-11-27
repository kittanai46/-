using System;
using System.Collections.Generic;
using System.Management;
using System.Windows;

namespace WpfStaticIPApp
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, ManagementObject> networkAdapters = new Dictionary<string, ManagementObject>();
        private string originalIPAddress;
        private string originalSubnetMask;
        private string originalGateway;

        public MainWindow()
        {
            InitializeComponent();
            LoadNetworkAdapters();
        }

        // โหลด Network Adapter
        private void LoadNetworkAdapters()
        {
            try
            {
                string query = "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string description = queryObj["Description"].ToString();
                    networkAdapters[description] = queryObj;
                    NetworkAdapterComboBox.Items.Add(description);
                }

                if (NetworkAdapterComboBox.Items.Count > 0)
                {
                    NetworkAdapterComboBox.SelectedIndex = 0;
                }
                else
                {
                    LblStatus.Content = "No active network adapters found.";
                    LblStatus.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                LblStatus.Content = $"Error loading adapters: {ex.Message}";
                LblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // เมื่อเลือก Network Adapter ใหม่
        private void NetworkAdapterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string selectedAdapter = NetworkAdapterComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedAdapter) || !networkAdapters.ContainsKey(selectedAdapter))
                {
                    LblStatus.Content = "Please select a valid network adapter.";
                    LblStatus.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                ManagementObject adapter = networkAdapters[selectedAdapter];
                LoadCurrentAdapterSettings(adapter);
            }
            catch (Exception ex)
            {
                LblStatus.Content = $"Error loading adapter details: {ex.Message}";
                LblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // โหลดค่าปัจจุบัน
        private void LoadCurrentAdapterSettings(ManagementObject adapter)
        {
            try
            {
                adapter.Get();

                string[] ipAddresses = (string[])adapter["IPAddress"];
                string[] subnetMasks = (string[])adapter["IPSubnet"];
                string[] gateways = (string[])adapter["DefaultIPGateway"];

                TxtIPAddress.Text = ipAddresses != null && ipAddresses.Length > 0 ? ipAddresses[0] : "";
                TxtSubnetMask.Text = subnetMasks != null && subnetMasks.Length > 0 ? subnetMasks[0] : "";
                TxtGateway.Text = gateways != null && gateways.Length > 0 ? gateways[0] : "";

                TxtCurrentIPAddress.Text = ipAddresses != null && ipAddresses.Length > 0 ? ipAddresses[0] : "";
                TxtCurrentSubnetMask.Text = subnetMasks != null && subnetMasks.Length > 0 ? subnetMasks[0] : "";
                TxtCurrentGateway.Text = gateways != null && gateways.Length > 0 ? gateways[0] : "";

                LblStatus.Content = "Adapter settings updated.";
                LblStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                LblStatus.Content = $"Error updating settings: {ex.Message}";
                LblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // ปุ่ม Save Original Settings
        private void SaveOriginalSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                originalIPAddress = TxtIPAddress.Text;
                originalSubnetMask = TxtSubnetMask.Text;
                originalGateway = TxtGateway.Text;

                LblStatus.Content = "Original settings saved.";
                LblStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                LblStatus.Content = $"Error saving settings: {ex.Message}";
                LblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // ปุ่ม Set IP ตามค่าปัจจุบัน
        private void SetCurrentIP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedAdapter = NetworkAdapterComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedAdapter) || !networkAdapters.ContainsKey(selectedAdapter))
                {
                    LblStatus.Content = "Please select a valid network adapter.";
                    LblStatus.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                ManagementObject adapter = networkAdapters[selectedAdapter];

                string ipAddress = TxtIPAddress.Text.Trim();
                string subnetMask = TxtSubnetMask.Text.Trim();
                string gateway = TxtGateway.Text.Trim();

                if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(subnetMask))
                {
                    LblStatus.Content = "Please provide both IP Address and Subnet Mask.";
                    LblStatus.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                ManagementBaseObject newIP = adapter.GetMethodParameters("EnableStatic");
                newIP["IPAddress"] = new string[] { ipAddress };
                newIP["SubnetMask"] = new string[] { subnetMask };
                adapter.InvokeMethod("EnableStatic", newIP, null);

                if (!string.IsNullOrEmpty(gateway))
                {
                    ManagementBaseObject newGateway = adapter.GetMethodParameters("SetGateways");
                    newGateway["DefaultIPGateway"] = new string[] { gateway };
                    newGateway["GatewayCostMetric"] = new int[] { 1 };
                    adapter.InvokeMethod("SetGateways", newGateway, null);
                }

                LblStatus.Content = "Current IP settings applied.";
                LblStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                LblStatus.Content = $"Error setting IP: {ex.Message}";
                LblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // ปุ่มคืนค่าเดิม + เปิด DHCP
        private void RestoreSettingsAndEnableDHCP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedAdapter = NetworkAdapterComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedAdapter) || !networkAdapters.ContainsKey(selectedAdapter))
                {
                    LblStatus.Content = "Please select a valid network adapter.";
                    LblStatus.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                if (string.IsNullOrEmpty(originalIPAddress) || string.IsNullOrEmpty(originalSubnetMask) || string.IsNullOrEmpty(originalGateway))
                {
                    LblStatus.Content = "No saved settings found. Please save settings first.";
                    LblStatus.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                ManagementObject adapter = networkAdapters[selectedAdapter];

                ManagementBaseObject newIP = adapter.GetMethodParameters("EnableStatic");
                newIP["IPAddress"] = new string[] { originalIPAddress };
                newIP["SubnetMask"] = new string[] { originalSubnetMask };
                adapter.InvokeMethod("EnableStatic", newIP, null);

                ManagementBaseObject newGateway = adapter.GetMethodParameters("SetGateways");
                newGateway["DefaultIPGateway"] = new string[] { originalGateway };
                newGateway["GatewayCostMetric"] = new int[] { 1 };
                adapter.InvokeMethod("SetGateways", newGateway, null);

                System.Threading.Thread.Sleep(2000);

                ManagementBaseObject enableDHCP = adapter.InvokeMethod("EnableDHCP", null, null);
                if (enableDHCP != null && (uint)enableDHCP["ReturnValue"] != 0)
                {
                    throw new Exception($"Failed to enable DHCP. Error code: {(uint)enableDHCP["ReturnValue"]}");
                }

                System.Threading.Thread.Sleep(5000);

                LoadCurrentAdapterSettings(adapter);

                LblStatus.Content = "Restored settings and DHCP enabled.";
                LblStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                LblStatus.Content = $"Error: {ex.Message}";
                LblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}
