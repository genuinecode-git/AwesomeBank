using AwesomeBank.Domain.Entities;

namespace AwesomeBank.API.Application.Helpers;

public static class TransactionLinkedListHelper
{
    public static List<TransactionViewModel> GetTransactionsBeforeByDate(this LinkedList<TransactionViewModel> transactions, DateTime filterDate)
    {
        List<TransactionViewModel> result = [];
        LinkedListNode<TransactionViewModel> current = transactions.First;

        while (current != null && current.Next != null)
        {
            if (current.Next.Value.Date.Date < filterDate)
            {
                result.Add(current.Value);
            }
            current = current.Next;
        }

        return result;
    }
    public static DateTime GetLastDayOfMonth(this DateTime date)
    {
        int lastDay = DateTime.DaysInMonth(date.Year, date.Month);
        return new DateTime(date.Year, date.Month, lastDay);
    }
    public static List<InterestRuleViewModel> FillEndDates(this List<InterestRuleViewModel> rules)
    {
        var interestRules = rules.OrderBy(r => r.Date).ToList();

        for (int i = 0; i < interestRules.Count - 1; i++)
        {
            interestRules[i].EndDate = interestRules[i + 1].Date.AddDays(-1);
        }

        interestRules[^1].EndDate = DateTime.MaxValue;
        return interestRules;
    }

}
