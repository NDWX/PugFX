using System.Collections.Generic;

namespace Pug.Application.Security
{
	public interface IAuthorizationProvider
	{
		bool UserIsAuthorized(IDictionary<string, string> context, ICredentials credentials, string operation, string objectType, string objectName = "");
	}
}
