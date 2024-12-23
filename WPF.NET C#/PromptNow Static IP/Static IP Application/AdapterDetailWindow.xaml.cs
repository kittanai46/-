using System;
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
                MessageBox.Show("Invalid IP Address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SubnetMaskTextBox.Text) || !System.Net.IPAddress.TryParse(SubnetMaskTextBox.Text, out _))
            {
                MessageBox.Show("Invalid Subnet Mask", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(GatewayTextBox.Text) && !System.Net.IPAddress.TryParse(GatewayTextBox.Text, out _))
            {
                MessageBox.Show("Invalid Gateway", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PreferredDNSTextBox.Text) && !System.Net.IPAddress.TryParse(PreferredDNSTextBox.Text, out _))
            {
                MessageBox.Show("Invalid Preferred DNS", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(AlternateDNSTextBox.Text) && !System.Net.IPAddress.TryParse(AlternateDNSTextBox.Text, out _))
            {
                MessageBox.Show("Invalid Alternate DNS", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            if (!ValidateInput())
            {
                return;
            }

            UpdateStatus("Saving...", Colors.Yellow);

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
                    bool successIP = networkAdapterManager.SetIPAddress(
                        connectionID,
                        IPAddressTextBox.Text,
                        SubnetMaskTextBox.Text,
                        GatewayTextBox.Text,
                        out error);

                    if (!successIP)
                    {
                        UpdateStatus($"Failed to set IP Address: {error}", Colors.Red);
                        return;
                    }

                    bool successDNS = networkAdapterManager.SetDNS(
                        connectionID,
                        PreferredDNSTextBox.Text,
                        AlternateDNSTextBox.Text,
                        out error);

                    if (!successDNS)
                    {
                        UpdateStatus($"Failed to set DNS: {error}", Colors.Red);
                        return;
                    }

                    UpdateStatus("Network settings saved successfully", Colors.Green);
                    LoadAdapterData();
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error: {ex.Message}", Colors.Red);
                }
            }
            else
            {
                UpdateStatus("Save operation cancelled", Colors.Red);
            }
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
