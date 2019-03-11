
using Pug.Application;

using Microsoft.Extensions.DependencyInjection;

namespace Pug.Web.AspNet.Core.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddRequestSessionProvider(this IServiceCollection services)
        {
            services.AddSingleton<RequestSessionProvider>();
            services.AddSingleton<IUserSessionProvider>(provider => provider.GetService<RequestSessionProvider>());
            services.AddSingleton<IAspNetCoreSessionListener>(provider => provider.GetService<RequestSessionProvider>());
        }
    }
}

