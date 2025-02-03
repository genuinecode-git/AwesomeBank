using AwesomeBank.API.Application.Queries;
using AwesomeBank.API.Application.Services;

namespace AwesomeBank.ServiceTests.Test;

[TestFixture]
public class StatementServiceTests
{
    private Mock<IAccountQueries> _accountQueriesMock;
    private Mock<IInterestRulesQueries> _interestRulesQueriesMock;
    private Mock<ILogger<StatementService>> _loggerMock;
    private StatementService _statementService;

    [SetUp]
    public void Setup()
    {
        this._accountQueriesMock = new Mock<IAccountQueries>();
        this._interestRulesQueriesMock = new Mock<IInterestRulesQueries>();
        this._loggerMock = new Mock<ILogger<StatementService>>();

        _statementService = new StatementService(
            this._accountQueriesMock.Object,
            this._interestRulesQueriesMock.Object,
            this._loggerMock.Object
        );
    }

    [Test]
    public void GetStatement_AccountNotFound_Null()
    {
        // Arrange
        this._accountQueriesMock
            .Setup(x => x.GetAccount(It.IsAny<string>()))
            .Returns((AccountViewModel)null);

        // Act
        var result = _statementService.GetStatement("AC001", "2023", "06");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetStatement_ValidAccountWithTransactions_ValidStatement()
    {
        // Arrange
        AccountViewModel account = new()
        {
            AccountNumber = "AC001",
            Transactions =
                [
                    new() { Date = new DateTime(2023, 5, 5), TransactionId = "T001", Type = "D", Amount = 100 },
                    new() { Date = new DateTime(2023, 6, 1), TransactionId = "T002", Type = "D", Amount = 150 },
                    new() { Date = new DateTime(2023, 6, 26), TransactionId = "T003", Type = "W", Amount = 20 },
                    new() { Date = new DateTime(2023, 6, 26), TransactionId = "T004", Type = "W", Amount = 100 }
                ]
        };

        this._accountQueriesMock.Setup(x => x.GetAccount("AC001")).Returns(account);
        this._interestRulesQueriesMock.Setup(x => x.GetAllInterstRules()).Returns(
            [
                new() { RuleId="R1" , Date = new DateTime(2023, 1, 1), Rate = 1.95m },
                new() { RuleId="R2" , Date = new DateTime(2023, 5, 20), Rate = 1.90m },
                new() { RuleId="R3" ,Date = new DateTime(2023, 6, 15), Rate = 2.20m }
            ]);

        // Act
        var result = this._statementService.GetStatement("AC001", "2023", "06");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccountNumber, Is.EqualTo("AC001"));
            Assert.That(result.Entries, Has.Count.EqualTo(4));
            Assert.That(result.Entries.Last().Type, Is.EqualTo("I"));
            Assert.That(result.Entries.Last().Amount, Is.EqualTo(0.39));
        });
    }


    [Test]
    public void CalculateInterest_CorrectlyAppliesRates()
    {
        // Arrange
        var account = new AccountViewModel
        {
            AccountNumber = "AC001",
            Transactions =
                [
                    new() { Date = new DateTime(2023, 6, 1), TransactionId = "T001", Type = "D", Amount = 250 },
                    new() { Date = new DateTime(2023, 6, 26), TransactionId = "T002", Type = "W", Amount = 20 },
                    new() { Date = new DateTime(2023, 6, 26), TransactionId = "T003", Type = "W", Amount = 100 }
                ]
        };

        this._accountQueriesMock.Setup(x => x.GetAccount("AC001")).Returns(account);
        this._interestRulesQueriesMock.Setup(x => x.GetAllInterstRules()).Returns(
            [
                new() { Date = new DateTime(2023, 6, 1), Rate = 1.90m },
                new() { Date = new DateTime(2023, 6, 15), Rate = 2.20m }
            ]);

        // Act
        var interest = _statementService.GetStatement("AC001", "2023", "06")?.Entries.LastOrDefault()?.Amount ?? 0;

        // Assert
        Assert.That(interest, Is.EqualTo(0.39m));
    }
}


