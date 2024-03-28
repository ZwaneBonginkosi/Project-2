using System.Text;
using CurrencyExchange.ApplicationCore.Interfaces;
using CurrencyExchange.ApplicationCore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyExchange.WebAPI.Extensions;

/// <summary>
/// Represents an attribute that enables response caching for controller actions.
/// Implements IAsyncActionFilter to provide functionality for intercepting action execution.
/// </summary>
public class CachedAttribute: Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CachedAttribute"/> class.
    /// </summary>
    /// <param name="timeToLiveSeconds">The duration (in seconds) for which responses should be cached.</param>
    public CachedAttribute(int timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }
    
    /// <summary>
    /// Intercepts action execution to implement caching logic.
    /// </summary>
    /// <param name="context">The context of the action being executed.</param>
    /// <param name="next">A delegate to execute the action.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var redisService = context.HttpContext.RequestServices.GetRequiredService<IRedisService>();
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cacheResponse = await redisService.GetCachedDataAsync<CurrencyResult>(cacheKey);
        
        if (cacheResponse != null)
        {
            context.Result = new OkObjectResult(cacheResponse);
            return;
        }
        
        var executedContext =  await next();

        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            await redisService.SetCachedDataAsync(cacheKey, cacheResponse, TimeSpan.FromSeconds(_timeToLiveSeconds));
        }
    }

    /// <summary>
    /// Generates a unique cache key based on the HTTP request's path and query parameters.
    /// </summary>
    /// <param name="request">The current HTTP request.</param>
    /// <returns>A string representing the cache key.</returns>
    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var builder = new StringBuilder();
        builder.Append($"{request.Path}");
        foreach (var (key, value) in request.Query.OrderBy(k=>k.Key))
        {
            builder.Append($"{key}_{value}");
        }

        return builder.ToString();
    }
}
