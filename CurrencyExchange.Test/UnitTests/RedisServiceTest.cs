using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;

namespace CurrencyExchange.Test.UnitTests;

[TestClass]
public class RedisServiceTest
{
    private readonly Mock<IDistributedCache> _cache;

    public RedisServiceTest()
    {
        _cache = new Mock<IDistributedCache>();
    }
    
    [TestMethod]
    public async Task GetCachedDataAsyncTest()
    {
        // Arrange
        var redisService = new RedisService(_cache.Object);

        // Act
        try
        {
            await redisService.GetCachedDataAsync<CurrencyPairEntity>("Test");
        }
        catch (Exception e)
        {
            Assert.IsTrue(e is JsonException);
        }
    }
}