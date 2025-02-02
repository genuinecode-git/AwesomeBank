namespace AwesomeBank.API.Commands
{
    public class AddTransactionCommandHandler(IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<AddTransactionCommandHandler> logger) : IRequestHandler<AddTransactionCommand, AccountViewModel>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper=mapper;
        private readonly ILogger<AddTransactionCommandHandler> _logger = logger;
        public async Task<AccountViewModel> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            this._logger.LogInformation("[Start] Processing transaction for account : {AccountNumber}", request.AccountNumber);

            Account account = _unitOfWork.Accounts.FirstOrDefaultWithIncludes(a => a.AccountNumber == request.AccountNumber, a=>a.Transactions);

            if (account == null)
            {
                this._logger.LogInformation("[Processing] Creating new account for : {AccountNumber}", request.AccountNumber);

                account = new Account(request.AccountNumber);
                this._unitOfWork.Accounts.Add(account);
            }

            this._logger.LogInformation("[Processing] Adding transaction for : {AccountNumber}", request.AccountNumber);
            string transactionId= account.AddTransaction(request.Date, request.Type, request.Amount);
            this._unitOfWork.Accounts.Update(account);
            this._logger.LogInformation("[Completed] Added transaction for : {AccountNumber} - TransactionId : {TransactionId}", request.AccountNumber, transactionId);

            return _mapper.Map<AccountViewModel>(account);
        }
    }
}
