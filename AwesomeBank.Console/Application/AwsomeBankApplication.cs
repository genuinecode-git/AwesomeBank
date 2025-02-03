using AwesomeBank.Console.Services;

namespace AwesomeBank.Console.Application;

public class AwsomeBankApplication(
    ILogger<AwsomeBankApplication> logger,
    TransactionService transactionService,
    InterestRuleService interestRuleService,
    Services.StatementService statementService
    ) : ConsoleApplicationBase(logger)
{
    private readonly TransactionService _transactionService = transactionService;
    private readonly InterestRuleService _interestRuleService = interestRuleService;
    private readonly Services.StatementService _statementService = statementService;

    protected override async Task MainMenuDisplay()
    {
        await DisplayMenu();
    }

    protected override async Task HandleTransactions()
    {
        await _transactionService.InputTransactionsAsync();
        await DisplayMenu(); 
    }

    protected override async Task HandleInterestRules()
    {
        await _interestRuleService.DefineInterestRulesAsync();
        await DisplayMenu(); 
    }

    protected override async Task HandlePrintStatement()
    {
        await _statementService.PrintStatementAsync();
        await DisplayMenu(); 
    }
}
