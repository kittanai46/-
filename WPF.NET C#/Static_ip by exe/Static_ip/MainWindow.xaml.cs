using System; // ใช้สำหรับฟังก์ชันพื้นฐาน เช่น การจัดการ Exception
using System.Diagnostics; // ใช้สำหรับเรียกใช้งาน Process (เช่น รันคำสั่ง netsh)
using System.Linq; // ใช้สำหรับการจัดการคอลเลกชัน เช่น LINQ
using System.Net.NetworkInformation; // ใช้สำหรับจัดการ Network Adapter และข้อมูลเครือข่าย
using System.Windows; // ใช้สำหรับสร้างหน้าต่างใน WPF
using System.Windows.Controls; // ใช้สำหรับควบคุม UI เช่น ComboBox, TextBox, Button

namespace StaticIPApp // กำหนดพื้นที่ชื่อสำหรับโปรแกรมนี้
{
    public partial class MainWindow : Window // กำหนดคลาสสำหรับหน้าต่างหลัก
    {
        private bool IsSimulationMode = false; // ตัวแปรสำหรับเปิด/ปิด Simulation Mode

        public MainWindow() // คอนสตรัคเตอร์สำหรับหน้าต่างหลัก
        {
            InitializeComponent(); // เรียกใช้การโหลด UI ที่กำหนดไว้ใน XAML
            LoadNetworkAdapters(); // โหลดรายการ Network Adapter ลงใน ComboBox
        }

        private void LoadNetworkAdapters() // ฟังก์ชันสำหรับโหลด Network Adapter ลงใน ComboBox
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces() // ดึง Network Adapter ทั้งหมด
                                           .Where(a => a.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                                       a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) // กรองเฉพาะ Ethernet และ Wireless
                                           .Select(a => a.Name); // ดึงชื่อของ Network Adapter

            AdapterComboBox.ItemsSource = adapters; // ตั้งค่ารายการที่ดึงได้ให้กับ ComboBox

            if (AdapterComboBox.Items.Count > 0) // ตรวจสอบว่ามีรายการใน ComboBox หรือไม่
            {
                AdapterComboBox.SelectedIndex = 0; // เลือกรายการแรกใน ComboBox
            }
        }

