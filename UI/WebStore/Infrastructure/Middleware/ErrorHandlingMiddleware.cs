using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DebugMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<DebugMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                HandleException(e, context);
                throw;
            }
        }

        private void HandleException(Exception exception, HttpContext context)
        {
            _logger.LogError(exception, $"Ошибка при обработке запроса {context.Request.Path}");
        }

    }
}
