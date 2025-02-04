namespace AwesomeBank.Domain.Entities;

public partial class Account
{
    public Account(string accountNumber)
    {
        Validate(accountNumber);
        this.AccountNumber = accountNumber;
    }

    public string AddTransaction(DateTime date, string type, decimal amount)
    {
        this.Transactions ??= [];
        Transaction transaction = new(GetTransactionNumber(date), date, type, amount);
        this.Transactions.Add(transaction);
        return transaction.TransactionId;
    }

    private string GetTransactionNumber(DateTime transactionDate)
    {
        return $"{transactionDate.Date:yyyyMMdd}-{this.Transactions.Where(x => x.Date.Date == transactionDate.Date).Count() + 1:D2}";
    }

    private static void Validate(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber)) throw new ArgumentNullException($"{nameof(accountNumber)} is required.");
    }
}