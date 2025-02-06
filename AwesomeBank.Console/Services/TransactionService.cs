namespace AwesomeBank.Console.Services;

public class TransactionService(ICommandHandleHelper commandHandle) : ITransactionService
{
    private readonly ICommandHandleHelper _commandHandle = commandHandle;

    public async Task InputTransactionsAsync()
    {
        System.Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format (or enter blank to go back):");

        System.Console.Write("> ");
        string input = System.Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input)) return;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 4 || !ValidateTransaction(parts, out var date, out var account, out var type, out var amount))
        {
            System.Console.WriteLine("Invalid input format.");
            return;
        }
        var command = new AddTransactionCommand(account, date, type, amount);
        var validator = new AddTransactionCommandValidator();

        var results = await _commandHandle.HandleCommandAsync<AddTransactionCommand,
            AddTransactionCommandValidator,
            AccountViewModel>(command, validator);

        if (results != null)
        {
            DisplayTransactions(results);
        }
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

        if (!decimal.TryParse(parts[3], out amount))
        {
            System.Console.WriteLine("Invalid amount. Must be number or decimal.");
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
    }
}