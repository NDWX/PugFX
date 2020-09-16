using System.Threading.Tasks;

using Pug.Application;

using Microsoft.AspNetCore.Http;

namespace Pug.Web.AspNet.Core
{
    public class RequestSessionProvider : IUserSessionProvider, IAspNetCoreSessionListener
    {
        readonly object HTTP_CONTEXT_ITEM_KEY = typeof(IUserSessionProvider).FullName;
        IHttpContextAccessor httpContextAccessor;

        public RequestSessionProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public event SessionEventHandler SessionStarted = delegate { };
        public event SessionEventHandler SessionEnded = delegate { };

        Pug.Application.IUserSession CreateAndRegisterSession(HttpContext httpContext)
        {
            Pug.Application.IUserSession session = new RequestSession(httpContext);

            httpContext.Items[HTTP_CONTEXT_ITEM_KEY] = session;

            return session;
        }

        public Pug.Application.IUserSession CurrentSession
        {
            get
            {
                Pug.Application.IUserSession session = null;

                HttpContext httpContext = httpContextAccessor.HttpContext;

                if (httpContext != null)
                    if (httpContext.Items.ContainsKey(HTTP_CONTEXT_ITEM_KEY))
                    {
                        session = (RequestSession)httpContext.Items[HTTP_CONTEXT_ITEM_KEY];
                    }
                    else
                    {
                        session = CreateAndRegisterSession(httpContext);
                    }

                return session;
            }
        }

        async Task IAspNetCoreSessionListener.OnSessionStartedAsync(HttpContext context)
        {
            Pug.Application.IUserSession session = CreateAndRegisterSession(context);

            await Task.Run(() => SessionStarted(session));
        }

        async Task IAspNetCoreSessionListener.OnSessionEndedAsync(HttpContext context)
        {
            RequestSession session = (RequestSession)context.Items[HTTP_CONTEXT_ITEM_KEY];

            session.NotifyEnding();

            await Task.Run(() => SessionEnded(session));
        }
    }
}