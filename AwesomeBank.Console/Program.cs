// See https://aka.ms/new-console-template for more information

using AwesomeBank.Console.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Create Host with DI and Configuration
var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        // Configure Serilog from appsettings.json
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(config["Logging:FilePath"], rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        // Register HttpClient with BaseUrl from appsettings.json
        services.AddHttpClient<IBankingService, BankingService>(client =>
        {
            client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]);
        });

        services.AddSingleton<IBankingService, BankingService>();
    })
    .UseSerilog() // Integrate Serilog
    .Build();

AwsomeBankApplication.StartApplication();
await host.StopAsync();