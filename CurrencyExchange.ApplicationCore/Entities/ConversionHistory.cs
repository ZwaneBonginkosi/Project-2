namespace CurrencyExchange.ApplicationCore.Entities;

public class ConversionHistory
{
    public int Id { get; set; }
    public string Base { get; set; }
    public double Amount { get; set; }
    public string Target { get; set; }
    public double TargetAmount { get; set; }
    public double Rate { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}
