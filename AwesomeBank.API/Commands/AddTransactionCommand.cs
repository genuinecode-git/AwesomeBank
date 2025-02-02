
namespace AwesomeBank.API.Commands
{
    public class AddTransactionCommand : IRequest<AccountViewModel>
    {
        public string AccountNumber { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }

        public AddTransactionCommand(string accountNumber, DateTime date, string type, decimal amount)
        {
            this.AccountNumber = accountNumber;
            this.Date = date;
            this.Type = type;
            this.Amount = amount;
        }
    }
}
