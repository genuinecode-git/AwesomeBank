namespace AwesomeBank.Domain.Entities;

public partial class Account
{
    public string AccountNumber { get; private set; }
    public List<Transaction> Transactions { get; private set; }
}