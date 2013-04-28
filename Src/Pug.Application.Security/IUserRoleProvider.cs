using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public interface IUserRoleProvider
	{
		bool UserIsInRole(string user, string role);

		bool UserIsInRoles(string user, ICollection<string> roles);

		ICollection<string> GetUserRoles(string user);
	}
}
