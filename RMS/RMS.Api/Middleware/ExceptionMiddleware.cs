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
            
            // [FIX] Differentiate between a Business Rule (400) and a Server Crash (500)
            // Because we used 'throw new Exception()' for our business rules, we can check the exact type!
            // If it's a NullReferenceException or Database drop, it will be 500.
            if (exception.GetType() == typeof(Exception))
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
