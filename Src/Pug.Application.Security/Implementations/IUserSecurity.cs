using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public interface IUserSecurity
	{
		bool UserIsAuthorized(ICredentials credentials, string operation, ICollection<string> objectNames, IDictionary<string, string> context);
	}
}
