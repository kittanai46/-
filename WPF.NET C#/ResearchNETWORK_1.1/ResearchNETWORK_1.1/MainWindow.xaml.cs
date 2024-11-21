using System;
using System.Collections.Generic;
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

        // Data Model สำหรับแสดงข้อมูลใน ListView
        public class NetworkAdapter
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string MacAddress { get; set; }
            public string IpAddress { get; set; }
        }

        private void OnGetAdaptersClick(object sender, RoutedEventArgs e)
        {
            AdaptersListView.Items.Clear(); // ล้างข้อมูลใน ListView ก่อนโหลดใหม่

            try
            {
                // ดึงข้อมูล Network Adapters ทั้งหมด
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                var adapterList = new List<NetworkAdapter>();

                foreach (var adapter in adapters)
                {
                    string status = adapter.OperationalStatus == OperationalStatus.Up ? "Up" : "Down";
                    string macAddress = string.Join(":", adapter.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
                    string ipAddress = string.Join(", ",
                        adapter.GetIPProperties().UnicastAddresses.Select(ip => ip.Address.ToString()));

                    adapterList.Add(new NetworkAdapter
                    {
                        Name = adapter.Name,
                        Description = adapter.Description,
                        Status = status,
                        MacAddress = macAddress,
                        IpAddress = ipAddress
                    });
                }

                // เพิ่มข้อมูลใน ListView
                AdaptersListView.ItemsSource = adapterList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
