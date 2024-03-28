namespace CurrencyExchange.ApplicationCore.Entities;

public class CurrencyPairEntity
{
    public int Id { get; set; }
    public string Base { get; set; }
    public string Target { get; set; }
    public double Rate { get; set; }
}