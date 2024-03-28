using CurrencyExchange.ApplicationCore.Model;
using Refit;

namespace CurrencyExchange.ApplicationCore.Interfaces;

public interface IFrankFurterClient
{
    [Get("/latest")]
    Task<ExchangeResult> GetCurrencyAsync([AliasAs("amount")]double amount
        , [AliasAs("from")]string baseCurrency
        , [AliasAs("to")]string targetCurrency);
}