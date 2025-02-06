using AwesomeBank.Console.Application;
using AwesomeBank.Console.Services.Interfaces;

namespace AwesomeBank.Test.FunctionalTests.ServiceMocks;

public class TestableAwsomeBankApplication(
    ILogger<AwsomeBankApplication> logger,
    ITransactionService transactionService,
    IInterestRuleService interestRuleService,
    IConsoleStatementService statementService
    ) : AwsomeBankApplication(logger, transactionService, interestRuleService, statementService)
{
    // Expose the protected methods for testing
    public new async Task TestHandleTransactions() => await transactionService.InputTransactionsAsync();

    public new async Task TestHandleInterestRules() => await interestRuleService.DefineInterestRulesAsync();

    public new async Task TestHandlePrintStatement() => await statementService.PrintStatementAsync();
}