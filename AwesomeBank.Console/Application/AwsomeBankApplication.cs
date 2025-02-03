using System.Globalization;

namespace AwesomeBank.Console.Application;

public static class AwsomeBankApplication
{
    public static void StartApplication()
    {
        while (true)
        {
            System.Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            System.Console.WriteLine("[T] Input transactions");
            System.Console.WriteLine("[I] Define interest rules");
            System.Console.WriteLine("[P] Print statement");
            System.Console.WriteLine("[Q] Quit");
            System.Console.Write("> ");

            string choice = System.Console.ReadLine().ToUpper();
            switch (choice)
            {
                case "T": InputTransactions(); break;
                case "I": DefineInterestRules(); break;
                case "P": PrintStatement(); break;
                case "Q": return;
                default: System.Console.WriteLine("Invalid option, try again."); break;
            }
        }
    }

    static void InputTransactions()
    {
        System.Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format (or enter blank to go back):");

        while (true)
        {
            System.Console.Write("> ");
            string input = System.Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) break;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 4)
            {
                System.Console.WriteLine("Invalid format. Please use <YYYYMMdd> <Account> <D/W> <Amount>");
                continue;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                System.Console.WriteLine("Invalid date format. Use YYYYMMdd.");
                continue;
            }

            string account = parts[1].Trim();
            string type = parts[2].Trim().ToUpper();

            if (type != "D" && type != "W")
            {
                System.Console.WriteLine("Invalid type. Use D for deposit or W for withdrawal.");
                continue;
            }

            if (!decimal.TryParse(parts[3], out decimal amount) || amount <= 0 || decimal.Round(amount, 2) != amount)
            {
                System.Console.WriteLine("Invalid amount. Must be greater than 0 with up to 2 decimal places.");
                continue;
            }

            //accounts[account].Add(new Transaction(date, type, amount));
            System.Console.WriteLine("Transaction recorded.");
        }
    }
    static void DefineInterestRules()
    {
        System.Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format (or enter blank to go back):");

        while (true)
        {
            System.Console.Write("> ");
            string input = System.Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) break;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                System.Console.WriteLine("Invalid format. Please use <YYYYMMdd> <RuleId> <Rate>");
                continue;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                System.Console.WriteLine("Invalid date format. Use YYYYMMdd.");
                continue;
            }

            string ruleId = parts[1].Trim();

            if (!decimal.TryParse(parts[2], out decimal rate) || rate <= 0 || rate >= 100)
            {
                System.Console.WriteLine("Invalid rate. It must be greater than 0 and less than 100.");
                continue;
            }

            //interestRules.Add(new InterestRule(date, ruleId, rate));
            System.Console.WriteLine("Interest rule recorded.");
        }
    }

        static void PrintStatement()
    {
        //transactions = transactions.OrderBy(t => t.Date).ToList();
        //decimal balance = 0m;
        //System.Console.WriteLine("| Date       | Type | Amount  | Balance | Interest |");
        //foreach (var t in transactions)
        //{
        //    balance += t.Type == "D" ? t.Amount : -t.Amount;
        //    decimal interest = CalculateInterest(t.Date, balance);
        //    System.Console.WriteLine($"| {t.Date:yyyy-MM-dd} | {t.Type}    | {t.Amount,6}  | {balance,7} | {interest,8} |");
        //}
    }

}
