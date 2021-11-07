using System.Collections.Generic;

namespace Pug.Application.Security
{
	public interface IUserRoleProvider
	{
		bool UserIsInRole(string user, string domain, string role);

		bool UserIsInRoles(string user, string domain, ICollection<string> roles);

		ICollection<string> GetUserRoles(string user, string domain);
	}
}
