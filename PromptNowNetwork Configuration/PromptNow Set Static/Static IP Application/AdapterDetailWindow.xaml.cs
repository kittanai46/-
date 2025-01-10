using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PromptNow.NetworkAdapter;
using WpfStaticIPApp.Dialogs;
using WpfStaticIPApp.Services;

namespace WpfStaticIPApp
{
    public partial class AdapterDetailWindow : Window
    {
        private DateTimeService dateTimeService;
        private NetworkAdapterManager networkAdapterManager;
        private string connectionID;
        private string description;
        private TextBox activeTextBox; // ตัวแปรเก็บ TextBox ที่ใช้งานอยู่

        public AdapterDetailWindow(string connectionID, string description)
        {
            InitializeComponent();

            this.connectionID = connectionID;
            this.description = description;

            // สร้าง Instance ของ NetworkAdapterManager
            networkAdapterManager = NetworkAdapterManager.GetInstance();

            // แสดงชื่อ Adapter ที่ด้านบน พร้อม Description
            AdapterNameText.Text = $"{connectionID} : {description}";

            // แสดงวันที่และเวลา
            dateTimeService = new DateTimeService();
            dateTimeService.StartDateTimeUpdate(DateText, TimeText);

            // โหลดข้อมูลของ Adapter
            LoadAdapterData();
        }

