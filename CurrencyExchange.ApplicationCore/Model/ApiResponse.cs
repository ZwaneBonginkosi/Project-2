namespace CurrencyExchange.ApplicationCore.Model;

public class ApiResponse
{
    public ApiResponse()
    {

    }
    public ApiResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultStatusCode(statusCode);

    }

    public int StatusCode { get; set; }
    public string Message { get; set; }

    private static string GetDefaultStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Not Authorized",
            404 => "Resource Not Found",
            500 => "Error Found",
            _ => null
        };
    }
}
