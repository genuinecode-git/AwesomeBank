using AwesomeBank.API.Application.Models.Requests;

namespace AwesomeBank.API.Application.Services
{
    public interface IStatementService
    {
        AccountStatementModel GetStatement(StatementRequest request);
    }
}