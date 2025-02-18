namespace AwesomeBank.API.Application.Commands
{
    public class AddInterestRuleCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AddInterestRuleCommandHandler> logger) : IRequestHandler<AddInterestRuleCommand, List<InterestRuleViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AddInterestRuleCommandHandler> _logger = logger;

        public async Task<List<InterestRuleViewModel>> Handle(AddInterestRuleCommand request, CancellationToken cancellationToken)
        {
            this._logger.LogInformation("[Start] Processing interest rule: {RuleId} - {Rate}%", request.RuleId, request.Rate);

            // Validate input
            if (request.Rate <= 0 || request.Rate >= 100)
            {
                throw new ArgumentException("Interest rate must be between 0 and 100.");
            }

            // Check if there's an existing rule on the same date
            InterestRule existingRule = this._unitOfWork.InterestRules
                .FirstOrDefault(r => r.Date.Date == request.Date.Date);

            if (existingRule != null)
            {
                this._unitOfWork.InterestRules.Remove(existingRule);
                this._logger.LogInformation("[Processing] Existing interest rule on {Date} removed.", request.Date.ToString("yyyyMMdd"));
            }

            // Add new interest rule
            InterestRule newRule = new(request.RuleId, request.Date, request.Rate);
            this._unitOfWork.InterestRules.Add(newRule);

            this._logger.LogInformation("[Completed] New interest rule added: {RuleId} - {Rate}%", request.RuleId, request.Rate);

            return [.. _unitOfWork.InterestRules.GetAll()
                        .OrderBy(r => r.Date)
                        .Select(s=> this._mapper.Map<InterestRuleViewModel>(s))];
        }
    }
}