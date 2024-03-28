using System.Net;
using CurrencyExchange.ApplicationCore.Exceptions;
using CurrencyExchange.ApplicationCore.Model;
using Newtonsoft.Json;
using Polly.RateLimiting;

namespace CurrencyExchange.WebAPI.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(httpContext, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        if (exception is InvalidCodeException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = JsonConvert.SerializeObject(new ApiResponse(context.Response.StatusCode,
                validationException.Message));
            
            await context.Response.WriteAsync(response);
        }
        else if (exception is PairNotFoundException pairNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = JsonConvert.SerializeObject(new ApiResponse(context.Response.StatusCode,
                pairNotFoundException.Message));
            await context.Response.WriteAsync(response);
        }
        else if (exception is RateLimiterRejectedException rateLimiterRejectedException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = JsonConvert.SerializeObject(new ApiResponse(context.Response.StatusCode,
                "Rate limiter"));
            await context.Response.WriteAsync(response);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = JsonConvert.SerializeObject(new ApiResponse(context.Response.StatusCode,
                exception.Message));
            await context.Response.WriteAsync(response);
        }
    }
    
}