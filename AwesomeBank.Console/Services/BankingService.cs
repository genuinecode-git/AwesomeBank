
namespace AwesomeBank.Console.Services;

public class BankingService(HttpClient httpClient, ILogger<BankingService> logger) : IBankingService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<BankingService> _logger = logger;
}
