using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Data.Persistence;

/// <summary>
/// Provides a repository for managing conversion history data, 
/// interacting with the underlying database context (AppDbContext).
/// </summary>
public class ConversionHistoryRepository: IConversionHistoryRepository
{
    private readonly AppDbContext _dbContext; // Represents the database context for data access
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionHistoryRepository"/> class.
    /// </summary>
    /// <param name="dbContext">An instance of <see cref="AppDbContext"/> for database interactions.</param>
    public ConversionHistoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Asynchronously adds a new conversion history record to the database.
    /// </summary>
    /// <param name="conversionHistory">The <see cref="ConversionHistory"/> object to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    public async Task AddAsync(ConversionHistory conversionHistory, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(conversionHistory, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Asynchronously retrieves the full list of conversion history records from the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of <see cref="ConversionHistory"/> objects.</returns>
    public async Task<List<ConversionHistory>> GetRatesHistoryListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.RatesHistory.ToListAsync(cancellationToken);
    }
}
