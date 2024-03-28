using CurrencyExchange.ApplicationCore.Common;
using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.ApplicationCore.Exceptions;
using CurrencyExchange.ApplicationCore.Interfaces;
using CurrencyExchange.ApplicationCore.Model;

namespace CurrencyExchange.ApplicationCore.Services;

/// <summary>
/// Provides a service layer for currency exchange operations, including conversion,
/// retrieval of rates history, and integration with external APIs and data stores.
/// </summary>
public class ExchangeService: IExchangeService
{
    private readonly IFrankFurterClient _frankFurterClient;
    private readonly IConversionHistoryRepository _conversionHistoryRepository;
    private readonly IRedisService _redisService;
    private readonly ICurrencyPairRepository _currencyPairRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeService"/> class.
    /// </summary>
    /// <param name="frankFurterClient">An instance of <see cref="IFrankFurterClient"/> for fetching currency data.</param>
    /// <param name="conversionHistoryRepository">An instance of <see cref="IConversionHistoryRepository"/> for managing conversion history.</param>
    /// <param name="redisService">An instance of <see cref="IRedisService"/> for caching.</param>
    /// <param name="currencyPairRepository">An instance of <see cref="ICurrencyPairRepository"/> for managing currency pairs.</param>
    public ExchangeService(IFrankFurterClient frankFurterClient, 
        IConversionHistoryRepository conversionHistoryRepository,
        IRedisService redisService,
        ICurrencyPairRepository currencyPairRepository)
    {
        _frankFurterClient = frankFurterClient;
        _conversionHistoryRepository = conversionHistoryRepository;
        _redisService = redisService;
        _currencyPairRepository = currencyPairRepository;
    }
    
    /// <summary>
    /// Converts a specified amount from one currency to another.
    /// </summary>
    /// <param name="baseCurrency">The base currency code (e.g., USD).</param>
    /// <param name="targetCurrency">The target currency code (e.g., EUR).</param>
    /// <param name="amount">The amount to convert.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="CurrencyResult"/> containing the converted amount and currency.</returns>
    /// <exception cref="PairNotFoundException">Thrown if the currency pair is not found.</exception>
    /// <exception cref="ArgumentException">Thrown if baseCurrency or targetCurrency are invalid.</exception>
    public async Task<CurrencyResult> Convert(string baseCurrency, string targetCurrency,
        double amount, CancellationToken cancellationToken = default)
    {
        Guard.InvalidCode(baseCurrency);
        Guard.InvalidCode(targetCurrency);
        
        var cacheKey = Helpers.GenerateCacheKeyFromPair(baseCurrency, targetCurrency);
        var currencyPair = await _redisService.GetCachedDataAsync<CurrencyPairEntity>(cacheKey);
        
        if (currencyPair is null)
        {
            var result = await _frankFurterClient.GetCurrencyAsync(1, baseCurrency.ToUpper(), targetCurrency.ToUpper());
            if (result is null)
            {
                throw new PairNotFoundException(baseCurrency.ToUpper(), targetCurrency.ToUpper());
            }
            
            currencyPair = new CurrencyPairEntity
            {
                Base = baseCurrency,
                Target = targetCurrency,
                Rate = result.Rates[targetCurrency.ToUpper()]
            };
            await _redisService.SetCachedDataAsync(cacheKey, currencyPair, TimeSpan.FromSeconds(900));
            await _currencyPairRepository.AddAsync(currencyPair, cancellationToken);
        }

        var targetAmount = Helpers.CalculateRate(amount, currencyPair.Rate);
        
       await _conversionHistoryRepository.AddAsync(new ConversionHistory
       {
           Created = DateTime.Now,
           Base = baseCurrency,
           Amount = amount,
           Target = targetCurrency,
           TargetAmount = targetAmount,
           LastModified = DateTime.Now,
           Rate = currencyPair.Rate
       }, cancellationToken);
       
        return new CurrencyResult
        {
            Currency = targetCurrency,
            Amount = targetAmount
        };
    }
    
    /// <summary>
    /// Retrieves the history of currency conversion rates.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of <see cref="ConversionHistory"/> objects.</returns>
    /// <exception cref="EmptyConversionHistoryException">Thrown if there is no conversion history.</exception>>
    public async Task<List<ConversionHistory>> GetRatesHistory(CancellationToken cancellationToken = default)
    {
        var result = await _conversionHistoryRepository.GetRatesHistoryListAsync(cancellationToken);
        if (!result.Any())
            throw new EmptyConversionHistoryException("conversion history is empty");

        return result;
    }
    
}
