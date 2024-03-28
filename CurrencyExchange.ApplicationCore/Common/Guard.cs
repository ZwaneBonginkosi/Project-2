using CurrencyExchange.ApplicationCore.Exceptions;

namespace CurrencyExchange.ApplicationCore.Common;

public static class Guard
{
    public static void InvalidCode(string value)
    {
        if (value.Length is not 3 || !value.All(char.IsLetter))
        {
            throw new InvalidCodeException(value);
        }
    }
}