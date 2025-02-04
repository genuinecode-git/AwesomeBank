using AwesomeBank.API.Application.Models.Requests;
using AwesomeBank.API.Application.Queries;
using AwesomeBank.API.Application.Services;

namespace AwesomeBank.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IMediator mediator, ILogger<AccountController> logger, IAccountQueries accountQueries, IStatementService statementService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IAccountQueries _accountQueries = accountQueries;
    private readonly IStatementService _statementService = statementService;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpPost("AddTransaction")]
    public async Task<IActionResult> AddTransaction([FromBody] AddTransactionCommand command)
    {
        _logger.LogInformation("Received request to add transaction for account {AccountNumber}", command.AccountNumber);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{accountNumber}/{year}/{month}")]
    public IActionResult GetTransactions(string accountNumber, string year, string month)
    {
        return Ok(this._accountQueries.GetAccountReportForMonth(accountNumber, $"{year}{month}"));
    }

    [HttpGet("{AccountNumber}/GetStatement/{Year}/{Month}")]
    public IActionResult GetStatement(StatementRequest request)
    {
        _logger.LogInformation("Received request to get statement for account {AccountNumber}- {Year}-{Month}", request.AccountNumber, request.Year, request.Month);

        return Ok(this._statementService.GetStatement(request));
    }
}