using AwesomeBank.API.Application.Models.Requests;

namespace AwesomeBank.API.Application.Validations
{
    public class StatementRequestValidator : AbstractValidator<StatementRequest>
    {
        public StatementRequestValidator()
        {
            RuleFor(x => x.AccountNumber)
               .NotEmpty().WithMessage("Account Number is Required.");
            RuleFor(x => x.Month)
               .NotEmpty().WithMessage("Month is Required.")
               .Length(2).WithMessage("Month must be exactly 2 digit.")
               .Must(CommonValdations.IsValidNumber).WithMessage("Month should be numeric.");
            RuleFor(x => x.Year)
               .NotEmpty().WithMessage("Year is Required.")
               .Length(4).WithMessage("Year must be exactly 4 digit.")
               .Must(CommonValdations.IsValidNumber).WithMessage("Year should be numeric.");
        }
    }
}