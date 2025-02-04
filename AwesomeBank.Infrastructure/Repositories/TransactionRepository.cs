namespace AwesomeBank.Infrastructure.Repositories;

public class TransactionRepository : InMemoryRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository() : base(t => t.TransactionId)
    {
    }
}