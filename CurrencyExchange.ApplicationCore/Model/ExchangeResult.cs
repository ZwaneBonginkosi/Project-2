using Newtonsoft.Json;

namespace CurrencyExchange.ApplicationCore.Model;

public record ExchangeResult
{
    [JsonProperty("amount")]
    public double Amount { get; set; }
    [JsonProperty("base")]
    public string @Base { get; set; }
    [JsonProperty("date")]
    public string Date { get; set; }
    [JsonProperty("rates")]
    public Dictionary<string, double> Rates { get; set; }
}