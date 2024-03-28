using CurrencyExchange.ApplicationCore.Entities;

namespace CurrencyExchange.ApplicationCore.Interfaces;

public interface IConversionHistoryRepository
{
    Task AddAsync(ConversionHistory conversionHistory, CancellationToken cancellationToken);
    Task<List<ConversionHistory>> GetRatesHistoryListAsync(CancellationToken cancellationToken= default);
}
