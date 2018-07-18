namespace Xperiments.Middleware
{
    using LendFoundry.Foundation.Logging;
    using Microsoft.AspNetCore.Http;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.Info("SimpleMiddleware.InvokeAsync()");

            if (context.Request.Path.ToString().Contains("swagger.json") ||
                context.Request.Path.ToString().Contains("api-docs"))
            {
                await _next(context);
                return; // Dont log request for API documentation page
            }

            var timer = Stopwatch.StartNew();

            _logger.Debug(await LogRequest2(context));
            timer.Stop();
            _logger.Debug($"Request logging took time: {timer.ElapsedMilliseconds}");

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                timer = Stopwatch.StartNew();

                _logger.Debug(await LogResponse(context));

                await responseBody.CopyToAsync(originalBodyStream);

                timer.Stop();
                _logger.Debug($"Response logging took time: {timer.ElapsedMilliseconds}");
            }

        }

        private async Task<string> LogRequest(HttpContext context)
        {
            // Body Logging as described in http://www.palador.com/2017/05/24/logging-the-body-of-http-request-and-response-in-asp-net-core/

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

                return requestLog;
            }
            finally
            {
                //injectedRequestStream.Dispose();
            }
        }

        private async Task<string> LogRequest2(HttpContext context)
        {
            var requestLog = $"REQUEST ";

            var injectedRequestStream = new MemoryStream();

            await Task.Run(() =>
            {
                requestLog = $"HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";
                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    var bodyAsText = bodyReader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                    {
                        requestLog += $", Body: {bodyAsText}";
                    }
                    else
                    {
                        requestLog += $", Body: <empty>";
                    }

                    // Copy data and push it back in the context
                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                    injectedRequestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = injectedRequestStream;
                }
            });
            
            return requestLog;
        }

        private async Task<string> LogResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            return $"Response {text}";
        }
    }
}
