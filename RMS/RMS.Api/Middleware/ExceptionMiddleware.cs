using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace RMS.Api.Middleware
{
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
                // 1. We tell ASP.NET Core to let the request pass through and run your Controllers.
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // [FIX] Log the actual error to the server console so developers can debug it!
                _logger.LogError(ex, "An unhandled exception occurred during the request.");
                
                // 2. If ANY Controller or Service throws an error it gets caught here!
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            // [FIX] Enterprise Future-Proofing
            // Instead of guessing based on typeof(Exception), we explicitly check if we threw a BadRequestException!
            // If we did, it's a 400 Bad Request. If it's literally anything else, it's a 500 Server Crash.
            if (exception is RMS.Application.Exceptions.BadRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            // 5. We package the error message cleanly so the React frontend can read it!
            var response = new 
            {
                error = exception.Message,
                isSuccess = false
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