        private void LoadAdapterData()
        {
            try
            {
                string error;
                string ipAddress, subnetMask, gateway, preferredDNS, alternateDNS;

                bool result = networkAdapterManager.GetAdapterProfile(connectionID,
                    out ipAddress, out subnetMask, out gateway, out preferredDNS, out alternateDNS, out error);

                if (result)
                {
                    IPAddressTextBox.Text = ipAddress ?? "N/A";
                    SubnetMaskTextBox.Text = subnetMask ?? "N/A";
                    GatewayTextBox.Text = gateway ?? "N/A";
                    PreferredDNSTextBox.Text = preferredDNS ?? "N/A";
                    AlternateDNSTextBox.Text = alternateDNS ?? "N/A";

                    // เพิ่ม Log หรือ Console สำหรับตรวจสอบค่าที่ได้
                    Console.WriteLine($"Gateway Loaded: {gateway}");
                }
                else
                {
                    MessageBox.Show($"Error fetching data: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(IPAddressTextBox.Text) || !System.Net.IPAddress.TryParse(IPAddressTextBox.Text, out _))
            {
                UpdateStatus("Invalid IP Address. Please enter a valid IP address.", Colors.Red);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SubnetMaskTextBox.Text) || !System.Net.IPAddress.TryParse(SubnetMaskTextBox.Text, out _))
            {
                UpdateStatus("Invalid Subnet Mask. Please enter a valid subnet mask.", Colors.Red);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(GatewayTextBox.Text) && !System.Net.IPAddress.TryParse(GatewayTextBox.Text, out _))
            {
                UpdateStatus("Invalid Gateway. Please enter a valid gateway.", Colors.Red);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PreferredDNSTextBox.Text) && !System.Net.IPAddress.TryParse(PreferredDNSTextBox.Text, out _))
            {
                UpdateStatus("Invalid Preferred DNS. Please enter a valid DNS address.", Colors.Red);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(AlternateDNSTextBox.Text) && !System.Net.IPAddress.TryParse(AlternateDNSTextBox.Text, out _))
            {
                UpdateStatus("Invalid Alternate DNS. Please enter a valid DNS address.", Colors.Red);
                return false;
            }

            UpdateStatus("All inputs are valid.", Colors.Green);
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // ซ่อนหน้าต่างปัจจุบันก่อนดำเนินการ
            this.Hide();

            // ตรวจสอบข้อมูล
            if (!ValidateInput())
            {
                // ถ้าข้อมูลไม่ถูกต้อง แสดงสถานะและคืนหน้าต่าง
                this.Show();
                return;
            }

            UpdateStatus("Saving...", Colors.Yellow);

            // แสดงหน้าต่างยืนยัน
            SaveConfirmation confirmationDialog = new SaveConfirmation(
                adapterName: connectionID,
                ipAddress: IPAddressTextBox.Text,
                subnetMask: SubnetMaskTextBox.Text,
                gateway: GatewayTextBox.Text,
                preferredDNS: PreferredDNSTextBox.Text,
                alternateDNS: AlternateDNSTextBox.Text
            );

            bool? result = confirmationDialog.ShowDialog();

            if (result == true)
            {
                string error = string.Empty;

                try
                {
                    // บันทึก IP Address
                    bool successIP = networkAdapterManager.SetIPAddress(
                        connectionID,
                        IPAddressTextBox.Text,
                        SubnetMaskTextBox.Text,
                        GatewayTextBox.Text,
                        out error);

                    if (!successIP)
                    {
                        UpdateStatus($"Failed to set IP Address: {error}", Colors.Red);
                        this.Show();
                        return;
                    }

                    // บันทึก DNS
                    bool successDNS = networkAdapterManager.SetDNS(
                        connectionID,
                        PreferredDNSTextBox.Text,
                        AlternateDNSTextBox.Text,
                        out error);

                    if (!successDNS)
                    {
                        UpdateStatus($"Failed to set DNS: {error}", Colors.Red);
                        this.Show();
                        return;
                    }

                    // ถ้าบันทึกสำเร็จ
                    UpdateStatus("Network settings saved successfully", Colors.Green);
                    LoadAdapterData(); // โหลดข้อมูลใหม่
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error: {ex.Message}", Colors.Red);
                    this.Show();
                    return;
                }
            }
            else
            {
                UpdateStatus("Save operation cancelled", Colors.Red);
            }

            // แสดงหน้าต่างกลับมา
            this.Show();
        }

        private void DHCPButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var dhcpWindow = new DHCPConfirmation(connectionID);

            bool? result = dhcpWindow.ShowDialog();

            if (result == true)
            {
                LoadAdapterData();
                UpdateStatus("DHCP settings applied successfully", Colors.Green);
            }
            else
            {
                UpdateStatus("DHCP operation cancelled", Colors.Red);
            }
            this.Show();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("Refreshing...", Colors.Yellow);

            IPAddressTextBox.Text = string.Empty;
            SubnetMaskTextBox.Text = string.Empty;
            GatewayTextBox.Text = string.Empty;
            PreferredDNSTextBox.Text = string.Empty;
            AlternateDNSTextBox.Text = string.Empty;

            await Task.Delay(2000);

            LoadAdapterData();

            UpdateStatus("Refreshed successfully", Colors.Cyan);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            IPAddressTextBox.Clear();
            SubnetMaskTextBox.Clear();
            GatewayTextBox.Clear();
            PreferredDNSTextBox.Clear();
            AlternateDNSTextBox.Clear();

            UpdateStatus("Cleared successfully", Colors.Yellow);
        }

        private void UpdateStatus(string message, Color color)
        {
            if (StatusMessageRun != null)
            {
                StatusMessageRun.Text = message;
                StatusMessageRun.Foreground = new SolidColorBrush(color);
            }
        }

