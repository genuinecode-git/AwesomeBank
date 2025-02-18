namespace AwesomeBank.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InterestRuleController(IMediator mediator, ILogger<InterestRuleController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<InterestRuleController> _logger = logger;

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddInterestRuleCommand command)
    {
        _logger.LogInformation("Received request to add interest Rate {RuleId}", command.RuleId);

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}