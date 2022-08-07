using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pug.Application.Security
{
	public interface IUserRoleProvider
	{
		bool UserIsInRole(string user, string role);

		bool UserIsInRoles(string user, ICollection<string> roles);

		Task<bool> UserIsInRolesAsync(string user, ICollection<string> roles);

		ICollection<string> GetUserRoles(string user, string domain);

		Task<IEnumerable<string>> GetUserRolesAsync(string user, string domain);
	}
}
