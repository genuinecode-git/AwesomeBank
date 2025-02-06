using AwesomeBank.Console.Application;
using AwesomeBank.Console.Services.Interfaces;
using AwesomeBank.Test.FunctionalTests.ServiceMocks;

namespace AwesomeBank.Test.FunctionalTests;

[TestFixture]
public class AwsomeBankApplicationTests
{
    private Mock<ILogger<AwsomeBankApplication>> _loggerMock;
    private Mock<ITransactionService> _transactionServiceMock;
    private Mock<IInterestRuleService> _interestRuleServiceMock;
    private Mock<IConsoleStatementService> _statementServiceMock;
    private TestableAwsomeBankApplication _awsomeBankApplication;

    [SetUp]
    public void SetUp()
    {
        // Setup mocks
        _loggerMock = new Mock<ILogger<AwsomeBankApplication>>();
        _transactionServiceMock = new Mock<ITransactionService>();
        _interestRuleServiceMock = new Mock<IInterestRuleService>();
        _statementServiceMock = new Mock<IConsoleStatementService>();

        // Initialize AwsomeBankApplication with mocked services
        _awsomeBankApplication = new TestableAwsomeBankApplication(
            _loggerMock.Object,
            _transactionServiceMock.Object,
            _interestRuleServiceMock.Object,
            _statementServiceMock.Object
        );
    }

    [Test]
    public async Task HandleTransactions_CallTransactionServiceInputTransactionsAsync_Sucess()
    {
        // Arrange
        _transactionServiceMock.Setup(x => x.InputTransactionsAsync()).Returns(Task.CompletedTask);

        // Act
        await _awsomeBankApplication.TestHandleTransactions();

        // Assert
        _transactionServiceMock.Verify(x => x.InputTransactionsAsync(), Times.Once);
    }

    [Test]
    public async Task HandleInterestRules_CallInterestRuleServiceDefineInterestRulesAsync_Sucess()
    {
        // Arrange
        _interestRuleServiceMock.Setup(x => x.DefineInterestRulesAsync()).Returns(Task.CompletedTask);

        // Act
        await _awsomeBankApplication.TestHandleInterestRules();

        // Assert
        _interestRuleServiceMock.Verify(x => x.DefineInterestRulesAsync(), Times.Once);
    }

    [Test]
    public async Task HandlePrintStatement_CallStatementServicePrintStatementAsync_Sucess()
    {
        // Arrange
        _statementServiceMock.Setup(x => x.PrintStatementAsync()).Returns(Task.CompletedTask);

        // Act
        await _awsomeBankApplication.TestHandlePrintStatement();

        // Assert
        _statementServiceMock.Verify(x => x.PrintStatementAsync(), Times.Once);
    }

    [Test]
    public async Task HandleTransactions_ShouldException_Application()
    {
        // Arrange
        var exception = new ArgumentException("Test argument exception");
        _transactionServiceMock.Setup(x => x.InputTransactionsAsync()).Throws(exception);

        // Act
        ArgumentException ex = Assert.ThrowsAsync<ArgumentException>(async () => await _awsomeBankApplication.TestHandleTransactions());

        // Assert
        Assert.That(ex.Message, Does.Contain("Test argument exception"));
    }

    [Test]
    public async Task HandleInterestRules_ShouldException_Application()
    {
        // Arrange
        var exception = new ArgumentException("Test argument exception");
        _interestRuleServiceMock.Setup(x => x.DefineInterestRulesAsync()).Throws(exception);

        // Act
        ArgumentException ex = Assert.ThrowsAsync<ArgumentException>(async () => await _awsomeBankApplication.TestHandleInterestRules());

        // Assert
        Assert.That(ex.Message, Does.Contain("Test argument exception"));
    }

    [Test]
    public async Task HandlePrintStatement_ShouldException_Application()
    {
        // Arrange
        var exception = new ArgumentException("Test argument exception");
        _statementServiceMock.Setup(x => x.PrintStatementAsync()).Throws(exception);

        // Act
        ArgumentException ex = Assert.ThrowsAsync<ArgumentException>(async () => await _awsomeBankApplication.TestHandlePrintStatement());

        // Assert
        Assert.That(ex.Message, Does.Contain("Test argument exception"));
    }
}