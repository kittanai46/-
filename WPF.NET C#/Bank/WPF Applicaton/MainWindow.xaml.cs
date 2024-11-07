using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace BankApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Transaction> transactions;
        private decimal balance = 0;

        public MainWindow()
        {
            InitializeComponent();
            transactions = new ObservableCollection<Transaction>();
            TransactionListView.ItemsSource = transactions;
            UpdateBalanceText();
        }

        // ฟังก์ชันการฝากเงิน
        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && amount > 0)
            {
                balance += amount;
                transactions.Add(new Transaction
                {
                    TransactionType = "Deposit",
                    Amount = amount,
                    Date = DateTime.Now
                });
                AmountTextBox.Clear();
                UpdateBalanceText(); // อัพเดตยอดเงินคงเหลือ
            }
            else
            {
                MessageBox.Show("กรุณากรอกจำนวนเงินที่ถูกต้อง");
            }
        }

        // ฟังก์ชันการถอนเงิน
        private void Withdraw_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && amount > 0)
            {
                if (amount <= balance)
                {
                    balance -= amount;
                    transactions.Add(new Transaction
                    {
                        TransactionType = "Withdraw",
                        Amount = amount,
                        Date = DateTime.Now
                    });
                    AmountTextBox.Clear();
                    UpdateBalanceText(); // อัพเดตยอดเงินคงเหลือ
                }
                else
                {
                    MessageBox.Show("ยอดเงินคงเหลือไม่พอสำหรับการถอน");
                }
            }
            else
            {
                MessageBox.Show("กรุณากรอกจำนวนเงินที่ถูกต้อง");
            }
        }

        // ฟังก์ชันอัพเดตยอดเงินคงเหลือ
        private void UpdateBalanceText()
        {
            BalanceTextBlock.Text = balance.ToString("C"); // แสดงยอดเงินในรูปแบบสกุลเงิน
        }
    }
}