        // ฟังก์ชันเมื่อ TextBox ได้รับโฟกัส
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                activeTextBox = textBox; // เก็บ TextBox ที่ใช้งาน
                KeyboardGrid.Visibility = Visibility.Visible; // แสดงคีย์บอร์ด
                ButtonPanel.Visibility = Visibility.Collapsed; // ซ่อนปุ่มอื่น
            }
        }

        // ฟังก์ชันปิดคีย์บอร์ด
        private void CloseKeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            KeyboardGrid.Visibility = Visibility.Collapsed; // ซ่อนคีย์บอร์ด
            ButtonPanel.Visibility = Visibility.Visible; // แสดงปุ่มอื่น
            activeTextBox = null; // ล้าง TextBox ที่ใช้งาน
        }

        // ฟังก์ชันสำหรับปุ่ม Backspace
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null && activeTextBox.Text.Length > 0)
            {
                activeTextBox.Text = activeTextBox.Text.Substring(0, activeTextBox.Text.Length - 1); // ลบตัวสุดท้าย
            }
        }

        // ฟังก์ชันสำหรับปุ่มบนคีย์บอร์ด
        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && activeTextBox != null)
            {
                // ตรวจสอบว่าปุ่มมี Tag หรือไม่ และเพิ่มค่าใน TextBox
                if (button.Tag != null)
                {
                    string value = button.Tag.ToString();
                    activeTextBox.Text += value; // เพิ่มค่าจากปุ่มลงใน TextBox
                }
            }
        }

        // ฟังก์ชันสำหรับปุ่ม CLEAR
        private void KeyboardClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null)
            {
                activeTextBox.Text = string.Empty; // ล้างข้อความทั้งหมดใน TextBox ที่กำลังใช้งาน
            }
        }

        // ฟังก์ชันสำหรับปุ่ม REFRESH
        private void KeyboardRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null)
            {
                try
                {
                    string refreshedValue = GetRefreshedValue(activeTextBox.Name); // ดึงค่าที่รีเฟรช
                    activeTextBox.Text = refreshedValue;
                    UpdateStatus($"Value refreshed for {activeTextBox.Name}", Colors.Cyan); // แจ้งสถานะ
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error refreshing value: {ex.Message}", Colors.Red); // แจ้งข้อผิดพลาด
                }
            }
            else
            {
                UpdateStatus("No active field to refresh!", Colors.Red); // แจ้งหากไม่มี TextBox ที่ใช้งาน
            }
        }

        // ฟังก์ชันสำหรับดึงค่าที่รีเฟรช
        private string GetRefreshedValue(string fieldName)
        {
            // ตัวอย่าง: ใช้ switch เพื่อดึงค่าที่เหมาะสม
            switch (fieldName)
            {
                case "IPAddressTextBox":
                    return networkAdapterManager.GetIPAddress(connectionID) ?? "N/A";
                case "SubnetMaskTextBox":
                    return networkAdapterManager.GetSubnetMask(connectionID) ?? "N/A";
                case "GatewayTextBox":
                    return networkAdapterManager.GetGateway(connectionID) ?? "N/A";
                case "PreferredDNSTextBox":
                    return networkAdapterManager.GetPreferredDNS(connectionID) ?? "N/A";
                case "AlternateDNSTextBox":
                    return networkAdapterManager.GetAlternateDNS(connectionID) ?? "N/A";
                default:
                    return string.Empty; // ถ้าไม่มีช่องที่ตรง
            }
        }

        // ฟังก์ชันสำหรับเปลี่ยนโฟกัสไปยังกล่องข้อความข้างบน
        private void KeyboardNavigateUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null)
            {
                // รายการกล่องข้อความทั้งหมด
                var textBoxes = new List<TextBox> { IPAddressTextBox, SubnetMaskTextBox, GatewayTextBox, PreferredDNSTextBox, AlternateDNSTextBox };

                int index = textBoxes.IndexOf(activeTextBox);
                if (index > 0) // ถ้าไม่ใช่กล่องแรก
                {
                    textBoxes[index - 1].Focus(); // เปลี่ยนโฟกัสไปยังกล่องก่อนหน้า
                }
            }
        }

        // ฟังก์ชันสำหรับเปลี่ยนโฟกัสไปยังกล่องข้อความข้างล่าง
        private void KeyboardNavigateDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null)
            {
                // รายการกล่องข้อความทั้งหมด
                var textBoxes = new List<TextBox> { IPAddressTextBox, SubnetMaskTextBox, GatewayTextBox, PreferredDNSTextBox, AlternateDNSTextBox };

                int index = textBoxes.IndexOf(activeTextBox);
                if (index < textBoxes.Count - 1) // ถ้าไม่ใช่กล่องสุดท้าย
                {
                    textBoxes[index + 1].Focus(); // เปลี่ยนโฟกัสไปยังกล่องถัดไป
                }
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
