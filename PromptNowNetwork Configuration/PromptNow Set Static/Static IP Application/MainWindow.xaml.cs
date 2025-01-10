using PromptNow.NetworkAdapter;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfStaticIPApp.Services; // Import DateTimeService

namespace WpfStaticIPApp
{
    public partial class MainWindow : Window
    {
        // ใช้ NetworkAdapterManager Instance
        public NetworkAdapterManager NetworkAdapterManager { get; private set; }

        // เพิ่ม DateTimeService Instance
        private DateTimeService dateTimeService;

        public MainWindow()
        {
            InitializeComponent();

            // สร้าง Instance
            NetworkAdapterManager = NetworkAdapterManager.GetInstance();
            dateTimeService = new DateTimeService();

            LoadNetworkAdapters(); // โหลด Network Adapters
            SetCurrentDate();      // แสดงวันที่และเวลา
        }

        private void LoadNetworkAdapters()
        {
            // ดึงข้อมูล Network Adapters
            if (!NetworkAdapterManager.GetInstance().GetNetworkAdaptersWithDescriptions(out List<string> adapterDetails, out string error))
            {
                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // ลบปุ่มเดิมใน AdapterGrid ก่อนเพิ่มใหม่
            AdapterGrid.Items.Clear();

            // เพิ่มปุ่มใหม่สำหรับแต่ละ Network Adapter
            foreach (var adapterDetail in adapterDetails)
            {
                // แยก connectionID และ description
                string[] details = adapterDetail.Split(new[] { " : " }, StringSplitOptions.None);
                string connectionID = details[0]; // ดึง connectionID
                string description = details.Length > 1 ? details[1] : string.Empty; // ดึง description (ถ้ามี)

                // สร้างปุ่มใหม่ที่จะแสดงเฉพาะ connectionID
                Button adapterButton = new Button
                {
                    Content = new TextBlock
                    {
                        Text = connectionID, // แสดงเฉพาะ connectionID
                        FontSize = 40,                       
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                    Background = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/Select.png")), // ใช้ Select.png เป็นพื้นหลัง
                        Stretch = Stretch.UniformToFill
                    },
                    Margin = new Thickness(10),
                    BorderThickness = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Width = 360,
                    Height = 266
                };

                // เพิ่ม Event Handler ให้กับปุ่ม
                adapterButton.Click += (s, e) =>
                {
                    
                    AdapterDetailWindow detailWindow = new AdapterDetailWindow(connectionID, description);
                    detailWindow.Show();
                    this.Close();
                };

                // เพิ่มปุ่มลงใน ItemsControl
                AdapterGrid.Items.Add(adapterButton);
            }
        }


        private void SetCurrentDate()
        {
            // ใช้ DateTimeService แสดงเวลาและวันที่
            try
            {
                dateTimeService.StartDateTimeUpdate(DateText, TimeText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting date and time: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // ตัวอย่างการใช้งานปุ่ม Back
            MessageBox.Show("Back button clicked.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
