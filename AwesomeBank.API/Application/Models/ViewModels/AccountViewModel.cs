namespace AwesomeBank.API.Application.Models.ViewModels
{
    public class AccountViewModel
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
    }

    public class TransactionViewModel
    {
        public string TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}