using AwesomeBank.API.Application.Models.Requests;

namespace AwesomeBank.Console.Services;

public class StatementService(IStatementService statementService) : IConsoleStatementService
{
    private IStatementService _statementService = statementService;

    public async Task PrintStatementAsync()
    {
        System.Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>");
        System.Console.WriteLine("(or enter blank to go back to the main menu):");
        string input = System.Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
            return;

        string[] parts = input.Split(' ');
        if (parts.Length != 2 || parts[1].Length != 6 || !int.TryParse(parts[1], out _))
        {
            System.Console.WriteLine("Invalid input format. Expected format: <Account> <YYYYMM>");
            return;
        }

        StatementRequest request = new()
        {
            AccountNumber = parts[0],
            Month = parts[1].Substring(4, 2),
            Year = parts[1].Substring(0, 4)
        };

        var validator = new StatementRequestValidator();
        ValidationResult result = await validator.ValidateAsync(request);

        if (result.IsValid)
        {
            var statement = _statementService.GetStatement(request);

            if (statement == null)
            {
                System.Console.WriteLine($"No transactions found for account {request.AccountNumber} in {request.Year}-{request.Month}.");
                return;
            }

            DisplayStatement(statement);
        }
        else
        {
            System.Console.WriteLine("\nValidation failed:");

            foreach (var failure in result.Errors)
            {
                System.Console.WriteLine($"Property: {failure.PropertyName}, Error: {failure.ErrorMessage}");
            }
            System.Console.WriteLine("\n");
            return;
        }
    }

    private void DisplayStatement(AccountStatementModel statement)
    {
        System.Console.WriteLine($"\nAccount: {statement.AccountNumber}");

        System.Console.WriteLine("| Date     | Txn Id      | Type | Amount  | Balance  |");
        if (statement.Entries.Count == 0)
        {
            System.Console.WriteLine("No Records for Display for the month.");
        }

        foreach (var txn in statement.Entries)
        {
            string txnIdDisplay = string.IsNullOrEmpty(txn.TransactionId) ? "           " : txn.TransactionId.PadRight(10);

            System.Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txnIdDisplay} | {txn.Type,-4} | {txn.Amount,7:F2} | {txn.Balance,8:F2} |");
        }
    }
}