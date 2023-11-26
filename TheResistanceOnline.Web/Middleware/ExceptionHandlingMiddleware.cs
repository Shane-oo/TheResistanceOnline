using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.Core.Exceptions;

namespace TheResistanceOnline.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    #region Fields

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private readonly RequestDelegate _next;

    #endregion

    #region Construction

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    #endregion

    #region Private Methods

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                                                                            StatusCodes.Status400BadRequest,
                                                                            "ValidationFailure",
                                                                            "Validation error",
                                                                            "One or more validation errors has occured",
                                                                            validationException.Errors),
            _ => new ExceptionDetails(StatusCodes.Status500InternalServerError, "ServerError", "Server Error", "An unexpected error has occured", null)
        };
    }

    #endregion

    #region Public Methods

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(Exception exception)
        {
            var exceptionDetails = GetExceptionDetails(exception);
            if (exceptionDetails.Status == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Internal Server Error occured: {Message}", exception.Message);
            }

            var problemDetails = new ProblemDetails
                                 {
                                     Status = exceptionDetails.Status,
                                     Type = exceptionDetails.Type,
                                     Title = exceptionDetails.Title,
                                     Detail = exceptionDetails.Detail
                                 };

            if (exceptionDetails.Errors != null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }

            httpContext.Response.StatusCode = exceptionDetails.Status;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    #endregion

    private record ExceptionDetails(int Status, string Type, string Title, string Detail, IEnumerable<object> Errors);
}
