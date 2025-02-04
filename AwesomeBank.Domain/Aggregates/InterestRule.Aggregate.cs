namespace AwesomeBank.Domain.Entities
{
    public partial class InterestRule
    {
        public InterestRule(string ruleId, DateTime date, decimal rate)
        {
            Validate(ruleId, date, rate);
            this.RuleId = ruleId;
            this.Date = date;
            this.Rate = rate;
            this.CreatedDate = DateTime.UtcNow;
        }

        private void Validate(string ruleId, DateTime date, decimal rate)
        {
            List<string> errors = [];

            if (string.IsNullOrEmpty(ruleId)) errors.Add("Rule Id is required.");
            if (rate < 0 || rate > 100) errors.Add("Rate must be in between 0 and 100.");
            if (date == default) errors.Add("Invalid rule date.");

            if (errors.Count != 0)
                throw new ArgumentException(string.Join(" ", errors));
        }
    }
}