namespace AwesomeBank.API.Application.Validations
{
    public class AddInterestRuleCommandValidator : AbstractValidator<AddInterestRuleCommand>
    {
        public AddInterestRuleCommandValidator()
        {
            RuleFor(x => x.RuleId)
                .NotEmpty().WithMessage("RuleId is Required.");
            RuleFor(x => x.Date)
               .Must(CommonValdations.IsValidDate).WithMessage("Date must be a valid date and not the default or maximum date.");
            RuleFor(x => x.Rate)
                .GreaterThan(0).WithMessage("Rate must be greater than zero.")
                .LessThan(100).WithMessage("Rate must be less than 100.")
                .Must((t, amount) => CommonValdations.IsValidDecimalPrecision(amount, 2)).WithMessage("Rate can have up to two decimal places.");
        }
    }
}