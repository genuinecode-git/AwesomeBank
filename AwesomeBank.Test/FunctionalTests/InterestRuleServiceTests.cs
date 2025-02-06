namespace AwesomeBank.Test.FunctionalTests;

[TestFixture]
public class InterestRuleServiceTests
{
    private Mock<ICommandHandleHelper> _commandHandleMock;
    private InterestRuleService _interestRuleService;

    [SetUp]
    public void SetUp()
    {
        _commandHandleMock = new Mock<ICommandHandleHelper>();
        _interestRuleService = new InterestRuleService(_commandHandleMock.Object);
    }

    [Test]
    public async Task DefineInterestRulesAsync_Sucess_ValidInput()
    {
        // Arrange
        var expectedResults = new List<InterestRuleViewModel>
        {
            new InterestRuleViewModel
            {
                Date = new DateTime(2023, 01, 01),
                RuleId = "Rule1",
                Rate = 1.5m
            }
        };

        _commandHandleMock.Setup(x => x.HandleCommandAsync<AddInterestRuleCommand, AddInterestRuleCommandValidator, IEnumerable<InterestRuleViewModel>>(
                It.IsAny<AddInterestRuleCommand>(),
                It.IsAny<AddInterestRuleCommandValidator>()
            ))
            .ReturnsAsync(expectedResults);

        var userInput = "20230101 Rule1 1.5";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _interestRuleService.DefineInterestRulesAsync();

        // Assert
        var output = consoleOutput.ToString();

        _commandHandleMock.Verify(x => x.HandleCommandAsync<AddInterestRuleCommand, AddInterestRuleCommandValidator, IEnumerable<InterestRuleViewModel>>(
                It.IsAny<AddInterestRuleCommand>(),
                It.IsAny<AddInterestRuleCommandValidator>()
            ), Times.Once);

        Assert.Multiple(() =>
        {
            Assert.That(output.Contains("Interest rules:"), Is.True);
            Assert.That(output.Contains("20230101"), Is.True);
            Assert.That(output.Contains("Rule1"), Is.True);
            Assert.That(output.Contains("1.50"), Is.True);
        });
    }

    [Test]
    public async Task DefineInterestRulesAsync_ValidationMessage_InvalidDateFormat()
    {
        // Arrange
        var userInput = "20231 Rule1 1.5";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _interestRuleService.DefineInterestRulesAsync();

        // Assert
        var output = consoleOutput.ToString();

        _commandHandleMock.Verify(x => x.HandleCommandAsync<AddInterestRuleCommand, AddInterestRuleCommandValidator, IEnumerable<InterestRuleViewModel>>(
            It.IsAny<AddInterestRuleCommand>(),
            It.IsAny<AddInterestRuleCommandValidator>()
        ), Times.Never);

        Assert.That(output.Contains("Invalid date format. Use YYYYMMdd."), Is.True);
    }

    [Test]
    public async Task DefineInterestRulesAsync_ValidationMessage_InvalidRate()
    {
        // Arrange
        var userInput = "20230101 Rule1 InvalidRate";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _interestRuleService.DefineInterestRulesAsync();

        // Assert
        var output = consoleOutput.ToString();

        _commandHandleMock.Verify(x => x.HandleCommandAsync<AddInterestRuleCommand, AddInterestRuleCommandValidator, IEnumerable<InterestRuleViewModel>>(
            It.IsAny<AddInterestRuleCommand>(),
            It.IsAny<AddInterestRuleCommandValidator>()
        ), Times.Never);

        Assert.That(output.Contains("Invalid rate. It must be number or decimal (ex: 1.01)."), Is.True);
    }

    [Test]
    public async Task DefineInterestRulesAsync_DoNothing_EmptyInput()
    {
        // Arrange
        var userInput = "";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        // Act
        await _interestRuleService.DefineInterestRulesAsync();

        // Assert
        _commandHandleMock.Verify(x => x.HandleCommandAsync<AddInterestRuleCommand, AddInterestRuleCommandValidator, IEnumerable<InterestRuleViewModel>>(
            It.IsAny<AddInterestRuleCommand>(),
            It.IsAny<AddInterestRuleCommandValidator>()
        ), Times.Never);
    }

    [Test]
    public async Task DefineInterestRulesAsync_Sucess_ValidData()
    {
        // Arrange
        var expectedResults = new List<InterestRuleViewModel>
        {
            new InterestRuleViewModel
            {
                Date = new DateTime(2023, 01, 01),
                RuleId = "Rule1",
                Rate = 1.5m
            }
        };

        _commandHandleMock.Setup(x => x.HandleCommandAsync<AddInterestRuleCommand, AddInterestRuleCommandValidator, IEnumerable<InterestRuleViewModel>>(
                It.IsAny<AddInterestRuleCommand>(),
                It.IsAny<AddInterestRuleCommandValidator>()
            ))
            .ReturnsAsync(expectedResults);

        var userInput = "20230101 Rule1 1.5";
        System.Console.SetIn(new System.IO.StringReader(userInput));

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        await _interestRuleService.DefineInterestRulesAsync();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Multiple(() =>
        {
            Assert.That(output.Contains("Interest rules:"), Is.True);
            Assert.That(output.Contains("20230101"), Is.True);
            Assert.That(output.Contains("Rule1"), Is.True);
            Assert.That(output.Contains("1.50"), Is.True);
        });
    }
}