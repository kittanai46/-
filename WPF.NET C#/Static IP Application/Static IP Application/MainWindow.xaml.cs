using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PromptNow.NetworkAdapter;

namespace WpfStaticIPApp
{
    public partial class MainWindow : Window
    {
        // ใช้ NetworkAdapterManager Instance
        public NetworkAdapterManager NetworkAdapterManager { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // กำหนดค่า NetworkAdapterManager
            NetworkAdapterManager = PromptNow.NetworkAdapter.NetworkAdapterManager.GetInstance();

            // โหลด Network Adapters
            LoadNetworkAdapters();
        }

        // ฟังก์ชันเพื่อโหลด network adapters จาก NetworkAdapterManager
        private void LoadNetworkAdapters()
        {
            try
            {
                string error;
                List<string> adapters = new List<string>();

                // เรียก GetNetworkAdapters และตรวจสอบผลลัพธ์
                bool result = NetworkAdapterManager.GetNetworkAdapters(out adapters, out error);

                // Clear any existing items in the ComboBox
                NetworkAdapterComboBox.Items.Clear();

                if (result && adapters.Count > 0)
                {
                    foreach (var adapterName in adapters)
                    {
                        NetworkAdapterComboBox.Items.Add(adapterName);
                    }

                    NetworkAdapterComboBox.SelectedIndex = 0; // เลือกตัวแรกเป็นค่าเริ่มต้น
                }
                else
                {
                    LblStatus.Content = string.IsNullOrEmpty(error) ? "No network adapters found." : "Error: " + error;
                }
            }
            catch (Exception ex)
            {
                LblStatus.Content = "Error loading network adapters: " + ex.Message;
            }
        }

        // ฟังก์ชันเพื่อโหลดข้อมูลปัจจุบันของ Adapter ที่เลือก
        private void NetworkAdapterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedAdapter = NetworkAdapterComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedAdapter))
            {
                LblStatus.Content = "Please select a network adapter.";
            }
            else
            {
                string ipAddress, subnetMask, gateway, preferredDNS, alternateDNS, error;

                // ใช้ฟังก์ชันจาก NetworkAdapterManager
                bool settingsLoaded = NetworkAdapterManager.GetAdapterProfile(
                    selectedAdapter, out ipAddress, out subnetMask, out gateway, out preferredDNS, out alternateDNS, out error);

                if (settingsLoaded)
                {
                    TxtIPAddress.Text = ipAddress;
                    TxtSubnetMask.Text = subnetMask;
                    TxtGateway.Text = gateway;
                    PerferredDNSServer.Text = preferredDNS;
                    AlternateDNSserver.Text = alternateDNS;

                    LblStatus.Content = $"Current settings loaded for adapter: {selectedAdapter}";
                }
                else
                {
                    LblStatus.Content = "Error loading settings: " + error;
                }
            }
        }

        // ฟังก์ชันการตั้งค่า Static IP และ DNS
        private void SetStaticIP_Click(object sender, RoutedEventArgs e)
        {
            string selectedAdapter = NetworkAdapterComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedAdapter))
            {
                LblStatus.Content = "Please select a network adapter.";
                return;
            }

            string ipAddress = TxtIPAddress.Text;
            string subnetMask = TxtSubnetMask.Text;
            string gateway = TxtGateway.Text;
            string preferredDNS = PerferredDNSServer.Text;
            string alternateDNS = AlternateDNSserver.Text;

            SetStaticIPAndDNS(selectedAdapter, ipAddress, subnetMask, gateway, preferredDNS, alternateDNS);
        }

        private void SetStaticIPAndDNS(string adapterName, string ipAddress, string subnetMask, string gateway, string preferredDNS, string alternateDNS)
        {
            try
            {
                string error;

                // ตั้งค่า IP และ Subnet Mask
                bool result = NetworkAdapterManager.SetIPAddress(adapterName, ipAddress, subnetMask, out error);
                if (!result)
                {
                    LblStatus.Content = $"Error setting IP and Subnet Mask: {error}";
                    return;
                }

                // ตั้งค่า Gateway
                result = NetworkAdapterManager.SetGateway(adapterName, gateway, out error);
                if (!result)
                {
                    LblStatus.Content = $"Error setting Gateway: {error}";
                    return;
                }

                // ตั้งค่า DNS
                result = NetworkAdapterManager.SetDNS(adapterName, preferredDNS, alternateDNS, out error);
                if (!result)
                {
                    LblStatus.Content = $"Error setting DNS: {error}";
                    return;
                }

                LblStatus.Content = $"Static IP, Subnet Mask, Gateway, and DNS settings applied for {adapterName}.";
            }
            catch (Exception ex)
            {
                LblStatus.Content = "Error applying settings: " + ex.Message;
            }
        }

        // ฟังก์ชันเปิดใช้งาน DHCP
        private void EnableDHCP_Click(object sender, RoutedEventArgs e)
        {
            if (NetworkAdapterComboBox.SelectedIndex != -1)
            {
                string selectedAdapter = NetworkAdapterComboBox.SelectedItem.ToString();
                string error;

                if (NetworkAdapterManager.SetDHCP(selectedAdapter, out error))
                {
                    string ipAddress, subnetMask, gateway, preferredDNS, alternateDNS;
                    if (NetworkAdapterManager.GetAdapterProfile(selectedAdapter, out ipAddress, out subnetMask, out gateway, out preferredDNS, out alternateDNS, out error))
                    {
                        TxtIPAddress.Text = ipAddress;
                        TxtSubnetMask.Text = subnetMask;
                        TxtGateway.Text = gateway;
                        PerferredDNSServer.Text = preferredDNS;
                        AlternateDNSserver.Text = alternateDNS;
                    }

                    LblStatus.Content = "DHCP has been enabled.";
                    LblStatus.Foreground = Brushes.Green;
                }
                else
                {
                    LblStatus.Content = "Error enabling DHCP: " + error;
                    LblStatus.Foreground = Brushes.Red;
                }
            }
            else
            {
                LblStatus.Content = "Please select a network adapter.";
                LblStatus.Foreground = Brushes.Red;
            }
        }

        // ปุ่ม Clear ข้อมูล
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            TxtIPAddress.Clear();
            TxtSubnetMask.Clear();
            TxtGateway.Clear();
            PerferredDNSServer.Clear();
            AlternateDNSserver.Clear();
            LblStatus.Content = "All fields have been cleared.";
        }

        // ปุ่ม Refresh
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (NetworkAdapterComboBox.SelectedIndex != -1)
            {
                string selectedAdapter = NetworkAdapterComboBox.SelectedItem.ToString();
                string ipAddress, subnetMask, gateway, preferredDNS, alternateDNS, error;

                if (NetworkAdapterManager.GetAdapterProfile(selectedAdapter, out ipAddress, out subnetMask, out gateway, out preferredDNS, out alternateDNS, out error))
                {
                    TxtIPAddress.Text = ipAddress;
                    TxtSubnetMask.Text = subnetMask;
                    TxtGateway.Text = gateway;
                    PerferredDNSServer.Text = preferredDNS;
                    AlternateDNSserver.Text = alternateDNS;

                    LblStatus.Content = "Refresh succeeded.";
                    LblStatus.Foreground = Brushes.Green;
                }
                else
                {
                    LblStatus.Content = "Error refreshing settings: " + error;
                    LblStatus.Foreground = Brushes.Red;
                }
            }
            else
            {
                LblStatus.Content = "Please select a network adapter.";
                LblStatus.Foreground = Brushes.Red;
            }
        }
    }
}
