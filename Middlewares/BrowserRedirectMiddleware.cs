using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace Middleware.Middlewares
{
    public class BrowserRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public BrowserRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();
            var clientInfo = parser.Parse(userAgent);

            var browserName = clientInfo.UA.Family.ToLower();

            if (browserName == "edge" || browserName == "edgechromium" || browserName == "ie")
            {
                context.Response.Redirect("https://www.mozilla.org/pl/firefox/new/");
                return;
            }

            await _next(context);
        }
    }

    public static class BrowserRedirectExtensions
    {
        public static IApplicationBuilder UseBrowserRedirect(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BrowserRedirectMiddleware>();
        }
    }
}
