namespace AwesomeBank.Infrastructure.Repositories;

public class AccountRepository : InMemoryRepository<Account>, IAccountRepository
{
    public AccountRepository() : base(a => a.AccountNumber)
    {
    }
}