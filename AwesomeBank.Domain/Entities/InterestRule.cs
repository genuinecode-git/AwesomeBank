namespace AwesomeBank.Domain.Entities
{
    public partial class InterestRule
    {
        public string RuleId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Rate { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}