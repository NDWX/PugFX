using System.Collections.Generic;

namespace Pug.Application.Security
{
	public interface IUserSecurity
	{
		bool UserIsInRole(ICredentials credentials, string role);

		bool UserIsAuthorized(ICredentials credentials, string operation, ICollection<string> objectNames, IDictionary<string, string> context);
	}
}
