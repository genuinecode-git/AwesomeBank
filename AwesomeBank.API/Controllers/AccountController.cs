
using AwesomeBank.API.Commands;

namespace AwesomeBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator, ILogger<AccountController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<AccountController> _logger = logger;


        [HttpPost("add-transaction")]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionCommand command)
        {
            _logger.LogInformation("Received request to add transaction for account {AccountNumber}", command.AccountNumber);

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding transaction for account {AccountNumber}", command.AccountNumber);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
