namespace AwesomeBank.Test.FunctionalTests;

[TestFixture]
public class TransactionServiceTests
{
    private Mock<ICommandHandleHelper> _commandHandleMock;
    private TransactionService _transactionService;

    [SetUp]
    public void SetUp()
    {
        _commandHandleMock = new Mock<ICommandHandleHelper>();
        _transactionService = new TransactionService(_commandHandleMock.Object);
    }

    [Test]
    public async Task InputTransactionsAsync_Sucess_CommandIsValid()
    {
        // Arrange
        var expectedResults = new AccountViewModel
        {
            AccountNumber = "12345",
            Transactions =
            [
                new() {
                    Date = new DateTime(2023, 01, 01),
                    TransactionId = "Txn123",
                    Type = TransactionType.Deposit,
                    Amount = 100.00m
                }
            ]
        };

        _commandHandleMock.Setup(x => x.HandleCommandAsync<AddTransactionCommand, AddTransactionCommandValidator, AccountViewModel>(
                It.IsAny<AddTransactionCommand>(),
                It.IsAny<AddTransactionCommandValidator>()
            ))
            .ReturnsAsync(expectedResults);

        var userInput = "20230101 12345 D 100.00";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _transactionService.InputTransactionsAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.Multiple(() =>
        {
            Assert.That(output.Contains("Account: 12345"), Is.True);
            Assert.That(output.Contains("| Date     | Txn Id          | Type | Amount   |"), Is.True);
            Assert.That(output.Contains("20230101"), Is.True);
            Assert.That(output.Contains("Txn123"), Is.True);
            Assert.That(output.Contains("D"), Is.True);
            Assert.That(output.Contains("100.00"), Is.True);
        });
    }

    [Test]
    public async Task InputTransactionsAsync_ValidationMessage_Invalid()
    {
        // Arrange
        var userInput = "20230101 12345 Deposit";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _transactionService.InputTransactionsAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.That(output.Contains("Invalid input format."), Is.True);
    }

    [Test]
    public async Task InputTransactionsAsync_DoNothing_IsBlank()
    {
        // Arrange
        var userInput = "";

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _transactionService.InputTransactionsAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.That(output.Contains("Account:"), Is.False);
    }
}