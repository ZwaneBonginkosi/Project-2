namespace CurrencyExchange.ApplicationCore.Exceptions;

public class InvalidCodeException: Exception
{
    public InvalidCodeException(string message):base($"{message} is not a valid currency code"){}
}