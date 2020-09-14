using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.SessionState;

namespace Pug.Application.Security.Web
{
	public interface IAspNetGlobalEventsListener
	{
		
	}

	public class AspNetGlobalEventsListener
	{
		public AspNetGlobalEventsListener(SessionStateModule sessionStateModule)
		{
			sessionStateModule.
		}
	}
	public class AspNetUserSessionProvider : IUserSessionProvider
	{
		const string SessionStateModuleName = "SessionStateModule";
		
		private readonly HttpContext _httpContext;
		public event SessionEventHandler SessionStarted;
		public event SessionEventHandler SessionEnded;
		SessionStateModule sessionStateModule;

		public AspNetUserSessionProvider(HttpContext httpContext)
		{
			_httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));

			if(_httpContext.ApplicationInstance.Modules.AllKeys.Contains(SessionStateModuleName))
			{
				sessionStateModule =
					(SessionStateModule) _httpContext.ApplicationInstance.Modules.Get(SessionStateModuleName);

				sessionStateModule.Start += (sender, args) =>
				{
					
				};

				sessionStateModule.End += (o, eventArgs) => { };
			}
		}

		public IUserSession CurrentSession
		{
			get
			{
				return new AspNetUserSession(_httpContext.Session);
			}
		}
	}

	class AspNetUserSession : IUserSession
	{
		public AspNetUserSession(System.Web.SessionState.HttpSessionState sessionState)
		{
			sessionState.
		}

	public T Get<T>(string identifier = "")
		{
			throw new NotImplementedException();
		}

		public void Set<T>(string identifier, T value)
		{
			throw new NotImplementedException();
		}

		public void Remove<T>(string identifier)
		{
			throw new NotImplementedException();
		}

		public event EventHandler Ending;
	}
}