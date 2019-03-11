using Microsoft.AspNetCore.Builder;

namespace Pug.Web.AspNet.Core.Application
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseRequestSessionNotifier(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestSessionNotifierMiddleware>();

            return builder;
        }
    }
}

