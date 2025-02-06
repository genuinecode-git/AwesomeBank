// See https://aka.ms/new-console-template for more information

using AwesomeBank.API.Application.Mappings;
using AwesomeBank.API.Application.Queries;
using AwesomeBank.Console.Services;
using AwesomeBank.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Create Host with DI and Configuration
var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.console.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddInterestRuleCommandHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddTransactionCommandHandler).Assembly));

        // Configure Serilog from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure Serilog to use the settings from appsettings.json
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
        services.AddAutoMapper(typeof(AutoMapperProfile));

        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IAccountQueries, AccountQueries>();
        services.AddSingleton<IInterestRulesQueries, InterestRulesQueries>();
        services.AddSingleton<IStatementService, AwesomeBank.API.Application.Services.StatementService>();
        services.AddSingleton<IConsoleStatementService, AwesomeBank.Console.Services.StatementService>();
        services.AddSingleton<ITransactionService, TransactionService>();
        services.AddSingleton<IInterestRuleService, InterestRuleService>();
        services.AddSingleton<ICommandHandleHelper, CommandHandleHelper>();

        services.AddSingleton<IConsoleApplication, AwsomeBankApplication>();
    })
    .UseSerilog() // Integrate Serilog
    .Build();

var consoleApplication = host.Services.GetRequiredService<IConsoleApplication>();
await consoleApplication.Run();
await host.StopAsync();