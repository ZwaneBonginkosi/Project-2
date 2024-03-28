using CurrencyExchange.ApplicationCore.Interfaces;
using CurrencyExchange.ApplicationCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;

namespace CurrencyExchange.ApplicationCore;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        var url = configuration["Url"] ?? string.Empty;
        
        services.AddRefitClient<IFrankFurterClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(url);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(
                    3, retryNumber => TimeSpan.FromMilliseconds(2)));
        
        services.AddScoped<IExchangeService, ExchangeService>();
        return services;
    }
}
