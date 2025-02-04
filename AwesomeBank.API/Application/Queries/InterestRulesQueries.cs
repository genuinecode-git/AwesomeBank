namespace AwesomeBank.API.Application.Queries
{
    public class InterestRulesQueries(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InterestRulesQueries> logger) : IInterestRulesQueries
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<InterestRulesQueries> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public List<InterestRuleViewModel> GetAllInterstRules()
        {
            this._logger.LogDebug("Getting all interst rate rules.");
            return this._mapper.Map<List<InterestRuleViewModel>>(_unitOfWork.InterestRules.GetAll());
        }
    }
}