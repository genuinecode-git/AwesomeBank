namespace AwesomeBank.API.Application.Commands
{
    public class AddInterestRuleCommand : IRequest<List<InterestRuleViewModel>>
    {
        public DateTime Date { get; }
        public string RuleId { get; }
        public decimal Rate { get; }

        public AddInterestRuleCommand(DateTime date, string ruleId, decimal rate)
        {
            Date = date;
            RuleId = ruleId;
            Rate = rate;
        }
    }
}