using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkAdaptersApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadNetworkAdapters();
        }

        private void LoadNetworkAdapters()
        {
            // ดึงรายการ Network Adapters
            var adapters = NetworkInterface.GetAllNetworkInterfaces()
                                           .Where(adapter => adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                                             adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                           .ToList();

            // เพิ่มกล่องสำหรับแต่ละ Network Adapter
            foreach (var adapter in adapters)
            {
                // สร้าง Border สำหรับแสดงข้อมูล
                var adapterBox = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Background = Brushes.White,
                    Padding = new Thickness(10),
                    Margin = new Thickness(5),
                    Width = 250,
                    Height = 100
                };

                // สร้าง StackPanel สำหรับใส่ข้อความ
                var stackPanel = new StackPanel();

                // แสดงชื่อ Adapter
                stackPanel.Children.Add(new TextBlock
                {
                    Text = adapter.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 5)
                });

                // แสดงคำอธิบาย
                stackPanel.Children.Add(new TextBlock
                {
                    Text = adapter.Description,
                    FontSize = 12,
                    Margin = new Thickness(0, 0, 0, 5)
                });

                // แสดงสถานะ
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Status: {(adapter.OperationalStatus == OperationalStatus.Up ? "Connected" : "Disconnected")}",
                    FontSize = 12,
                    Foreground = adapter.OperationalStatus == OperationalStatus.Up ? Brushes.Green : Brushes.Red
                });

                // เพิ่ม StackPanel ลงใน Border
                adapterBox.Child = stackPanel;

                // เพิ่ม Border ลงใน WrapPanel
                AdapterPanel.Children.Add(adapterBox);
            }
        }
    }
}
