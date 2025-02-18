namespace AwesomeBank.Console.Services;

public class InterestRuleService(ICommandHandleHelper commandHandle) : IInterestRuleService
{
    private readonly ICommandHandleHelper _commandHandle = commandHandle;

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

            var command = new AddInterestRuleCommand(date, ruleId, rate);
            var validator = new AddInterestRuleCommandValidator();

            var results = await _commandHandle.HandleCommandAsync<AddInterestRuleCommand,
                AddInterestRuleCommandValidator,
                IEnumerable<InterestRuleViewModel>>(command, validator);

            if (results != null)
            {
                DisplayInterestRules(results);
            }
            return;
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

        if (!decimal.TryParse(parts[2], out rate))
        {
            System.Console.WriteLine("Invalid rate. It must be number or decimal (ex: 1.01).");
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