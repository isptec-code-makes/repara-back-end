using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Repara.Shared.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        var statusCode = GetStatusCode(exception);

        var problemDetails = new ProblemDetails
        {
            Title = "Erro na requisição",
            Status = statusCode,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        
        return true; // Indica que a exceção foi tratada
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            // Add your custom exceptions here, for example:
             BadRequestException => StatusCodes.Status400BadRequest,
             ConflictRequestException => StatusCodes.Status409Conflict,
             ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            // InvalidCredentialsException => StatusCodes.Status401Unauthorized,
            // BusinessRuleException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}