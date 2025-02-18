namespace AwesomeBank.API.Application.Validations
{
    public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
    {
        public AddTransactionCommandValidator()
        {
            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account Number is Required.");
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Transaction Amount is Required.")
                .GreaterThan(0).WithMessage("Transaction Amount must be greater than zero.")
                .Must((t, amount) => CommonValdations.IsValidDecimalPrecision(amount, 2)).WithMessage("Transaction Amount can have up to two decimal places.");
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Transaction Type is Required.")
                .Length(1, 1).WithMessage("Transaction Type must be exactly one character.")
                .Must(name => name == TransactionType.Withdrawal || name == TransactionType.Deposit).WithMessage("Name must be either 'W' or 'D'.");
            RuleFor(x => x.Date)
                .Must(CommonValdations.IsValidDate).WithMessage("Transaction Date must be a valid date and not the default or maximum date.");
        }
    }
}