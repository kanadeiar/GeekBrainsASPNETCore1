using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.WebAPI.Infrastructure.Middleware
{
    /// <summary> Промежуточное ПО обработки исключений </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary> Конструктор </summary>
        /// <param name="next">Следующий вызов</param>
        /// <param name="logger">Логер</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary> Вызов ПО </summary>
        /// <param name="context">Окружение</param>
        /// <returns></returns>
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
