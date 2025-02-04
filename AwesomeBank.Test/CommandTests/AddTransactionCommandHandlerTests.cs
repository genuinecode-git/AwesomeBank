namespace AwesomeBank.CommandTests.Test;

[TestFixture]
public class AddTransactionCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<AddTransactionCommandHandler>> _loggerMock;
    private AddTransactionCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        this._unitOfWorkMock = new Mock<IUnitOfWork>();
        this._loggerMock = new Mock<ILogger<AddTransactionCommandHandler>>();
        this._mapperMock = new Mock<IMapper>();
        this._handler = new AddTransactionCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_Sucess_Command_IsValid()
    {
        // Arrange
        string accountNumber = "AC001";
        var account = new Account(accountNumber);
        _unitOfWorkMock.Setup(u => u.Accounts.FirstOrDefaultWithIncludes(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Expression<Func<Account, object>>[]>()))
            .Returns(account);

        var command = new AddTransactionCommand("AC001", DateTime.UtcNow, "D", 100);
        _mapperMock.Setup(m => m.Map<AccountViewModel>(It.IsAny<Account>())).Returns(new AccountViewModel());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);

        _unitOfWorkMock.Verify(u => u.Accounts.Update(account), Times.Once);
    }

    [Test]
    public async Task Handle_Sucess_Command_ValidBalance()
    {
        // Arrange
        var account = new Account("AC001");
        account.AddTransaction(DateTime.UtcNow, TransactionType.Deposit, 100);
        account.AddTransaction(DateTime.UtcNow, TransactionType.Withdrawal, 80);

        _unitOfWorkMock.Setup(u => u.Accounts.FirstOrDefaultWithIncludes(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Expression<Func<Account, object>>[]>()))
            .Returns(account);

        var command = new AddTransactionCommand("AC001", DateTime.UtcNow, TransactionType.Withdrawal, 20);
        _mapperMock.Setup(m => m.Map<AccountViewModel>(It.IsAny<Account>())).Returns(new AccountViewModel());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);

        _unitOfWorkMock.Verify(u => u.Accounts.Update(account), Times.Once);
    }

    [Test]
    public async Task Handle_Sucess_Command_AccountIsNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Accounts.FirstOrDefaultWithIncludes(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Expression<Func<Account, object>>[]>()))
            .Returns((Account)null);

        var command = new AddTransactionCommand("AC002", DateTime.UtcNow, "D", 200);
        _mapperMock.Setup(m => m.Map<AccountViewModel>(It.IsAny<Account>())).Returns(new AccountViewModel());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        _unitOfWorkMock.Verify(u => u.Accounts.Add(It.IsAny<Account>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Accounts.Update(It.IsAny<Account>()), Times.Once);
    }

    [Test]
    public void Handle_Exception_Command_InvalidTransactionType()
    {
        // Arrange
        var account = new Account("AC001");
        _unitOfWorkMock.Setup(u => u.Accounts.FirstOrDefaultWithIncludes(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Expression<Func<Account, object>>[]>()))
            .Returns(account);

        var command = new AddTransactionCommand("AC001", DateTime.UtcNow, "X", 100);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Does.Contain("Transaction type must be 'D' (Deposit) or 'W' (Withdrawal)."));
    }

    [Test]
    public void Handle_Exception_Command_InvalidBalance()
    {
        // Arrange
        var account = new Account("AC001");
        account.AddTransaction(DateTime.UtcNow, TransactionType.Deposit, 100);
        account.AddTransaction(DateTime.UtcNow, TransactionType.Withdrawal, 80);

        _unitOfWorkMock.Setup(u => u.Accounts.FirstOrDefaultWithIncludes(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Expression<Func<Account, object>>[]>()))
            .Returns(account);

        var command = new AddTransactionCommand("AC001", DateTime.UtcNow, TransactionType.Withdrawal, 100);

        // Act & Assert
        var ex = Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Does.Contain("AC001 do not have enouch balance to withdraw.(current balance : 20$)"));
    }
}