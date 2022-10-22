using SystemGlobalServices.TestTask.CurrencyRates.CurrencyRatesContracts;

namespace SystemGlobalServices.TestTask.CurrencyRates.Responses;

public sealed class CurrencyPaginationResponse
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    public string? PreviousUrl { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, CurrencyContract> Currencies { get; set; } = null!;
}