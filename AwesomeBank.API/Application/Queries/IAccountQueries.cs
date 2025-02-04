namespace AwesomeBank.API.Application.Queries
{
    public interface IAccountQueries
    {
        AccountViewModel GetAccountReportForMonth(string accountNumber, string yearMonth);

        AccountViewModel GetAccount(string accountNumber);
    }
}