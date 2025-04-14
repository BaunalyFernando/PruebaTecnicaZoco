using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PruebaTecnicaZoco.Common.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;

        if (exception is NotFoundException)
            statusCode = HttpStatusCode.NotFound;
        else if (exception is BadRequestException)
            statusCode = HttpStatusCode.BadRequest;
        else if (exception is UnauthorizedException)
            statusCode = HttpStatusCode.Unauthorized;
        else
            statusCode = HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(new
        {
            error = exception.Message
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(result);
    }

}
