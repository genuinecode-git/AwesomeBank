namespace AwesomeBank.Domain.Entities;

public partial class Transaction
{
    public Transaction(string transactionId, DateTime date, string type, decimal amount)
    {
        Validate(transactionId, date, type, amount);
        this.TransactionId = transactionId;
        this.Date = date;
        this.Type = type.ToUpper();
        this.Amount = amount;
    }

    private static void Validate(string transactionId, DateTime date, string type, decimal amount)
    {
        List<string> errors = [];

        if (string.IsNullOrEmpty(transactionId)) errors.Add("Transaction Id is required.");
        if (string.IsNullOrEmpty(type) || type.Length > 1) errors.Add("Transaction type is required and length must be 1.");
        if (!"DW".Contains(type.ToUpper())) errors.Add("Transaction type must be 'D' (Deposit) or 'W' (Withdrawal).");
        if (amount <= 0) errors.Add("Transaction Amount must be greater than zero.");
        if (date == default) errors.Add("Invalid transaction date.");

        if (errors.Count != 0)
            throw new ArgumentException(string.Join(" ", errors));
    }
}