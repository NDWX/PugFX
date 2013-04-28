using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Pug.Application
{
	public class AspNetUserSessionProvider : IApplicationUserSessionProvider
	{
		string aspNetSessionKey = "_Pug.Application.UserSession.AspNet_";

		public AspNetUserSessionProvider()
		{

		}

		public ApplicationUserSession CurrentSession
		{
			get
			{
				System.Web.SessionState.HttpSessionState currentSession = HttpContext.Current.Session;

				ApplicationUserSession session;

				if (currentSession[aspNetSessionKey] == null)
				{
					session = new ApplicationUserSession(currentSession.SessionID);
					currentSession[aspNetSessionKey] = session;
				}
				else
					session = (ApplicationUserSession)currentSession[aspNetSessionKey];

				return session;
			}
		}
	}
}
