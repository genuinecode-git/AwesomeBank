namespace AwesomeBank.Console.Application;

public abstract class ConsoleApplicationBase(ILogger<ConsoleApplicationBase> logger) : IConsoleApplication
{
    protected readonly ILogger<ConsoleApplicationBase> _logger = logger;

    public async Task Run()
    {
        _logger.LogInformation("Application started.");
        while (true)
        {
            System.Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            await MainMenuDisplay();
        }
    }

    protected abstract Task MainMenuDisplay();

    protected async Task DisplayMenu()
    {
        System.Console.WriteLine("[T] Input transactions");
        System.Console.WriteLine("[I] Define interest rules");
        System.Console.WriteLine("[P] Print statement");
        System.Console.WriteLine("[Q] Quit");
        System.Console.Write("> ");

        string choice = System.Console.ReadLine()?.ToUpper();
        switch (choice)
        {
            case "T":
                await HandleTransactions();
                break;

            case "I":
                await HandleInterestRules();
                break;

            case "P":
                await HandlePrintStatement();
                break;

            case "Q":
                Environment.Exit(0);
                break;

            default:
                System.Console.WriteLine("Invalid option, try again.");
                break;
        }
    }

    protected virtual Task HandleTransactions() => Task.CompletedTask;

    protected virtual Task HandleInterestRules() => Task.CompletedTask;

    protected virtual Task HandlePrintStatement() => Task.CompletedTask;
}