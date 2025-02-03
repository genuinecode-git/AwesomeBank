namespace AwesomeBank.Console.Services;

public class InterestRuleService(IMediator mediator, ILogger<InterestRuleService> logger)
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<InterestRuleService> _logger = logger;
    public async Task DefineInterestRulesAsync()
    {
        System.Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format (or enter blank to go back):");

        while (true)
        {
            System.Console.Write("> ");
            string input = System.Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) break;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3 || !ValidateInterestRule(parts, out var date, out var ruleId, out var rate))
            {
                continue;
            }

            try
            {
                var results = await _mediator.Send(new AddInterestRuleCommand(date, ruleId, rate));

                if (results != null)
                {
                    DisplayInterestRules(results);
                }
                return;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                _logger.LogError("Exception Occurs : {exception}", ex.Message);
            }
        }
    }

    private bool ValidateInterestRule(string[] parts, out DateTime date, out string ruleId, out decimal rate)
    {
        date = default;
        ruleId = parts[1].Trim();
        rate = default;

        if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            System.Console.WriteLine("Invalid date format. Use YYYYMMdd.");
            return false;
        }

        if (!decimal.TryParse(parts[2], out rate) || rate <= 0 || rate >= 100)
        {
            System.Console.WriteLine("Invalid rate. It must be greater than 0 and less than 100.");
            return false;
        }

        return true;
    }

    private void DisplayInterestRules(IEnumerable<InterestRuleViewModel> results)
    {
        System.Console.WriteLine("\nInterest rules:");
        System.Console.WriteLine("| Date     | RuleId      | Rate (%) |");
        foreach (var rule in results)
        {
            System.Console.WriteLine($"| {rule.Date,-8:yyyyMMdd} | {rule.RuleId,-11} | {rule.Rate,8:F2} |");
        }

        System.Console.WriteLine("\nIs there anything else you'd like to do?");
    }
}
