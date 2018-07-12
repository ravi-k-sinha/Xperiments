namespace Xperiments.Middleware
{
    using LendFoundry.Foundation.Logging;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public SimpleMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.Info("SimpleMiddleware.InvokeAsync()");
            await _next(context);
        }

    }
}
