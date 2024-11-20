using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace NetworkAdaptersApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnGetAdaptersClick(object sender, RoutedEventArgs e)
        {
            AdaptersListBox.Items.Clear(); // ล้างรายการใน ListBox ก่อนโหลดใหม่

            try
            {
                // ดึงข้อมูล Network Adapters ทั้งหมด
                var adapters = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var adapter in adapters)
                {
                    // แสดงชื่อ Adapter และสถานะ
                    string status = adapter.OperationalStatus == OperationalStatus.Up ? "Up" : "Down";
                    AdaptersListBox.Items.Add($"{adapter.Name} - {adapter.Description} ({status})");

                    // แสดง MAC Address
                    string macAddress = string.Join(":", adapter.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
                    AdaptersListBox.Items.Add($"  MAC Address: {macAddress}");

                    // แสดง IP Address (IPv4/IPv6)
                    var ipProperties = adapter.GetIPProperties();
                    foreach (var ip in ipProperties.UnicastAddresses)
                    {
                        AdaptersListBox.Items.Add($"  IP Address: {ip.Address}");
                    }

                    AdaptersListBox.Items.Add(""); // แยกข้อมูลแต่ละ Adapter
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
