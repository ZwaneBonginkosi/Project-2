using System.Threading.RateLimiting;
using CurrencyExchange.ApplicationCore.Interfaces;
using CurrencyExchange.Infrastructure.Data;
using CurrencyExchange.Infrastructure.Data.Persistence;
using CurrencyExchange.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace CurrencyExchange.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IConversionHistoryRepository, ConversionHistoryRepository>();
        services.AddScoped<ICurrencyPairRepository, CurrencyPairRepository>();
        services.AddScoped<IRedisService, RedisService>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "CurrencyExchange_";
        });
        services.AddDbContext<AppDbContext>(c =>
            c.UseMySQL(configuration.GetConnectionString("MySqlConnection")));
        
        services.AddResiliencePipeline("my-pipeline", builder =>
        {
            builder
                .AddRateLimiter(new SlidingWindowRateLimiter(
                    new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1)
                    }))   
                .AddTimeout(TimeSpan.FromSeconds(2));
        });
        
        return services;
    }
}
