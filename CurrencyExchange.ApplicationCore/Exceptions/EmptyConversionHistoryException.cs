namespace CurrencyExchange.ApplicationCore.Exceptions;

public class EmptyConversionHistoryException: Exception
{
    public EmptyConversionHistoryException(string message): base(message) {}
}