using System.Collections.Generic;

namespace Pug.Application.Security
{
	public interface IAuthorizationProvider
	{
		bool UserIsAuthorized(IDictionary<string, string> context, IUser user, string operation, string objectType,
							string objectName = "", string purpose = "", string domain = "");
	}
}
