namespace AwesomeBank.Console.Helpers;

public interface ICommandHandleHelper
{
    public Task<TReturn?> HandleCommandAsync<TCommand, TValidator, TReturn>(TCommand command, TValidator validator)
        where TCommand : class
        where TValidator : AbstractValidator<TCommand>;
}