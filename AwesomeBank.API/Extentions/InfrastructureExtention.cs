using AwesomeBank.API.Application.Queries;
using AwesomeBank.API.Application.Services;

namespace AwesomeBank.API.Extentions;

public static class InfrastructureExtention
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountQueries, AccountQueries>();
        services.AddScoped<IInterestRulesQueries, InterestRulesQueries>();
        services.AddScoped<IStatementService, StatementService>();
        return services;
    }
}