namespace AwesomeBank.Test.Domain;

public class TransactionTests
{
    [Test]
    public void Constructor_Sucess_AllParameters_IsValid()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = TransactionType.Deposit;
        decimal amount = 101.30m;

        Transaction transaction = new(dummyTransactionId, dateNow, type, amount);
        Assert.Multiple(() =>
        {
            Assert.That(transaction.Type, Is.EqualTo(type));
            Assert.That(transaction.Amount, Is.EqualTo(amount));
            Assert.That(transaction.Date.Date, Is.EqualTo(dateNow.Date));
            Assert.That(transaction.TransactionId, Is.EqualTo(dummyTransactionId));
        });
    }

    [Test]
    public void Constructor_Sucess_AllParametersTypeIsLowerCase_IsValid()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = TransactionType.Deposit.ToLower();
        decimal amount = 101.30m;

        Transaction transaction = new(dummyTransactionId, dateNow, type, amount);
        Assert.Multiple(() =>
        {
            Assert.That(transaction.Type, Is.EqualTo(type.ToUpper()));
            Assert.That(transaction.Amount, Is.EqualTo(amount));
            Assert.That(transaction.Date.Date, Is.EqualTo(dateNow.Date));
            Assert.That(transaction.TransactionId, Is.EqualTo(dummyTransactionId));
        });
    }

    [Test]
    public void Constructor_ArgumentException_TransactionId_IsEmpty()
    {
        DateTime dateNow = DateTime.UtcNow;
        string type = TransactionType.Deposit;
        decimal amount = 101.30m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(string.Empty, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction Id is required."));
    }

    [Test]
    public void Constructor_ArgumentException_Date_IsInvalid()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = new DateTime();
        string type = TransactionType.Deposit;
        decimal amount = 101.30m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Invalid transaction date."));
    }

    [Test]
    public void Constructor_ArgumentException_Type_IsInvalid()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = "E";
        decimal amount = 101.30m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction type must be 'D' (Deposit) or 'W' (Withdrawal)."));
    }

    [Test]
    public void Constructor_ArgumentException_Type_IsEmpty()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = "";
        decimal amount = 101.30m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction type is required and length must be 1."));
    }

    [Test]
    public void Constructor_ArgumentException_Type_IsLargeinLength()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = "ABC";
        decimal amount = 101.30m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction type is required and length must be 1."));
    }

    [Test]
    public void Constructor_ArgumentException_Amount_IsNegative()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = "ABC";
        decimal amount = -10.0m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction Amount must be greater than zero."));
    }

    [Test]
    public void Constructor_ArgumentException_Amount_IsZero()
    {
        string dummyTransactionId = "20230505-01";
        DateTime dateNow = DateTime.UtcNow;
        string type = "ABC";
        decimal amount = 0;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction Amount must be greater than zero."));
    }

    [Test]
    public void Constructor_ArgumentException_Allparameters_IsInvalid()
    {
        string dummyTransactionId = string.Empty;
        DateTime dateNow = new DateTime();
        string type = "ABC";
        decimal amount = 0;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new Transaction(dummyTransactionId, dateNow, type, amount));
        Assert.That(ex.Message, Does.Contain("Transaction Amount must be greater than zero."));
        Assert.That(ex.Message, Does.Contain("Transaction type is required and length must be 1."));
        Assert.That(ex.Message, Does.Contain("Invalid transaction date."));
        Assert.That(ex.Message, Does.Contain("Transaction Id is required."));
    }
}