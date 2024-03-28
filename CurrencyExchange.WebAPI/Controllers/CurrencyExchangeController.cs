using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.ApplicationCore.Interfaces;
using CurrencyExchange.ApplicationCore.Model;
using CurrencyExchange.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.WebAPI.Controllers;

/// <summary>
/// Defines the API controller responsible for currency exchange operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CurrencyExchangeController : ControllerBase
{
    private readonly IExchangeService _exchangeService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyExchangeController"/> class.
    /// </summary>
    /// <param name="exchangeService">The exchange service required for currency calculations.</param>
    public CurrencyExchangeController(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }
    
    /// <summary>
    /// Handles requests for currency conversion, caching results for 900 seconds (15 minutes).
    /// </summary>
    /// <param name="baseCurrency">The three-letter code (e.g., USD) of the base currency.</param>
    /// <param name="targetCurrency">The three-letter code (e.g., EUR) of the target currency.</param>
    /// <param name="amount">The amount in the base currency to convert.</param>
    /// <returns>An ActionResult containing the calculated CurrencyResult.</returns>
    [Cached(900)]
    [HttpGet("/convert")]
    public async Task<ActionResult<CurrencyResult>> Get([FromQuery(Name = "base")] string baseCurrency,
        [FromQuery(Name = "target")] string targetCurrency,
        [FromQuery(Name = "amount")] double amount)
    {
        var result = await _exchangeService.Convert(baseCurrency, targetCurrency, amount);
        return Ok(result);
    }
    
    /// <summary>
    /// Handles requests to retrieve historical currency exchange rates.
    /// </summary>
    /// <returns>An ActionResult containing a list of ConversionHistory objects.</returns>
    [HttpGet("/history")]
    public async Task<ActionResult<List<ConversionHistory>>> Get()
    {
        var result = await _exchangeService.GetRatesHistory();
        return Ok(result);
    }
}
