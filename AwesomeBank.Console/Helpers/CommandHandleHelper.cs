namespace AwesomeBank.Console.Helpers;

public class CommandHandleHelper(IMediator mediator) : ICommandHandleHelper
{
    private readonly IMediator _mediator = mediator;

    public async Task<TReturn?> HandleCommandAsync<TCommand, TValidator, TReturn>(TCommand command, TValidator validator)
        where TCommand : class
        where TValidator : AbstractValidator<TCommand>
    {
        // Validate the command
        ValidationResult result = await validator.ValidateAsync(command);

        if (result.IsValid)
        {
            var response = await _mediator.Send(command);

            return response is TReturn resultReturn ? resultReturn : default;
        }
        else
        {
            System.Console.WriteLine("\nValidation failed:");

            foreach (var failure in result.Errors)
            {
                System.Console.WriteLine($"Property: {failure.PropertyName}, Error: {failure.ErrorMessage}");
            }
            System.Console.WriteLine("\n");
        }
        return default;
    }
}