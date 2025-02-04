using System.ComponentModel.DataAnnotations;

namespace AwesomeBank.API.Application.Commands
{
    public class AddTransactionCommand : IRequest<AccountViewModel>
    {
        public string AccountNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Length(1, 1)]
        public string Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public AddTransactionCommand(string accountNumber, DateTime date, string type, decimal amount)
        {
            AccountNumber = accountNumber;
            Date = date;
            Type = type;
            Amount = amount;
        }
    }
}