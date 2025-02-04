namespace AwesomeBank.Console.Services;

public class TransactionService(IMediator mediator, ILogger<TransactionService> logger)
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<TransactionService> _logger = logger;

    public async Task InputTransactionsAsync()
    {
        System.Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format (or enter blank to go back):");

        System.Console.Write("> ");
        string input = System.Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input)) return;  // Exit if input is blank

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 4 || !ValidateTransaction(parts, out var date, out var account, out var type, out var amount))
        {
            System.Console.WriteLine("Invalid input format.");
            return;  // Return if the input is invalid
        }

        try
        {
            var results = await _mediator.Send(new AddTransactionCommand(account, date, type, amount));

            if (results != null)
            {
                DisplayTransactions(results);
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            _logger.LogError("Exception Occurs: {exception}", ex.Message);
        }

        System.Console.WriteLine("\nIs there anything else you'd like to do?");
    }


    private bool ValidateTransaction(string[] parts, out DateTime date, out string account, out string type, out decimal amount)
    {
        date = default;
        account = parts[1].Trim();
        type = parts[2].Trim().ToUpper();
        amount = default;

        if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            System.Console.WriteLine("Invalid date format. Use YYYYMMdd.");
            return false;
        }

        if (type != TransactionType.Deposit && type != TransactionType.Withdrawal)
        {
            System.Console.WriteLine("Invalid type. Use D for deposit or W for withdrawal.");
            return false;
        }

        if (!decimal.TryParse(parts[3], out amount) || amount <= 0 || decimal.Round(amount, 2) != amount)
        {
            System.Console.WriteLine("Invalid amount. Must be greater than 0 with up to 2 decimal places.");
            return false;
        }

        return true;
    }

    private void DisplayTransactions(AccountViewModel results)
    {
        System.Console.WriteLine($"\nAccount: {results.AccountNumber}");
        System.Console.WriteLine("| Date     | Txn Id          | Type | Amount   |");
        foreach (var txn in results.Transactions)
        {
            System.Console.WriteLine($"| {txn.Date,-8:yyyyMMdd} | {txn.TransactionId,-15} | {txn.Type,-4} | {txn.Amount,8:F2} |");
        }
        System.Console.WriteLine("\nIs there anything else you'd like to do?");
    }
}
