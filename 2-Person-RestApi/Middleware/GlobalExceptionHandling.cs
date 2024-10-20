using Microsoft.AspNetCore.Diagnostics;
namespace Person_RestApi.Middleware;

public class GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger) : IExceptionHandler 
{
    private readonly ILogger<GlobalExceptionHandling> _logger = logger;
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Could not process a request on the Machine {MachineName}. TraceID: {TraceId}",
            Environment.MachineName, httpContext.TraceIdentifier);

        var (stauscode, title) = MapException(exception);
        
        await Results.Problem(
            title: title,
            statusCode: stauscode,
            extensions: new Dictionary<string, object?>()
            {
                { "traceId", httpContext.TraceIdentifier }
            }).ExecuteAsync(httpContext);
        
        return true;
    }

    private static (int statusCode, string title) MapException(Exception exception)
    {
        
        return exception switch
        {
            ArgumentNullException => (StatusCodes.Status400BadRequest, "You made a mistake, fix it!"),
            _ => (StatusCodes.Status500InternalServerError, "We made a mistake, but we a re working on it!")
        };
    }
}