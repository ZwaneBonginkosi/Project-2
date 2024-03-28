using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.ApplicationCore.Interfaces;

namespace CurrencyExchange.Infrastructure.Data.Persistence;

/// <summary>
/// Provides a repository for managing currency pair data, interacting with the 
/// underlying database context (AppDbContext).
/// </summary>
public class CurrencyPairRepository: ICurrencyPairRepository
{
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyPairRepository"/> class.
    /// </summary>
    /// <param name="dbContext">An instance of <see cref="AppDbContext"/> for database interactions.</param>
    public CurrencyPairRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Asynchronously adds a new currency pair record to the database.
    /// </summary>
    /// <param name="currencyPair">The <see cref="CurrencyPairEntity"/> object to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    public async Task AddAsync(CurrencyPairEntity currencyPair, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(currencyPair, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}