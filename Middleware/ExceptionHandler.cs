using Domain.Abstraction;
using Microsoft.AspNetCore.Diagnostics;

namespace ChatManagement.Middleware;

public class ExceptionHandler : IExceptionHandler
{

    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception : ${Message}", exception.Message);

        var exceptionMessage = string.Empty;

        if (exception is not ChatAppException && httpContext.Response.StatusCode >= StatusCodes.Status500InternalServerError)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            exceptionMessage = $"Something wentwrong !";
        }
        else
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        ProblemDetails problemDetails = new(httpContext.Response.StatusCode, string.IsNullOrEmpty(exceptionMessage) ? exception.Message : exceptionMessage);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    public record ProblemDetails(int StatusCode, string Description);
}
