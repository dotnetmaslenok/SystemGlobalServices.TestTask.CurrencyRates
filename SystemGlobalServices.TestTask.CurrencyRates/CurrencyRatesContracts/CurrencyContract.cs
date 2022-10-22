using System.Text.Json.Serialization;

namespace SystemGlobalServices.TestTask.CurrencyRates.CurrencyRatesContracts;

public sealed class CurrencyContract 
{
    [JsonPropertyName("ID")]
    public string? Id { get; set; }
    public string? NumCode { get; set; }
    public string? CharCode { get; set; }
    [JsonPropertyName("Nominal")]
    public int Denomination { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("Value")]
    public decimal CurrentValue { get; set; }
    [JsonPropertyName("Previous")]
    public decimal PreviousValue { get; set; }
}