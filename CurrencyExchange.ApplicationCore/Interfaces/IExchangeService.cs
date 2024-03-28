using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.ApplicationCore.Model;

namespace CurrencyExchange.ApplicationCore.Interfaces;

public interface IExchangeService
{
    Task<CurrencyResult> Convert(string baseCurrency, string targetCurrency, double amount, CancellationToken cancellationToken = default);
    Task<List<ConversionHistory>> GetRatesHistory(CancellationToken cancellationToken = default);
}