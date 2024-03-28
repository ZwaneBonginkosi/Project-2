using System.Text.Json;
using CurrencyExchange.ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CurrencyExchange.Infrastructure.Services;

/// <summary>
/// Provides a service layer for interacting with a Redis distributed cache.
/// Handles caching, retrieval, and serialization of data. 
/// </summary>
public class RedisService: IRedisService
{
    private readonly IDistributedCache _cache;
    /// <summary>
    /// Initializes a new instance of the <see cref="RedisService"/> class.
    /// </summary>
    /// <param name="cache">An instance of <see cref="IDistributedCache"/> for interacting with Redis.</param>
    public RedisService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    /// <summary>
    /// Asynchronously retrieves cached data from Redis.
    /// </summary>
    /// <typeparam name="T">The type of data to deserialize.</typeparam>
    /// <param name="key">The unique key associated with the cached data.</param>
    /// <returns>The deserialized data of type T, or the default value for T if the data is not found.</returns>
     public async Task<T> GetCachedDataAsync<T>(string key)
    {
        var jsonData = await _cache.GetStringAsync(key);
        if (jsonData == null)
            return default(T);
        return JsonSerializer.Deserialize<T>(jsonData);
    }
    
    /// <summary>
    /// Asynchronously caches data in Redis with a specified expiration time.
    /// </summary>
    /// <typeparam name="T">The type of data to serialize.</typeparam>
    /// <param name="key">The unique key to store the data under.</param>
    /// <param name="data">The data to be cached.</param>
    /// <param name="cacheDuration">The duration for which the data should be cached.</param>
    public async Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration
        };
        var jsonData = JsonSerializer.Serialize(data);
        await _cache.SetStringAsync(key, jsonData, options);
    }
}