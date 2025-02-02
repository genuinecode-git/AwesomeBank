using AwesomeBank.API.Application.Queries;

namespace AwesomeBank.API.Application.Services
{
    public class StatementService(IAccountQueries accountQueries, IInterestRulesQueries interestRulesQueries, ILogger<StatementService> logger) : IStatementService
    {
        private readonly IAccountQueries _accountQueries = accountQueries;
        private readonly IInterestRulesQueries _interestRulesQueries = interestRulesQueries;
        private readonly ILogger<StatementService> _logger = logger;

        public AccountStatementModel GetStatement(string accountNumber, string year, string month)
        {
            var account = _accountQueries.GetAccount(accountNumber);

            if (account == null)
            {
                this._logger.LogWarning("Account {AccountNumber} not found.", accountNumber);
                return null;
            }


            decimal balance = 0;
            var statementEntries = new List<StatementEntryModel>();

            foreach (var transaction in account.Transactions)
            {
                this._logger.LogDebug("Statement balance calculating for : {Transaction}", transaction.TransactionId);
                balance += transaction.Type.Equals("D", StringComparison.CurrentCultureIgnoreCase) ? transaction.Amount : -transaction.Amount;
                statementEntries.Add(new StatementEntryModel(transaction.Date, transaction.TransactionId, transaction.Type, transaction.Amount, balance));
            }

            int yearInt = int.Parse(year);
            int monthInt = int.Parse(month);

            var filteredAccount = _accountQueries.GetAccountReportForMonth(accountNumber, $"{year}{month}");

            decimal interest = CalculateInterest(filteredAccount, yearInt, monthInt);

            if (interest > 0)
            {
                balance += interest;
                statementEntries.Add(new StatementEntryModel(new DateTime(yearInt, monthInt, DateTime.DaysInMonth(yearInt, monthInt)), "", "I", interest, balance));
            }

            return new AccountStatementModel(account.AccountNumber, statementEntries);
        }

        private decimal CalculateInterest(AccountViewModel account, int year, int month)
        {
            this._logger.LogDebug("Calculating Interset for Month : {Month}, Year : {Year}", year, month);

            var dailyBalances = new Dictionary<DateTime, decimal>();

            decimal balance = 0;
            foreach (var transaction in account.Transactions)
            {
                this._logger.LogDebug("Calculating end of the day balance for transaction : {TransactionId}", transaction.TransactionId);
                balance += transaction.Type.Equals("D", StringComparison.CurrentCultureIgnoreCase) ? transaction.Amount : -transaction.Amount;
                dailyBalances[transaction.Date] = balance;
            }

            decimal totalInterest = 0;

            this._logger.LogDebug("Calculating Total interest");
            object lockObj = new(); //for parallel operations

            var interestRules = this._interestRulesQueries.GetAllInterstRules().OrderBy(r => r.Date).ToList();

            Parallel.ForEach(interestRules, rule =>
            {
                decimal localInterest = 0;
                foreach (var date in dailyBalances.Keys)
                {
                    if (date >= rule.Date)
                    {
                        localInterest += dailyBalances[date] * (rule.Rate / 100) * 1;
                    }
                }

                lock (lockObj)
                {
                    totalInterest += localInterest;
                }
            });

            return Math.Round(totalInterest / 365, 2);
        }
    }
}
