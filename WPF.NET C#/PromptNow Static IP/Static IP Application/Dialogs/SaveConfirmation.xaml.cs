using System;
using System.Windows;
using WpfStaticIPApp.Services; // สำหรับการแสดงเวลาและวันที่

namespace WpfStaticIPApp.Dialogs
{
    public partial class SaveConfirmation : Window
    {
        private string adapterName;
        private string ipAddress;
        private string subnetMask;
        private string gateway;
        private string preferredDNS;
        private string alternateDNS;

        private DateTimeService dateTimeService;
        public SaveConfirmation(string adapterName, string ipAddress, string subnetMask,
                                string gateway, string preferredDNS, string alternateDNS)
        {

            InitializeComponent();

            dateTimeService = new DateTimeService();
            dateTimeService.StartDateTimeUpdate(DateText, TimeText);

            // รับข้อมูลทั้งหมดที่ส่งมา
            this.adapterName = adapterName;
            this.ipAddress = ipAddress;
            this.subnetMask = subnetMask;
            this.gateway = gateway;
            this.preferredDNS = preferredDNS;
            this.alternateDNS = alternateDNS;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // เมื่อกดปุ่ม "ตกลง"
            this.DialogResult = true; // กำหนดผลลัพธ์เป็น true
            this.Close(); // ปิดหน้าต่าง
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // เมื่อกดปุ่ม "ยกเลิก"
            this.DialogResult = false; // กำหนดผลลัพธ์เป็น false
            this.Close(); // ปิดหน้าต่าง
        }
    }
}
