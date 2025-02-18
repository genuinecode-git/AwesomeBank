namespace AwesomeBank.Domain.Entities;

public partial class Transaction
{
    public string TransactionId { get; private set; }
    public DateTime Date { get; private set; }
    public string Type { get; private set; }
    public decimal Amount { get; private set; }
}