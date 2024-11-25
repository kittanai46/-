using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation; // สำหรับจัดการ Network Adapter
using System.Windows;
using System.Windows.Controls;

namespace StaticIPApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // โหลด UI จาก XAML
            LoadNetworkAdapters(); // โหลด Network Adapter ลงใน ComboBox
        }

        // ฟังก์ชันสำหรับโหลด Network Adapter
        private void LoadNetworkAdapters()
        {
            try
            {
                // ดึง Network Interface ทั้งหมด
                var adapters = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(a =>
                        (a.NetworkInterfaceType == NetworkInterfaceType.Ethernet || a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) // เฉพาะ Ethernet และ Wi-Fi
                        && a.OperationalStatus == OperationalStatus.Up) // เฉพาะที่เชื่อมต่อ
                    .GroupBy(a => a.Id) // กรองไม่ให้ซ้ำด้วย ID ของ Adapter
                    .Select(group =>
                    {
                        var adapter = group.First();

                        // กรณี Wi-Fi ให้แสดง SSID
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                        {
                            string ssid = GetWifiSsid();
                            return ssid != null ? $"Wi-Fi-[{ssid}]:Connected" : null;
                        }

                        // กรณี Ethernet
                        return $"Ethernet-[{adapter.Name}]:Connected";
                    })
                    .Where(a => a != null) // ตัดรายการ null
                    .ToList();

                AdapterComboBox.ItemsSource = adapters; // ตั้งค่ารายการใน ComboBox

                if (AdapterComboBox.Items.Count > 0)
                {
                    AdapterComboBox.SelectedIndex = 0; // เลือกรายการแรกโดยอัตโนมัติ
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading network adapters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // ฟังก์ชันดึง SSID ของ Wi-Fi
        private string GetWifiSsid()
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/C netsh wlan show interfaces",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // ดึง SSID จากผลลัพธ์
                var ssidLine = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                     .FirstOrDefault(line => line.TrimStart().StartsWith("SSID") && !line.Contains("BSSID"));
                if (!string.IsNullOrWhiteSpace(ssidLine))
                {
                    return ssidLine.Split(new[] { ':' }, 2)[1].Trim(); // ตัด SSID ออกมา
                }
            }
            catch
            {
                // ถ้าดึง SSID ไม่ได้ ให้ส่งค่า null
            }
            return null;
        }


        // ฟังก์ชันสำหรับแสดงข้อมูลของ Network Adapter
        private void AdapterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = AdapterComboBox.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedItem)) return;

            // ดึงชื่อ Adapter จาก ComboBox (แยกชื่อออกจากรูปแบบ)
            string adapterName = selectedItem.Split('-')[1].Split(':')[0].Trim('[').Trim(']');

            try
            {
                // ใช้ WMI ค้นหา Adapter Configuration
                var searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description LIKE '%{adapterName}%'");
                var adapter = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                if (adapter != null)
                {
                    // ดึงข้อมูล IP Address, Subnet Mask, Gateway, และ DNS
                    var ipAddresses = adapter["IPAddress"] as string[];
                    var subnetMasks = adapter["IPSubnet"] as string[];
                    var gateways = adapter["DefaultIPGateway"] as string[];
                    var dnsServers = adapter["DNSServerSearchOrder"] as string[];

                    // อัปเดต TextBox ด้วยข้อมูล
                    IpAddressTextBox.Text = ipAddresses?.FirstOrDefault() ?? "N/A";
                    SubnetMaskTextBox.Text = subnetMasks?.FirstOrDefault() ?? "N/A";
                    GatewayTextBox.Text = gateways?.FirstOrDefault() ?? "N/A";
                    DnsTextBox.Text = dnsServers?.FirstOrDefault() ?? "N/A";
                }
                else
                {
                    // เคลียร์ข้อมูลหากไม่พบ
                    IpAddressTextBox.Clear();
                    SubnetMaskTextBox.Clear();
                    GatewayTextBox.Clear();
                    DnsTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving adapter information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ฟังก์ชันสำหรับตั้งค่า Static IP
        private void SetStaticIp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedItem = AdapterComboBox.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedItem)) return;

                // ดึงชื่อ Adapter จาก ComboBox
                string adapterName = selectedItem.Split('-')[1].Split(':')[0].Trim('[').Trim(']');

                string ipAddress = IpAddressTextBox.Text; // ดึงค่า IP Address
                string subnetMask = SubnetMaskTextBox.Text; // ดึงค่า Subnet Mask
                string gateway = GatewayTextBox.Text; // ดึงค่า Gateway
                string dns = DnsTextBox.Text; // ดึงค่า DNS

                if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(subnetMask) ||
                    string.IsNullOrWhiteSpace(gateway) || string.IsNullOrWhiteSpace(dns))
                {
                    MessageBox.Show("Please fill in all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ใช้ WMI ค้นหา Adapter Configuration
                var searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description LIKE '%{adapterName}%' AND IPEnabled = TRUE");
                var adapter = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                if (adapter != null)
                {
                    // ตั้งค่า Static IP
                    adapter.InvokeMethod("EnableStatic", new object[] { new string[] { ipAddress }, new string[] { subnetMask } });
                    adapter.InvokeMethod("SetGateways", new object[] { new string[] { gateway }, new int[] { 1 } });
                    adapter.InvokeMethod("SetDNSServerSearchOrder", new object[] { new string[] { dns } });

                    MessageBox.Show("Static IP configured successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Could not find the selected network adapter.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error configuring static IP: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // ฟังก์ชันสำหรับตั้งค่า DHCP
        private void SetDhcp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedItem = AdapterComboBox.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedItem)) return;

                // ดึงชื่อ Adapter จาก ComboBox
                string adapterName = selectedItem.Split('-')[1].Split(':')[0].Trim('[').Trim(']');

                // ใช้ WMI ค้นหา Adapter Configuration
                var searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description LIKE '%{adapterName}%' AND IPEnabled = TRUE");
                var adapter = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                if (adapter != null)
                {
                    // เปิดใช้งาน DHCP
                    adapter.InvokeMethod("EnableDHCP", null);

                    MessageBox.Show("DHCP enabled successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Could not find the selected network adapter.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enabling DHCP: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // ฟังก์ชันสำหรับรีเฟรชรายการ Network Adapter
        private void RefreshNetworkAdapters_Click(object sender, RoutedEventArgs e)
        {
            LoadNetworkAdapters(); // โหลด Network Adapter ใหม่
        }
    }
}
