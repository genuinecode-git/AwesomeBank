namespace AwesomeBank.API.Application.Queries
{
    public class AccountQueries(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AccountQueries> logger) : IAccountQueries
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AccountQueries> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public AccountViewModel GetAccountReportForMonth(string accountNumber, string yearMonth)
        {
            _logger.LogInformation("Retrieving account {AccountNumber}", accountNumber);

            Account account = _unitOfWork.Accounts.FirstOrDefaultWithIncludes(a => a.AccountNumber == accountNumber, a => a.Transactions)
                ?? throw new ArgumentException("Account Not found.");

            var transactionsForMonth = account.Transactions
                .Where(t => t.Date.ToString("yyyyMM") == yearMonth)
                .Select(_mapper.Map<TransactionViewModel>)
                .OrderBy(t => t.Date);

            AccountViewModel result = _mapper.Map<AccountViewModel>(account);
            result.Transactions = [.. transactionsForMonth];
            return result;
        }

        public AccountViewModel GetAccount(string accountNumber)
        {
            _logger.LogInformation("Retrieving account {AccountNumber}", accountNumber);

            Account account = _unitOfWork.Accounts.FirstOrDefaultWithIncludes(a => a.AccountNumber == accountNumber, a => a.Transactions)
                ?? throw new ArgumentException("Account Not found.");

            return _mapper.Map<AccountViewModel>(account);
        }
    }
}