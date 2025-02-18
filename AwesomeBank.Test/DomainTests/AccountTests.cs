namespace AwesomeBank.Test.Domain;

[TestFixture]
public class AccountTests
{
    [Test]
    public void Constructor_Sucess_AccountNumber_IsValid()
    {
        string dummyAccountNumber = "AC001";
        Account dummyAccount = new(dummyAccountNumber);
        Assert.That(dummyAccountNumber, Is.EqualTo(dummyAccount.AccountNumber));
    }

    [Test]
    public void Constructor_ArgumentNullException_AccountNumber_IsNull()
    {
        ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new Account(null));
        Assert.That(ex.Message, Does.Contain("accountNumber is required."));
    }

    [Test]
    public void Constructor_ArgumentNullException_AccountNumber_Empty()
    {
        ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new Account(string.Empty));
        Assert.That(ex.Message, Does.Contain("accountNumber is required."));
    }

    [Test]
    public void AddTransaction_Sucess_TransactionDetails_IsValid()
    {
        string dummyAccountNumber = "AC001";
        Account dummyAccount = new(dummyAccountNumber);
        DateTime dateNow = DateTime.UtcNow;

        dummyAccount.AddTransaction(DateTime.UtcNow, TransactionType.Deposit, 100.52m);

        string dummyTransactionId = $"{dateNow.Date:yyyyMMdd}-{dummyAccount.Transactions.Count:D2}";

        Assert.That(dummyAccount.Transactions.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dummyAccount.Transactions[0].Type, Is.EqualTo(TransactionType.Deposit));
            Assert.That(dummyAccount.Transactions[0].Amount, Is.EqualTo(100.52m));
            Assert.That(dummyAccount.Transactions[0].Date.Date, Is.EqualTo(dateNow.Date));
            Assert.That(dummyAccount.Transactions[0].TransactionId, Is.EqualTo(dummyTransactionId));
        });
    }

    [Test]
    public void AddTransaction_ArgumentNullException_TransactionDetails_InValid()
    {
        string dummyAccountNumber = "AC001";
        Account dummyAccount = new(dummyAccountNumber);

        ArgumentException ex = Assert.Throws<ArgumentException>(() => dummyAccount.AddTransaction(new DateTime(), TransactionType.Deposit, 100.52m));

        Assert.That(ex.Message.ToLower(), Does.Contain("transaction"));
    }
}