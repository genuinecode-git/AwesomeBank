namespace AwesomeBank.Console.Application;

public class AwsomeBankApplication(
    ILogger<AwsomeBankApplication> logger,
    ITransactionService transactionService,
    IInterestRuleService interestRuleService,
    IConsoleStatementService statementService
    ) : ConsoleApplicationBase(logger)
{
    private readonly ITransactionService _transactionService = transactionService;
    private readonly IInterestRuleService _interestRuleService = interestRuleService;
    private readonly IConsoleStatementService _statementService = statementService;

    protected override async Task MainMenuDisplay()
    {
        await DisplayMenu();
    }

    protected override async Task HandleTransactions()
    {
        try
        {
            await _transactionService.InputTransactionsAsync();
        }
        catch (ArgumentException ex)
        {
            System.Console.WriteLine($"\nError Occurs: {ex.Message}");
            _logger.LogWarning("\nException Occurs : {exception}", ex.Message);
        }
        catch (ValidationException ex)
        {
            System.Console.WriteLine($"\nSystem Unable to process this record: {ex.Message}");
            _logger.LogWarning("\nException Occurs : {exception}", ex.Message);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("[ERROR] System is unable to Process request. Please contact system administrator.\n");
            _logger.LogError("\nException Occurs : {exception}", ex.Message);
        }
        await DisplayIntermedMenu();
    }

    protected override async Task HandleInterestRules()
    {
        try
        {
            await _interestRuleService.DefineInterestRulesAsync();
        }
        catch (ArgumentException ex)
        {
            System.Console.WriteLine($"\nError Occurs: {ex.Message}");
            _logger.LogWarning("\nException Occurs : {exception}", ex.Message);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("[ERROR] System is unable to Process request. Please contact system administrator.\n");
            _logger.LogError("\nException Occurs : {exception}", ex.Message);
        }
        await DisplayIntermedMenu();
    }

    protected override async Task HandlePrintStatement()
    {
        try
        {
            await _statementService.PrintStatementAsync();
        }
        catch (ArgumentException ex)
        {
            System.Console.WriteLine($"\nError Occurs: {ex.Message}");
            _logger.LogWarning("\nException Occurs : {exception}", ex.Message);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("[ERROR] System is unable to Process request. Please contact system administrator.\n");
            _logger.LogError("\nException Occurs : {exception}", ex.Message);
        }
        await DisplayIntermedMenu();
    }

    private async Task DisplayIntermedMenu()
    {
        System.Console.WriteLine("\nIs there anything else you'd like to do?");
        await DisplayMenu();
    }
}