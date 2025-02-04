namespace AwesomeBank.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public IAccountRepository Accounts { get; }
    public ITransactionRepository Transactions { get; }
    public IInterestRuleRepository InterestRules { get; }

    public UnitOfWork()
    {
        Accounts = new AccountRepository();
        Transactions = new TransactionRepository();
        InterestRules = new InterestRuleRepository();
    }

    // In-memory implementation does not require commit handling.
    // In EF Core, this would call DbContext.SaveChanges();
    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    // Clean up resources when using EF Core (not needed for in-memory)
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}