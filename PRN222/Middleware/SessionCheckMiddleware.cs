using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProductManagementASPNETCoreRazorPages.Middleware
{
    public class SessionCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path != null &&
                (path.StartsWith("/credential/login") ||
                 path.StartsWith("/credential/register") ||
                 path.StartsWith("/css") ||
                 path.StartsWith("/js") ||
                 path.StartsWith("/lib") ||
                 path.Contains("favicon")))
            {
                await _next(context);
                return;
            }

            var userId = context.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.Redirect("/Credential/Login");
                return;
            }

            await _next(context);
        }
    }
}
