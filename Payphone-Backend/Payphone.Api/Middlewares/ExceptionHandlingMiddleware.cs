using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Payphone.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorMessage = "Ha ocurrido un error inesperado.";
                        
            if (ex is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = ex.Message;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = errorMessage,
                details = ex.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }

    }
}
