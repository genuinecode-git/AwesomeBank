namespace AwesomeBank.Test.Domain;

public class InterestRuleTests
{
    [Test]
    public void Constructor_Sucess_AllParameters_IsValid()
    {
        string dummyRuleId = "RULE-01";
        DateTime dateNow = DateTime.UtcNow;
        decimal rate = 10.50m;

        InterestRule interestRule = new(dummyRuleId, dateNow, rate);
        Assert.Multiple(() =>
        {
            Assert.That(interestRule.Rate, Is.EqualTo(rate));
            Assert.That(interestRule.Date.Date, Is.EqualTo(dateNow.Date));
            Assert.That(interestRule.RuleId, Is.EqualTo(dummyRuleId));
        });
    }

    [Test]
    public void Constructor_Sucess_AllParametersRateIsZero_IsValid()
    {
        string dummyRuleId = "RULE-01";
        DateTime dateNow = DateTime.UtcNow;
        decimal rate = 0;

        InterestRule interestRule = new(dummyRuleId, dateNow, rate);
        Assert.Multiple(() =>
        {
            Assert.That(interestRule.Rate, Is.EqualTo(rate));
            Assert.That(interestRule.Date.Date, Is.EqualTo(dateNow.Date));
            Assert.That(interestRule.RuleId, Is.EqualTo(dummyRuleId));
        });
    }

    [Test]
    public void Constructor_Sucess_AllParametersRateIsHundrad_IsValid()
    {
        string dummyRuleId = "RULE-01";
        DateTime dateNow = DateTime.UtcNow;
        decimal rate = 100;

        InterestRule interestRule = new(dummyRuleId, dateNow, rate);
        Assert.Multiple(() =>
        {
            Assert.That(interestRule.Rate, Is.EqualTo(rate));
            Assert.That(interestRule.Date.Date, Is.EqualTo(dateNow.Date));
            Assert.That(interestRule.RuleId, Is.EqualTo(dummyRuleId));
        });
    }

    [Test]
    public void Constructor_ArgumentException_RuleId_IsEmpty()
    {
        string dummyRuleId = string.Empty;
        DateTime dateNow = DateTime.UtcNow;
        decimal rate = 10.50m;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new InterestRule(dummyRuleId, dateNow, rate));
        Assert.That(ex.Message, Does.Contain("Rule Id is required."));
    }

    [Test]
    public void Constructor_ArgumentException_Date_IsInvalid()
    {
        string dummyRuleId = "RULE-01";
        decimal rate = 10.50m;
        DateTime dateNow = new();

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new InterestRule(dummyRuleId, dateNow, rate));
        Assert.That(ex.Message, Does.Contain("Invalid rule date."));
    }

    [Test]
    public void Constructor_ArgumentException_Rate_IsHigher()
    {
        string dummyRuleId = "RULE-01";
        decimal rate = 103.50m;
        DateTime dateNow = DateTime.UtcNow;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new InterestRule(dummyRuleId, dateNow, rate));
        Assert.That(ex.Message, Does.Contain("Rate must be in between 0 and 100"));
    }

    [Test]
    public void Constructor_ArgumentException_Rate_IsLower()
    {
        string dummyRuleId = "RULE-01";
        decimal rate = -1;
        DateTime dateNow = DateTime.UtcNow;

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new InterestRule(dummyRuleId, dateNow, rate));
        Assert.That(ex.Message, Does.Contain("Rate must be in between 0 and 100"));
    }

    [Test]
    public void Constructor_ArgumentException_Allparameters_IsInvalid()
    {
        string dummyRuleId = string.Empty;
        decimal rate = 150;
        DateTime dateNow = new DateTime();

        ArgumentException ex = Assert.Throws<ArgumentException>(() => new InterestRule(dummyRuleId, dateNow, rate));
        Assert.That(ex.Message, Does.Contain("Rate must be in between 0 and 100"));
        Assert.That(ex.Message, Does.Contain("Invalid rule date."));
        Assert.That(ex.Message, Does.Contain("Rule Id is required."));
    }
}