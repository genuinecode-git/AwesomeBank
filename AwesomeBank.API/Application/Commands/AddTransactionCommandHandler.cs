namespace AwesomeBank.API.Application.Commands
{
    public class AddTransactionCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AddTransactionCommandHandler> logger) : IRequestHandler<AddTransactionCommand, AccountViewModel>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AddTransactionCommandHandler> _logger = logger;

        public async Task<AccountViewModel> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Start] Processing transaction for account : {AccountNumber}", request.AccountNumber);

            Account account = _unitOfWork.Accounts.FirstOrDefaultWithIncludes(a => a.AccountNumber == request.AccountNumber, a => a.Transactions);

            if (account == null)
            {
                _logger.LogInformation("[Processing] Creating new account for : {AccountNumber}", request.AccountNumber);

                account = new Account(request.AccountNumber);
                _unitOfWork.Accounts.Add(account);
            }

            if (request.Type.Equals(TransactionType.Withdrawal, StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogDebug("[Processing] Checking for : {AccountNumber} have enouch balance to withdraw.", request.AccountNumber);

                var balance = account.Transactions.Sum(s => s.Type.Equals(TransactionType.Withdrawal, StringComparison.OrdinalIgnoreCase) ? -s.Amount : s.Amount);
                if (balance < request.Amount)
                {
                    _logger.LogDebug("[Processing] {AccountNumber} do not have enouch balance ({Balance}) to withdraw.", request.AccountNumber, balance);
                    throw new ValidationException($"{request.AccountNumber} do not have enouch balance to withdraw.(current balance : {balance}$)");
                }
            }

            _logger.LogInformation("[Processing] Adding transaction for : {AccountNumber}", request.AccountNumber);
            string transactionId = account.AddTransaction(request.Date, request.Type, request.Amount);
            _unitOfWork.Accounts.Update(account);
            _logger.LogInformation("[Completed] Added transaction for : {AccountNumber} - TransactionId : {TransactionId}", request.AccountNumber, transactionId);

            return _mapper.Map<AccountViewModel>(account);
        }
    }
}