namespace AwesomeBank.Console.Services;

public class StatementService(IStatementService statementService)
{
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

        string accountNumber = parts[0];
        string year = parts[1].Substring(0, 4);
        string month = parts[1].Substring(4, 2);

        var statement = statementService.GetStatement(accountNumber, year, month);

        if (statement == null)
        {
            System.Console.WriteLine($"No transactions found for account {accountNumber} in {year}-{month}.");
            return;
        }

        DisplayStatement(statement);
    }

    private void DisplayStatement(AccountStatementModel statement)
    {
        System.Console.WriteLine($"\nAccount: {statement.AccountNumber}");
        System.Console.WriteLine("| Date     | Txn Id      | Type | Amount  | Balance  |");
        foreach (var txn in statement.Entries)
        {
            System.Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txn.TransactionId,-10} | {txn.Type,-4} | {txn.Amount,7:F2} | {txn.Balance,8:F2} |");
        }
        System.Console.WriteLine("\nIs there anything else you'd like to do?");
    }
}

