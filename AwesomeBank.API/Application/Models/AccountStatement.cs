namespace AwesomeBank.API.Application.Models
{
    public class AccountStatementModel
    {
        public string AccountNumber { get; set; }
        public List<StatementEntryModel> Entries { get; set; }

        public AccountStatementModel(string accountNumber, List<StatementEntryModel> entries)
        {
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            Entries = entries ?? throw new ArgumentNullException(nameof(entries));
        }
    }

    public class StatementEntryModel
    {
        public DateTime Date { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        public StatementEntryModel(DateTime date, string transactionId, string type, decimal amount, decimal balance)
        {
            Date = date;
            TransactionId = transactionId;
            Type = type;
            Amount = amount;
            Balance = balance;
        }
    }

    public class StatementEntryRecordModel
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
        public decimal Interst { get; set; }

        public StatementEntryRecordModel(DateTime date, decimal balance, decimal interst)
        {
            Date = date;
            Balance = balance;
            Interst = interst;
        }
    }
}