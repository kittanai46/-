public class Transaction
{
    public string TransactionType { get; set; } // Deposit หรือ Withdraw
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