        private void AdapterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // ฟังก์ชันสำหรับจัดการเมื่อเลือก Network Adapter
        {
            string selectedAdapter = AdapterComboBox.SelectedItem?.ToString(); // ดึงชื่อ Network Adapter ที่ถูกเลือก
            if (string.IsNullOrWhiteSpace(selectedAdapter)) return; // ถ้าไม่มีการเลือก ไม่ต้องทำอะไร

            var adapter = NetworkInterface.GetAllNetworkInterfaces() // ดึงรายการ Network Adapter อีกครั้ง
                                          .FirstOrDefault(a => a.Name == selectedAdapter); // หา Adapter ที่มีชื่อเดียวกับที่เลือก

            if (adapter != null) // ถ้าพบ Adapter ที่เลือก
            {
                var properties = adapter.GetIPProperties(); // ดึงข้อมูลการตั้งค่าของ Adapter

                var ipv4Address = properties.UnicastAddresses // ดึงรายการที่อยู่ IP
                                             .FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString(); // ดึง IPv4 Address
                var subnetMask = properties.UnicastAddresses // ดึง Subnet Mask
                                            .FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.IPv4Mask.ToString();
                var gateway = properties.GatewayAddresses // ดึง Default Gateway
                                         .FirstOrDefault()?.Address.ToString();
                var dns = string.Join(", ", properties.DnsAddresses // รวม DNS Server ทั้งหมดเป็นข้อความ
                                                      .Where(d => d.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                                      .Select(d => d.ToString()));

                IpAddressTextBox.Text = ipv4Address ?? "N/A"; // แสดง IPv4 Address ใน TextBox
                SubnetMaskTextBox.Text = subnetMask ?? "N/A"; // แสดง Subnet Mask ใน TextBox
                GatewayTextBox.Text = gateway ?? "N/A"; // แสดง Gateway ใน TextBox
                DnsTextBox.Text = dns ?? "N/A"; // แสดง DNS Server ใน TextBox
            }
        }

        private void SetStaticIp_Click(object sender, RoutedEventArgs e) // ฟังก์ชันสำหรับตั้งค่า Static IP
        {
            try
            {
                string adapterName = AdapterComboBox.SelectedItem?.ToString(); // ดึงชื่อ Network Adapter ที่เลือก
                string ipAddress = IpAddressTextBox.Text; // ดึงค่า IP Address จาก TextBox
                string subnetMask = SubnetMaskTextBox.Text; // ดึงค่า Subnet Mask จาก TextBox
                string gateway = GatewayTextBox.Text; // ดึงค่า Gateway จาก TextBox
                string dns = DnsTextBox.Text; // ดึงค่า DNS Server จาก TextBox

                if (string.IsNullOrWhiteSpace(adapterName) || // ตรวจสอบว่าทุกช่องกรอกข้อมูลครบหรือไม่
                    string.IsNullOrWhiteSpace(ipAddress) ||
                    string.IsNullOrWhiteSpace(subnetMask) ||
                    string.IsNullOrWhiteSpace(gateway) ||
                    string.IsNullOrWhiteSpace(dns))
                {
                    MessageBox.Show("Please fill in all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); // แจ้งเตือนถ้ามีช่องว่าง
                    return;
                }

                if (!IsValidIpAddress(ipAddress) || !IsValidIpAddress(subnetMask) || // ตรวจสอบรูปแบบ IP ว่าถูกต้องหรือไม่
                    !IsValidIpAddress(gateway) || !IsValidIpAddress(dns))
                {
                    MessageBox.Show("One or more IP fields are invalid. Please check and try again.", // แจ้งเตือนถ้า IP ไม่ถูกต้อง
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ExecuteCommand($"netsh interface ip set address name=\"{adapterName}\" static {ipAddress} {subnetMask} {gateway}"); // รันคำสั่งตั้งค่า Static IP
                ExecuteCommand($"netsh interface ip set dns name=\"{adapterName}\" static {dns}"); // รันคำสั่งตั้งค่า DNS Server

                MessageBox.Show($"Static IP configuration simulated for {adapterName}.", "Simulation Mode", // แจ้งผู้ใช้ว่าเสร็จแล้ว
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) // จัดการข้อผิดพลาด
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // แสดงข้อความข้อผิดพลาด
            }
        }

        private void SetDhcp_Click(object sender, RoutedEventArgs e) // ฟังก์ชันสำหรับตั้งค่า DHCP
        {
            try
            {
                string adapterName = AdapterComboBox.SelectedItem?.ToString(); // ดึงชื่อ Network Adapter ที่เลือก

                if (string.IsNullOrWhiteSpace(adapterName)) // ตรวจสอบว่ามีการเลือก Adapter หรือไม่
                {
                    MessageBox.Show("Please select a network adapter!", "Error", // แจ้งเตือนถ้าไม่มีการเลือก
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ExecuteCommand($"netsh interface ip set address name=\"{adapterName}\" source=dhcp"); // รันคำสั่งตั้งค่า DHCP สำหรับ IP
                ExecuteCommand($"netsh interface ip set dns name=\"{adapterName}\" source=dhcp"); // รันคำสั่งตั้งค่า DHCP สำหรับ DNS

                MessageBox.Show($"DHCP simulation enabled for {adapterName}.", "Simulation Mode", // แจ้งผู้ใช้ว่าเสร็จแล้ว
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) // จัดการข้อผิดพลาด
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // แสดงข้อความข้อผิดพลาด
            }
        }

        private void ExecuteCommand(string command) // ฟังก์ชันสำหรับรันคำสั่ง netsh
        {
            if (IsSimulationMode) // ถ้าอยู่ใน Simulation Mode
            {
                MessageBox.Show($"Simulation Mode: {command}", "Command Simulation", // แสดงคำสั่งที่ควรรัน
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Process process = new Process // สร้าง Process สำหรับรันคำสั่ง
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe", // ใช้ Command Prompt
                        Arguments = $"/C {command}", // รันคำสั่งที่รับมา
                        RedirectStandardOutput = true, // รับผลลัพธ์ออก
                        RedirectStandardError = true, // รับข้อผิดพลาด
                        UseShellExecute = false, // ไม่ใช้ Shell Execute
                        CreateNoWindow = true, // ไม่แสดงหน้าต่าง Command Prompt
                        Verb = "runas" // รันคำสั่งในโหมด Administrator
                    }
                };
                process.Start(); // เริ่มการรันคำสั่ง
                process.WaitForExit(); // รอให้คำสั่งรันเสร็จ

                string output = process.StandardOutput.ReadToEnd(); // อ่านผลลัพธ์ที่สำเร็จ
                string error = process.StandardError.ReadToEnd(); // อ่านข้อผิดพลาด

                if (!string.IsNullOrEmpty(error)) // ถ้ามีข้อผิดพลาด
                {
                    MessageBox.Show($"Command failed:\n{error}", "Command Error", // แสดงข้อผิดพลาด
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(output, "Command Output", MessageBoxButton.OK, MessageBoxImage.Information); // แสดงผลลัพธ์
                }
            }
            catch (Exception ex) // จัดการข้อผิดพลาดที่ไม่คาดคิด
            {
                MessageBox.Show($"Unexpected error occurred:\n{ex.Message}", "Error", // แสดงข้อผิดพลาด
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidIpAddress(string ipAddress) // ฟังก์ชันตรวจสอบรูปแบบ IP Address
        {
            return System.Net.IPAddress.TryParse(ipAddress, out _); // คืนค่า true ถ้ารูปแบบถูกต้อง
        }

        private void RefreshNetworkAdapters_Click(object sender, RoutedEventArgs e) // ฟังก์ชันสำหรับรีเฟรชรายการ Network Adapter
        {
            LoadNetworkAdapters(); // โหลด Network Adapter ใหม่
        }
    }
}
