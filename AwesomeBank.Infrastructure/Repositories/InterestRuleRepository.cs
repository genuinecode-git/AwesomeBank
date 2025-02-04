namespace AwesomeBank.Infrastructure.Repositories;

public class InterestRuleRepository : InMemoryRepository<InterestRule>, IInterestRuleRepository
{
    public InterestRuleRepository() : base(a => a.RuleId)
    {
    }
}