namespace AwesomeBank.API.Application.Services
{
    public interface IStatementService
    {
        AccountStatementModel GetStatement(string accountNumber, string year, string month);
    }
}
