namespace AwesomeBank.CommandTests.Test;

[TestFixture]
public class AddInterestRuleCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<AddInterestRuleCommandHandler>> _loggerMock;
    private AddInterestRuleCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        this._unitOfWorkMock = new Mock<IUnitOfWork>();
        this._loggerMock = new Mock<ILogger<AddInterestRuleCommandHandler>>();
        this._mapperMock = new Mock<IMapper>();
        this._handler = new AddInterestRuleCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_Sucess_AddNewRule_IsValid()
    {
        // Arrange
        var date = new DateTime(2024, 3, 1);
        var command = new AddInterestRuleCommand(date, "RULE03", 3.0m);

        var newRule = new InterestRule("RULE03", date, 3.0m);
        var rulesList = new List<InterestRule> { newRule };

        this._unitOfWorkMock.Setup(u => u.InterestRules.FirstOrDefault(It.IsAny<Expression<Func<InterestRule, bool>>>()))
            .Returns((InterestRule)null);

        this._unitOfWorkMock.Setup(u => u.InterestRules.GetAll())
            .Returns(rulesList.AsQueryable());

        this._mapperMock.Setup(m => m.Map<InterestRuleViewModel>(It.IsAny<InterestRule>()))
            .Returns(new InterestRuleViewModel { RuleId = "RULE03", Rate = 3.0m, Date = date });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        this._unitOfWorkMock.Verify(u => u.InterestRules.Add(It.IsAny<InterestRule>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].RuleId, Is.EqualTo("RULE03"));
            Assert.That(result[0].Rate, Is.EqualTo(3.0m));
        });
    }

    [Test]
    public async Task Handle_Sucess_RemoveExistingRuleAndAddNewRule_IsValid()
    {
        // Arrange
        var date = new DateTime(2024, 2, 1);
        var command = new AddInterestRuleCommand(date, "RULE02", 2.5m);

        var existingRule = new InterestRule("RULE01", date, 1.9m);
        var newRule = new InterestRule("RULE02", date, 2.5m);
        var rulesList = new List<InterestRule> { newRule };

        this._unitOfWorkMock.Setup(u => u.InterestRules.FirstOrDefault(It.IsAny<Expression<Func<InterestRule, bool>>>()))
            .Returns(existingRule);

        this._unitOfWorkMock.Setup(u => u.InterestRules.GetAll())
            .Returns(rulesList.AsQueryable());

        this._mapperMock.Setup(m => m.Map<InterestRuleViewModel>(It.IsAny<InterestRule>()))
            .Returns(new InterestRuleViewModel { RuleId = "RULE02", Rate = 2.5m, Date = date });

        // Act
        var result = await this._handler.Handle(command, CancellationToken.None);

        // Assert
        this._unitOfWorkMock.Verify(u => u.InterestRules.Remove(existingRule), Times.Once);
        this._unitOfWorkMock.Verify(u => u.InterestRules.Add(It.IsAny<InterestRule>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0].RuleId, Is.EqualTo("RULE02"));
            Assert.That(result[0].Rate, Is.EqualTo(2.5m));
            Assert.That(result.Count, Is.EqualTo(1));
        });
    }

    [Test]
    public void Handle_ThrowException_Rule_RateIsInvalid()
    {
        // Arrange
        var command = new AddInterestRuleCommand(DateTime.UtcNow, "RULE01", 0);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo("Interest rate must be between 0 and 100."));
    }
}