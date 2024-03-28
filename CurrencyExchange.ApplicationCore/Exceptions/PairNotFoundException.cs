namespace CurrencyExchange.ApplicationCore.Exceptions;

public class PairNotFoundException: Exception
{
    public PairNotFoundException(string baseCurrency,string targetCurrency):base($"{baseCurrency} and {targetCurrency} was not found") {}
}