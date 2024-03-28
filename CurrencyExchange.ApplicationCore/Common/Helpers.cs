using System.Text;

namespace CurrencyExchange.ApplicationCore.Common;

public static class Helpers
{
    public static string GenerateCacheKeyFromPair(string baseCurrency, string targetCurrency)
    {
        var builder = new StringBuilder();
        builder.Append($"{baseCurrency}-{targetCurrency}");
        
        return builder.ToString();
    }
    
    public static double CalculateRate(double amount, double rate)
    {
        return rate * amount;
    }
}