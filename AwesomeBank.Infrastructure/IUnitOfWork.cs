namespace AwesomeBank.Infrastructure;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }
    ITransactionRepository Transactions { get; }
    IInterestRuleRepository InterestRules { get; }
}