
namespace AwesomeBank.Console.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }

    }
}
