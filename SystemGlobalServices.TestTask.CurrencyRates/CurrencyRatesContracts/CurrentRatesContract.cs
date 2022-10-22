using System.Text.Json.Serialization;

namespace SystemGlobalServices.TestTask.CurrencyRates.CurrencyRatesContracts;

public sealed class CurrentRatesContract
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    [JsonPropertyName("PreviousURL")]
    public string? PreviousUrl { get; set; }
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("Valute")] 
    public Dictionary<string, CurrencyContract> Currencies { get; set; } = null!;
}