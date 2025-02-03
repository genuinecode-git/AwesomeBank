using AwesomeBank.API.Application.Queries;
using System.Data;
using System.Linq;

namespace AwesomeBank.API.Application.Services;

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
            balance += transaction.Type.Equals("W", StringComparison.CurrentCultureIgnoreCase) ? -transaction.Amount : transaction.Amount;
            statementEntries.Add(new StatementEntryModel(transaction.Date, transaction.TransactionId, transaction.Type, transaction.Amount, balance));
        }

        int yearInt = int.Parse(year);
        int monthInt = int.Parse(month);

        decimal interest = CalculateInterest_Optimized(account, yearInt, monthInt);

        if (interest > 0)
        {
            balance += interest;
            statementEntries.Add(new StatementEntryModel(new DateTime(yearInt, monthInt, DateTime.DaysInMonth(yearInt, monthInt)), "", "I", interest, balance));
        }

        return new AccountStatementModel(account.AccountNumber, [..statementEntries.Where(x=>x.Date >= new DateTime(yearInt, monthInt, 1).Date)]);
    }

    private decimal CalculateInterest_Optimized(AccountViewModel account, int year, int month)
    {
        this._logger.LogInformation("Starting interest calculation for account {AccountId} for {Year}-{Month}.", account.AccountNumber, year, month);
        DateTime dateStart = new DateTime(year, month, 1).Date;
        DateTime dateEnd = dateStart.GetLastDayOfMonth();
        this._logger.LogDebug("Date range for calculation: {DateStart} to {DateEnd}.", dateStart, dateEnd);

        List<InterestRuleViewModel> interestRules = this._interestRulesQueries.GetAllInterstRules()
            .Where(x => x.Date <= dateEnd)
            .OrderBy(r => r.Date)
            .ToList()
            .FillEndDates();

        if (interestRules.Count == 0)
        {
            this._logger.LogError("No interest rules defined for the given date range.");
            throw new ArgumentException("No Rules defined.");
        }

        this._logger.LogDebug("Retrieved {InterestRuleCount} interest rules.", interestRules.Count);

        decimal balance = account.Transactions
            .Where(x => x.Date.Date < dateStart)
            .Sum(s => s.Type.Equals("W", StringComparison.OrdinalIgnoreCase) ? -s.Amount : s.Amount);
        this._logger.LogDebug("Initial balance before interest calculation: {Balance}.", balance);

        List<StatementEntryRecordModel> records =[new(dateStart, balance, 0m)];
        this._logger.LogDebug("Initial record added with balance: {Balance}.", balance);

        List<TransactionViewModel> dailyBalances = [.. account.Transactions
            .Where(x => x.Date >= dateStart && x.Date <= dateEnd)
            .GroupBy(x => x.Date.Date)
            .Select(g => new TransactionViewModel
            {
                Date = g.Key,
                Amount = g.Sum(t => t.Type.Equals("D", StringComparison.OrdinalIgnoreCase) ? t.Amount : -t.Amount),
                Type = "D"
            })
            .OrderBy(x => x.Date)];

        this._logger.LogDebug("Processed {DailyBalanceCount} daily balances.", dailyBalances.Count);

        foreach (var transaction in dailyBalances)
        {
            balance += transaction.Type.Equals("W", StringComparison.OrdinalIgnoreCase) ? -transaction.Amount : transaction.Amount;
            this._logger.LogDebug("Updated balance after transaction on {TransactionDate}: {Balance}.", transaction.Date, balance);

            List<InterestRuleViewModel> rulesToApply = [.. interestRules.Where(x => x.EndDate.Value.Date > transaction.Date.Date).OrderBy(o => o.Date)];
            DateTime nextItemDate = dailyBalances.FirstOrDefault(t => t.Date > transaction.Date)?.Date.AddDays(-1) ?? DateTime.MaxValue;
            bool isLastRecord = nextItemDate == DateTime.MaxValue;
            this._logger.LogDebug("Applying {RuleCount} interest rules for transaction on {TransactionDate}.", rulesToApply.Count, transaction.Date);

            foreach (var rule in rulesToApply)
            {
                int numberOfDays = CalculateNumberOfDays(rule, transaction.Date, nextItemDate, isLastRecord, dateEnd);
                decimal interest = balance * rule.Rate * numberOfDays / 100;
                records.Add(new StatementEntryRecordModel(transaction.Date.Date, balance, interest));
                this._logger.LogDebug("Applied interest rule {RuleId} for {NumberOfDays} days. Interest calculated: {Interest}.", rule.RuleId, numberOfDays, interest);
            }
        }

        return Math.Round(records.Sum(x => x.Interst) / 365, 2);
    }

    private int CalculateNumberOfDays(InterestRuleViewModel rule, DateTime transactionDate, DateTime nextItemDate, bool isLastRecord, DateTime dateEnd)
    {
        if (rule.Date.Date >= transactionDate.Date && nextItemDate <= rule.EndDate.Value.Date)
        {
            return (nextItemDate - rule.Date.Date).Days + 1;
        }
        if (rule.Date.Date >= transactionDate.Date && nextItemDate > rule.EndDate.Value.Date)
        {
            return (rule.EndDate.Value.Date - rule.Date.Date).Days + 1;
        }
        if (rule.Date.Date < transactionDate.Date && nextItemDate <= rule.EndDate.Value.Date)
        {
            return (nextItemDate - transactionDate.Date).Days + 1;
        }
        if (rule.Date.Date < transactionDate.Date && nextItemDate > rule.EndDate.Value.Date)
        {
            return ((isLastRecord ? dateEnd : rule.EndDate.Value.Date) - transactionDate.Date).Days + 1;
        }
        return 0;
    }


    private decimal CalculateInterest_Initial(AccountViewModel account, int year, int month)
    {
        List<StatementEntryRecordModel> records = [];
        decimal balance = 0m;
        DateTime lastInterestDate = account.Transactions.Min(t => t.Date).Date;
        DateTime dateStart = new DateTime(year, month, 1).Date;
        DateTime dateEnd = dateStart.GetLastDayOfMonth();

        //Avoid geting future rules 
        List<InterestRuleViewModel> interestRules = this._interestRulesQueries.GetAllInterstRules()
            .Where(x => x.Date <= dateEnd)
            .OrderBy(r => r.Date).ToList()
            .FillEndDates();

        if (interestRules == null) throw new ArgumentException("No Rules defined.");


        //Calculate all sum before strat interst rules 
        var transactionsEffectedRates = account.Transactions.Where(x => x.Date.Date < dateStart);
        balance = transactionsEffectedRates.Sum(s => s.Type.Equals("W") ? -s.Amount : s.Amount);

        records.Add(new(interestRules[0].Date.Date.AddDays(-1), balance, 0m));

        //calculate interst and add that to balance , for records till given month start
        //Get remaining transactions eligible for calculation

        List<TransactionViewModel> dailyBalances = [.. account.Transactions.Where(x => x.Date >= dateStart && x.Date <= dateEnd)
            .GroupBy(x => x.Date.Date).Select(g => new TransactionViewModel()
            {
                Date = g.Key,
                Amount = g.Sum(t => t.Type.Equals("D", StringComparison.OrdinalIgnoreCase) ? t.Amount : -t.Amount),
                Type = "D"
            })
            .OrderBy(x => x.Date)];


        LinkedList<TransactionViewModel> transactionsLinkedList = new(dailyBalances);

        if (transactionsLinkedList != null)
        {
            LinkedListNode<TransactionViewModel> current = transactionsLinkedList.First;
            while (current != null)
            {

                //add current transaction to balance 
                balance += current.Value.Type.Equals("W") ? -current.Value.Amount : current.Value.Amount;
                //Only rules valid within period
                var rulesToApply = interestRules.Where(x => x.EndDate.Value.Date > current.Value.Date.Date).OrderBy(o => o.Date).ToList();
                DateTime nextItemDate = current.Next == null ? DateTime.MaxValue : current.Next.Value.Date.Date.AddDays(-1);
                bool isLastRecord = current.Next == null;

                decimal interst = 0m;
                foreach (var rule in rulesToApply)
                {
                    int numberOfDays = 0;

                    //Get Number of days 
                    if (rule.Date.Date >= current.Value.Date.Date && nextItemDate <= rule.EndDate.Value.Date)
                    {
                        numberOfDays = (nextItemDate - rule.Date.Date).Days + 1;
                    }
                    else
                    {
                        if (rule.Date.Date >= current.Value.Date.Date && nextItemDate > rule.EndDate.Value.Date)
                        {
                            numberOfDays = (rule.EndDate.Value.Date - rule.Date.Date).Days + 1;
                        }
                        else
                        {
                            if (rule.Date.Date < current.Value.Date.Date && nextItemDate <= rule.EndDate.Value.Date)
                            {
                                numberOfDays = (current.Value.Date.Date - nextItemDate).Days + 1;
                            }
                            else
                            {
                                if (rule.Date.Date < current.Value.Date.Date && nextItemDate > rule.EndDate.Value.Date)
                                {
                                    numberOfDays = ((isLastRecord ? dateEnd : rule.EndDate.Value.Date) - current.Value.Date.Date).Days + 1;
                                }
                            }
                        }
                    }

                    interst = (balance * rule.Rate * numberOfDays / 100);
                    records.Add(new(current.Value.Date.Date, balance, interst));
                }

                current = current.Next; // Move to the next node
            }
        }


        return Math.Round(records.Sum(x => x.Interst) / 365, 2); // Annualized interest
    }

}
