namespace AwesomeBank.Test.FunctionalTests;

[TestFixture]
public class StatementServiceTests
{
    private Mock<IStatementService> _statementServiceMock;
    private Console.Services.StatementService _statementService;

    [SetUp]
    public void SetUp()
    {
        _statementServiceMock = new Mock<IStatementService>();
        _statementService = new Console.Services.StatementService(_statementServiceMock.Object);
    }

    [Test]
    public async Task PrintStatementAsync_Sucess_ValidInput()
    {
        // Arrange
        var expectedStatement = new AccountStatementModel("12345", new List<StatementEntryModel>
            {
                new StatementEntryModel(new DateTime(2023, 01, 01),"Txn123",TransactionType.Deposit,100.00m,1000.00m)
            });

        _statementServiceMock.Setup(x => x.GetStatement(It.IsAny<StatementRequest>()))
                             .Returns(expectedStatement);

        var userInput = "12345 202301";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _statementService.PrintStatementAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.Multiple(() =>
        {
            Assert.That(output.Contains("Account: 12345"), Is.True);
            Assert.That(output.Contains("| Date     | Txn Id      | Type | Amount  | Balance  |"), Is.True);
            Assert.That(output.Contains("20230101"), Is.True);
            Assert.That(output.Contains("Txn123"), Is.True);
            Assert.That(output.Contains(TransactionType.Deposit), Is.True);
            Assert.That(output.Contains("100.00"), Is.True);
            Assert.That(output.Contains("1000.00"), Is.True);
        });
    }

    [Test]
    public async Task PrintStatementAsync_ErrorMessage_InvalidInput()
    {
        // Arrange
        var userInput = "12345 2023";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _statementService.PrintStatementAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.That(output.Contains("Invalid input format. Expected format: <Account> <YYYYMM>"), Is.True);
    }

    [Test]
    public async Task PrintStatementAsync_NoTransactionsMessage_NoTransactions()
    {
        var emptyStatement = new AccountStatementModel("12345", []);

        _statementServiceMock.Setup(x => x.GetStatement(It.IsAny<StatementRequest>()))
                             .Returns(emptyStatement);

        var userInput = "12345 202301";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _statementService.PrintStatementAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.That(output.Contains("No Records for Display for the month."), Is.True);
    }

    [Test]
    public async Task PrintStatementAsync_DoNothing_BlankInput()
    {
        // Arrange
        var userInput = "";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _statementService.PrintStatementAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.That(output.Contains("Account:"), Is.False);
    }

    [Test]
    public async Task PrintStatementAsync_NoTransactionError_ValidInput()
    {
        // Arrange
        var request = new StatementRequest
        {
            AccountNumber = "12345",
            Month = "01",
            Year = "2023"
        };
        var validator = new StatementRequestValidator();
        ValidationResult validationResult = new ValidationResult(
            new List<ValidationFailure>
            {
                new ValidationFailure("AccountNumber", "Account number is required")
            });

        _statementServiceMock.Setup(x => x.GetStatement(It.IsAny<StatementRequest>()))
                             .Returns((AccountStatementModel)null);

        var userInput = "12345 202301";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _statementService.PrintStatementAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.That(output.Contains("No transactions found for account 12345 in 2023-01"), Is.True);
    }
}