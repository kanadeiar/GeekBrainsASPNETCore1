using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastructure.Middleware
{
    public class DebugMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DebugMiddleware> _logger;

        public DebugMiddleware(RequestDelegate next, ILogger<DebugMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //до
            var process = _next(context);
            //во время
            #if DEBUG
            Debug.WriteLine($"Запрос-метод: {context.Request.Method} путь: {context.Request.Path}");
            #endif
            await process;
            //после
        }
    }
}
