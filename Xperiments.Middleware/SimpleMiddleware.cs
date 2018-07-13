namespace Xperiments.Middleware
{
    using LendFoundry.Foundation.Logging;
    using Microsoft.AspNetCore.Http;
    using System.IO;
    using System.Text;
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

            var injectedRequestStream = new MemoryStream();

            try
            {
                var requestLog = $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    var bodyAsText = bodyReader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                    {
                        requestLog += $", Body: {bodyAsText}";
                    }

                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                    injectedRequestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = injectedRequestStream;
                }

                _logger.Debug(requestLog);
                await _next(context);
            }
            finally
            {
                injectedRequestStream.Dispose();
            }
            
        }

    }
}
