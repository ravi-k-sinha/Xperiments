namespace Xperiments.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class SimpleMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleMiddleware>();
        }
    }
}
