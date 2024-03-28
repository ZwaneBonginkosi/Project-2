using CurrencyExchange.ApplicationCore.Entities;

namespace CurrencyExchange.ApplicationCore.Interfaces;

public interface ICurrencyPairRepository
{
    Task AddAsync(CurrencyPairEntity currencyPair, CancellationToken cancellationToken);
}