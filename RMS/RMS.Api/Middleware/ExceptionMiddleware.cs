using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace RMS.Api.Middleware
{
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
                // 1. We tell ASP.NET Core to let the request pass through and run your Controllers.
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // 2. If ANY Controller or Service throws an error (like "Insufficient Stock"),
                // it bubbles all the way up and gets caught right here!
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 3. Instead of returning a raw HTML crash page, we force it to return clean JSON!
            context.Response.ContentType = "application/json";
            
            // 4. We set the status code to 400 (Bad Request) instead of 500 (Server Crash)
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

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
